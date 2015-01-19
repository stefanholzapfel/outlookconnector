using OutlookAddIn;
using Shared;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddIn
{
    /// <summary>
    /// This class provides all the logic to interact with an Outlook calendar
    /// </summary>
    public class CalendarHandler : ICalendarSyncable
    {
        private Outlook.Application _outlookApp;
        private Outlook.MAPIFolder _primaryCalendar;
        private Outlook.MAPIFolder _customCalendar;
        private String _calendarName;

        private Outlook.Items _items;
        private SyncDataStorage _syncStorage;

        /// <summary>
        /// Local data storage for synchronization data
        /// </summary>
        public class SyncDataStorage
        {
            public List<String> DeletedItems = new List<string>();
            public DateTime LastSyncTime = DateTime.MinValue;
        }

        private const String SYNCSTORAGE_FILENAME = "OutlookSyncStorage";
        private const String ITEM_PROPERTY_GLOBAL_A_ID = "GAI";
        private const String ITEM_PROPERTY_SYNC_ID = "SyncID";
        private const String ITEM_PROPERTY_SYNC_UPDATE = "SyncUpdate";

        /// <summary>
        /// Name of this connector / calendar
        /// </summary>
        public String ConnectorName
        {
            get { return _calendarName; }
        }

        public ConnectorSettings Settings
        {
            // the interface to Outlook does not require this
            set { }
        }

        /// <summary>
        /// Part of the interface but not needed on Outlook side. Always returns 0 (OK)
        /// </summary>
        /// <returns></returns>
        public int CheckConnectivity(String connector, String url, String username, String password)
        {
            return 0;
        }

        /// <summary>
        /// Initializes the CalendarHandler
        /// </summary>
        /// <param name="outlookApp">reference to the active Outlook application</param>
        public CalendarHandler(Outlook.Application outlookApp, String calendarName)
        {
            this._outlookApp = outlookApp;
            this._calendarName = calendarName;
            Initialize();
        }

        /// <summary>
        /// Sets references to all important calendars and events
        /// </summary>
        private void Initialize()
        {
            _primaryCalendar = (Outlook.MAPIFolder)_outlookApp.ActiveExplorer().Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

            // check if the custom calendar already exists
            foreach (Outlook.MAPIFolder calendar in _primaryCalendar.Folders)
            {
                if (calendar.Name == _calendarName)
                {
                    _customCalendar = calendar;
                    break;
                }
            }

            if (_customCalendar != null) SetEvents();

            LoadLocalStorage();
            if (_syncStorage == null) _syncStorage = new SyncDataStorage();
        }

        /// <summary>
        /// Loads the SyncDataStorage from the local storage file
        /// </summary>
        private void LoadLocalStorage()
        {
            FileManager fileManager = new FileManager();
            _syncStorage = fileManager.LoadXML<SyncDataStorage>(SYNCSTORAGE_FILENAME);
        }

        /// <summary>
        /// Saves the SyncDataStorage into the local storage file
        /// </summary>
        private void SaveToLocalStorage()
        {
            FileManager fileManager = new FileManager();
            fileManager.SaveXML<SyncDataStorage>(_syncStorage, SYNCSTORAGE_FILENAME);
        }

        /// <summary>
        /// Creates a new custom calendar in Outlook (if it does not exist yet)
        /// </summary>
        public void CreateCustomCalendar()
        {
            if (_customCalendar != null) return;

            try
            {
                // create new calendar
                _customCalendar = _primaryCalendar.Folders.Add(_calendarName, Outlook.OlDefaultFolders.olFolderCalendar);

                // add the new custom calendar to the navigation panel
                Outlook.NavigationPane objPane = _outlookApp.ActiveExplorer().NavigationPane;
                Outlook.CalendarModule objModule = (Outlook.CalendarModule)objPane.Modules.GetNavigationModule(Outlook.OlNavigationModuleType.olModuleCalendar);
                Outlook.NavigationGroup objGroup = objModule.NavigationGroups.GetDefaultNavigationGroup(Outlook.OlGroupType.olMyFoldersGroup);
                Outlook.NavigationFolder objNavFolder = objGroup.NavigationFolders.Add(_customCalendar);

                // Set the navigation folder to be displayed in overlay mode by default. The IsSelected property can't be set to True 
                // unless the CalendarModule object is the current module displayed in the Navigation Pane
                objPane.CurrentModule = objPane.Modules.GetNavigationModule(Outlook.OlNavigationModuleType.olModuleCalendar);
                objNavFolder.IsSelected = true;
                objNavFolder.IsSideBySide = false;

                SetEvents();

            }
            catch (Exception ex)
            {
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Renames the custom calendar
        /// </summary>
        /// <param name="newCalendarName">new calendar name</param>
        public void RenameCustomCalendar(String newCalendarName)
        {
            if (_customCalendar == null || String.IsNullOrEmpty(newCalendarName)) return;

            try
            {
                _customCalendar.Name = newCalendarName;
                _calendarName = newCalendarName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Deletes the custom calendar
        /// </summary>
        public void DeleteCustomCalendar()
        {
            if (_customCalendar == null) return;

            try
            {
                _customCalendar.Delete();
                _customCalendar = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Returns an AppointmentSyncCollection of the full calendar
        /// </summary>
        /// <returns>AppointmentSyncCollection, with all appointments as "add"</returns>
        public AppointmentSyncCollection GetInitialSync()
        {
            if (_customCalendar == null) return null;
            AppointmentSyncCollection syncCollection = GetUpdates(DateTime.MinValue);

            // this is a request for a full inital sync, so there are no "updates" or "delete", but only "adds"
            syncCollection.AddList.AddRange(syncCollection.UpdateList);
            syncCollection.UpdateList.Clear();
            syncCollection.DeleteList.Clear();

            return syncCollection;
        }

        /// <summary>
        /// Returns an AppointmentSyncCollection, with all updates since the last request
        /// </summary>
        /// <returns></returns>
        public AppointmentSyncCollection GetUpdates()
        {
            return GetUpdates(GetLastSyncTime());
        }

        /// <summary>
        /// Returns a AppointmentSyncCollection, with all updates since the specified timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private AppointmentSyncCollection GetUpdates(DateTime timestamp)
        {
            if (_customCalendar == null) return null;
            AppointmentSyncCollection syncCollection = new AppointmentSyncCollection();

            // Debug.WriteLine("CalendarHandler: TimeStamp -> " + timestamp);

            foreach (Outlook.AppointmentItem item in _customCalendar.Items)
            {
                Boolean updatedBySync = false;

                //Debug.WriteLine("CalendarHandler: Item (" + item.Subject + ") LastModificationTime -> " + item.LastModificationTime);

                // if the date from the SYNC_UPDATE is newer or as the LastModificationTime, the item was not updated by the user
                if (item.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE] != null && DateTime.Parse(item.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE].Value) >= item.LastModificationTime)
                    updatedBySync = true;

                //Debug.WriteLine("CalendarHandler: Item (" + item.Subject + ") SyncUpdate -> " + syncUpdate);
                //Debug.WriteLine("CalendarHandler: Item (" + item.Subject + ") wasUpdatedBySync -> " + wasUpdatedBySync);

                if (item.LastModificationTime >= timestamp && !updatedBySync)
                {
                    // ADDING
                    // if SyncID does not exist, it is not yet synced and needs to be added to the other calendar
                    if (item.ItemProperties[ITEM_PROPERTY_SYNC_ID] == null)
                    {
                        syncCollection.AddList.Add(new OutlookAppointment(item));

                        if (item.ItemProperties[ITEM_PROPERTY_GLOBAL_A_ID] == null)
                        {
                            // GAI (GlobalAppointmentID) needs to be added as item property, otherwise it cannot be found later
                            Outlook.ItemProperty newProp = item.ItemProperties.Add(ITEM_PROPERTY_GLOBAL_A_ID, Outlook.OlUserPropertyType.olText);
                            item.Save();
                            newProp.Value = item.GlobalAppointmentID;
                            item.Save();
                        }
                    }

                    // UPDATING
                    // if a SyncID exist, it is already synced and needs to be updated in the other calendar
                    else
                    {
                        syncCollection.UpdateList.Add(new OutlookAppointment(item));
                    }
                }
            }

            // DELETING
            foreach (String syncID in GetAppointmentsForDeleting())
            {
                OutlookAppointment item = new OutlookAppointment();
                item.SyncID = syncID;
                syncCollection.DeleteList.Add(item);
            }

            Debug.WriteLine("CalendarHandler (GetUpdates): Added: " + syncCollection.AddList.Count + " | Updated: " + syncCollection.UpdateList.Count + " | Deleted: " + syncCollection.DeleteList.Count);

            ResetDeleteStorage();
            SetSyncTime(DateTime.Now);

            return syncCollection;
        }

        /// <summary>
        /// Applies all the updates to the calendar
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns>null</returns>
        public Dictionary<string, string> DoUpdates(AppointmentSyncCollection syncItems)
        {
            if (syncItems == null || _customCalendar == null) return null;

            // add new appointments
            if (syncItems.AddList != null)
            {
                foreach (OutlookAppointment appointment in syncItems.AddList)
                {
                    CreateAppointment(appointment);
                }
            }

            // update appointments
            if (syncItems.UpdateList != null)
            {
                foreach (OutlookAppointment appointment in syncItems.UpdateList)
                {
                    UpdateAppointment(appointment);
                }
            }

            // delete appointments
            if (syncItems.DeleteList != null)
            {
                foreach (OutlookAppointment appointment in syncItems.DeleteList)
                {
                    DeleteAppointment(appointment);
                }
            }

            Debug.WriteLine("CalendarHandler (DoUpdates): Added: " + syncItems.AddList.Count + " | Updated: " + syncItems.UpdateList.Count + " | Deleted: " + syncItems.DeleteList.Count);

            return null;
        }

        /// <summary>
        /// Updates the appointments (GlobalAppointmentID) with new SyncIDs
        /// </summary>
        /// <param name="idMapping">GlobalAppointmentID -> SyncID</param>
        public void UpdateSyncIDs(Dictionary<string, string> idMapping)
        {
            foreach (KeyValuePair<string, string> entry in idMapping)
            {
                Outlook.AppointmentItem foundItem = _customCalendar.Items.Find(String.Format("[GAI] = '{0}'", entry.Key));

                // updating the SyncIDs
                if (foundItem.ItemProperties[ITEM_PROPERTY_SYNC_ID] == null)
                {
                    foundItem.ItemProperties.Add(ITEM_PROPERTY_SYNC_ID, Outlook.OlUserPropertyType.olText);
                    foundItem.Save();
                }

                foundItem.ItemProperties[ITEM_PROPERTY_SYNC_ID].Value = entry.Value;
                foundItem.Save();

                // updating the information, that this item was updated by the sync
                if (foundItem.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE] == null)
                {
                    foundItem.ItemProperties.Add(ITEM_PROPERTY_SYNC_UPDATE, Outlook.OlUserPropertyType.olText);
                    foundItem.Save();
                }

                foundItem.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE].Value = foundItem.LastModificationTime;
                foundItem.Save();

                Marshal.ReleaseComObject(foundItem);
                foundItem = null;
            }

        }

        /// <summary>
        /// Creates a new appointment in the custom calendar
        /// </summary>
        /// <param name="appointment">new appointment</param>
        /// <returns>GlobalAppointmentID of the new appointment in Outlook</returns>
        private String CreateAppointment(OutlookAppointment appointment)
        {
            if (_customCalendar == null || appointment == null) return null;

            Outlook.AppointmentItem newAppointment = (Outlook.AppointmentItem)_customCalendar.Items.Add(Outlook.OlItemType.olAppointmentItem);

            newAppointment.Subject = appointment.Subject;
            newAppointment.Body = appointment.Body;
            newAppointment.Start = appointment.Start;
            newAppointment.End = appointment.End;
            newAppointment.ReminderSet = false;
            //newAppointment.ReminderMinutesBeforeStart = appointment.ReminderMinutesBeforeStart;
            newAppointment.Location = appointment.Location;
            newAppointment.AllDayEvent = appointment.AllDayEvent;

            //if (appointment.Attachments != null)
            //newAppointment.Attachments.Add(appointment.Attachments);

            newAppointment.Duration = appointment.Duration;
            newAppointment.Importance = Outlook.OlImportance.olImportanceNormal;

            // GlobalAppointmentID must be stored as custom item property as well, because GlobalAppointmentID property cannot be searched for
            newAppointment.ItemProperties.Add(ITEM_PROPERTY_GLOBAL_A_ID, Outlook.OlUserPropertyType.olText);
            newAppointment.ItemProperties.Add(ITEM_PROPERTY_SYNC_ID, Outlook.OlUserPropertyType.olText);
            newAppointment.ItemProperties.Add(ITEM_PROPERTY_SYNC_UPDATE, Outlook.OlUserPropertyType.olText);

            newAppointment.Save();

            newAppointment.ItemProperties[ITEM_PROPERTY_GLOBAL_A_ID].Value = newAppointment.GlobalAppointmentID;
            newAppointment.ItemProperties[ITEM_PROPERTY_SYNC_ID].Value = appointment.SyncID;
            newAppointment.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE].Value = newAppointment.LastModificationTime;

            newAppointment.Save();

            return newAppointment.GlobalAppointmentID;
        }

        /// <summary>
        /// Deletes the appointment in the custom calendar
        /// </summary>
        /// <param name="appointment">appointment to be deleted</param>
        /// <returns>returns true if successfull</returns>
        private bool DeleteAppointment(OutlookAppointment appointment)
        {
            if (_customCalendar == null || appointment == null) return false;
            return DeleteAppointment(appointment.SyncID);
        }

        /// <summary>
        /// Deletes the appointment in the custom calendar
        /// </summary>
        /// <param name="syncID">SyncID of the appointment</param>
        /// <returns>returns true if successfull</returns>
        private bool DeleteAppointment(String syncID)
        {
            if (_customCalendar == null || String.IsNullOrEmpty(syncID)) return false;

            Outlook.AppointmentItem foundItem = _customCalendar.Items.Find(String.Format("[" + ITEM_PROPERTY_SYNC_ID + "] = '{0}'", syncID));
            if (foundItem != null)
            {
                foundItem.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates an appointment in the custom calendar
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        private bool UpdateAppointment(OutlookAppointment appointment)
        {
            if (_customCalendar == null || appointment == null) return false;
            Outlook.AppointmentItem foundItem;

            foundItem = _customCalendar.Items.Find(String.Format("[" + ITEM_PROPERTY_SYNC_ID + "] = '{0}'", appointment.SyncID));

            if (foundItem == null)
                foundItem = _customCalendar.Items.Find(String.Format("[" + ITEM_PROPERTY_GLOBAL_A_ID + "] = '{0}'", appointment.GlobalAppointmentID));

            if (foundItem != null)
            {
                foundItem.Subject = appointment.Subject;
                foundItem.Body = appointment.Body;
                foundItem.Start = appointment.Start;
                foundItem.End = appointment.End;
                //foundItem.ReminderSet = appointment.ReminderSet;
                //foundItem.ReminderMinutesBeforeStart = appointment.ReminderMinutesBeforeStart;
                foundItem.Location = appointment.Location;
                foundItem.AllDayEvent = appointment.AllDayEvent;

                //if (appointment.Attachments != null)
                //foundItem.Attachments.Add(appointment.Attachments);

                foundItem.Duration = appointment.Duration;
                //foundItem.Importance = appointment.Importance;

                if (foundItem.ItemProperties[ITEM_PROPERTY_SYNC_ID] == null)
                {
                    foundItem.ItemProperties.Add(ITEM_PROPERTY_SYNC_ID, Outlook.OlUserPropertyType.olText);
                    foundItem.Save();
                }

                foundItem.ItemProperties[ITEM_PROPERTY_SYNC_ID].Value = appointment.SyncID;
                foundItem.Save();

                // updating the information, that this item was updated by the sync
                if (foundItem.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE] == null)
                {
                    foundItem.ItemProperties.Add(ITEM_PROPERTY_SYNC_UPDATE, Outlook.OlUserPropertyType.olText);
                    foundItem.Save();
                }

                foundItem.ItemProperties[ITEM_PROPERTY_SYNC_UPDATE].Value = foundItem.LastModificationTime;
                foundItem.Save();

                Marshal.ReleaseComObject(foundItem);
                foundItem = null;

                return true;
            }

            // couldn't find the appointment
            return false;
        }

        /// <summary>
        /// Returns a list of all SyncIDs that have been deleted
        /// </summary>
        /// <returns>list of SyncIDs</returns>
        private List<String> GetAppointmentsForDeleting()
        {
            return _syncStorage.DeletedItems;
        }

        /// <summary>
        /// Saves an item in the Delete Storage
        /// </summary>
        /// <param name="item"></param>
        private void AddItemToDeleteStorage(Outlook.AppointmentItem item)
        {
            // only synced items need to be remembered
            if (item != null && item.ItemProperties[ITEM_PROPERTY_SYNC_ID] != null)
                _syncStorage.DeletedItems.Add(item.ItemProperties[ITEM_PROPERTY_SYNC_ID].Value);

            SaveToLocalStorage();
        }

        /// <summary>
        /// Resets the Delete Storage
        /// </summary>
        private void ResetDeleteStorage()
        {
            _syncStorage.DeletedItems.Clear();
            SaveToLocalStorage();
        }

        /// <summary>
        /// Gets the last synchronization time
        /// </summary>
        /// <returns></returns>
        private DateTime GetLastSyncTime()
        {
            return _syncStorage.LastSyncTime;
        }

        /// <summary>
        /// Sets the synchronization timer
        /// </summary>
        /// <param name="time"></param>
        public void SetSyncTime(DateTime time)
        {
            _syncStorage.LastSyncTime = time;
            SaveToLocalStorage();
        }

        /// <summary>
        /// Resets the synchronization timer
        /// </summary>
        private void ResetSyncTime()
        {
            _syncStorage.LastSyncTime = DateTime.MinValue;
            SaveToLocalStorage();
        }

        /// <summary>
        /// Sets all appropiate events in the custom calendar
        /// (required for i.e. tracking the delete actions)
        /// </summary>
        private void SetEvents()
        {
            if (_customCalendar == null) return;

            Outlook.Folder eventsFolder = (Outlook.Folder)_customCalendar;
            eventsFolder.BeforeItemMove += new Outlook.MAPIFolderEvents_12_BeforeItemMoveEventHandler(Events_BeforeItemMove);

            _items = eventsFolder.Items;
            _items.ItemAdd += new Outlook.ItemsEvents_ItemAddEventHandler(Items_ItemAdd);
        }

        /// <summary>
        /// Executed when a new item is added to the calendar folder, and is adding additional parameters to it
        /// </summary>
        /// <param name="Item"></param>
        private void Items_ItemAdd(object Item)
        {
            Outlook.AppointmentItem item = Item as Outlook.AppointmentItem;

            //Debug.WriteLine("CalendarHandler: Items_ItemAdd fired for '" + item.Subject + "'");

            // GAI (GlobalAppointmentID) needs to be added as item property, otherwise it cannot be found later
            Outlook.ItemProperty newPropGAI = item.ItemProperties.Add(ITEM_PROPERTY_GLOBAL_A_ID, Outlook.OlUserPropertyType.olText);
            item.Save();
            newPropGAI.Value = item.GlobalAppointmentID;
            item.Save();

            Marshal.ReleaseComObject(item);
            item = null;
        }

        /// <summary>
        /// Executed before an item is moved within Outlook, and checks if the item is deleted (moved to Trash)
        /// </summary>
        private void Events_BeforeItemMove(object Item, Outlook.MAPIFolder MoveTo, ref bool Cancel)
        {
            Outlook.AppointmentItem item = Item as Outlook.AppointmentItem;
            Outlook.MAPIFolder deletedFolder = (Outlook.MAPIFolder)_outlookApp.ActiveExplorer().Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems);

            if (MoveTo.Name == deletedFolder.Name)
            {
                AddItemToDeleteStorage(item);
            }
        }

    }
}

using Shared;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddIn
{
    public class CalendarHandler : ICalendarSyncable
    {
        private Outlook.Application _outlookApp;
        private Outlook.MAPIFolder _primaryCalendar;
        private Outlook.MAPIFolder _customCalendar;

        // TODO: to be retrieved from local storage
        private List<String> _tempDeleteStorage = new List<string>();
        private DateTime _tempLastSyncTime = DateTime.MinValue;

        // TODO: to be retrieved from configuration
        private const String CALENDAR_NAME = "Caldav Calendar";

        public String ConnectorName
        {
            get { return CALENDAR_NAME; }
        }

        public ConnectorSettings Settings
        {
            // the interface to Outlook does not require this
            set { }
        }

        /// <summary>
        /// Initializes the CalendarHandler
        /// </summary>
        /// <param name="outlookApp">reference to the active Outlook application</param>
        public CalendarHandler(Outlook.Application outlookApp)
        {
            this._outlookApp = outlookApp;
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
                if (calendar.Name == CALENDAR_NAME)
                {
                    _customCalendar = calendar;
                    break;
                }
            }

            if (_customCalendar != null) SetEvents();
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
                _customCalendar = _primaryCalendar.Folders.Add(CALENDAR_NAME, Outlook.OlDefaultFolders.olFolderCalendar);

                // add the new custom calendar to the navigation panel
                Outlook.NavigationPane objPane = _outlookApp.ActiveExplorer().NavigationPane;
                Outlook.CalendarModule objModule = (Outlook.CalendarModule)objPane.Modules.GetNavigationModule(Outlook.OlNavigationModuleType.olModuleCalendar);
                Outlook.NavigationGroup objGroup = objModule.NavigationGroups.GetDefaultNavigationGroup(Outlook.OlGroupType.olMyFoldersGroup);
                Outlook.NavigationFolder objNavFolder = objGroup.NavigationFolders.Add(_customCalendar);

                // Set the navigation folder to be displayed in overlay mode by default. The IsSelected property can't be set to True 
                // unless the CalendarModule object is the current module displayed in the Navigation Pane
                objPane.CurrentModule = objPane.Modules.GetNavigationModule(Outlook.OlNavigationModuleType.olModuleCalendar);
                //objNavFolder.IsSelected = true;
                //objNavFolder.IsSideBySide = false;

                SetEvents();

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
        /// Returns a AppointmentSyncCollection, with all updates since the last request
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

            foreach (Outlook.AppointmentItem item in _customCalendar.Items)
            {
                if (item.LastModificationTime > timestamp)
                {
                    // ADDING
                    // if SyncID does not exist, it is not yet synced and needs to be added
                    if (item.ItemProperties["SyncID"] == null)
                    {
                        syncCollection.AddList.Add(new OutlookAppointment(item));

                        // GAI (GlobalAppointmentID) needs to be added as item property, otherwise it cannot be found later
                        Outlook.ItemProperty newProp = item.ItemProperties.Add("GAI", Outlook.OlUserPropertyType.olText);
                        item.Save();
                        newProp.Value = item.GlobalAppointmentID;
                        item.Save();
                    }

                    // UPDATING
                    // if a SyncID exist, it is already synced and needs to be updated
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

                if (foundItem.ItemProperties["SyncID"] == null) { 
                    foundItem.ItemProperties.Add("SyncID", Outlook.OlUserPropertyType.olText);
                    foundItem.Save();
                }

                foundItem.ItemProperties["SyncID"].Value = entry.Value;
                foundItem.Save();
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
            newAppointment.ReminderSet = appointment.ReminderSet;
            newAppointment.ReminderMinutesBeforeStart = appointment.ReminderMinutesBeforeStart;
            newAppointment.Location = appointment.Location;
            newAppointment.AllDayEvent = appointment.AllDayEvent;

            if (appointment.Attachments != null)
                newAppointment.Attachments.Add(appointment.Attachments);

            newAppointment.Duration = appointment.Duration;
            newAppointment.Importance = appointment.Importance;

            // GlobalAppointmentID must be stored as custom item property as well, because GlobalAppointmentID property cannot be searched for
            newAppointment.ItemProperties.Add("GAI", Outlook.OlUserPropertyType.olText);
            newAppointment.ItemProperties.Add("SyncID", Outlook.OlUserPropertyType.olText);

            newAppointment.Save();

            newAppointment.ItemProperties["GAI"].Value = newAppointment.GlobalAppointmentID;
            newAppointment.ItemProperties["SyncID"].Value = appointment.SyncID;

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

            Outlook.AppointmentItem foundItem = _customCalendar.Items.Find(String.Format("[SyncID] = '{0}'", syncID));
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

            foundItem = _customCalendar.Items.Find(String.Format("[SyncID] = '{0}'", appointment.SyncID));

            if (foundItem == null)
                foundItem = _customCalendar.Items.Find(String.Format("[GAI] = '{0}'", appointment.GlobalAppointmentID));

            if (foundItem != null)
            {
                foundItem.Subject = appointment.Subject;
                foundItem.Body = appointment.Body;
                foundItem.Start = appointment.Start;
                foundItem.End = appointment.End;
                foundItem.ReminderSet = appointment.ReminderSet;
                foundItem.ReminderMinutesBeforeStart = appointment.ReminderMinutesBeforeStart;
                foundItem.Location = appointment.Location;
                foundItem.AllDayEvent = appointment.AllDayEvent;

                if (appointment.Attachments != null)
                    foundItem.Attachments.Add(appointment.Attachments);

                foundItem.Duration = appointment.Duration;
                foundItem.Importance = appointment.Importance;

                if (foundItem.ItemProperties["SyncID"] == null)
                    foundItem.ItemProperties.Add("SyncID", Outlook.OlUserPropertyType.olText);

                foundItem.Save();

                foundItem.ItemProperties["SyncID"].Value = appointment.SyncID;

                foundItem.Save();

                return true;
            }

            // couldn't find the appointment
            // TODO: adding as new?
            return false;
        }

        /// <summary>
        /// Returns a list of all SyncIDs that have been deleted
        /// </summary>
        /// <returns>list of SyncIDs</returns>
        private List<String> GetAppointmentsForDeleting()
        {
            return _tempDeleteStorage;
        }

        /// <summary>
        /// Saves an item in the Delete Storage
        /// </summary>
        /// <param name="item"></param>
        private void AddItemToDeleteStorage(Outlook.AppointmentItem item)
        {
            // only synced items need to be remembered
            if (item != null && item.ItemProperties["SyncID"] != null)
                _tempDeleteStorage.Add(item.ItemProperties["SyncID"].Value);
        }

        /// <summary>
        /// Resets the Delete Storage
        /// </summary>
        private void ResetDeleteStorage()
        {
            _tempDeleteStorage.Clear();
        }

        private DateTime GetLastSyncTime()
        {
            return _tempLastSyncTime;
        }

        private void SetSyncTime(DateTime time)
        {
            _tempLastSyncTime = time;
        }

        private void ResetSyncTime()
        {
            _tempLastSyncTime = DateTime.MinValue;
        }

        /// <summary>
        /// Sets all appropiate events in the custom calendar
        /// (required for i.e. tracking the delete actions)
        /// </summary>
        private void SetEvents()
        {
            if (_customCalendar == null) return;

            Outlook.Folder eventsFolder = (Outlook.Folder)_customCalendar;
            eventsFolder.BeforeItemMove += events_BeforeItemMove;
        }

        /// <summary>
        /// Executed before an item is moved within Outlook, and checks if the item is deleted (moved to Trash)
        /// </summary>
        private void events_BeforeItemMove(object Item, Outlook.MAPIFolder MoveTo, ref bool Cancel)
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

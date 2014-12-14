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

        private List<String> _tempDeleteStorage = new List<string>();

        // TODO: to be retrieved from configuration
        private const String CALENDAR_NAME = "Caldav Calendar";

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
            // calendar already exists
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
            // calendar does not exist
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

        public AppointmentSyncCollection GetUpdates(DateTime timestamp)
        {
            if (_customCalendar == null) return null;
            AppointmentSyncCollection syncCollection = new AppointmentSyncCollection();
            
            // adding and updating
            foreach (Outlook.AppointmentItem item in GetAppointments(timestamp)) {

                // if GAI does not exist, it is not yet synced and needs to be added
                if (item.ItemProperties["GAI"] == null)
                {
                    syncCollection.AddList.Add(new OutlookAppointment(item));

                    // creating new custom item property, also marking it as "synced" this way
                    Outlook.ItemProperty newProp = item.ItemProperties.Add("GAI", Outlook.OlUserPropertyType.olText);
                    item.Save();
                    newProp.Value = item.GlobalAppointmentID;
                    item.Save();
                }
                // if GAI does exist, it is already synced and needs to be updated
                else
                {
                    syncCollection.UpdateList.Add(new OutlookAppointment(item));
                }
            }

            // deleting
            foreach (String appointmentID in GetAppointmentsForDeleting())
            {
                OutlookAppointment item = new OutlookAppointment();
                item.GlobalAppointmentID = appointmentID;
                syncCollection.DeleteList.Add(item);
            }

            ClearDeleteStorage();

            return syncCollection;
        }

        public AppointmentSyncCollection GetUpdates()
        {
            if (_customCalendar == null) return null;
            AppointmentSyncCollection syncCollection = GetUpdates(DateTime.MinValue);

            // this is a request for a full update, so there are no "updates" or "delete", but only "adds"
            syncCollection.AddList.AddRange(syncCollection.UpdateList);
            syncCollection.UpdateList.Clear();
            syncCollection.DeleteList.Clear();

            return syncCollection;
        }

        // TODO: Test this
        public void DoUpdates(AppointmentSyncCollection syncItems)
        {
            if (syncItems == null || _customCalendar == null) return;

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
        }

        /// <summary>
        /// Creates a new appointment in the custom calendar
        /// </summary>
        /// <param name="appointment">new appointment</param>
        /// <returns>GlobalAppointmentID of appointment in Outlook</returns>
        public String CreateAppointment(OutlookAppointment appointment)
        {
            if (_customCalendar == null || appointment == null) return null;

            Outlook.AppointmentItem newAppointment = (Outlook.AppointmentItem) _customCalendar.Items.Add(Outlook.OlItemType.olAppointmentItem);
            
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
            Outlook.ItemProperty newProp = newAppointment.ItemProperties.Add("GAI", Outlook.OlUserPropertyType.olText);

            newAppointment.Save();
          
            newProp.Value = newAppointment.GlobalAppointmentID;
            newAppointment.Save();

            return newAppointment.GlobalAppointmentID;
        }

        /// <summary>
        /// Deletes the appointment in the custom calendar
        /// </summary>
        /// <param name="appointment">appointment to be deleted</param>
        /// <returns>returns true if successfull</returns>
        public bool DeleteAppointment(OutlookAppointment appointment)
        {
            if (_customCalendar == null || appointment == null) return false;
            return DeleteAppointment(appointment.GlobalAppointmentID);
        }

        /// <summary>
        /// Deletes the appointment in the custom calendar
        /// </summary>
        /// <param name="globalAppointmentID">GlobalAppointmentID of the appointment</param>
        /// <returns>returns true if successfull</returns>
        public bool DeleteAppointment(String globalAppointmentID)
        {
            if (_customCalendar == null || String.IsNullOrEmpty(globalAppointmentID)) return false;

            Outlook.AppointmentItem foundItem = _customCalendar.Items.Find(String.Format("[GAI] = '{0}'", globalAppointmentID));
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
        public bool UpdateAppointment(OutlookAppointment appointment)
        {
            if (_customCalendar == null || appointment == null) return false;

            Outlook.AppointmentItem foundItem = _customCalendar.Items.Find(String.Format("[GAI] = '{0}'", appointment.GlobalAppointmentID));
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

                foundItem.Save();

                return true;
            }
            // couldn't find the appointment
            // TODO: adding as new?
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a list of all appointments in the custom calendar
        /// </summary>
        /// <returns>list of appointments</returns>
        private List<Outlook.AppointmentItem> GetAppointments()
        {
            /*
            if (_customCalendar == null) return null;

            List<Outlook.AppointmentItem> returnList = new List<Outlook.AppointmentItem>();
            foreach (Outlook.AppointmentItem item in _customCalendar.Items)
            {
                returnList.Add(item);
            }

            return returnList;
             */

            return GetAppointments(DateTime.MinValue);
        }

        /// <summary>
        /// Returns a list of appointments in the custom calendar that have been changed since the TimeStamp
        /// </summary>
        /// <param name="timeStamp">time stamp to check against</param>
        /// <returns>list of appointments</returns>
        private List<Outlook.AppointmentItem> GetAppointments(DateTime timeStamp)
        {
            if (_customCalendar == null) return null;

            List<Outlook.AppointmentItem> returnList = new List<Outlook.AppointmentItem>();
            foreach (Outlook.AppointmentItem item in _customCalendar.Items)
            {
                if (item.LastModificationTime > timeStamp) returnList.Add(item);
            }

            return returnList;
        }

        /// <summary>
        /// Returns a list of all appointment IDs that have been deleted
        /// </summary>
        /// <returns>list of GlobalAppointmentIDs</returns>
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
            if (item == null) return;
            _tempDeleteStorage.Add(item.GlobalAppointmentID);
        }

        /// <summary>
        /// Resets the Delete Storage
        /// </summary>
        private void ClearDeleteStorage()
        {
            _tempDeleteStorage.Clear();
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
                //MessageBox.Show("Event: Item deleted");
            }
        }
    }
}

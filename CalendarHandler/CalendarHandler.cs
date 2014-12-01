using Shared;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace CalendarHandler
{
    public class CalendarHandler : ICalendarSyncable
    {
        private Outlook.Application _outlookApp;
        private Outlook.MAPIFolder _primaryCalendar;
        private Outlook.MAPIFolder _customCalendar;

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
            throw new NotImplementedException();
        }

        public AppointmentSyncCollection GetUpdates(DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public AppointmentSyncCollection GetUpdates()
        {
            throw new NotImplementedException();
        }

        public void DoUpdates(AppointmentSyncCollection syncItems)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new appointment in the custom calendar
        /// </summary>
        private void CreateAppointment(Outlook.AppointmentItem appointment)
        {
            if (_customCalendar == null) return;

            Outlook.AppointmentItem newAppointment = _customCalendar.Items.Add(appointment);
            appointment.Save();
        }

        /// <summary>
        /// Deletes the appointment in the custom calendar
        /// </summary>
        /// <param name="appointment"></param>
        private void DeleteAppointment(Outlook.AppointmentItem appointment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of all appointments in the custom calendar
        /// </summary>
        /// <returns>list of appointments</returns>
        private List<Outlook.AppointmentItem> GetAllAppointments()
        {
            if (_customCalendar == null) return null;

            List<Outlook.AppointmentItem> returnList = new List<Outlook.AppointmentItem>();
            foreach (Outlook.AppointmentItem item in _customCalendar.Items)
            {
                returnList.Add(item);
            }

            return returnList;
        }

        /// <summary>
        /// Returns a list of appointments in the custom calendar that have been changed since the timeStamp
        /// </summary>
        /// <param name="timeStamp">time stamp to check against</param>
        /// <returns>list of appointments</returns>
        private List<Outlook.AppointmentItem> GetAllAppointments(DateTime timeStamp)
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
        /// Executed before an item is moved in Outlook, and checks if the item is deleted (moved to Trash)
        /// </summary>
        private void events_BeforeItemMove(object Item, Outlook.MAPIFolder MoveTo, ref bool Cancel)
        {
            Outlook.AppointmentItem item = Item as Outlook.AppointmentItem;
            Outlook.MAPIFolder deletedFolder = (Outlook.MAPIFolder)_outlookApp.ActiveExplorer().Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderDeletedItems);

            if (MoveTo.Name == deletedFolder.Name)
            {
                MessageBox.Show("Item deleted");
            }
        }
    }
}

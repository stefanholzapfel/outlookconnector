using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Shared;
using SyncLogic;
using ConfigManager;

namespace OutlookAddIn
{
    public partial class SyncRibbon
    {
        CalendarHandler _calHandler;
        SyncService _synService = new SyncService(null, null, 1000);
        String _syncID = "1";
        DateTime _syncTime = DateTime.Now;
        ConfigurationManager _confManager = new ConfigurationManager();


        private void SyncRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            _calHandler = new CalendarHandler(Globals.ThisAddIn.Application);
        }

        private void btn_CreateCalendar_Click(object sender, RibbonControlEventArgs e)
        {
            _calHandler.CreateCustomCalendar();
        }

        private void btn_DeleteCalendar_Click(object sender, RibbonControlEventArgs e)
        {
            _calHandler.DeleteCustomCalendar();
        }

        private void btn_FullGetUpdates_Click(object sender, RibbonControlEventArgs e)
        {
            AppointmentSyncCollection syncCollection = _calHandler.GetInitialSync();
            if (syncCollection != null)
                MessageBox.Show("Added: " + syncCollection.AddList.Count + "; Updated: " + syncCollection.UpdateList.Count + "; Deleted: " + syncCollection.DeleteList.Count);
        }

        private void btn_IncrGetUpdates_Click(object sender, RibbonControlEventArgs e)
        {
            AppointmentSyncCollection syncCollection = _calHandler.GetUpdates();
            if (syncCollection != null)
                MessageBox.Show("Added: " + syncCollection.AddList.Count + "; Updated: " + syncCollection.UpdateList.Count + "; Deleted: " + syncCollection.DeleteList.Count);

            _syncTime = DateTime.Now;
        }

        private void btn_DoUpdatesSet1_Click(object sender, RibbonControlEventArgs e)
        {
            OutlookAppointment newAppointment = new OutlookAppointment();

            newAppointment.Subject = "Test Appointment";
            newAppointment.Body = "Testing the CalendarHandler";
            newAppointment.Start = DateTime.Now.AddHours(1);
            newAppointment.End = DateTime.Now.AddHours(1.25);
            //newAppointment.Importance = Outlook.OlImportance.olImportanceNormal;
            //newAppointment.ReminderSet = false;

            newAppointment.SyncID = _syncID;

            AppointmentSyncCollection syncCollection = new AppointmentSyncCollection();
            syncCollection.AddList.Add(newAppointment);

            _calHandler.DoUpdates(syncCollection);
        }

        private void btn_DoUpdatesSet2_Click(object sender, RibbonControlEventArgs e)
        {
            if (String.IsNullOrEmpty(_syncID))
            {
                MessageBox.Show("No ID for appointment provided");
                return;
            }

            OutlookAppointment updateAppointment = new OutlookAppointment();

            updateAppointment.Subject = "Test Appointment 2";
            updateAppointment.Body = "Testing the CalendarHandler, v2";
            updateAppointment.Start = DateTime.Now.AddDays(-1);
            updateAppointment.End = DateTime.Now.AddDays(-1);
            //updateAppointment.Importance = Outlook.OlImportance.olImportanceHigh;
            //updateAppointment.ReminderSet = false;

            updateAppointment.SyncID = _syncID;

            AppointmentSyncCollection syncCollection = new AppointmentSyncCollection();
            syncCollection.UpdateList.Add(updateAppointment);

            _calHandler.DoUpdates(syncCollection);
        }

        private void btn_DoUpdatesSet3_Click(object sender, RibbonControlEventArgs e)
        {
            if (String.IsNullOrEmpty(_syncID))
            {
                MessageBox.Show("No ID for appointment provided");
                return;
            }

            OutlookAppointment deleteAppointment = new OutlookAppointment();
            deleteAppointment.SyncID = _syncID;

            AppointmentSyncCollection syncCollection = new AppointmentSyncCollection();
            syncCollection.DeleteList.Add(deleteAppointment);

            _calHandler.DoUpdates(syncCollection);
        }

        private void btn_UpdateSyncIDs_Click(object sender, RibbonControlEventArgs e)
        {
            AppointmentSyncCollection syncCollection = _calHandler.GetInitialSync();
            Dictionary<String, String> idMapping = new Dictionary<string, string>();

            foreach (OutlookAppointment appointment in syncCollection.AddList)
            {
                idMapping.Add(appointment.GlobalAppointmentID, "02");
            }

            _calHandler.UpdateSyncIDs(idMapping);
        }

        private void btn_DoManualSync_Click(object sender, RibbonControlEventArgs e)
        {
            _synService.ExecuteOnce();
        }

        private void btn_StartSync_Click(object sender, RibbonControlEventArgs e)
        {
            _synService.Start();
        }

        private void btn_StopSync_Click(object sender, RibbonControlEventArgs e)
        {
            _synService.Stop();
        }

        private void btn_ChangeInterval_Click(object sender, RibbonControlEventArgs e)
        {
            _synService.SetInterval(2000);
        }

        private void btn_Settings_Click(object sender, RibbonControlEventArgs e)
        {
            ConfigManagerUI formMain = new ConfigManagerUI(_confManager);
            formMain.ShowDialog();
        }
    }
}

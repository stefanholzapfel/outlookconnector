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

namespace OutlookAddIn
{
    public partial class SyncRibbon
    {
        CalendarHandler _calHandler;
        String _appointmentID;

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

        private void btn_CreateAppointment_Click(object sender, RibbonControlEventArgs e)
        {
            OutlookAppointment newAppointment = new OutlookAppointment();

            newAppointment.Subject = "Test Appointment";
            newAppointment.Body = "Testing the CalendarHandler";
            newAppointment.Start = DateTime.Now.AddHours(1);
            newAppointment.End = DateTime.Now.AddHours(1.25);
            newAppointment.Importance = Outlook.OlImportance.olImportanceNormal;
            newAppointment.ReminderSet = false;

            _appointmentID = _calHandler.CreateAppointment(newAppointment);
            MessageBox.Show("New ID: " + _appointmentID);
        }

        private void btn_DeleteAppointment_Click(object sender, RibbonControlEventArgs e)
        {
            if (String.IsNullOrEmpty(_appointmentID))
            {
                MessageBox.Show("No ID for appointment provided");
                return;
            }
            _calHandler.DeleteAppointment(_appointmentID);
        }
    }
}

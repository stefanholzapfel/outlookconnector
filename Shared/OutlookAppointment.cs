using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace Shared
{
    /// <summary>
    /// Custom appointment which holds all data which can be communicated between calendars
    /// </summary>
    public class OutlookAppointment
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the synchronization logic, defined by the primary server calendar
        /// </summary>
        public String SyncID { get; set; }

        /// <summary>
        /// Appointment ID set by Outlook
        /// </summary>
        public String GlobalAppointmentID { get; set; }

        /// <summary>
        /// Subject of the appointment
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        /// Body and content of the appointment
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        /// Start date and time
        /// </summary>
        public DateTime Start { get; set; }

        private DateTime _end;

        /// <summary>
        /// End date and time
        /// </summary>
        public DateTime End
        {
            get { return _end; }
            set
            {
                _end = value;
                if (Start != null && _end != null)
                {
                    TimeSpan tSpan = _end - Start;
                    _duration = (int)tSpan.TotalMinutes;
                }
            }
        }

        /// <summary>
        /// Location of the appointment
        /// </summary>
        public String Location { get; set; }

        /// <summary>
        /// If this is an all-day event
        /// </summary>
        public bool AllDayEvent { get; set; }

        private int _duration;

        /// <summary>
        /// Duration of the appointment in minutes
        /// </summary>
        public int Duration
        {
            get { return _duration; }
        }

        /// <summary>
        /// Date and time of the last modification
        /// </summary>
        public DateTime LastModificationTime { get; set; }

        //public Object RTFBody { get; set; }
        //public DateTime StartUTC { get; set; }
        //public DateTime EndUTC { get; set; }
        //public bool ReminderSet { get; set; }
        //public int ReminderMinutesBeforeStart { get; set; }
        //public Outlook.Attachments Attachments { get; set; }
        //public DateTime CreationTime { get; set; }
        //public Outlook.OlImportance Importance { get; set; }
        //public bool IsRecurring { get; set; }
        //public OlRecurrenceState RecurrenceState { get; set; }

        #endregion

        /// <summary>
        /// Creates an empty appointment
        /// </summary>
        public OutlookAppointment() { }

        /// <summary>
        /// Creates an appointment, as a copy from an Outlook.AppointmentItem
        /// </summary>
        /// <param name="item"></param>
        public OutlookAppointment(Outlook.AppointmentItem item)
        {
            this.GlobalAppointmentID = item.GlobalAppointmentID;
            this.Subject = item.Subject;
            this.Body = item.Body;
            this.Start = item.Start;
            this.End = item.End;
            this.Location = item.Location;
            this.AllDayEvent = item.AllDayEvent;

            this.LastModificationTime = item.LastModificationTime;

            if (item.ItemProperties["SyncID"] != null)
                this.SyncID = item.ItemProperties["SyncID"].Value;

            //this.RTFBody = item.RTFBody;
            //this.StartUTC = item.StartUTC;
            //this.EndUTC = item.EndUTC;
            //this.ReminderSet = item.ReminderSet;
            //this.ReminderMinutesBeforeStart = item.ReminderMinutesBeforeStart;
            //this.Attachments = item.Attachments;
            //this.CreationTime = item.CreationTime;
            //this.Duration = item.Duration;
            //this.Importance = item.Importance;
            //this.IsRecurring = item.IsRecurring;
            //this.RecurrenceState = item.RecurrenceState;
        }
    }
}

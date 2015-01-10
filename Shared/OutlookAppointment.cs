using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace Shared
{
    public class OutlookAppointment
    {
        #region Properties

        public String SyncID { get; set; }

        public String GlobalAppointmentID { get; set; }

        public String Subject { get; set; }

        public String Body { get; set; }

        public DateTime Start { get; set; }

        private DateTime _end;
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

        public String Location { get; set; }

        public bool AllDayEvent { get; set; }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
        }

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

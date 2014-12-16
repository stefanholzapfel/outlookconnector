using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class OutlookAppointment
    {
        #region Properties

        public String SyncID { get; set; }

        public String GlobalAppointmentID { get; set; }

        public String Subject { get; set; }
        
        public String Body { get; set; }
        
        //public Object RTFBody { get; set; }
        
        public DateTime Start { get; set; }
        
        //public DateTime StartUTC { get; set; }

        private DateTime _end;
        public DateTime End
        {
            get { return _end; }
            set
            {
                _end = value;
                if (Start != null && _end != null) _duration = (_end - Start).Minutes;
            }
        }

        //public DateTime EndUTC { get; set; }
        
        public bool ReminderSet { get; set; }
        
        public int ReminderMinutesBeforeStart { get; set; }
        
        public String Location { get; set; }
        
        public bool AllDayEvent { get; set; }
        
        public Attachments Attachments { get; set; }
        
        public DateTime CreationTime { get; set; }
        
        public DateTime LastModificationTime { get; set; }

        private int _duration;
        public int Duration
        {
            get { return _duration; }

        }
        
        public OlImportance Importance { get; set; }
        
        //public bool IsRecurring { get; set; }

        //public OlRecurrenceState RecurrenceState { get; set; }

        #endregion

        /// <summary>
        /// Creates an empty appointment
        /// </summary>
        public OutlookAppointment() {}

        /// <summary>
        /// Creates an appointment, as a copy from an Outlook.AppointmentItem
        /// </summary>
        /// <param name="item"></param>
        public OutlookAppointment(AppointmentItem item)
        {
            this.GlobalAppointmentID = item.GlobalAppointmentID;
            this.Subject = item.Subject;
            this.Body = item.Body;
            //this.RTFBody = item.RTFBody;
            this.Start = item.Start;
            //this.StartUTC = item.StartUTC;
            this.End = item.End;
            //this.EndUTC = item.EndUTC;
            this.ReminderSet = item.ReminderSet;
            this.ReminderMinutesBeforeStart = item.ReminderMinutesBeforeStart;
            this.Location = item.Location;
            this.AllDayEvent = item.AllDayEvent;
            this.Attachments = item.Attachments;
            this.CreationTime = item.CreationTime;
            this.LastModificationTime = item.LastModificationTime;
            //this.Duration = item.Duration;
            this.Importance = item.Importance;
            //this.IsRecurring = item.IsRecurring;
            //this.RecurrenceState = item.RecurrenceState;
        }
    }
}

using CaldavConnector.Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.Converter
{
    public static class IcsToAppointmentItemConverter
    {
        public static OutlookAppointment Convert(CalDavElement _myElement)
        {
            OutlookAppointment _myAppointment = new OutlookAppointment();
            _myAppointment.SyncID = _myElement.Guid;
            _myAppointment.Subject = _myElement.Summary;
            _myAppointment.Body = _myElement.Description;
            _myAppointment.Start = (DateTime)_myElement.Start;
            _myAppointment.End = (DateTime)_myElement.End;
            _myAppointment.Location = _myElement.Location;
            _myAppointment.LastModificationTime = (DateTime)_myElement.LastModified;
            //_myAppointment.ReminderSet = appointment.ReminderSet;
            //_myAppointment.ReminderMinutesBeforeStart = appointment.ReminderMinutesBeforeStart;
            //_myAppointment.AllDayEvent = appointment.AllDayEvent;
            //_myAppointment.Duration = appointment.Duration;
            //_myAppointment.Importance = appointment.Importance;

            return _myAppointment;
        }

    }
}

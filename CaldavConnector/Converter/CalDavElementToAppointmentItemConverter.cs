using CaldavConnector.Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.Converter
{
    /// <summary>
    /// This helper class converts a CalDavElement into an OutlookAppointment.
    /// </summary>
    public static class CalDavElementToAppointmentItemConverter
    {
        /// <summary>
        /// Does the convertion from CalDavElement into OutlookAppointment.
        /// </summary>
        /// <param name="_myElement">CalDavElement to convert.</param>
        /// <returns>Converted OutlookAppointment.</returns>
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
            _myAppointment.AllDayEvent = _myElement.AllDayEvent;

            return _myAppointment;
        }

    }
}

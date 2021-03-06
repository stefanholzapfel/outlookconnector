﻿using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaldavConnector;

namespace CaldavConnector.Utilities
{
    /// <summary>
    /// A helper class that parses an Outlook appointment into XML for a CalDav request.
    /// </summary>
    public static class AppointmentItemXmlParser
    {
        /// <summary>
        /// Parses an Outlook appointment into XML for a CalDav request.
        /// </summary>
        /// <param name="_appointment">The OutlookAppointment to parse.</param>
        /// <returns>The XML string to use for CalDav request.</returns>
        public static String Parse(OutlookAppointment _appointment) {
            String querystring = "";
            String starttimestamp;
            String endtimestamp;
            if (_appointment.AllDayEvent)
            {
                starttimestamp = ";VALUE=DATE:" + _appointment.Start.ToString(@"yyyyMMdd");
                endtimestamp = ";VALUE=DATE:" + _appointment.End.ToString(@"yyyyMMdd");
            }
            else
            {
                starttimestamp = ";VALUE=DATE-TIME:" + _appointment.Start.ToString(@"yyyyMMdd\THHmmss");
                endtimestamp = ";VALUE=DATE-TIME:" + _appointment.End.ToString(@"yyyyMMdd\THHmmss");
            }

            String lastmodified = DateTime.Now.AddHours(CaldavConnector.LASTMODIFIED_DATE_OFFSET).ToString(@"yyyyMMdd\THHmmss");

            querystring += "BEGIN:VCALENDAR\n";
            querystring += "VERSION:2.0\n";
            querystring += "BEGIN:VEVENT\n";
            if (_appointment.SyncID != null && !_appointment.SyncID.Equals(""))
            querystring += "UID:" + _appointment.SyncID + "\n";
            if (_appointment.Subject != null && !_appointment.Subject.Equals(""))
            querystring += "SUMMARY:" + _appointment.Subject + "\n";
            if (starttimestamp != null && !starttimestamp.Equals(""))
            querystring += "DTSTART" + starttimestamp + "\n";
            if (endtimestamp != null && !endtimestamp.Equals(""))
            querystring += "DTEND" + endtimestamp + "\n";
            if (_appointment.Location != null && !_appointment.Location.Equals(""))
            querystring += "LOCATION:" + _appointment.Location + "\n";
            if (_appointment.Body != null && !_appointment.Body.Equals(""))
            querystring += "DESCRIPTION:" + _appointment.Body + "\n";
            querystring += "LAST-MODIFIED:" + lastmodified + "\n";
            querystring += "END:VEVENT\n";
            querystring += "END:VCALENDAR";

            return querystring;
        }
    }
}

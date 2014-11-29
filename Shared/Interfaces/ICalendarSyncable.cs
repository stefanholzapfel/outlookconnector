using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    /// <summary>
    /// Interface for interacting with any calendar (Outlook or remote connector)
    /// </summary>
    public interface ICalendarSyncable
    {
        /// <summary>
        /// returns the updates in the calendar since a timestamp
        /// </summary>
        /// <param name="calendarName">name of calendar</param>
        /// <param name="timestamp">since when the changes should be returned</param>
        /// <returns>appointments that have been updated</returns>
        AppointmentSyncCollection GetUpdates(String calendarName, DateTime timestamp);

        /// <summary>
        /// returns the full calendar
        /// </summary>
        /// <param name="calendarName">name of calendar</param>
        /// <returns>all appointments as 'add'</returns>
        AppointmentSyncCollection GetUpdates(String calendarName);

        /// <summary>
        /// applies the updates to the calendar
        /// </summary>
        /// <param name="calendarName">name of calendar</param>
        /// <param name="syncItems">appointments to be updated</param>
        void DoUpdates(String calendarName, AppointmentSyncCollection syncItems);
    }
}

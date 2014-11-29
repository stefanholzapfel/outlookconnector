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
        /// returns the updates in the calendar since the given timestamp
        /// </summary>
        /// <param name="timestamp">since when the changes should be returned</param>
        /// <returns>appointments that have been added/updated/deleted in the respective collection of the AppointmentSyncCollection</returns>
        AppointmentSyncCollection GetUpdates(DateTime timestamp);

        /// <summary>
        /// returns the full calendar
        /// </summary>
        /// <returns>all appointments as 'add'</returns>
        AppointmentSyncCollection GetUpdates();

        /// <summary>
        /// applies the updates to the calendar
        /// </summary>
        /// <param name="syncItems">appointments to be added/updated/deleted</param>
        void DoUpdates(AppointmentSyncCollection syncItems);
    }
}

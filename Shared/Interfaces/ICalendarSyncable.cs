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

        String ConnectorName { get; }

        ConnectorSettings Settings { get; }

        /// <summary>
        /// returns the full calendar
        /// </summary>
        /// <returns>appointments that have been added/updated/deleted in the respective collection of the AppointmentSyncCollection</returns>
        AppointmentSyncCollection GetUpdates();

        /// <summary>
        /// initializes the connector and returns the full connected calendar
        /// </summary>
        /// <returns>returns AppointmentSyncCollection with all calendar items</returns>
        AppointmentSyncCollection GetInitialSync();

        /// <summary>
        /// applies the updates to the calendar and returns a key value pair with GlobalAppointmentID -> SyncID
        /// </summary>
        /// <param name="syncItems">appointments to be added/updated/deleted</param>
        Dictionary<String, String> DoUpdates(AppointmentSyncCollection syncItems);
    }
}

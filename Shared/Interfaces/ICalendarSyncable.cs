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
        /// Name of the connector
        /// </summary>
        String ConnectorName { get; }

        /// <summary>
        /// All settings to connect
        /// </summary>
        ConnectorSettings Settings { set; }

        /// <summary>
        /// Checks weather the connector can connect with the given parameters, if not it returns:
        /// </summary>
        /// <returns>Int: 0=Connectivity ok, 1=No connector choosen, 2=Invalid/unreachable URL, 3=Incorrect username/password, 4=Other error</returns>
        int CheckConnectivity(String connector, String url, String username, String password);
        
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
        /// <returns>dictionary with GlobalAppointmentID -> SyncID assignment</returns>
        Dictionary<String, String> DoUpdates(AppointmentSyncCollection syncItems);
    }
}

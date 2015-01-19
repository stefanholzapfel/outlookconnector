using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Holds all the settings for a connector
    /// </summary>
    public class ConnectorSettings
    {

        /// <summary>
        /// Instantiates the connector
        /// </summary>
        /// <param name="_username">username for accessing the calendar / service</param>
        /// <param name="_password">password</param>
        /// <param name="_calendarUrl">HTTP Url of the calendar / service</param>
        public ConnectorSettings(String _username, String _password, String _calendarUrl)
        {
            Username = _username;
            Password = _password;
            CalendarUrl = _calendarUrl;
        }

        /// <summary>
        /// Username for accessing the calendar / service
        /// </summary>
        public String Username { get; set; }

        /// <summary>
        /// Password for accessing the calendar / service
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// HTTP Url of the calendar / service
        /// </summary>
        public String CalendarUrl { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ConnectorSettings
    {

        public ConnectorSettings(String _username, String _password, String _calendarUrl)
        {
            Username = _username;
            Password = _password;
            CalendarUrl = _calendarUrl;
        }
        public String Username { get; set; }

        public String Password { get; set; }

        public String CalendarUrl { get; set; }

    }
}

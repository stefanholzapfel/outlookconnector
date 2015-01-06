using OutlookAddIn;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ConnectorHandler myHandler = new ConnectorHandler();

            myHandler.ChooseConnector("CaldavConnector");
            myHandler.Settings = new ConnectorSettings("fst5", "fst5", "https://nas.apfelstrudel.net/owncloud/remote.php/caldav/calendars/fst5/fst5");

            AppointmentSyncCollection test = myHandler.GetInitialSync();

            Console.ReadLine();
        }
    }
}

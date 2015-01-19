using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaldavConnector;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            CaldavConnector.CaldavConnector c = new CaldavConnector.CaldavConnector();
            int r = c.CheckConnectivity("", "https://nas.apfelstrudel.net/owncloud/remote.php/caldav/calendars/fst5/", "fst5", "fst5");
            Console.Write(r.ToString());
            Console.ReadLine();
        }
    }
}

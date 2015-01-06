using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaldavConnector.CalDav;
using CaldavConnector.Client;
using Shouldly;


namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var server = new CaldavConnector.Client.Server("https://nas.apfelstrudel.net/owncloud/remote.php/caldav", "fst5", "fst5");
            
            /*CaldavConnector.Client.Calendar cal = new CaldavConnector.Client.Calendar();
            cal.Credentials = new System.Net.NetworkCredential("fst5", "fst5");
            cal.Url = new Uri("https://nas.apfelstrudel.net/owncloud/remote.php/caldav");
            cal.Name = "fst5";*/
            /*var sets = server.GetCalendars();
            var calendar = sets[0];
            var e = new Event
            {
                Description = "this is a description",
                Summary = "summary",
                Sequence = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds,
            };
            calendar.Save(e);
            Console.WriteLine(e.Url);*/
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            CaldavConnector.CaldavConnector test = new CaldavConnector.CaldavConnector();
            Console.Write("DEINE MUDDAAAA");
            //test.Test();
            Console.ReadKey();
        }
    }
}

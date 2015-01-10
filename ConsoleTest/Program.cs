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
            ConnectorHandler myHandler = new ConnectorHandler();

            myHandler.ChooseConnector("CaldavConnector");
            myHandler.Settings = new ConnectorSettings("fst5", "fst5", "https://nas.apfelstrudel.net/owncloud/remote.php/caldav/calendars/fst5/fst5");

            //AppointmentSyncCollection test = myHandler.GetInitialSync();

            //TEST GetUpdates()

            AppointmentSyncCollection test1 = myHandler.GetUpdates();

            //TEST DELETING

            /*AppointmentSyncCollection test2 = new AppointmentSyncCollection();
            test2.DeleteList.Add(new OutlookAppointment()
            {
                SyncID = "20b27af5-b001-42ac-921a-a92094242c55"
            });
            myHandler.DoUpdates(test2);*/



            //TEST UPDATING

            AppointmentSyncCollection test3 = new AppointmentSyncCollection();
            test3.UpdateList.Add(new OutlookAppointment()
            {
                SyncID = "b15605ca-4837-4e4f-af1e-ef2e8d4e7f9e",
                Location = "JUHULOCATION!",
                Subject = "nur blabla2",
                Start = DateTime.Now,
                End = DateTime.Now
            });
            myHandler.DoUpdates(test3);



            //TEST ADDING

            /*AppointmentSyncCollection test4 = new AppointmentSyncCollection();
            test4.AddList.Add(new OutlookAppointment()
            {
                GlobalAppointmentID = System.Guid.NewGuid().ToString(),
                Location = "JUHULOCATION!",
                Subject = "Ganz neu!",
                Start = DateTime.Now,
                End = DateTime.Now
            });
            myHandler.DoUpdates(test4);*/


            Console.ReadLine();
        }
    }
}

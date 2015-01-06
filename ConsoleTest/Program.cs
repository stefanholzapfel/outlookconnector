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
            myHandler.Settings = new ConnectorSettings("fst5", "fst5", "stefan bitte ausbbessern");

            AppointmentSyncCollection test = myHandler.GetUpdates();

            Console.ReadLine();
        }
    }
}

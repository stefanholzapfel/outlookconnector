using CaldavConnector.DataLayer;
using OutlookAddIn;
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

            LocalStorageProvider myProvider = new LocalStorageProvider();
            //myProvider.deleteEntry("12345");
            String test = myProvider.findEntry("1234");

            Console.ReadLine();
        }
    }
}

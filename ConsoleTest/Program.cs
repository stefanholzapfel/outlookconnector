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
            //myProvider.editCTag("12345","CTag2");
            Dictionary<String, String> test = myProvider.getAll();

            Console.ReadLine();
        }
    }
}

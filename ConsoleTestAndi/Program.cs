using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigManager;



namespace ConsoleTestAndi
{
    class Program
    {

        static void Main(string[] args)
        { 
            /*
            FileManager filem = new FileManager();
            saves sav = new saves();
            sav.Name = "Test";
            sav.Password = "Test";
            sav.CalendarName = "Test";
            sav.Connector = "Test";
            sav.URL = "Test";
            sav.UpdateInterval = "Test";

            filem.Save(sav, "config.xml");
            ConfigurationManager conf = new ConfigurationManager();
            conf.GetConfig();
             */
            ConfigurationManager confman = new ConfigurationManager();
            confman.SetConfig("Test1", "Test1", "Test1", "Test1", "Test1", 1000,1);

            Console.WriteLine(confman.GetPassword());
            Console.Read();

       

        }

    }
    public class saves
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string CalendarName { get; set; }
        public string Connector { get; set; }
        public string URL { get; set; }
        public string UpdateInterval { get; set; }
    }
}

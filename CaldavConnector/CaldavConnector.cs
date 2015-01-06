using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using Shared;
using System.Threading.Tasks;
using CaldavConnector.DataLayer;
using System.Xml;
using CaldavConnector.Model;
using CaldavConnector.Converter;
using CaldavConnector.Utilities;

namespace CaldavConnector
{
    [Export(typeof(ICalendarSyncable))]
    public class CaldavConnector : ICalendarSyncable
    {
        private static String _name = "CaldavConnector";
        private LocalStorageProvider _localStorage;

        private String Username;
        private String Password;
        private String CalendarUrl;

        public CaldavConnector()
        {
            //Deactivate certificate validation
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            _localStorage = new LocalStorageProvider();
        }

        public ConnectorSettings Settings
        {
            set {
                Username = value.Username;
                Password = value.Password;
                CalendarUrl = value.CalendarUrl;
            }
        }

        /// <summary>
        /// Property that holds the connectors name.
        /// </summary>
        public string ConnectorName
        {
            get { return _name; }
        }

        /// <summary>
        /// Drops an existing database, creates a new one, fetches all ics elements from server, 
        /// writes their ctags, uid and url to the database, converts the ics to Outlook Appointments 
        /// and returns them.
        /// </summary>
        /// <returns>A collection of all appointments on serverside.</returns>
        public AppointmentSyncCollection GetInitialSync()
        {
            _localStorage.RebuildDatabase();

            System.IO.Stream ResponseStream;
            System.Xml.XmlDocument ResponseXmlDoc;

            //string uri = "https://nas.apfelstrudel.net/owncloud/remote.php/caldav/calendars/fst5/fst5";
            //string uName = "fst5";
            //string uPasswd = "fst5";

            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Depth", "1");
            headers.Add("Prefer", "return-minimal");
            XmlDocument ResponseXmlDoc;
            string query = "<c:calendar-query xmlns:d=\"DAV:\" xmlns:c=\"urn:ietf:params:xml:ns:caldav\">" +
                                "<d:prop>" +
                                    "<d:getetag />" +
                                    "<c:calendar-data />" +
                                "</d:prop>" +
                                "<c:filter>" +
                                    "<c:comp-filter name=\"VCALENDAR\">" +
                                        "<c:comp-filter name=\"VEVENT\" />" +
                                    "</c:comp-filter>" +
                                "</c:filter>" +
                            "</c:calendar-query>";
            ResponseXmlDoc = this.QueryCaldavServer("REPORT", headers, query, "application/xml");

            List<CalDavElement> responseListCalDav = XmlCalDavParser.Parse(ResponseXmlDoc);
            AppointmentSyncCollection responseList = new AppointmentSyncCollection();
            responseListCalDav.ForEach(delegate(CalDavElement element)
            {
                _localStorage.WriteEntry(element.Guid, element.ETag, element.Url);
                responseList.AddList.Add(IcsToAppointmentItemConverter.Convert(element));
            });

            return responseList;
        }

        /// <summary>
        /// Asks the server for a list of all etags, compares them to the local database and 
        /// then asks the server for new, updated and deleted items as full ics items. Ics items
        /// are converted to Outlook Appointments and returned.
        /// </summary>
        /// <returns>A collection with all new, updated and deleted items on serverside.</returns>
        public AppointmentSyncCollection GetUpdates()
        {
            Console.WriteLine("Get updates CalDav executed from: " + this.GetType().Name);
            return new Shared.AppointmentSyncCollection();
        }

        public Dictionary<string, string> DoUpdates(AppointmentSyncCollection syncItems)
        {
            throw new NotImplementedException();
        }
    }
}

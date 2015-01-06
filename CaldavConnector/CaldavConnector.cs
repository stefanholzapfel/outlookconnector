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
        /// writes their etags and uid to the database, converts the ics to Outlook Appointments 
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
            string query = "<c:calendar-query xmlns:d=\"DAV:\" xmlns:c=\"urn:ietf:params:xml:ns:caldav\">" +
                                "<d:prop>" +
                                    "<d:getetag />" +
                                    "<c:calendar-data />" +
                                "</d:prop>" +
                                "<c:filter>" +
                                    "<c:comp-filter name=\"VCALENDAR\" />" +
                                "</c:filter>" +
                            "</c:calendar-query>";
            HttpWebRequest ReportRequest = (HttpWebRequest)WebRequest.Create(CalendarUrl);
            ReportRequest.Method = "REPORT";
            ReportRequest.Credentials = new NetworkCredential(Username, Password);
            ReportRequest.PreAuthenticate = true;
            ReportRequest.Headers = headers;
            ReportRequest.ContentType = "application/xml";
            byte[] optionsArray = Encoding.UTF8.GetBytes(query);
            ReportRequest.ContentLength = optionsArray.Length;
            System.IO.Stream requestStream = ReportRequest.GetRequestStream();
            requestStream.Write(optionsArray, 0, optionsArray.Length);
            requestStream.Close();
            HttpWebResponse ReportResponse = (HttpWebResponse)ReportRequest.GetResponse();
            ResponseStream = ReportResponse.GetResponseStream();
            ResponseXmlDoc = new System.Xml.XmlDocument();
            ResponseXmlDoc.Load(ResponseStream);

            List<CalDavElement> responseListCalDav = XmlCalDavParser.Parse(ResponseXmlDoc);
            AppointmentSyncCollection responseList = new AppointmentSyncCollection();
            responseListCalDav.ForEach(delegate(CalDavElement element)
            {
                _localStorage.WriteEntry(element.Guid, element.CTag, element.Url);
                responseList.AddList.Add(IcsToAppointmentItemConverter.Convert(element));
            });

            return responseList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

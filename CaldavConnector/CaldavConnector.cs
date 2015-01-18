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
using System.Diagnostics;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CaldavConnector
{
    [Export(typeof(ICalendarSyncable))]
    public class CaldavConnector : ICalendarSyncable
    {
        private static String _name = "CaldavConnector";
        private LocalStorageProvider _localStorage;

        private String username;
        private String password;
        private String calendarUrl;

        public CaldavConnector()
        {
            //Deactivate certificate validation
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            _localStorage = new LocalStorageProvider();
        }

        public ConnectorSettings Settings
        {
            set {
                username = value.Username;
                password = value.Password;
                //Ensure that the calendarUrl always ends with a slash
                calendarUrl = CheckSlashAtEnd(value.CalendarUrl);
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
        /// Checks weather the connector can connect, if not it returns:
        /// </summary>
        /// <returns>Int: 0=Connectivity ok, 1=No connector choosen (not checked here), 2=Invalid/unreachable URL, 3=Incorrect username/password, 4=Other error</returns>
        public int CheckConnectivity(String _connector, String _url, String _username, String _password)
        {
            System.IO.Stream ResponseStream;
            HttpWebRequest CaldavRequest;
            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Depth", "0");
            headers.Add("Prefer", "return-minimal");
            string query = "<d:propfind xmlns:d=\"DAV:\">" +
                                "<d:prop>" +
                                "</d:prop>" +
                            "</d:propfind>";
            try
            {
                CaldavRequest = (HttpWebRequest)WebRequest.Create(CheckSlashAtEnd(_url));
                CaldavRequest.Method = "PROPFIND";
                CaldavRequest.Credentials = new NetworkCredential(_username, _password);
                CaldavRequest.PreAuthenticate = true;
                CaldavRequest.Headers = headers;
                CaldavRequest.ContentType = "application/xml";
                byte[] optionsArray = Encoding.UTF8.GetBytes(query);
                CaldavRequest.ContentLength = optionsArray.Length;
                System.IO.Stream requestStream = CaldavRequest.GetRequestStream();
                requestStream.Write(optionsArray, 0, optionsArray.Length);
                requestStream.Close();
                HttpWebResponse ReportResponse = (HttpWebResponse)CaldavRequest.GetResponse();
                ResponseStream = ReportResponse.GetResponseStream();
            }
            catch (Exception e)
            {
                if (e.InnerException is WebException || e.InnerException is SocketException)
                {
                    MessageBox.Show("The URL is incorrect or not reachable!");
                    return 2;
                }
                else
                {
                    MessageBox.Show("The following error occurred: " + e.Message);
                    return 4;
                }
            }
            return 0;
        }

        /// <summary>
        /// Drops an existing database, creates a new one, fetches all ics elements from server, 
        /// writes their etags, uids and urls to the database, converts the ics to Outlook Appointments 
        /// and returns them.
        /// </summary>
        /// <returns>A collection of all appointments on serverside.</returns>
        public AppointmentSyncCollection GetInitialSync()
        {
            _localStorage.RebuildDatabase();

            AppointmentSyncCollection responseList = new AppointmentSyncCollection();
            List<CalDavElement> responseListCalDav = GetAllItemsFromServer();
            responseListCalDav.ForEach(delegate(CalDavElement element)
            {
                _localStorage.WriteEntry(element.Guid, element.ETag, element.Url);
                responseList.AddList.Add(CalDavElementToAppointmentItemConverter.Convert(element));
            });

            return responseList;
        }

        /// <summary>
        /// Checks the server for new, updated and deleted items and returns them.
        /// </summary>
        /// <returns>A collection with all new, updated and deleted items on serverside.</returns>
        public AppointmentSyncCollection GetUpdates()
        {
            AppointmentSyncCollection returnCollection = new AppointmentSyncCollection();
            List<CalDavElement> responseListCalDav = GetAllItemsFromServer();

            //Check for new and updated items
            foreach (var remoteitem in responseListCalDav)
            {
                String foundETag = _localStorage.FindEtag(remoteitem.Guid);
                if (foundETag == null)
                {
                    returnCollection.AddList.Add(CalDavElementToAppointmentItemConverter.Convert(remoteitem));
                    _localStorage.WriteEntry(remoteitem.Guid, remoteitem.ETag, remoteitem.Url);
                }
                else if (foundETag != remoteitem.ETag)
                {
                    returnCollection.UpdateList.Add(CalDavElementToAppointmentItemConverter.Convert(remoteitem));
                    _localStorage.EditETag(remoteitem.Guid, remoteitem.ETag);
                }
            };

            //Check for deleted items
            Boolean deleted;
            OutlookAppointment deletedAppointment = new OutlookAppointment();
            List<String> guidsToDelete = new List<String>();
            foreach (var localitem in _localStorage.GetAll())
            {
                deleted = true;
                foreach (var remoteitem in responseListCalDav)
                {
                    if (remoteitem.Guid.Equals(localitem.Key))
                        deleted = false;
                }
                if (deleted)
                {  
                    deletedAppointment.SyncID = localitem.Key;
                    returnCollection.DeleteList.Add(deletedAppointment);
                    guidsToDelete.Add(localitem.Key);
                }         
            }
            foreach (var item in guidsToDelete)
            {
                _localStorage.DeleteEntry(item);
            }

            return returnCollection;
        }

        /// <summary>
        /// Writes the items added, updated and deleted in Outlook back to the server
        /// </summary>
        /// <param name="syncItems">A collection with all new, updated and deleted items on Outlooks side.</param>
        /// <returns>The servers SyncIDs for items newly added in Outlook</returns>
        public Dictionary<string, string> DoUpdates(AppointmentSyncCollection syncItems)
        {
            Dictionary<string, string> newSyncIds = new Dictionary<string, string>();

            //Delete items from server
            foreach (var deleteItem in syncItems.DeleteList) {
                if (_localStorage.FindUrl(deleteItem.SyncID) != null && !_localStorage.FindUrl(deleteItem.SyncID).Equals(""))
                {
                    try
                    {
                        this.QueryCaldavServer("DELETE", new WebHeaderCollection(), "", null, _localStorage.FindUrl(deleteItem.SyncID));
                    } catch(WebException e) {
                        Debug.WriteLine(e.Message);
                        MessageBox.Show("The following error occurred: " + e.Message);
                    }
                    _localStorage.DeleteEntry(deleteItem.SyncID);
                }
            }

            //Update items on server
            foreach (var updateItem in syncItems.UpdateList)
            {
                if (_localStorage.FindUrl(updateItem.SyncID) != null && !_localStorage.FindUrl(updateItem.SyncID).Equals(""))
                {
                    try
                    {
                        this.QueryCaldavServer("PUT", new WebHeaderCollection(), AppointmentItemXmlParser.Parse(updateItem), "text/calendar", _localStorage.FindUrl(updateItem.SyncID));
                    }
                    catch (WebException e)
                    {
                        Debug.WriteLine(e.Message);
                        MessageBox.Show("The following error occurred: " + e.Message);
                    }
                    _localStorage.EditETag(updateItem.SyncID, GetSingleItemFromServer(_localStorage.FindUrl(updateItem.SyncID)).ETag);
                }
            }

            //Add items to server
            foreach (var addItem in syncItems.AddList) {
                String guid = System.Guid.NewGuid().ToString();
                addItem.SyncID = guid;
                newSyncIds.Add(addItem.GlobalAppointmentID, guid);
                String url = guid + ".ics";
                try
                {
                    this.QueryCaldavServer("PUT", new WebHeaderCollection(), AppointmentItemXmlParser.Parse(addItem), "text/calendar", url);
                    string url_corrected = CheckSlashAtEnd(new Uri(calendarUrl).PathAndQuery) + url;
                    CalDavElement newElement = GetSingleItemFromServer(url_corrected);
                    _localStorage.WriteEntry(guid, newElement.ETag, newElement.Url);
                }
                catch (WebException e)
                {
                    Debug.WriteLine(e.Message);
                    MessageBox.Show("The following error occurred: " + e.Message);
                }
            }

            //Return the dictionary with the new SyncIDs
            return newSyncIds;
        }

        /// <summary>
        /// Returns the CalDavElement from server with the provided relative url on the server.
        /// </summary>
        /// <param name="url">relative Url to concrete .ics</param>
        /// <returns>Found CalDavElement</returns>
        private CalDavElement GetSingleItemFromServer(String url)
        {
            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Depth", "1");
            headers.Add("Prefer", "return-minimal");
            XmlDocument ResponseXmlDoc;
            String query = "<c:calendar-multiget xmlns:d=\"DAV:\" xmlns:c=\"urn:ietf:params:xml:ns:caldav\">" +
                        "<d:prop>" +
                            "<d:getetag />" +
                            "<c:calendar-data />" +
                        "</d:prop>" +
                        "<d:href>" + url + "</d:href>" +
                    "</c:calendar-multiget>";
            ResponseXmlDoc = this.QueryCaldavServer("REPORT", headers, query, "application/xml", null);
            List<CalDavElement> responseListCalDav = XmlCalDavParser.Parse(ResponseXmlDoc);
            return responseListCalDav.First();
        }

        /// <summary>
        /// Returns all CalDavElements from server with full details
        /// </summary>
        /// <returns>List with all CalDavElements with full details</returns>
        private List<CalDavElement> GetAllItemsFromServer()
        {
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
                                    "   <c:comp-filter name=\"VEVENT\" />" +
                                    "</c:comp-filter>" +
                                "</c:filter>" +
                            "</c:calendar-query>";
            ResponseXmlDoc = this.QueryCaldavServer("REPORT", headers, query, "application/xml", null);
            List<CalDavElement> responseListCalDav = XmlCalDavParser.Parse(ResponseXmlDoc);
            return responseListCalDav;
        }

        /// <summary>
        /// Helper method to query a CalDav server.
        /// </summary>
        /// <param name="requestMethod"></param>
        /// <param name="headers"></param>
        /// <param name="query"></param>
        /// <param name="contentType"></param>
        /// <param name="url">If null the settings url is used (for REPORT), if provided the settings url is combined with the path to concrete .ics (for PUT and DELETE)</param>
        /// <returns></returns>
        private XmlDocument QueryCaldavServer(String requestMethod, WebHeaderCollection headers, String query, String contentType, String url)
        {
            System.IO.Stream ResponseStream;
            System.Xml.XmlDocument ResponseXmlDoc = new XmlDocument();
            HttpWebRequest CaldavRequest;
            try
            {
                if (url == null)
                    CaldavRequest = (HttpWebRequest)WebRequest.Create(calendarUrl);
                else
                {
                    string[] url_parts = url.Split(new Char[] { '\\', '/' });
                    string url_corrected = calendarUrl + url_parts.Last();
                    CaldavRequest = (HttpWebRequest)WebRequest.Create(url_corrected);
                }
                CaldavRequest.Method = requestMethod;
                CaldavRequest.Credentials = new NetworkCredential(username, password);
                CaldavRequest.PreAuthenticate = true;
                CaldavRequest.Headers = headers;
                if (contentType != null)
                    CaldavRequest.ContentType = contentType;
                byte[] optionsArray = Encoding.UTF8.GetBytes(query);
                CaldavRequest.ContentLength = optionsArray.Length;
                System.IO.Stream requestStream = CaldavRequest.GetRequestStream();
                requestStream.Write(optionsArray, 0, optionsArray.Length);
                requestStream.Close();
                HttpWebResponse ReportResponse = (HttpWebResponse)CaldavRequest.GetResponse();
                ResponseStream = ReportResponse.GetResponseStream();
                if (!requestMethod.Equals("DELETE") && !requestMethod.Equals("PUT"))
                {
                    ResponseXmlDoc.Load(ResponseStream);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                MessageBox.Show("The following error occurred: " + e.Message);
                return ResponseXmlDoc;
            }
            return ResponseXmlDoc;
        }

        /// <summary>
        /// Checks if a given url ends with "/" and if not corrects it.
        /// </summary>
        /// <param name="url">Url to check</param>
        /// <returns>Corrected url</returns>
        private String CheckSlashAtEnd(String url)
        {
            if (url.Length > 0)
            {
                String lastChar = url[url.Length - 1].ToString();
                if (!lastChar.Equals("/"))
                    return url + "/";
            }
            return url;
        }
    }
}

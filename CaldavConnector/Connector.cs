using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector
{
    public class Connector
    {

        public void Test()
        {
            System.IO.Stream ResponseStream;
            System.Xml.XmlDocument ResponseXmlDoc;

            string uri = "https://nas.apfelstrudel.net/owncloud/remote.php/caldav";
            string uName = "fst5";
            string uPasswd = "fst5";

            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add("Depth", "1");

            string content = "<?xml version=\"1.0\" encoding=\"utf-8\"?><C:calendar-query xmlns:C=\"urn:ietf:params:xml:ns:caldav\">" +
             "<D:prop xmlns:D=\"DAV:\">" +
               "<D:getetag/>" +
               "<C:calendar-data/>" +
             "</D:prop>" +
             "<C:filter>" +
               "<C:comp-filter name=\"VCALENDAR\">" +
                 "<C:comp-filter name=\"VEVENT\"/>" +
               "</C:comp-filter>" +
             "</C:filter>" +
           "</C:calendar-query>";

            HttpWebRequest ReportRequest = (HttpWebRequest)WebRequest.Create(uri);
            ReportRequest.Method = "REQUEST";
            ReportRequest.Credentials = new NetworkCredential(uName, uPasswd);
            ReportRequest.PreAuthenticate = true;
            ReportRequest.Headers = headers;
            ReportRequest.ContentType = "application/xml";
            byte[] optionsArray = Encoding.UTF8.GetBytes(content);
            ReportRequest.ContentLength = optionsArray.Length;

            System.IO.Stream requestStream = ReportRequest.GetRequestStream();
            requestStream.Write(optionsArray, 0, optionsArray.Length);
            requestStream.Close();

            HttpWebResponse ReportResponse = (HttpWebResponse)ReportRequest.GetResponse();

            ResponseStream = ReportResponse.GetResponseStream();

            ResponseXmlDoc = new System.Xml.XmlDocument();
            ResponseXmlDoc.Load(ResponseStream);
        }
    }
}

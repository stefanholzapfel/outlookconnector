using CaldavConnector.Converter;
using CaldavConnector.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CaldavConnector.Utilities
{
    public static class XmlCalDavParser
    {

        /// <summary>
        /// Parses a CalDav response XML into a List of CalDavElements.
        /// </summary>
        /// <param name="ResponseXmlDoc">XML to parse.</param>
        /// <returns>All CalDavElements contained in the Xml.</returns>
        public static List<CalDavElement> Parse(XmlDocument ResponseXmlDoc) {
            List<CalDavElement> allElements = new List<CalDavElement>();
            foreach (XmlNode xnode in ResponseXmlDoc.DocumentElement)
            {
                CalDavElement myElement = new CalDavElement();
                if (xnode.SelectSingleNode("*[local-name()='href']") != null) myElement.Url = xnode.SelectSingleNode("*[local-name()='href']").InnerText;
                if (xnode.SelectSingleNode("*[local-name()='propstat']/*[local-name()='prop']/*[local-name()='getetag']") != null) myElement.CTag = xnode.SelectSingleNode("*[local-name()='propstat']/*[local-name()='prop']/*[local-name()='getetag']").InnerText;
                if (xnode.SelectSingleNode("*[local-name()='propstat']/*[local-name()='prop']/*[local-name()='calendar-data']") != null)
                {
                    string[] parameters = xnode.SelectSingleNode("*[local-name()='propstat']/*[local-name()='prop']/*[local-name()='calendar-data']").InnerText.Split(new char[] { '\n' });
                    foreach (var item in parameters)
                    {
                        string[] specificParameter = item.Split(new char[] { ':' }, 2);
                        if (specificParameter[0].ToUpper().StartsWith("UID") && specificParameter[1] != null) myElement.Guid = specificParameter[1];
                        if (specificParameter[0].ToUpper().StartsWith("SUMMARY") && specificParameter[1] != null) myElement.Summary = specificParameter[1];
                        if (specificParameter[0].ToUpper().StartsWith("DESCRIPTION") && specificParameter[1] != null) myElement.Description = specificParameter[1];
                        if (specificParameter[0].ToUpper().StartsWith("LAST-MODIFIED") && specificParameter[1] != null) myElement.LastModified = StringToDateTimeConverter.Convert(specificParameter[1]);
                        if (specificParameter[0].ToUpper().StartsWith("LOCATION") && specificParameter[1] != null) myElement.Location = specificParameter[1];
                        if (specificParameter[0].ToUpper().StartsWith("DTSTART") && specificParameter[1] != null)
                        {
                            if (specificParameter[1].Contains("T"))
                                myElement.AllDayEvent = false;
                            else
                            {
                                myElement.AllDayEvent = true;
                            }
                            myElement.Start = StringToDateTimeConverter.Convert(specificParameter[1]);
                        }
                        if (specificParameter[0].ToUpper().StartsWith("DTEND") && specificParameter[1] != null) myElement.End = StringToDateTimeConverter.Convert(specificParameter[1]);
                    }
                }
                allElements.Add(myElement);
            }
            return allElements;
        }

    }
}

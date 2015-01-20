using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OutlookAddIn
{
    /// <summary>
    /// The FileManager class can save or load a generic XML File
    /// </summary>
    public class FileManager
    {
        private static string path = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)+@"\Microsoft\Outlook\OutlookConnector\";

        /// <summary>
        /// XML Serializer to save a generic object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Any object you want to save as xml</param>
        /// <param name="Filename">Filename of the xml.</param>
        public void SaveXML<T>(T obj, string filename)
        {
            Directory.CreateDirectory(path);            
            using (var fileStream = new FileStream(path+filename+".xml", FileMode.Create))
            {
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(fileStream, obj);
            }

        }

        /// <summary>
        /// XML Serializer to load a generic object.
        /// </summary>
        /// <typeparam name="T">Any object you want to from an xml file.</typeparam>
        /// <param name="Filename">Filename of the xml.</param>
        /// <returns>Returns any spezified object with the xml data.</returns>
        public T LoadXML<T>(string filename)
        {
            T result;

            if (Directory.Exists(path))
            {
                var ser = new XmlSerializer(typeof(T));
                try
                {
                    using (var tr = new StreamReader(path + filename + ".xml"))
                    {
                        result = (T)ser.Deserialize(tr);
                        return result;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    return result = default(T);
                }
            }
            else
                return result = default(T);

        }
    }
}

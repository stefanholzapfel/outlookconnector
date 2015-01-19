using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.DataLayer
{

    /// <summary>
    /// This class handles the local cache of the remote CalDav server.
    /// </summary>
    public class LocalStorageProvider
    {
        private static String filepath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Outlook\OutlookConnector\Data\CalDavConnectorCache.sqlite";
        private static String foldername = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Outlook\OutlookConnector\Data";
        Dictionary<String, String[]> localCache;
        SQLiteConnection myConnection;

        /// <summary>
        /// The default constructor checks wether the database exists and loads it if yes or
        /// creates it if no.
        /// </summary>
        public LocalStorageProvider() {
            myConnection = new SQLiteConnection("Data Source=" + filepath + ";Version=3;");
            if (!File.Exists(filepath))
                RebuildDatabase();
            localCache = ExecuteQuery("SELECT * FROM localETagCache order by Guid desc");
        }

        /// <summary>
        /// Creates a new SQLite database for local cache storage of Guids and ETags 
        /// for CalDAV synchronization. If the database already exists, it will 
        /// be overwritten.
        /// </summary>
        public void RebuildDatabase() {
            if (File.Exists(filepath))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(filepath);
            }
            if (!Directory.Exists(foldername))
                Directory.CreateDirectory(foldername);
            SQLiteConnection.CreateFile(filepath);
            myConnection = new SQLiteConnection("Data Source=" + filepath + ";Version=3;");
            ExecuteNonQuery("CREATE TABLE localETagCache (Guid VARCHAR(100), ETag VARCHAR(100), Url VARCHAR(255))");
        }

        /// <summary>
        /// Searches the database for an ETag relating to a given GUID.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <param name="guid">The Guid to look up.</param>
        /// <returns>The matching ETag for the given Guid or null if nothing found.</returns>
        public String FindEtag(String guid) {
            String[] temp = null;
            if (guid != null && localCache.ContainsKey(guid))
                temp = localCache[guid];
            if (temp == null)
                return null;
            else
                return temp[0];
        }

        /// <summary>
        /// Searches the database for an url relating to a given GUID.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <param name="guid">The Guid to look up.</param>
        /// <returns>The matching Url for the given Guid or null if nothing found.</returns>
        public String FindUrl(String guid)
        {
            String[] temp = null;
            if (guid != null && localCache.ContainsKey(guid))
                temp = localCache[guid];
            if (temp == null)
                return null;
            else
                return temp[1];
        }

        /// <summary>
        /// Returns all current entries of the database.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <returns>Dictionary of all entries</returns>
        public Dictionary<String, String[]> GetAll()
        {
            return localCache;
        }


        /// <summary>
        /// Edits the ETag of an existing entry.
        /// </summary>
        /// <param name="guid">Guid of entry to edit.</param>
        /// <param name="eTag">New value for ETag.</param>
        public void EditETag(String guid, String eTag) {
            if (localCache.ContainsKey(guid))
            {
                localCache[guid][0] = eTag;
                ExecuteNonQuery("UPDATE localETagCache SET ETag ='"+eTag+"' WHERE Guid='"+guid+"'");
            }
        }

        /// <summary>
        /// Adds a new entry to the database if Guid is not already
        /// present.
        /// </summary>
        /// <param name="guid">Guid to add.</param>
        /// <param name="eTag">ETag to add.</param>
        /// <param name="url">Url to add</param>
        public void WriteEntry(String guid, String eTag, String url) {
            if (!localCache.ContainsKey(guid))
            {
                localCache.Add(guid, new String[] { eTag, url });
                ExecuteNonQuery("INSERT INTO localETagCache (Guid, ETag, Url) values ('" + guid + "', '" + eTag + "', '" + url + "')");
            }
        }

        /// <summary>
        /// Deletes an entry from the database if it exists.
        /// </summary>
        /// <param name="guid">Guid to delete.</param>
        public void DeleteEntry(String guid) {
            if (localCache.ContainsKey(guid))
            {
                localCache.Remove(guid);
                ExecuteNonQuery("DELETE FROM localETagCache WHERE Guid='" + guid + "'");
            }
        }

        /// <summary>
        /// Helper method that executes a query against the database 
        /// that receives no data back.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        /// <returns>Number of rows affected.</returns>
        private int ExecuteNonQuery(String query)
        {
            myConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            int number = command.ExecuteNonQuery();
            myConnection.Close();
            return number;
        }


        /// <summary>
        /// Helper method that executes a query against the database 
        /// that receives data back.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        /// <returns>Dictionary with results from query.</returns>
        private Dictionary<String,String[]> ExecuteQuery(String query)
        {
            Dictionary<String, String[]> tempDictionary = new Dictionary<String, String[]>();
            myConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                tempDictionary.Add(reader["Guid"].ToString(), new String[] { reader["ETag"].ToString(), reader["Url"].ToString() });
            }
            myConnection.Close();
            return tempDictionary;
        }

    }
}

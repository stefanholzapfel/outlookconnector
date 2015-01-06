﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector.DataLayer
{
    public class LocalStorageProvider
    {
        private static String filepath = "Data/CalDavConnectorCache.sqlite";
        private static String foldername = "Data";
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
        /// Search the database for a GUID.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <param name="Guid">The Guid to look up.</param>
        /// <returns>The matching ETag for the given Guid or null if nothing found.</returns>
        public String FindEntry(String Guid) {
            String[] temp = null;
            if (Guid != null && localCache.ContainsKey(Guid))
                temp = localCache[Guid];
            if (temp == null)
                return null;
            else
                return temp[0];
        }

        /// <summary>
        /// Return all current entries of the database.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <returns>Dictionary of all entries</returns>
        public Dictionary<String, String[]> GetAll()
        {
            return localCache;
        }


        /// <summary>
        /// Edit the ETag of an existing entry.
        /// </summary>
        /// <param name="Guid">Guid of entry to edit.</param>
        /// <param name="ETag">New value for ETag</param>
        public void EditETag(String Guid, String ETag) {
            if (localCache.ContainsKey(Guid))
            {
                localCache[Guid][0] = ETag;
                ExecuteNonQuery("UPDATE localETagCache SET ETag ='"+ETag+"' WHERE Guid='"+Guid+"'");
            }
        }

        /// <summary>
        /// Add a new entry to the database if Guid is not already
        /// present.
        /// </summary>
        /// <param name="Guid">Guid to add.</param>
        /// <param name="ETag">ETag to add.</param>
        /// <param name="Url">Url to add</param>
        public void WriteEntry(String Guid, String ETag, String Url) {
            if (!localCache.ContainsKey(Guid))
            {
                localCache.Add(Guid, new String[] {ETag, Url});
                ExecuteNonQuery("INSERT INTO localETagCache (Guid, ETag, Url) values ('" + Guid + "', '" + ETag + "', '" + Url + "')");
            }
        }

        /// <summary>
        /// Delete an entry from the database if it exists.
        /// </summary>
        /// <param name="Guid">Guid to delete.</param>
        public void DeleteEntry(String Guid) {
            if (localCache.ContainsKey(Guid))
            {
                localCache.Remove(Guid);
                ExecuteNonQuery("DELETE FROM localETagCache WHERE Guid='" + Guid + "'");
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
                tempDictionary.Add(reader["Guid"].ToString(), new String[] { reader["ETag"].ToString(), reader["Url"].ToString() });
            myConnection.Close();
            return tempDictionary;
        }

    }
}

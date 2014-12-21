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
        /// The default constructor checks weather the database exists and loads it if yes or
        /// creates if if no.
        /// </summary>
        public LocalStorageProvider() {
            myConnection = new SQLiteConnection("Data Source=" + filepath + ";Version=3;");
            if (!File.Exists(filepath))
                rebuildDatabase();
            localCache = executeQuery("SELECT * FROM localCTagCache order by Guid desc");
        }

        /// <summary>
        /// Creates a new SQLite database for local cache storage of Guids and CTags 
        /// for CalDAV synchronization. If the database already exists, it will 
        /// be overwritten.
        /// </summary>
        public void rebuildDatabase() {
            if (File.Exists(filepath))
                File.Delete(filepath);
            if (!Directory.Exists(foldername))
                Directory.CreateDirectory(foldername);
            SQLiteConnection.CreateFile(filepath);
            executeNonQuery("CREATE TABLE localCTagCache (Guid VARCHAR(100), CTag VARCHAR(100), Url VARCHAR(100))");
        }

        /// <summary>
        /// Search the database for a GUID.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <param name="Guid">The Guid to look up.</param>
        /// <returns>The matching CTag for the given Guid or null if nothing found.</returns>
        public String[] findEntry(String Guid) {
            String[] temp = null;
            if (localCache.ContainsKey(Guid))
                temp = localCache[Guid];
            return temp;
        }

        /// <summary>
        /// Return all current entries of the database.
        /// EFFICIENT - No SQL statements executed!
        /// </summary>
        /// <returns>Dictionary of all entries</returns>
        public Dictionary<String, String[]> getAll()
        {
            return localCache;
        }


        /// <summary>
        /// Edit the CTag of an existing entry.
        /// </summary>
        /// <param name="Guid">Guid of entry to edit.</param>
        /// <param name="CTag">New value for CTag</param>
        public void editCTag(String Guid, String CTag) {
            if (localCache.ContainsKey(Guid))
            {
                localCache[Guid][0] = CTag;
                executeNonQuery("UPDATE localCTagCache SET CTag ='"+CTag+"' WHERE Guid='"+Guid+"'");
            }
        }

        /// <summary>
        /// Add a new entry to the database if Guid is not already
        /// present.
        /// </summary>
        /// <param name="Guid">Guid to add.</param>
        /// <param name="Etag">CTag to add.</param>
        /// 
        public void writeEntry(String Guid, String CTag, String Url) {
            if (!localCache.ContainsKey(Guid))
            {
                localCache.Add(Guid, new String[] {CTag, Url});
                executeNonQuery("INSERT INTO localCTagCache (Guid, CTag, Url) values ('" + Guid + "', '" + CTag + "', '" + Url + "')");
            }
        }

        /// <summary>
        /// Delete an entry from the database if it exists.
        /// </summary>
        /// <param name="Guid">Guid to delete.</param>
        public void deleteEntry(String Guid) {
            if (localCache.ContainsKey(Guid))
            {
                localCache.Remove(Guid);
                executeNonQuery("DELETE FROM localCTagCache WHERE Guid="+ Guid);
            }
        }


        /// <summary>
        /// Helper method that executes a query against the database 
        /// that receives no data back.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        /// <returns>Number of rows affected.</returns>
        private int executeNonQuery(String query)
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
        private Dictionary<String,String[]> executeQuery(String query)
        {
            Dictionary<String, String[]> tempDictionary = new Dictionary<String, String[]>();
            myConnection.Open();
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                tempDictionary.Add(reader["Guid"].ToString(), new String[] { reader["CTag"].ToString(), reader["Url"].ToString() });
            myConnection.Close();
            return tempDictionary;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace OutlookAddIn
{
    /// <summary>
    /// The ConfigurationManager class provides everything necessary to save, load and update a configuration
    /// </summary>
    public class ConfigurationManager
    {
        Config conf = new Config();

        private static byte[] entropy = { 5, 19, 22, 8, 16, 27, 35, 65, 91 };
        private static string filename = @"config";

        /// <summary>
        /// minimum interval time
        /// </summary>
        public const int MIN_INTERVAL = 10000;

        /// <summary>
        /// Instantiates a new FileManager and loads the config.xml if it exists
        /// </summary>
        public ConfigurationManager()
        {
            conf.updateInterval = MIN_INTERVAL;
            FileManager fileMan = new FileManager();
            if (fileMan.LoadXML<Config>(filename) != null)
            {
                conf = fileMan.LoadXML<Config>(filename);
            }
        }
        /// <summary>
        /// Get the current configuration without encrypted password.
        /// To get decrypted password use GetPassword method.
        /// </summary>
        /// <returns>Object of type Config</returns>
        public Config GetConfig()
        {
            if (conf != null)
            {
                return conf;
            }
            else
                return null;
        }
        /// <summary>
        /// Get the current password.
        /// </summary>
        /// <returns>Password of type String or null if there is no password set</returns>
        public String GetPassword()
        {
            if (conf.password != null)
            {
                byte[] password = ProtectedData.Unprotect(conf.password, entropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(password);
            }
            else
                return null;
        }
        /// <summary>
        /// Returns the current Update Interval
        /// </summary>
        /// <returns>Returns the current Update Interval</returns>
        public int GetUpdateInterval()
        {
            return conf.updateInterval;
        }
        /// <summary>
        /// Set the synced Parameter in the config.xml
        /// </summary>
        /// <param name="_synced">0 = not synced, 1 = synced</param>
        public void SetSynced(byte _synced)
        {
            conf.synced = _synced;
            SaveConfig();
        }
        /// <summary>
        /// Sets the autosync parameter in the configuration and saves it
        /// </summary>
        /// <param name="_autosync">Autosync 0 = off, 1 = on</param>
        public void SetAutoSync(byte _autosync)
        {
            if (conf.calendarName != null)
            {
                conf.autosync = _autosync;
                SaveConfig();
            }
        }
        /// <summary>
        /// Updates the update interval in the current configuration and saves it
        /// </summary>
        /// <param name="_updateInterval">Update Interval</param>
        public void SetUpdateInterval(int _updateInterval)
        {
            if (conf.calendarName != null)
            {
                conf.updateInterval = _updateInterval;
                SaveConfig();
            }
        }
        /// <summary>
        /// Sets the config variables
        /// </summary>
        /// <param name="_userName">Username</param>
        /// <param name="_password">Password</param>
        /// <param name="_claendarName">Calendar Name</param>
        /// <param name="_connector">Connector Name</param>
        /// <param name="_URL">URL</param>
        /// <param name="_updateInterval">Update Interval</param>
        /// <param name="_synced">Already synced 0 = no, 1 = yes</param>
        /// <param name="_autosync">Autosync 0 = off, 1 = on</param>
        public void SetConfig(string _userName, string _password, string _claendarName, string _connector, string _URL, int _updateInterval, byte _synced, byte _autosync)
        {
            conf.userName = _userName;
            conf.password = Protect(Encoding.UTF8.GetBytes(_password));
            conf.calendarName = _claendarName;
            conf.connector = _connector;
            conf.URL = _URL;
            conf.updateInterval = _updateInterval;
            conf.synced = _synced;
            conf.autosync = _autosync;
            SaveConfig();
        }
        /// <summary>
        /// This method uses the FileManager save method to save the current configuration
        /// </summary>
        public void SaveConfig()
        {
            FileManager fileMan = new FileManager();
            fileMan.SaveXML(conf, filename);
        }
        /// <summary>
        /// This method encrypts a byte array
        /// </summary>
        /// <param name="data">Needs a byte array, which should be encrypted</param>
        /// <returns>Returns an encrypted byte array or null if encryption wasn't possible</returns>
        public static byte[] Protect(byte[] data)
        {
            try
            {
                return ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
    /// <summary>
    /// The Config class creates an object with all the information for the sync. The password in this class is normaly encrypted.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Username for the connection
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// Password for the connection (should be encrypted)
        /// </summary>
        public byte[] password { get; set; }

        /// <summary>
        /// Name of the calendar in Outlook
        /// </summary>
        public string calendarName { get; set; }

        /// <summary>
        /// Namer of the connector
        /// </summary>
        public string connector { get; set; }

        /// <summary>
        /// URL for the connection
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Interval for the synchronization (in milliseconds)
        /// </summary>
        public int updateInterval { get; set; }

        /// <summary>
        /// Already synced 0 = no, 1 = yes
        /// </summary>
        public byte synced { get; set; }

        /// <summary>
        /// Autosync 0 = off, 1 = on
        /// </summary>
        public byte autosync { get; set; }
    }
}

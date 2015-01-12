using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace ConfigManager
{
    public class ConfigurationManager
    {
        Config conf = new Config();

        private static byte[] entropy = { 5, 19, 22, 8, 16, 27, 35, 65, 91 };
        private static string filename = @"config";

        public ConfigurationManager()
        {
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
        /// Set the synced Parameter in the config.xml
        /// </summary>
        /// <param name="_synced">0 = not synced, 1 = synced</param>
        public void SetSynced(byte _synced)
        {
            conf.synced = _synced;
            SaveConfig();
        }
        public void SetAutoSync(byte _autosync)
        {
            conf.autosync = _autosync;
            SaveConfig();
        }
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
        public void SaveConfig()
        {
            FileManager fileMan = new FileManager();
            fileMan.SaveXML(conf, filename);        
          
        }        
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
    public class Config
    {
        public string userName { get; set; }
        public byte[] password { get; set; }
        public string calendarName { get; set; }
        public string connector { get; set; }
        public string URL { get; set; }
        public int updateInterval { get; set; }
        public byte synced { get; set; }
        public byte autosync { get; set; }
        
    }
}

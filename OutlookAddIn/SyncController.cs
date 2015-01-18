using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlookAddIn;
using OutlookAddIn;
using Shared;
using System.Windows.Forms;

namespace OutlookAddIn
{
    public class SyncController
    {
        ConfigurationManager _confManager;
        Config _config;
        private bool init = false;
        private bool autosync = false;

        ConnectorHandler _connHandler;
        CalendarHandler _calHandler;
        SyncService _synService;

        public SyncController(ConfigurationManager confManager)
        {
            _confManager = confManager;
            _connHandler = new ConnectorHandler();
            _config = _confManager.GetConfig();
        }
        /// <summary>
        /// Instantiates and gets everything needed for the sync
        /// </summary>
        public void InitializeSync()
        {             

            if (_config.calendarName != null)
            {
                _connHandler.ChooseConnector(_config.connector);
                _connHandler.Settings = new ConnectorSettings(_config.userName, _confManager.GetPassword(), _config.URL);
                _calHandler = new CalendarHandler(Globals.ThisAddIn.Application, _config.calendarName);
                _synService = new SyncService(_calHandler, _connHandler, _config.updateInterval);
                init = true;
            }             
        }

        public Boolean CheckConnectivity(String connector, String url, String username, String password)
        {
            if (_connHandler.CheckConnectivity(connector, url, username, password) == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Start Autosync
        /// </summary>
        public void InitializeAutoSync()
        {
            if (_config.calendarName != null)
            {
                if (init == false)
                {
                    InitializeSync();
                }
                if (_config.autosync == 1)
                {
                    autosync = true;
                    _synService.Start();                    
                }
            }
            else
                MessageBox.Show("Please enter settings first.");
        }
        /// <summary>
        /// Start manual Sync
        /// </summary>
        public void IntitializeManualSync()
        {
            if (_config.calendarName != null)
            {
                if (init == false)
                {
                    InitializeSync();
                }
                _synService.ExecuteOnce();
                
            }
            else
                MessageBox.Show("Please enter settings first.");
        }
        /// <summary>
        /// Stop the current Sync
        /// </summary>
        public void StopSync()
        {
            if (init == true)
            {
                _synService.Stop();
                autosync = false;
                init = false;
            }
        }
        /// <summary>
        /// Change the current Interval and save it
        /// </summary>
        /// <param name="_updateInterval"></param>
        public void ChangeInterval(int _updateInterval)
        {
            if (_config.calendarName != null)
            {
                _confManager.SetUpdateInterval(_updateInterval);

                if (init == true)
                {                    
                    _synService.SetInterval(_updateInterval);
                }
            }
            else            
                MessageBox.Show("Please enter settings first.");           

        }
        /// <summary>
        /// Reset the current Sync
        /// </summary>
        public void ResetSync(bool newSettings)
        {
            if (newSettings == false)
            {
                _calHandler.DeleteCustomCalendar();
            }
            if (init == false)
            {
                InitializeSync();
            }
            _synService.Reset();
            _confManager.SetSynced(1);
        }
        /// <summary>
        /// Get autosync state
        /// </summary>
        /// <returns>Bool with autosync state</returns>
        public bool GetAutosync()
        {
            return autosync;
        }
    }
}

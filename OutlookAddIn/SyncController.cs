﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigManager;
using SyncLogic;
using Shared;
using System.Windows.Forms;

namespace OutlookAddIn
{
    public class SyncController
    {
        ConfigurationManager _confManager;
        Config _config;
        private bool init = false;      

        ConnectorHandler _connHandler;
        CalendarHandler _calHandler;
        SyncService _synService;

        public SyncController(ConfigurationManager confManager)
        {
            _confManager = confManager;
        }
        /// <summary>
        /// Instantiates and gets everything needed for the sync
        /// </summary>
        public void InitializeSync()
        {
            _connHandler = new ConnectorHandler();
            _config = _confManager.GetConfig();

            if (_config.calendarName != null)
            {
                _connHandler.ChooseConnector(_config.connector);
                _connHandler.Settings = new ConnectorSettings(_config.userName, _confManager.GetPassword(), _config.URL);
                _calHandler = new CalendarHandler(Globals.ThisAddIn.Application, _config.calendarName);
                _synService = new SyncService(_calHandler, _connHandler, _config.updateInterval);
                init = true;
            }             
        }
        /// <summary>
        /// Start Autosync
        /// </summary>
        public void InitializeAutoSync()
        {
            if (init == false)
            {
                InitializeSync();
            }            
            if (_config.autosync == 1)
            {
                if (_config.synced == 0)
                {                    
                    _synService.Reset();
                    _confManager.SetSynced(1);
                    _synService.Start();
                }
                else
                {
                    _synService.Start();
                }
            }
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
                if (_config.synced == 0)
                {                    
                    _synService.Reset();
                    _confManager.SetSynced(1);
                    _synService.ExecuteOnce();

                }
                else
                {
                    _synService.ExecuteOnce();
                }
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
                init = false;
            }
        }
        /// <summary>
        /// Reset the current Sync
        /// </summary>
        public void ResetSync()
        {
            if (init == true)
            {
                _synService.Reset();
                _confManager.SetSynced(1);
            }
        }
    }
}
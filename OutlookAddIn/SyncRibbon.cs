using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using Shared;
using SyncLogic;
using ConfigManager;

namespace OutlookAddIn
{
    public partial class SyncRibbon
    {        

        ConfigurationManager _confManager;        
        SyncController _syncController;
        private int updateInterval;         
                   
        private void SyncRibbon_Load(object sender, RibbonUIEventArgs e)
        {            
            _confManager = new ConfigurationManager();
            _syncController = new SyncController(_confManager);
            _syncController.InitializeSync();            
            updateInterval = (_confManager.GetUpdateInterval()/1000);
            edb_interval.Text = updateInterval.ToString();
                             

            if (_syncController.GetAutosync() == true)
            {
                _syncController.InitializeAutoSync();
                btn_autosync.Label = "Deactivate";
            }
        }        
        private void btn_Settings_Click(object sender, RibbonControlEventArgs e)
        {                        
            ConfigManagerUI formConfigManager = new ConfigManagerUI(_confManager, _syncController);              
            formConfigManager.ShowDialog();           
        }
        private void btn_manualSync_Click(object sender, RibbonControlEventArgs e)
        {
            _syncController.IntitializeManualSync();
        }

        private void btn_autosync_Click(object sender, RibbonControlEventArgs e)
        {
            if (_syncController.GetAutosync() == false)
            {
                _confManager.SetAutoSync(1);
                _syncController.InitializeAutoSync();
                btn_autosync.Label = "Deactivate";
            }
            else
            {
                _confManager.SetAutoSync(0);
                _syncController.StopSync();
                btn_autosync.Label = "Activate";
            }
        }

        private void edb_interval_TextChanged(object sender, RibbonControlEventArgs e)
        {
            if (!int.TryParse(edb_interval.Text, out updateInterval))
            {
                MessageBox.Show("Update Interval only allows natural numbers between 10 and 3600");
            }
            else if ((Int32.Parse(edb_interval.Text) < 10) || (Int32.Parse(edb_interval.Text) > 3600))
            {
                MessageBox.Show("Update Interval only allows natural numbers between 10 and 3600");
            }
            else
            {
                updateInterval = Int32.Parse(edb_interval.Text);
                _syncController.ChangeInterval(updateInterval*1000);
            }
        }
    }
}

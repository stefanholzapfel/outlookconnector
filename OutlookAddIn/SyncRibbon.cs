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
       
        private void SyncRibbon_Load(object sender, RibbonUIEventArgs e)
        {            
            _confManager = new ConfigurationManager();
            _syncController = new SyncController(_confManager);
            _syncController.InitializeSync();
            _syncController.InitializeAutoSync();              
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

    }
}

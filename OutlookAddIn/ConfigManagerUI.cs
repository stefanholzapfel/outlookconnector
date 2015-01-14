using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConfigManager;
using System.ComponentModel.Composition.Hosting;
using SyncLogic;
using Shared;

namespace OutlookAddIn
{
    public partial class ConfigManagerUI : Form
    {
        
        ConfigurationManager confManager;
        Config conf = new Config();

        SyncController syncController;

        ConnectorHandler conHan = new ConnectorHandler();

        List<String> availableConnectors = new List<string>();

        private string userName;
        private string password;
        private string calendarName;
        private string connector;
        private string URL;
        private byte synced;

        private bool newSettings = true;
        public ConfigManagerUI(ConfigurationManager _configManager, SyncController _syncController)
        {            
            InitializeComponent();
            confManager = _configManager;
            conf = confManager.GetConfig();
            syncController = _syncController;

            if (syncController.GetAutosync() == true)
            {
                syncController.StopSync();
            }

            availableConnectors = conHan.GetAvailableConnectors();                       
            foreach (var item in availableConnectors)
            {
                cbo_Connector.Items.Add(item);
            }            
            if (conf != null)
            {                
                userName = conf.userName;
                synced = conf.synced;
               
                txt_Username.Text = userName;
                calendarName = conf.calendarName;
                txt_CalendarName.Text = calendarName;
                connector = conf.connector;
                cbo_Connector.SelectedIndex = cbo_Connector.FindStringExact(connector);               
                URL = conf.URL;
                txt_URL.Text = URL;
                password = confManager.GetPassword();
                txt_Password.Text = password;                
            }           
        }
        private void btn_Save_Click(object sender, EventArgs e)            
        {
            if (txt_Username.Text == "" || txt_CalendarName.Text == "" || txt_Password.Text == "" || txt_URL.Text == "" || cbo_Connector.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all forms");
            }
            else
            {
                if (calendarName != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you really want to change these settings? This will automatically reset the synchronization.", "Change Settings", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        userName = txt_Username.Text;
                        calendarName = txt_CalendarName.Text;
                        connector = cbo_Connector.SelectedItem.ToString();
                        URL = txt_URL.Text;
                        synced = 0;
                        password = txt_Password.Text;                        
                                                                    
                        confManager.SetConfig(userName, password, calendarName, connector, URL, conf.updateInterval, synced, conf.autosync);
                        newSettings = false;
                        btn_Save.Text = "In Progress...";
                        btn_Save.Enabled = false;
                        backgroundWorker1.RunWorkerAsync();
                        
                       
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                }
                else
                {
                    userName = txt_Username.Text;
                    calendarName = txt_CalendarName.Text;
                    connector = cbo_Connector.SelectedItem.ToString();
                    URL = txt_URL.Text;
                    synced = 0;
                    password = txt_Password.Text;                    

                    confManager.SetConfig(userName, password, calendarName, connector, URL, conf.updateInterval, synced, 0);
                    newSettings = true;
                    btn_Save.Text = "In Progress...";
                    btn_Save.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();            
                }
            }
        }
        private void ConfigManagerUI_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (conf.autosync == 1)
            {
                syncController.InitializeAutoSync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            syncController.ResetSync(newSettings);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}

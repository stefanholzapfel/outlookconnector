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
        private int updateInterval;
        private byte synced;
        private byte autosync;
        
        public ConfigManagerUI(ConfigurationManager _configManager, SyncController _syncController)
        {           
            InitializeComponent();
            confManager = _configManager;
            conf = confManager.GetConfig();

            syncController = _syncController;

            availableConnectors = conHan.GetAvailableConnectors();                       
            foreach (var item in availableConnectors)
            {
                cbo_Connector.Items.Add(item);
            }
            
            if (conf != null)
            {                
                userName = conf.userName;
                synced = conf.synced;
                autosync = conf.autosync;
                if (autosync==1) check_autosync.Checked = true;
                txt_Username.Text = userName;
                calendarName = conf.calendarName;
                txt_CalendarName.Text = calendarName;
                connector = conf.connector;
                cbo_Connector.SelectedIndex = cbo_Connector.FindStringExact(connector);               
                URL = conf.URL;
                txt_URL.Text = URL;
                updateInterval = conf.updateInterval/1000;
                txt_UpdateInterval.Text = updateInterval.ToString();
                password = confManager.GetPassword();
                txt_Password.Text = password;                
            }           
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you really want to reset the synchronization?", "Reset Synchronization", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                syncController.ResetSync();
            }
            else if (dialogResult == DialogResult.No)
            {                
            }
        }
        private void btn_Save_Click(object sender, EventArgs e)            
        {
            if (txt_Username.Text == "" || txt_CalendarName.Text == "" || txt_Password.Text == "" || txt_URL.Text == "" || txt_UpdateInterval.Text == "" || cbo_Connector.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all forms");
            }
            else if (!int.TryParse(txt_UpdateInterval.Text, out updateInterval))
            {
                MessageBox.Show("Update Interval only allows natural numbers between 10 and 3600");
            }
            else if ((Int32.Parse(txt_UpdateInterval.Text) < 10) || (Int32.Parse(txt_UpdateInterval.Text) > 3600))
            {
                MessageBox.Show("Update Interval only allows natural numbers between 10 and 3600");
            }
            else
            {
                if ((calendarName != null) && ((userName != txt_Username.Text) || (calendarName != txt_CalendarName.Text) || (connector != cbo_Connector.SelectedItem.ToString()) || (URL != txt_URL.Text)))
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
                        updateInterval = Int32.Parse(txt_UpdateInterval.Text);

                        if (check_autosync.Checked == true) autosync = 1;
                        else autosync = 0;

                        syncController.StopSync();
                        confManager.SetConfig(userName, password, calendarName, connector, URL, updateInterval*1000, synced, autosync);
                        
                        if (autosync == 1) syncController.InitializeAutoSync();
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

                    password = txt_Password.Text;
                    updateInterval = Int32.Parse(txt_UpdateInterval.Text);

                    if (check_autosync.Checked == true) autosync = 1;
                    else autosync = 0;

                    syncController.StopSync();
                    confManager.SetConfig(userName, password, calendarName, connector, URL, updateInterval*1000, synced, autosync);
              
                    if (autosync == 1) syncController.InitializeAutoSync();
                }
            }
        }
    }
}

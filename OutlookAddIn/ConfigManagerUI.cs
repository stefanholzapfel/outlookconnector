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

namespace OutlookAddIn
{
    public partial class ConfigManagerUI : Form
    {
        
        ConfigurationManager confManager;
        Config conf = new Config();

        ConnectorHandler conHan = new ConnectorHandler();

        List<String> availableConnectors = new List<string>();

        private string userName;
        private string password;
        private string calendarName;
        private string connector;
        private string URL;
        private int updateInterval;
        private byte synced;
        
        public ConfigManagerUI(ConfigurationManager _configManager)
        {           
            InitializeComponent();
            confManager = _configManager;
            conf = confManager.GetConfig();
            
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
                updateInterval = conf.updateInterval;
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
                if (synced == 1)
                {
                    synced = 0;
                    confManager.SetSynced(synced);
                }
                else { }
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
                MessageBox.Show("Update Interval only allows numbers between 1000 and 2.147.483.647");
            }
            else if (Int32.Parse(txt_UpdateInterval.Text) < 1000)
            {
                MessageBox.Show("Minimum update intervall = 1000");
            }
            else
            {
                userName = txt_Username.Text;
                calendarName = txt_CalendarName.Text;
                password = txt_Password.Text;
                connector = cbo_Connector.SelectedItem.ToString();
                URL = txt_URL.Text;
                updateInterval = Int32.Parse(txt_UpdateInterval.Text);
                confManager.SetConfig(userName, password, calendarName, connector, URL, updateInterval, synced);
            }
        }
    }
}

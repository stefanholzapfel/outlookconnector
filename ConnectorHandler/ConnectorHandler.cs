using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutlookAddIn
{
    public class ConnectorHandler : ICalendarSyncable

    {
        private String choosenConnector;
        private String path = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Outlook\OutlookConnector\Connectors";
        [ImportMany(typeof(ICalendarSyncable))]
        public List<ICalendarSyncable> MefCalendarConnectors { get; set; }

        /// <summary>
        /// This constrcutor builds a blank ConnectorHandler to be capable of returning a list of available connectors via GetAvailableConnectors(). No connector choosen so far.
        /// </summary>
        public ConnectorHandler()
        {
            choosenConnector = null;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var catalog = new DirectoryCatalog(path);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        /// <summary>
        /// Checks weather the connector can connect, if not it returns:
        /// </summary>
        /// <returns>Int: 0=Connectivity ok, 1=No connector choosen, 2=Invalid/unreachable URL, 3=Incorrect username/password, 4=Other error</returns>
        public int CheckConnectivity(String connector, String url, String username, String password)
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().Name.Equals(connector))
                    return item.CheckConnectivity(connector, url, username, password);
            }
            MessageBox.Show("Error: No connector choosen!");
            return 1;
        }

        /// <summary>
        /// Return all available connectors
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns>List with the names of all available connectors</returns>
        public List<String> GetAvailableConnectors()
        {
            List<String> availableConnectors = new List<String>();
            foreach (var item in MefCalendarConnectors)
            {
                availableConnectors.Add(item.ConnectorName);
            }
            return availableConnectors;
        }

        /// <summary>
        /// Choose a connector to use for sync methods
        /// </summary>
        /// <param name="_choosenConnector">Name of connector to choose</param>
        public void ChooseConnector(String _choosenConnector)
        {
            choosenConnector = _choosenConnector;
        }

        /// <summary>
        /// Selects the choosen connector and executes its GetUpdates(DateTime timestamp) method with choosen connector.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public Shared.AppointmentSyncCollection GetInitialSync()
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().Name.Equals(choosenConnector))
                    return item.GetInitialSync();
            }
            return null;
        }

        /// <summary>
        /// Selects the choosen connector and executes its GetUpdates() method with choosen connector.
        /// </summary>
        /// <returns></returns>
        public Shared.AppointmentSyncCollection GetUpdates()
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().Name.Equals(choosenConnector))
                    return item.GetUpdates();
            }
            return null;
        }

        /// <summary>
        /// Selects the choosen connector and executes its DoUpdates(AppointmentSyncCollection syncItems) method with choosen connector.
        /// </summary>
        /// <param name="syncItems"></param>
        public Dictionary<string, string> DoUpdates(Shared.AppointmentSyncCollection syncItems)
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().Name.Equals(choosenConnector))
                    return item.DoUpdates(syncItems);
            }
            return null;
        }

        public string ConnectorName
        {
            get { return choosenConnector; }
        }

        public Shared.ConnectorSettings Settings
        {
            set {
                foreach (var item in MefCalendarConnectors)
                {
                    if (item.GetType().Name.Equals(choosenConnector))
                    item.Settings = value;
                }
            }
        }
    }
}

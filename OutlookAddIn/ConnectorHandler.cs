using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddIn
{
    public class ConnectorHandler : ICalendarSyncable

    {
        private String choosenConnector;
        private String path = @".\Connectors";
        [ImportMany(typeof(ICalendarSyncable))]
        public List<ICalendarSyncable> MefCalendarConnectors { get; set; }

        public ConnectorHandler(String _choosenConnector)
        {
            choosenConnector = _choosenConnector;
            var catalog = new DirectoryCatalog(path);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
        /// <summary>
        /// Selects the choosen connector and executes its GetUpdates(DateTime timestamp) method.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public Shared.AppointmentSyncCollection GetUpdates(DateTime timestamp)
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().ToString().Equals(choosenConnector))
                {
                    return item.GetUpdates(timestamp);
                }
            }
            return new Shared.AppointmentSyncCollection();
        }

        /// <summary>
        /// Selects the choosen connector and executes its GetUpdates() method.
        /// </summary>
        /// <returns></returns>
        public Shared.AppointmentSyncCollection GetUpdates()
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().ToString().Equals(choosenConnector))
                {
                    return item.GetUpdates();
                }
            }
            return new Shared.AppointmentSyncCollection();
        }

        /// <summary>
        /// Selects the choosen connector and executes its DoUpdates(AppointmentSyncCollection syncItems) method.
        /// </summary>
        /// <param name="syncItems"></param>
        public void DoUpdates(Shared.AppointmentSyncCollection syncItems)
        {
            foreach (var item in MefCalendarConnectors)
            {
                if (item.GetType().ToString().Equals(choosenConnector))
                {
                    item.DoUpdates(syncItems);
                }
            }
        }
    }
}

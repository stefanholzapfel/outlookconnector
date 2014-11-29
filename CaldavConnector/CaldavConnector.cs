using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaldavConnector
{
    [Export(typeof(ICalendarSyncable))]
    public class CaldavConnector : ICalendarSyncable
    {
        public Shared.AppointmentSyncCollection GetUpdates(DateTime timestamp)
        {
            Console.WriteLine("GetUpdates CalDav executed at: " + timestamp.ToString() + " from: " + this.GetType().Name);
            return new Shared.AppointmentSyncCollection();
        }

        public Shared.AppointmentSyncCollection GetUpdates()
        {
            Console.WriteLine("Get updates CalDav executed from: " + this.GetType().Name);
            return new Shared.AppointmentSyncCollection();
        }

        public void DoUpdates(Shared.AppointmentSyncCollection syncItems)
        {
            Console.WriteLine("Do updates CalDav executed from: " + this.GetType().Name);
        }
    }
}

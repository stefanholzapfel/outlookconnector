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
            throw new NotImplementedException();
        }

        public Shared.AppointmentSyncCollection GetUpdates()
        {
            throw new NotImplementedException();
        }

        public void DoUpdates(Shared.AppointmentSyncCollection syncItems)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class AppointmentSyncCollection
    {
        /// <summary>
        /// List of appointments that should be added
        /// </summary>
        public List<OutlookAppointment> AddList { get; set; }

        /// <summary>
        /// List of appointments that should be updated
        /// </summary>
        public List<OutlookAppointment> UpdateList { get; set; }

        /// <summary>
        /// List of appointments that should be deleted
        /// </summary>
        public List<OutlookAppointment> DeleteList { get; set; }

        public AppointmentSyncCollection()
        {
            AddList = new List<OutlookAppointment>();
            UpdateList = new List<OutlookAppointment>();
            DeleteList = new List<OutlookAppointment>();
        }
    }
}

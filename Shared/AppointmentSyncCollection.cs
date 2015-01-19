using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// This class holds all collections for adding, deleting and updating appointments between the calendars
    /// </summary>
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

        /// <summary>
        /// Instantiates all collections
        /// </summary>
        public AppointmentSyncCollection()
        {
            AddList = new List<OutlookAppointment>();
            UpdateList = new List<OutlookAppointment>();
            DeleteList = new List<OutlookAppointment>();
        }
    }
}

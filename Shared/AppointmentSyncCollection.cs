using Microsoft.Office.Interop.Outlook;
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
        /// list of appointments that should be added
        /// </summary>
        public List<AppointmentItem> AddList { get; set; }

        /// <summary>
        /// list of appointments that should be updated
        /// </summary>
        public List<AppointmentItem> UpdateList { get; set; }

        /// <summary>
        /// list of appointments that should be deleted
        /// </summary>
        public List<AppointmentItem> DeleteList { get; set; }

        public void AddAppointment(AppointmentItem item)
        {
            if (AddList == null) AddList = new List<AppointmentItem>();
            AddList.Add(item);
        }

        public void UpdateAppointment(AppointmentItem item)
        {
            if (UpdateList == null) UpdateList = new List<AppointmentItem>();
            UpdateList.Add(item);
        }

        public void DeleteAppointment(AppointmentItem item)
        {
            if (DeleteList == null) DeleteList = new List<AppointmentItem>();
            DeleteList.Add(item);
        }
    }
}

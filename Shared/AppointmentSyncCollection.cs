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

        /*
        /// <summary>
        /// SyncCollection: Add this item when syncing 
        /// </summary>
        /// <param name="item">appointment to be added</param>
        public void AddAppointment(OutlookAppointment item)
        {
            if (AddList == null) AddList = new List<OutlookAppointment>();
            AddList.Add(item);
        }

        /// <summary>
        ///  SyncCollection: Update this item when syncing 
        /// </summary>
        /// <param name="item">appointment to be updated</param>
        public void UpdateAppointment(OutlookAppointment item)
        {
            if (UpdateList == null) UpdateList = new List<OutlookAppointment>();
            UpdateList.Add(item);
        }

        /// <summary>
        ///  SyncCollection: Delete this item when syncing 
        /// </summary>
        /// <param name="item">appointment to be deleted</param>
        public void DeleteAppointment(OutlookAppointment item)
        {
            if (DeleteList == null) DeleteList = new List<OutlookAppointment>();
            DeleteList.Add(item);
        }
        */
    }
}

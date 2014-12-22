using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SyncLogic
{
    public class SyncService
    {
        private ICalendarSyncable _syncMain;
        private ICalendarSyncable _syncSecondary;
        private Timer _syncThread = new Timer();

        public SyncService(ICalendarSyncable syncMain, ICalendarSyncable syncSecondary, double interval)
        {
            this._syncMain = syncMain;
            this._syncSecondary = syncSecondary;

            SetInterval(interval);

            _syncThread.Elapsed += _syncThread_Elapsed;
        }

        /// <summary>
        /// Sets the interval for the repeating synchronization
        /// </summary>
        /// <param name="interval">interval in milliseconds</param>
        public void SetInterval(double interval)
        {
            if (interval <= 0) return;
            _syncThread.Interval = interval;
        }

        /// <summary>
        /// Starts the repeating synchronization
        /// </summary>
        public void Start()
        {
            if (_syncThread.Interval <= 0) return;
            _syncThread.Start();
        }

        /// <summary>
        /// Stops the synchronization
        /// </summary>
        public void Stop()
        {
            _syncThread.Stop();
        }

        /// <summary>
        /// Executes the synchronization once
        /// </summary>
        public void ExecuteSyncOnce()
        {
            throw new NotImplementedException();
            //Debug.WriteLine("Executed: " + DateTime.Now.Second);
        }

        private void _syncThread_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSyncOnce();
        }

    }
}

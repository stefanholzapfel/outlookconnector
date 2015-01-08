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
        private bool _isStarted = false;
        private bool _isRunning = false;

        /// <summary>
        /// minimum interval time
        /// </summary>
        public const double MIN_INTERVAL = 1000;

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
        /// <returns>true if the interval is within allowed range</returns>
        public bool SetInterval(double interval)
        {
            if (interval < MIN_INTERVAL) return false;

            _syncThread.Interval = interval;
            return true;
        }

        /// <summary>
        /// Starts the continuous synchronization
        /// </summary>
        /// <returns>true if starting was successful</returns>
        public bool Start()
        {
            if (_syncThread.Interval < MIN_INTERVAL) return false;

            _syncThread.Start();
            _isStarted = true;
            return true;
        }

        /// <summary>
        /// Stops the synchronization
        /// </summary>
        public void Stop()
        {
            _syncThread.Stop();
            _isStarted = false;
        }

        /// <summary>
        /// Executes the synchronization once
        /// </summary>
        public void ExecuteOnce()
        {
            // if the timer is already running, it needs to be stopped before the manual sync and then restarted

            if (_isStarted) _syncThread.Stop();
            Synchronize();
            if (_isStarted) _syncThread.Start();
        }

        private void _syncThread_Elapsed(object sender, ElapsedEventArgs e)
        {
            // checking if another sync thread is already running
            if (!_isRunning)
            {
                _isRunning = true;
                Synchronize();
                _isRunning = false;
            }
        }

        /// <summary>
        /// Synchronizes both calendars
        /// </summary>
        private void Synchronize()
        {
            //throw new NotImplementedException();
            Debug.WriteLine("Executing Synchronize() ... (" + DateTime.Now.ToLongTimeString() + ")");
        }

    }
}

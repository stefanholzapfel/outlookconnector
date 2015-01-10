﻿using OutlookAddIn;
using Shared;
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
        private CalendarHandler _syncOutlook;
        private ICalendarSyncable _syncExternal;
        private Timer _syncThread = new Timer();
        private bool _isStarted = false;
        private bool _isRunning = false;

        /// <summary>
        /// minimum interval time
        /// </summary>
        public const double MIN_INTERVAL = 1000;

        public SyncService(CalendarHandler syncOutlook, ICalendarSyncable syncExternal, double interval)
        {
            this._syncOutlook = syncOutlook;
            this._syncExternal = syncExternal;

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
        /// Resets the outlook calendar, pulls a new copy from the external calendar and adds it to outlook calendar.
        /// </summary>
        public bool Reset()
        {
            _syncOutlook.DeleteCustomCalendar();
            _syncOutlook.CreateCustomCalendar();
            _syncOutlook.DoUpdates(_syncExternal.GetInitialSync());
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
            //Get changes since last snyc
            AppointmentSyncCollection _outlookGetUpdates = _syncOutlook.GetUpdates();
            AppointmentSyncCollection _externalGetUpdates = _syncExternal.GetUpdates();

            //Find updating conflicts and solve them
            List<OutlookAppointment> deleteFromOutlookCollection = new List<OutlookAppointment>();
            List<OutlookAppointment> deleteFromExternalCollection = new List<OutlookAppointment>();
            if (_outlookGetUpdates.UpdateList.Count > 0 && _externalGetUpdates.UpdateList.Count > 0)
            {
                foreach (var itemOutlook in _outlookGetUpdates.UpdateList)
                {
                    foreach (var itemExternal in _externalGetUpdates.UpdateList)
                    {
                        if (itemOutlook.SyncID.Equals(itemExternal.SyncID))
                        {
                            int comparison = DateTime.Compare(itemOutlook.LastModificationTime, itemExternal.LastModificationTime);
                            //Item was edited in Outlook prior to the external source or at the same time --> external wins
                            if (comparison <= 0)
                            {
                                deleteFromOutlookCollection.Add(itemOutlook);
                            }
                            //Item was edited in Outlook after last modification on external source --> outlook wins
                            else
                            {
                                deleteFromExternalCollection.Add(itemExternal);
                            }
                        }
                    }
                }
                foreach (var item in deleteFromExternalCollection)
                    _externalGetUpdates.UpdateList.Remove(item);
                foreach (var item in deleteFromOutlookCollection)
                    _outlookGetUpdates.UpdateList.Remove(item);
            }


            //Write the changes to the destinations
            _syncOutlook.DoUpdates(_externalGetUpdates);
            _syncOutlook.UpdateSyncIDs(_syncExternal.DoUpdates(_outlookGetUpdates));
        }
    }
}

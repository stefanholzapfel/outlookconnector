namespace OutlookAddIn
{
    partial class SyncRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public SyncRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btn_CreateCalendar = this.Factory.CreateRibbonButton();
            this.btn_DeleteCalendar = this.Factory.CreateRibbonButton();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.btn_DoUpdatesSet1 = this.Factory.CreateRibbonButton();
            this.btn_DoUpdatesSet2 = this.Factory.CreateRibbonButton();
            this.btn_DoUpdatesSet3 = this.Factory.CreateRibbonButton();
            this.separator3 = this.Factory.CreateRibbonSeparator();
            this.btn_UpdateSyncIDs = this.Factory.CreateRibbonButton();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.btn_IncrGetUpdates = this.Factory.CreateRibbonButton();
            this.btn_FullGetUpdates = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.btn_DoManualSync = this.Factory.CreateRibbonButton();
            this.btn_StartSync = this.Factory.CreateRibbonButton();
            this.btn_StopSync = this.Factory.CreateRibbonButton();
            this.btn_ChangeInterval = this.Factory.CreateRibbonButton();
            this.btn_Reset = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.btn_Settings = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Label = "Sync";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btn_CreateCalendar);
            this.group1.Items.Add(this.btn_DeleteCalendar);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.btn_DoUpdatesSet1);
            this.group1.Items.Add(this.btn_DoUpdatesSet2);
            this.group1.Items.Add(this.btn_DoUpdatesSet3);
            this.group1.Items.Add(this.separator3);
            this.group1.Items.Add(this.btn_UpdateSyncIDs);
            this.group1.Items.Add(this.separator2);
            this.group1.Items.Add(this.btn_IncrGetUpdates);
            this.group1.Items.Add(this.btn_FullGetUpdates);
            this.group1.Label = "Test CalendarHandler";
            this.group1.Name = "group1";
            // 
            // btn_CreateCalendar
            // 
            this.btn_CreateCalendar.Label = "Create Calendar";
            this.btn_CreateCalendar.Name = "btn_CreateCalendar";
            this.btn_CreateCalendar.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_CreateCalendar_Click);
            // 
            // btn_DeleteCalendar
            // 
            this.btn_DeleteCalendar.Label = "Delete Calendar";
            this.btn_DeleteCalendar.Name = "btn_DeleteCalendar";
            this.btn_DeleteCalendar.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_DeleteCalendar_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // btn_DoUpdatesSet1
            // 
            this.btn_DoUpdatesSet1.Label = "DoUpdates (Add)";
            this.btn_DoUpdatesSet1.Name = "btn_DoUpdatesSet1";
            this.btn_DoUpdatesSet1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_DoUpdatesSet1_Click);
            // 
            // btn_DoUpdatesSet2
            // 
            this.btn_DoUpdatesSet2.Label = "DoUpdates (Update)";
            this.btn_DoUpdatesSet2.Name = "btn_DoUpdatesSet2";
            this.btn_DoUpdatesSet2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_DoUpdatesSet2_Click);
            // 
            // btn_DoUpdatesSet3
            // 
            this.btn_DoUpdatesSet3.Label = "DoUpdates (Delete)";
            this.btn_DoUpdatesSet3.Name = "btn_DoUpdatesSet3";
            this.btn_DoUpdatesSet3.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_DoUpdatesSet3_Click);
            // 
            // separator3
            // 
            this.separator3.Name = "separator3";
            // 
            // btn_UpdateSyncIDs
            // 
            this.btn_UpdateSyncIDs.Label = "Update SyncIDs";
            this.btn_UpdateSyncIDs.Name = "btn_UpdateSyncIDs";
            this.btn_UpdateSyncIDs.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_UpdateSyncIDs_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // btn_IncrGetUpdates
            // 
            this.btn_IncrGetUpdates.Label = "Incremental GetUpdates";
            this.btn_IncrGetUpdates.Name = "btn_IncrGetUpdates";
            this.btn_IncrGetUpdates.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_IncrGetUpdates_Click);
            // 
            // btn_FullGetUpdates
            // 
            this.btn_FullGetUpdates.Label = "Full GetUpdates";
            this.btn_FullGetUpdates.Name = "btn_FullGetUpdates";
            this.btn_FullGetUpdates.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_FullGetUpdates_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.btn_DoManualSync);
            this.group2.Items.Add(this.btn_StartSync);
            this.group2.Items.Add(this.btn_StopSync);
            this.group2.Items.Add(this.btn_ChangeInterval);
            this.group2.Items.Add(this.btn_Reset);
            this.group2.Label = "Test SyncLogic";
            this.group2.Name = "group2";
            // 
            // btn_DoManualSync
            // 
            this.btn_DoManualSync.Label = "Execute manual sync";
            this.btn_DoManualSync.Name = "btn_DoManualSync";
            this.btn_DoManualSync.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_DoManualSync_Click);
            // 
            // btn_StartSync
            // 
            this.btn_StartSync.Label = "Start Sync";
            this.btn_StartSync.Name = "btn_StartSync";
            this.btn_StartSync.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_StartSync_Click);
            // 
            // btn_StopSync
            // 
            this.btn_StopSync.Label = "Stop Sync";
            this.btn_StopSync.Name = "btn_StopSync";
            this.btn_StopSync.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_StopSync_Click);
            // 
            // btn_ChangeInterval
            // 
            this.btn_ChangeInterval.Label = "Change interval";
            this.btn_ChangeInterval.Name = "btn_ChangeInterval";
            this.btn_ChangeInterval.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_ChangeInterval_Click);
            // 
            // btn_Reset
            // 
            this.btn_Reset.Label = "Reset";
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Reset_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.btn_Settings);
            this.group3.Label = "Settings";
            this.group3.Name = "group3";
            // 
            // btn_Settings
            // 
            this.btn_Settings.Label = "Settings";
            this.btn_Settings.Name = "btn_Settings";
            this.btn_Settings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Settings_Click);
            // 
            // SyncRibbon
            // 
            this.Name = "SyncRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.SyncRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_CreateCalendar;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DeleteCalendar;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_FullGetUpdates;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_IncrGetUpdates;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DoUpdatesSet1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DoUpdatesSet2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DoUpdatesSet3;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_UpdateSyncIDs;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DoManualSync;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_StartSync;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_StopSync;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_ChangeInterval;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Settings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Reset;
    }

    partial class ThisRibbonCollection
    {
        internal SyncRibbon SyncRibbon
        {
            get { return this.GetRibbon<SyncRibbon>(); }
        }
    }
}

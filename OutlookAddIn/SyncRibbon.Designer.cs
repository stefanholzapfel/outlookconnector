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
            this.group3 = this.Factory.CreateRibbonGroup();
            this.btn_Settings = this.Factory.CreateRibbonButton();
            this.btn_manualSync = this.Factory.CreateRibbonButton();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.label1 = this.Factory.CreateRibbonLabel();
            this.edb_interval = this.Factory.CreateRibbonEditBox();
            this.btn_autosync = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group3.SuspendLayout();
            this.group1.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group3);
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "Sync";
            this.tab1.Name = "tab1";
            // 
            // group3
            // 
            this.group3.Items.Add(this.btn_Settings);
            this.group3.Items.Add(this.btn_manualSync);
            this.group3.Label = "Sync";
            this.group3.Name = "group3";
            // 
            // btn_Settings
            // 
            this.btn_Settings.Image = global::OutlookAddIn.Properties.Resources.file_edit;
            this.btn_Settings.Label = "Settings";
            this.btn_Settings.Name = "btn_Settings";
            this.btn_Settings.ShowImage = true;
            this.btn_Settings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Settings_Click);
            // 
            // btn_manualSync
            // 
            this.btn_manualSync.Image = global::OutlookAddIn.Properties.Resources.Refresh;
            this.btn_manualSync.Label = "Manual sync";
            this.btn_manualSync.Name = "btn_manualSync";
            this.btn_manualSync.ShowImage = true;
            this.btn_manualSync.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_manualSync_Click);
            // 
            // group1
            // 
            this.group1.Items.Add(this.label1);
            this.group1.Items.Add(this.edb_interval);
            this.group1.Items.Add(this.btn_autosync);
            this.group1.Label = "Auto Sync";
            this.group1.Name = "group1";
            // 
            // label1
            // 
            this.label1.Label = "Interval in Seconds";
            this.label1.Name = "label1";
            // 
            // edb_interval
            // 
            this.edb_interval.Label = "in seconds";
            this.edb_interval.Name = "edb_interval";
            this.edb_interval.ShowLabel = false;
            this.edb_interval.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.edb_interval_TextChanged);
            // 
            // btn_autosync
            // 
            this.btn_autosync.Image = global::OutlookAddIn.Properties.Resources.Refresh;
            this.btn_autosync.Label = "Activate";
            this.btn_autosync.Name = "btn_autosync";
            this.btn_autosync.ShowImage = true;
            this.btn_autosync.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_autosync_Click);
            // 
            // SyncRibbon
            // 
            this.Name = "SyncRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.SyncRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Settings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_manualSync;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel label1;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox edb_interval;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_autosync;
    }

    partial class ThisRibbonCollection
    {
        internal SyncRibbon SyncRibbon
        {
            get { return this.GetRibbon<SyncRibbon>(); }
        }
    }
}

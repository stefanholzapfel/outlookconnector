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
            this.btn_CreateAppointment = this.Factory.CreateRibbonButton();
            this.btn_DeleteAppointment = this.Factory.CreateRibbonButton();
            this.btn_UpdateAppointment = this.Factory.CreateRibbonButton();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.btn_FullGetUpdates = this.Factory.CreateRibbonButton();
            this.btn_IncrGetUpdates = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "Sync";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btn_CreateCalendar);
            this.group1.Items.Add(this.btn_DeleteCalendar);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.btn_CreateAppointment);
            this.group1.Items.Add(this.btn_UpdateAppointment);
            this.group1.Items.Add(this.btn_DeleteAppointment);
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
            // btn_CreateAppointment
            // 
            this.btn_CreateAppointment.Label = "Create Test Appointment";
            this.btn_CreateAppointment.Name = "btn_CreateAppointment";
            this.btn_CreateAppointment.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_CreateAppointment_Click);
            // 
            // btn_DeleteAppointment
            // 
            this.btn_DeleteAppointment.Label = "Delete Test Appointment";
            this.btn_DeleteAppointment.Name = "btn_DeleteAppointment";
            this.btn_DeleteAppointment.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_DeleteAppointment_Click);
            // 
            // btn_UpdateAppointment
            // 
            this.btn_UpdateAppointment.Label = "Update Test Appointment";
            this.btn_UpdateAppointment.Name = "btn_UpdateAppointment";
            this.btn_UpdateAppointment.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_UpdateAppointment_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // btn_FullGetUpdates
            // 
            this.btn_FullGetUpdates.Label = "Full GetUpdates";
            this.btn_FullGetUpdates.Name = "btn_FullGetUpdates";
            this.btn_FullGetUpdates.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_FullGetUpdates_Click);
            // 
            // btn_IncrGetUpdates
            // 
            this.btn_IncrGetUpdates.Label = "Incremental GetUpdates";
            this.btn_IncrGetUpdates.Name = "btn_IncrGetUpdates";
            this.btn_IncrGetUpdates.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_IncrGetUpdates_Click);
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

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_CreateCalendar;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DeleteCalendar;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_CreateAppointment;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_DeleteAppointment;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_UpdateAppointment;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_FullGetUpdates;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_IncrGetUpdates;
    }

    partial class ThisRibbonCollection
    {
        internal SyncRibbon SyncRibbon
        {
            get { return this.GetRibbon<SyncRibbon>(); }
        }
    }
}

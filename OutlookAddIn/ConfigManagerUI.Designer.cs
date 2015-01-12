namespace OutlookAddIn
{
    partial class ConfigManagerUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbo_Connector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_CalendarName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Username = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_URL = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_UpdateInterval = new System.Windows.Forms.TextBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.check_autosync = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connector";
            // 
            // cbo_Connector
            // 
            this.cbo_Connector.FormattingEnabled = true;
            this.cbo_Connector.Location = new System.Drawing.Point(133, 24);
            this.cbo_Connector.Name = "cbo_Connector";
            this.cbo_Connector.Size = new System.Drawing.Size(195, 21);
            this.cbo_Connector.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Calendar Name";
            // 
            // txt_CalendarName
            // 
            this.txt_CalendarName.Location = new System.Drawing.Point(133, 52);
            this.txt_CalendarName.Name = "txt_CalendarName";
            this.txt_CalendarName.Size = new System.Drawing.Size(195, 20);
            this.txt_CalendarName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // txt_Username
            // 
            this.txt_Username.Location = new System.Drawing.Point(133, 79);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.Size = new System.Drawing.Size(195, 20);
            this.txt_Username.TabIndex = 5;
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(133, 106);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(195, 20);
            this.txt_Password.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "URL";
            // 
            // txt_URL
            // 
            this.txt_URL.Location = new System.Drawing.Point(133, 132);
            this.txt_URL.Name = "txt_URL";
            this.txt_URL.Size = new System.Drawing.Size(195, 20);
            this.txt_URL.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Update Interval";
            // 
            // txt_UpdateInterval
            // 
            this.txt_UpdateInterval.Location = new System.Drawing.Point(133, 159);
            this.txt_UpdateInterval.Name = "txt_UpdateInterval";
            this.txt_UpdateInterval.Size = new System.Drawing.Size(82, 20);
            this.txt_UpdateInterval.TabIndex = 12;
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(133, 200);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(90, 23);
            this.btn_Reset.TabIndex = 13;
            this.btn_Reset.Text = "Reset Sync";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(238, 200);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(90, 23);
            this.btn_Save.TabIndex = 14;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(221, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "sec";
            // 
            // check_autosync
            // 
            this.check_autosync.AutoSize = true;
            this.check_autosync.Location = new System.Drawing.Point(258, 160);
            this.check_autosync.Name = "check_autosync";
            this.check_autosync.Size = new System.Drawing.Size(70, 17);
            this.check_autosync.TabIndex = 17;
            this.check_autosync.Text = "Autosync";
            this.check_autosync.UseVisualStyleBackColor = true;
            // 
            // ConfigManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(408, 275);
            this.Controls.Add(this.check_autosync);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.txt_UpdateInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_URL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_Username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_CalendarName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbo_Connector);
            this.Controls.Add(this.label1);
            this.Name = "ConfigManagerUI";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbo_Connector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_CalendarName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Username;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_URL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_UpdateInterval;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox check_autosync;
    }
}
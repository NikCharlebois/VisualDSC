namespace VisualDSC
{
    partial class AzureManager
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
            this.picMain = new System.Windows.Forms.PictureBox();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.ddlAutomationAccount = new System.Windows.Forms.ComboBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelLogin = new System.Windows.Forms.Panel();
            this.lblLoginMessage = new System.Windows.Forms.Label();
            this.lblAzureHeader = new System.Windows.Forms.Label();
            this.lblAzureAutomation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.panelLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // picMain
            // 
            this.picMain.Image = global::ExtendedVisioAddin1.Properties.Resources.CloudLoading;
            this.picMain.Location = new System.Drawing.Point(215, 74);
            this.picMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(335, 304);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.Visible = false;
            // 
            // lblClose
            // 
            this.lblClose.AutoSize = true;
            this.lblClose.BackColor = System.Drawing.Color.DimGray;
            this.lblClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(719, 1);
            this.lblClose.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(36, 36);
            this.lblClose.TabIndex = 1;
            this.lblClose.Text = "X";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // lblDetails
            // 
            this.lblDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(78, 343);
            this.lblDetails.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(496, 100);
            this.lblDetails.TabIndex = 5;
            this.lblDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ddlAutomationAccount
            // 
            this.ddlAutomationAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlAutomationAccount.FormattingEnabled = true;
            this.ddlAutomationAccount.Location = new System.Drawing.Point(163, 129);
            this.ddlAutomationAccount.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ddlAutomationAccount.Name = "ddlAutomationAccount";
            this.ddlAutomationAccount.Size = new System.Drawing.Size(396, 45);
            this.ddlAutomationAccount.TabIndex = 6;
            this.ddlAutomationAccount.Visible = false;
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.ForeColor = System.Drawing.Color.White;
            this.btnSelect.Location = new System.Drawing.Point(417, 179);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(142, 58);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.ForeColor = System.Drawing.Color.Silver;
            this.txtUsername.Location = new System.Drawing.Point(82, 99);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(477, 44);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Text = "Email address";
            this.txtUsername.Enter += new System.EventHandler(this.txtUsername_Enter);
            this.txtUsername.Leave += new System.EventHandler(this.txtUsername_Leave);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Silver;
            this.txtPassword.Location = new System.Drawing.Point(82, 179);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(477, 44);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "Password";
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(417, 241);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(142, 56);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Connect";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.DimGray;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, -2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(755, 45);
            this.lblTitle.TabIndex = 13;
            this.lblTitle.Text = "    Connect to your Azure Account";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.lblAzureAutomation);
            this.panelLogin.Controls.Add(this.lblLoginMessage);
            this.panelLogin.Controls.Add(this.ddlAutomationAccount);
            this.panelLogin.Controls.Add(this.btnSelect);
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.txtUsername);
            this.panelLogin.Controls.Add(this.txtPassword);
            this.panelLogin.Controls.Add(this.lblDetails);
            this.panelLogin.Controls.Add(this.picMain);
            this.panelLogin.Location = new System.Drawing.Point(50, 100);
            this.panelLogin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(650, 509);
            this.panelLogin.TabIndex = 14;
            // 
            // lblLoginMessage
            // 
            this.lblLoginMessage.AutoSize = true;
            this.lblLoginMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginMessage.ForeColor = System.Drawing.Color.Black;
            this.lblLoginMessage.Location = new System.Drawing.Point(24, 11);
            this.lblLoginMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLoginMessage.Name = "lblLoginMessage";
            this.lblLoginMessage.Size = new System.Drawing.Size(624, 31);
            this.lblLoginMessage.TabIndex = 13;
            this.lblLoginMessage.Text = "Professional, school or personal Microsoft Account";
            // 
            // lblAzureHeader
            // 
            this.lblAzureHeader.AutoSize = true;
            this.lblAzureHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzureHeader.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblAzureHeader.Location = new System.Drawing.Point(64, 60);
            this.lblAzureHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAzureHeader.Name = "lblAzureHeader";
            this.lblAzureHeader.Size = new System.Drawing.Size(337, 51);
            this.lblAzureHeader.TabIndex = 13;
            this.lblAzureHeader.Text = "Microsoft Azure";
            // 
            // lblAzureAutomation
            // 
            this.lblAzureAutomation.AutoSize = true;
            this.lblAzureAutomation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzureAutomation.ForeColor = System.Drawing.Color.Black;
            this.lblAzureAutomation.Location = new System.Drawing.Point(17, 11);
            this.lblAzureAutomation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAzureAutomation.Name = "lblAzureAutomation";
            this.lblAzureAutomation.Size = new System.Drawing.Size(450, 31);
            this.lblAzureAutomation.TabIndex = 14;
            this.lblAzureAutomation.Text = "Microsoft Azure Automation account";
            this.lblAzureAutomation.Visible = false;
            // 
            // AzureManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(756, 620);
            this.Controls.Add(this.lblAzureHeader);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panelLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AzureManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Azure Manager";
            this.Load += new System.EventHandler(this.AzureManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.ComboBox ddlAutomationAccount;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Label lblLoginMessage;
        private System.Windows.Forms.Label lblAzureHeader;
        private System.Windows.Forms.Label lblAzureAutomation;
    }
}
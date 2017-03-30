using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Windows.Forms;

namespace VisualDSC
{
    public partial class AzureManager : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        PowerShell posh;
        PSDataCollection<PSObject> outputStack, outputStackImport, outputStackCompile;
        Dictionary<string,string> resourceGroupName;
        string selectedAutomationAccount = "", selectedResourceGroupName = "";
        public AzureManager()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AzureManager_Load(object sender, EventArgs e)
        {
            /*this.Width = 522;
            this.Height = 471;
            picMain.Width = 282;
            picMain.Height = 283;
            picMain.Left = 133;
            picMain.Top = 77;
            lblClose.Left = 474;
            lblClose.Top = 9;
            lblClose.Width = 36;
            lblClose.Height = 36;
            ddlAutomationAccount.Left = 149;
            ddlAutomationAccount.Top = 164;
            btnLogin.Top = 258;
            btnLogin.Left = 356;*/
        }

        private void ConnectToAzureRMAccount()
        {
            posh = PowerShell.Create();
            posh.AddScript("$verbosepreference='continue';");
            posh.AddScript("$azureAccountName =\"" + txtUsername.Text + "\"; $azurePassword = ConvertTo-SecureString \"" + txtPassword.Text + "\" -AsPlainText -Force; $psCred = New-Object System.Management.Automation.PSCredential($azureAccountName, $azurePassword); Login-AzureRmAccount -Credential $psCred -Verbose;Get-AzureRMAutomationAccount -Verbose");
            
            var verboseStack = new PSDataCollection<PSObject>();
            posh.Streams.Verbose.DataAdded += VerboseStack_DataAdded;
            outputStack = new PSDataCollection<PSObject>();
            var errorStack = new PSDataCollection<PSObject>();
            posh.Streams.Error.DataAdded += Error_DataAdded;

            IAsyncResult asyncResult = posh.BeginInvoke<PSObject, PSObject>(null, outputStack, null, AsyncCallback, null);
        }
        public void AsyncCallback(IAsyncResult ar)
        {
            resourceGroupName = new Dictionary<string, string>();
            if (ar.IsCompleted)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    lblAzureAutomation.Visible = true;
                    ddlAutomationAccount.Visible = true;
                    for (int i = 1; i < outputStack.Count; i++)
                    {
                        ddlAutomationAccount.Items.Add(((Microsoft.Azure.Commands.Automation.Model.AutomationAccount)(outputStack[i].BaseObject)).AutomationAccountName);
                        resourceGroupName.Add(((Microsoft.Azure.Commands.Automation.Model.AutomationAccount)(outputStack[i].BaseObject)).AutomationAccountName, ((Microsoft.Azure.Commands.Automation.Model.AutomationAccount)(outputStack[i].BaseObject)).ResourceGroupName);
                    }
                    picMain.Visible = false;
                    btnSelect.Visible = true;
                    lblTitle.Text = "    Select your Azure Automation Account";
                    lblDetails.Text = "";
                }), null);
            }
        }

        public void AsyncCallbackImport(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    PasswordManager passManager = new VisualDSC.PasswordManager(ref posh, "SPFarm", ddlAutomationAccount.SelectedItem.ToString(), resourceGroupName[ddlAutomationAccount.SelectedItem.ToString()], SendCompileRequest);
                    passManager.Show();
                    
                }), null);
            }
        }
        public void SendCompileRequest()
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                lblDetails.Text = "DSC Node Configuration was successfully uploaded to Azure.";
                ddlAutomationAccount.Enabled = false;
            }), null);
            posh.Commands.Clear();
            posh.AddScript("$verbosepreference='continue';");
            posh.AddScript("Start-AzureRmAutomationDscCompilationJob -ResourceGroupName " + selectedResourceGroupName + " –AutomationAccountName " + selectedAutomationAccount + " -ConfigurationName SharePointFarm -Verbose");
            
            var verboseStack = new PSDataCollection<PSObject>();
            posh.Streams.Verbose.DataAdded += VerboseStack_DataAdded;
            outputStackCompile = new PSDataCollection<PSObject>();
            var errorStack = new PSDataCollection<PSObject>();
            posh.Streams.Error.DataAdded += Error_DataAdded;            
            IAsyncResult asyncResult = posh.BeginInvoke<PSObject, PSObject>(null, outputStackCompile, null, AsyncCallbackCompile, null);
        }

        public void AsyncCallbackCompile(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    lblDetails.Text = "Your DSC configuration was successfully compiled in Azure.";
                    picMain.Image = ExtendedVisioAddin1.Properties.Resources.completed;
                    //picMain.Visible = false;
                }), null);
            }
        }
        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            bool ignoreError = false;
            foreach (var i in (PSDataCollection<ErrorRecord>)sender)
            {
                ignoreError = false;
                
                if (((ErrorRecord)i).ToString().StartsWith("The term 'Login-AzureRmAccount' is not recognized"))
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        lblDetails.Text = "The PowerShell module AzureRM was not found. Please open a new PowerShell window as an administrator and run \"Install-Module AzureRM\".";
                    }), null);
                }
                else if(((ErrorRecord)i).ToString().StartsWith("Sequence contains no elements"))
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        lblDetails.Text = "You cannot use a live ID to connect. Please use an Azure account that uses Azure AD.";
                    }), null);
                }
                else if(((ErrorRecord)i).ToString().StartsWith("AADSTS50079: The user is required to use multi-factor"))
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        lblDetails.Text = "VisualDSC does not support using an account that has Multi-Factor Authentication turned on. Please use another account to connect.";
                    }), null);
                }
                else if(((ErrorRecord)i).ToString().StartsWith("user_realm_discovery_failed: User realm discovery failed: The remote name could not be resolved"))
                {
                        synchronizationContext.Post(new SendOrPostCallback(o =>
                        {
                            lblDetails.Text = "Please make sure you are connected to the Internet";
                        }), null);
                }
                else if(((ErrorRecord)i).CategoryInfo.ToString() == "CloseError: (:) [New-AzureRmAutomationCredential], FormatException")
                {
                    ignoreError = true;
                    SendCompileRequest();
                }
                else
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        lblDetails.Text = ((ErrorRecord)i).ToString();
                    }), null);
                }
                if (ignoreError == false)
                {
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        picMain.Image = ExtendedVisioAddin1.Properties.Resources.error;
                    }), null);
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            picMain.Visible = true;
            //panelLogin.Visible = false;
            txtPassword.Visible = false;
            txtUsername.Visible = false;
            btnLogin.Visible = false;
            //btnSelect.Visible = true;
            //ddlAutomationAccount.Visible = true;
            lblAzureAutomation.Visible = true;
            lblLoginMessage.Visible = false;
            ConnectToAzureRMAccount();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, new EventArgs());
            }
        }
        
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Email address" && txtUsername.ForeColor == Color.Silver)
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password" && txtPassword.ForeColor == Color.Silver)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if(txtUsername.Text == "")
            {
                txtUsername.Text = "Email address";
                txtUsername.ForeColor = Color.Silver;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = Color.Silver;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void VerboseStack_DataAdded(object sender, DataAddedEventArgs e)
        {
            foreach (var i in (PSDataCollection<VerboseRecord>)sender)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    lblDetails.Text = ((VerboseRecord)i).Message.Replace("  ", "");
                }), null);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            lblAzureAutomation.Visible = false;
            posh.Commands.Clear();
            posh.AddScript("$verbosepreference='continue';");
            posh.AddScript("Import-AzureRmAutomationDscConfiguration -SourcePath C:\\temp\\SharePointFarm.ps1 -ResourceGroupName '" + resourceGroupName[ddlAutomationAccount.SelectedItem.ToString()] + "' -AutomationAccountName '" + ddlAutomationAccount.SelectedItem + "' -Verbose -Published -Force");
            ddlAutomationAccount.Visible = false;
            btnSelect.Visible = false;
            picMain.Visible = true;
            var verboseStack = new PSDataCollection<PSObject>();
            posh.Streams.Verbose.DataAdded += VerboseStack_DataAdded;
            outputStackImport = new PSDataCollection<PSObject>();
            var errorStack = new PSDataCollection<PSObject>();
            posh.Streams.Error.DataAdded += Error_DataAdded;

            selectedAutomationAccount = ddlAutomationAccount.SelectedItem.ToString();
            selectedResourceGroupName = resourceGroupName[ddlAutomationAccount.SelectedItem.ToString()];

            IAsyncResult asyncResult = posh.BeginInvoke<PSObject, PSObject>(null, outputStackImport, null, AsyncCallbackImport, null);
        }
    }
}

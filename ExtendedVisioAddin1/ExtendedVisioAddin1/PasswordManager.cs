using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;

namespace VisualDSC
{
    public partial class PasswordManager : Form
    {
        PSDataCollection<PSObject> outputStack;
        string _automationAccount = "";
        string _resourceGroup = "";
        PowerShell _azureSession = null;
        Action _callBack = null;
        public PasswordManager(ref PowerShell azureSession, string credentialName, string automationAccount, string resourceGroup, Action callback)
        {
            InitializeComponent();
            lblTitle.Text = credentialName;
            this._automationAccount = automationAccount;
            this._resourceGroup = resourceGroup;
            this._azureSession = azureSession;
            this._callBack = callback;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _azureSession.Commands.Clear();
            _azureSession.AddScript("$verbosepreference='continue';");
            _azureSession.AddScript("$userName =\"" + txtUserName.Text + "\";");
            _azureSession.AddScript("$password = ConvertTo-SecureString \"" + txtPassword.Text + "\" -AsPlainText -Force;");
            _azureSession.AddScript("$cred = New-Object –TypeName System.Management.Automation.PSCredential –ArgumentList $userName, $password;");
            _azureSession.AddScript("New-AzureRMAutomationCredential -AutomationAccountName \"" + _automationAccount + "\" -ResourceGroupName \"" + _resourceGroup + "\" -Name \"" + lblTitle.Text + "\" -Value $cred;");
            
            outputStack = new PSDataCollection<PSObject>();
            IAsyncResult asyncResult = _azureSession.BeginInvoke<PSObject, PSObject>(null, outputStack, null, AsyncCallbackSetCreds, null);
            this.Hide();
        }

        public void AsyncCallbackSetCreds(IAsyncResult ar)
        {
            if(ar.IsCompleted)
            {
                if (this._callBack != null)
                    this._callBack();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace VisualDSC.DeploymentManager
{
    public partial class DeploymentManagerForm : Form
    {
        public DeploymentManagerForm()
        {
            InitializeComponent();
            this.Show();
            GenerateMOF();
        }

        private void GenerateMOF()
        {
            System.Security.SecureString password = new System.Security.SecureString();
            password.AppendChar('p');
            password.AppendChar('a');
            password.AppendChar('s');
            password.AppendChar('s');
            password.AppendChar('@');
            password.AppendChar('w');
            password.AppendChar('o');
            password.AppendChar('r');
            password.AppendChar('d');
            password.AppendChar('1');
            PSCredential creds = new PSCredential("contoso\\sp_farm", password);
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                lblDebug.Text += "Initializing PowerShell Session...\r\n";
                PowerShellInstance.AddScript("cd C:\\temp; c:\\temp\\tempDSC.ps1");
                PowerShellInstance.AddParameter("farmAccountCreds", creds);
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                foreach (PSObject outputItem in PSOutput)
                {
                    if (outputItem != null)
                        lblDebug.Text += outputItem.BaseObject.ToString() + "...\r\n";
                }
                lblDebug.Text += "MOF Files Generated!\r\n";
                PowerShellInstance.AddScript("Start-DSCConfiguration SharePointFarm -Force");
                PSOutput = PowerShellInstance.Invoke();
                foreach (PSObject outputItem in PSOutput)
                {
                    if (outputItem != null)
                        lblDebug.Text += outputItem.BaseObject.ToString() + "...\r\n";
                }
            }
            MessageBox.Show("The Topology was successfully deployed locally");
        }
    }
}

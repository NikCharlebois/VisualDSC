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
    public partial class DeploymentManager : Form
    {
        System.Windows.Forms.Form currentForm;
        private readonly SynchronizationContext synchronizationContext;
        PowerShell posh;
        public DeploymentManager()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            currentForm = this;
            currentForm.Width = 1010;
            currentForm.Height = 570;
            picStatus.Width = 995;
            picStatus.Height = 339;
            picStatus.Top = 11;
            lblClose.Left = 962;
            lblDetails.Top = 417;
            lblDetails.Width = 995;
            lblDetails.Height = 144;
            lblStatus.Height = 67;
            lblStatus.Width = 995;
            lblStatus.Top = 350;
            panel1.Width = 1010;
            panel1.Height = 570;
            currentForm.Show();
            bgDeployLocally.RunWorkerAsync();
        }
        private void GenerateMOF()
        {
            object status = "";
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript("cd C:\\temp; c:\\temp\\SharePointFarm.ps1");
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                foreach (PSObject outputItem in PSOutput)
                {
                    if (outputItem != null)
                    {
                        status = outputItem.BaseObject.ToString() + "...";
                    }
                }
                status = "Microsoft Operations Framework (MOF) Files Generated";
                bgDeployLocally.ReportProgress(20, status);
                status = "Applying Desired State...";
                bgDeployLocally.ReportProgress(40, status);
                PowerShell posh = PowerShell.Create();
                posh.AddScript("$verbosepreference='continue'");
                posh.AddScript("cd C:\\temp; Start-DSCConfiguration SharePointFarm -Force -Wait -Verbose");

                var verboseStack = new PSDataCollection<PSObject>();
                posh.Streams.Verbose.DataAdded += VerboseStack_DataAdded;

                var errorStack = new PSDataCollection<PSObject>();
                posh.Streams.Error.DataAdded += Error_DataAdded;

                IAsyncResult asyncResult = posh.BeginInvoke<PSObject, PSObject>(null, verboseStack, null, AsyncCallback, null);
            }
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            foreach (var i in (PSDataCollection<ErrorRecord>)sender)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    picStatus.Image = ExtendedVisioAddin1.Properties.Resources.error;
                    lblStatus.Text = "Errors have Occurred";
                    lblDetails.Text = ((ErrorRecord)i).ErrorDetails.Message.Replace("  ", "");
                }), null);
            }

            posh.Stop();
        }

        public void AsyncCallback(IAsyncResult ar)
        {
            if(ar.IsCompleted)
            {
                picStatus.Image = ExtendedVisioAddin1.Properties.Resources.completed;
                string status = "Topology Successfully Deployed";
                bgDeployLocally.ReportProgress(100, status);
                posh.Stop();
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
        
        private void bgDeployLocally_DoWork(object sender, DoWorkEventArgs e)
        {
            GenerateMOF();
        }

        private void bgDeployLocally_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!e.UserState.ToString().StartsWith("***"))
                lblStatus.Text = e.UserState.ToString();
            else
                lblDetails.Text = e.UserState.ToString().Replace("***", "");
        }

        private void lblClose_MouseHover(object sender, EventArgs e)
        {
            lblClose.Font = new Font("Arial", 12, FontStyle.Underline);
            lblClose.BackColor = Color.Silver;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            lblClose.Font = new Font("Arial", 12, FontStyle.Regular);
            lblClose.BackColor = Color.White;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            currentForm.Close();
        }
    }
}

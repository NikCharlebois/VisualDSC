using System;
using Microsoft.Office.Tools.Ribbon;

namespace VisualDSC
{
    public partial class AddinRibbonComponent
    {
        private void buttonExport_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.ExportAndValidate();
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            if(IsAdministrator())
                Globals.ThisAddIn.DeployLocally();
        }
        public static bool IsAdministrator()
        {
            return (new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()))
                    .IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        private void AddinRibbonComponent_Load(object sender, RibbonUIEventArgs e)
        {
            if (!IsAdministrator())
            {
                this.button2.ScreenTip = "You need to run Visio as an administrator to use this feature.";
                this.button2.Enabled = false;
            }
        }

        private void btnInspect_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TogglePanel();
        }

        private void btnAzure_Click(object sender, RibbonControlEventArgs e)
        {
            if (!System.IO.Directory.Exists("C:\\temp\\"))
                System.IO.Directory.CreateDirectory("C:\\temp\\");
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\temp\SharePointFarm.ps1"))
            {
                sw.Write(Globals.ThisAddIn.GetDSCContent());
            }
            AzureManager azureMgr = new VisualDSC.AzureManager();
            azureMgr.Show();
        }

        private void btnReverseDSC_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.OpenFileForReverse();
        }
    }
}

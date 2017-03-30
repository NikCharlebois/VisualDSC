using System;
using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

namespace VisualDSC
{
    public partial class ThisAddIn
    {
        public void ExportAndValidate()
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "DSC Script|*.ps1";

            string dscContent = GetDSCContent();

            saveDlg.ShowDialog();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveDlg.FileName, false))
            {
                sw.Write(dscContent.ToString());
            }
            System.Diagnostics.Process.Start(saveDlg.FileName);
        }

        public void OpenFileForReverse()
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "DSC Script|*.ps1";
            openDlg.Title = "Select a DSC Configuration file to generate diagram from";
            openDlg.ShowDialog();
            ReverseManager reverseMgr = new VisualDSC.ReverseManager(Globals.ThisAddIn.Application.ActiveWindow, Globals.ThisAddIn, openDlg.FileName);
        }

        public string GetDSCContent()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Visio.Application app = this.Application;
            Visio.Document doc = app.ActiveDocument;            
            sb.AppendLine("Configuration SharePointFarm");
            sb.AppendLine("{");
            sb.AppendLine("    Import-DscResource -ModuleName PSDesiredStateConfiguration");
            sb.AppendLine("    Import-DscResource -ModuleName SharePointDSC");
            //sb.AppendLine("    Import-DscResource -ModuleName xActiveDirectory");
            sb.AppendLine("    Import-DscResource -ModuleName xWebAdministration");
            sb.AppendLine("    Import-DscResource -ModuleName xCredSSP");
            sb.AppendLine("    $secPassword = ConvertTo-SecureString \"pass@word1\" -AsPlainText -Force");
            sb.AppendLine("    $farmAccountCreds = New-Object System.Management.Automation.PSCredential(\"contoso\\sp_farm\", $secPassword)");
            sb.AppendLine("    node " + Environment.MachineName);
            sb.AppendLine("    {");
            sb.AppendLine("        xCredSSP CredSSPServer { Ensure = \"Present\"; Role = \"Server\"; }");
            sb.AppendLine("        xCredSSP CredSSPClient { Ensure = \"Present\"; Role = \"Client\"; DelegateComputers = \"*.contoso.com\"; }");
            foreach (Visio.Page page in doc.Pages)
            {
                foreach (Visio.Shape shape in page.Shapes)
                {
                    sb.AppendLine(GetDSCBlockContent(shape));
                }
            }
            sb.AppendLine("        LocalConfigurationManager");
            sb.AppendLine("        {");
            sb.AppendLine("            RebootNodeIfNeeded = $true");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("$ConfigData = @{");
            sb.AppendLine("AllNodes = @(");
            sb.AppendLine("    @{");
            sb.AppendLine("        NodeName = \"" + Environment.MachineName + "\";");
            sb.AppendLine("        PSDscAllowPlainTextPassword = $true;");
            sb.AppendLine("    }");
            sb.AppendLine(")}");
            sb.AppendLine("SharePointFarm -ConfigurationData $ConfigData");
            return sb.ToString();
        }        

        public string GetDSCBlockContent(Visio.Shape shape)
        {
            string dependsOn = "";
            if (shape.Master.Name != "Dynamic connector")
            {
                dependsOn = VisualDSC.Util.ConnectionManager.GetDependsOn(shape);                
            
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.Collections.Hashtable properties = new System.Collections.Hashtable();
                short customProps = (short)Visio.VisSectionIndices.visSectionProp;
                for (short i = 0; i < shape.RowCount[customProps]; i++)
                {
                    string propType = shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsType].FormulaU.Replace("\"", "").ToString();
                    switch (propType)
                    {
                        case "1":
                            if (shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsLabel].FormulaU.Replace("\"", "") == "AuthenticationMethod")
                            {
                                string nik = shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsValue].Row.ToString();
                            }
                            break;
                        case "3":
                            properties.Add(shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsLabel].FormulaU.Replace("\"", ""), "$" + shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsValue].FormulaU.Replace("\"", ""));
                            break;
                        case "0":
                        default:                            
                            properties.Add(shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsLabel].FormulaU.Replace("\"", ""), shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsValue].FormulaU);
                            break;
                    }
                }
                string shapeName = "";
                if (properties.Contains("Name"))
                    shapeName = properties["Name"].ToString();
                else
                    shapeName = Guid.NewGuid().ToString();
                sb.AppendLine("        " + shape.Master.Name + " " + shapeName.Replace(" ", "").Replace("\"", ""));
                sb.AppendLine("        {");
                foreach (System.Collections.DictionaryEntry prop in properties)
                {
                    if(prop.Value.ToString() != "" && prop.Value.ToString() != "$")
                        sb.AppendLine("            " + prop.Key + " = " + prop.Value + "");
                }
                sb.AppendLine("            PsDscRunAsCredential = $farmAccountCreds");
                if (!string.IsNullOrEmpty(dependsOn) && dependsOn != "\"")
                    sb.AppendLine(dependsOn);
                sb.AppendLine("        }");
                return sb.ToString();
            }
            return "";
        }

        private PanelManager _panelManager;
        public bool Shown {get;set;}
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            _panelManager = new PanelManager(this);
            //this.Application.ShapeAdded += Application_ShapeAdded;
        }
        

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {

        }
        public void TogglePanel()
        {
            _panelManager.TogglePanel(Application.ActiveWindow); 
            if (_panelManager.IsPanelOpened(Application.ActiveWindow))
                Shown = true;
            else
                Shown = false;
        }

        public void DeployLocally()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\temp\SharePointFarm.ps1"))
            {
                sw.Write(GetDSCContent());
            }
            DeploymentManager deployManager = new DeploymentManager();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }

    }
}

using System.Windows.Forms;
using System;
using Visio = Microsoft.Office.Interop.Visio;
using System.Threading;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace VisualDSC
{
    public partial class ReverseManager : Form
    {
        Visio.Window _window;
        ThisAddIn _addin;
        string _filePath;
        Form currentForm;
        string contentDSC = "";
        private readonly SynchronizationContext synchronizationContext;
        public enum GraphStyles { TopDown, LeftRight };
        PowerShell posh;
        public ReverseManager(Visio.Window window, ThisAddIn addinInstance, string filePath)
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            this._window = window;
            this._addin = addinInstance;
            this._filePath = filePath;
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

        public void GenerateDiagram()
        {
            using(System.IO.StreamReader sr = new System.IO.StreamReader(_filePath))
            {
                contentDSC = sr.ReadToEnd();
            }

            foreach(Visio.Document doc in _addin.Application.Documents)
            {
                if(doc.Name.EndsWith(".vssx") || doc.Name.EndsWith(".vss"))
                {
                    foreach(Visio.Master master in doc.Masters)
                    {
                        GenerateShape(doc.Name, master.Name);
                    }
                }
            }

            foreach(Visio.Shape shape in _addin.Application.ActivePage.Shapes)
            {
                string hapeName = shape.Text;
                DrawDependencies(shape);
            }

            ArrangeGraph(GraphStyles.TopDown);

            picStatus.Image = ExtendedVisioAddin1.Properties.Resources.completed;
            object status = "Reverse Diagram Successfully Generated";

            bgDeployLocally.ReportProgress(100, status);
            
            currentForm.Close();
        }

        public void ArrangeGraph(GraphStyles Style)
        {
            if (Style == GraphStyles.TopDown)
            {
                // set 'PlaceStyle'
                var placeStyleCell = this._window.Application.ActivePage.PageSheet.get_CellsSRC(
                    (short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowPageLayout,
                    (short)Visio.VisCellIndices.visPLOPlaceStyle).ResultIU = 1;
                // set 'RouteStyle'
                var routeStyleCell = this._window.Application.ActivePage.PageSheet.get_CellsSRC(
                    (short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowPageLayout,
                    (short)Visio.VisCellIndices.visPLORouteStyle).ResultIU = 5;
                // set 'PageShapeSplit'
                var pageShapeSplitCell = this._window.Application.ActivePage.PageSheet.get_CellsSRC(
                    (short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowPageLayout,
                    (short)Visio.VisCellIndices.visPLOSplit).ResultIU = 1;
            }
            else if (Style == GraphStyles.LeftRight)
            {
                // set 'PlaceStyle'
                var placeStyleCell = this._window.Application.ActivePage.PageSheet.get_CellsSRC(
                    (short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowPageLayout,
                    (short)Visio.VisCellIndices.visPLOPlaceStyle).ResultIU = 2;
                // set 'RouteStyle'
                var routeStyleCell = this._window.Application.ActivePage.PageSheet.get_CellsSRC(
                    (short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowPageLayout,
                    (short)Visio.VisCellIndices.visPLORouteStyle).ResultIU = 6;
                // set 'PageShapeSplit'
                var pageShapeSplitCell = this._window.Application.ActivePage.PageSheet.get_CellsSRC(
                    (short)Visio.VisSectionIndices.visSectionObject,
                    (short)Visio.VisRowIndices.visRowPageLayout,
                    (short)Visio.VisCellIndices.visPLOSplit).ResultIU = 1;
            }
            else { throw new NotImplementedException("GraphStyle " + Style.ToString() + " is not supported"); }
            this._window.Application.ActivePage.Layout();
        }

        private void DrawDependencies(Visio.Shape shape)
        {
            string shapeText = shape.Text;
            string shapeName = shape.Master.Name;
            int start = this.contentDSC.IndexOf(shapeName + " " + shapeText), startBracket = -1;
            int end = -1, endBracket = -1;
            string shapeProps = "";
            string[] propLines;
            short customProps = (short)Visio.VisSectionIndices.visSectionProp;

            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                lblStatus.Text = "Linking shapes... ";
                lblDetails.Text = shapeText;
            }), null);
            /*while (start >= 0)
            {*/
            start = start + shapeText.Length;
            end = contentDSC.IndexOf("\r\n", start);

            /* Get all properties for the shape */
            if (end >= 0)
            {
                startBracket = contentDSC.IndexOf("{", end + 2) + 1;
                endBracket = contentDSC.IndexOf("}", end + 2);
                shapeProps = contentDSC.Substring(startBracket, endBracket - startBracket);

                propLines = shapeProps.Split('\n');

                foreach (string prop in propLines)
                {
                    // Nik20161101 - This shape depends on another. Add a dynamic connector and link the two together;
                    if (prop.Split('=')[0].Trim().ToLower() == "dependson")
                    {
                        string[] dependsOn = prop.Split('=')[1].Trim().Replace("\"", "").Split(';');
                        if(dependsOn.Length == 1 && dependsOn[0].Contains(","))
                        {
                            dependsOn = dependsOn[0].Split(',');
                            for (int i = 0; i < dependsOn.Length; i++)
                            {
                                dependsOn[i] = dependsOn[i].Replace("@(", "").Replace("'", "").Replace(")", "");
                            }
                        }
                        Visio.Document basicStencil = _addin.Application.Documents.OpenEx("Basic Flowchart Shapes (US units).vss", (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);
                        Visio.Master connector = basicStencil.Masters.get_ItemU("Dynamic Connector");
                        foreach (string dependency in dependsOn)
                        {
                            if (!string.IsNullOrEmpty(dependency))
                            {
                                Visio.Shape connectorShape = _addin.Application.ActivePage.Drop(connector, 0.0, 0.0);

                                // Connect the begin point. 
                                Visio.Cell endXCell = connectorShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXForm1D, (short)Visio.VisCellIndices.vis1DEndX);
                                endXCell.GlueTo(shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXFormOut, (short)Visio.VisCellIndices.visXFormPinX));

                                Visio.Cell beginXCell = connectorShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXForm1D, (short)Visio.VisCellIndices.vis1DBeginX);

                                // Nik20161101 - Get the type of resource the shape depends on.
                                string dependsOnResourceType = "";
                                int startType = dependency.IndexOf('[') + 1;
                                int endType = -1;
                                if (startType >= 1)
                                {
                                    endType = dependency.IndexOf("]");
                                    if (endType > startType)
                                        dependsOnResourceType = dependency.Substring(startType, endType - startType);
                                }

                                Visio.Document doc = _window.Document;
                                string masterShapeName = "", curPropName = "", curPropValue = "", curShapeText = "";
                                foreach (Visio.Page page in doc.Pages)
                                {
                                    foreach (Visio.Shape curShape in page.Shapes)
                                    {
                                        curShapeText = curShape.Text.Trim();
                                        for (short i = 0; i < curShape.RowCount[customProps]; i++)
                                        {
                                            masterShapeName = curShape.Master.Name;
                                            curPropName = curShape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsLabel].Formula.Replace("\"", "").ToLower();
                                            curPropValue = curShape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsValue].FormulaU.Replace(" ", ""); //TODO - Replace the space trimming logic by something more robust
                                                
                                            if (curShape != shape && masterShapeName == dependsOnResourceType &&  
                                            ((curPropName == "name" && curPropValue == "\"" + dependency.Split(']')[1] + "\"") ||
                                            curShapeText == dependency.Split(']')[1].Split(',')[0].Replace("'", "")))
                                            {
                                                beginXCell.GlueTo(curShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXFormOut, (short)Visio.VisCellIndices.visXFormPinX));
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GenerateShape(string stencilName, string shapeName)
        {
            int start = this.contentDSC.IndexOf(shapeName + " "), startBracket = -1;
            int end = -1, endBracket = -1;
            Visio.Document visioStencil = _addin.Application.Documents.OpenEx(stencilName, (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);
            
            Visio.Master visioRectMaster = visioStencil.Masters.get_ItemU(shapeName);
            string shapeProps = "";
            string[] propLines;
            short customProps = (short)Visio.VisSectionIndices.visSectionProp;
            short rowIndex = -1;
            while (start >= 0)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    lblStatus.Text = "Generating Shape for " + shapeName;
                }), null);

                start = start + shapeName.Length + 1;
                Visio.Shape shape = _addin.Application.ActivePage.Drop(visioRectMaster, 4.25, 5.5);
                end = contentDSC.IndexOf("\r\n", start);
                shape.Text = contentDSC.Substring(start, end - start);
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    lblDetails.Text = contentDSC.Substring(start, end - start);
                }), null);

                /* Get all properties for the shape */
                if (end >= 0)
                {
                    startBracket = contentDSC.IndexOf("{", end + 2) + 1;
                    endBracket = contentDSC.IndexOf("}", end + 2);
                    shapeProps = contentDSC.Substring(startBracket, endBracket - startBracket);
                    
                    propLines = shapeProps.Split('\n');
                    
                    foreach (string prop in propLines)
                    {
                        if (!string.IsNullOrEmpty(prop) && !string.IsNullOrEmpty(prop.Replace(" ", "")) && prop.Replace(" ", "") != "\r" && prop.Split('=')[0].Trim().ToLower() != "psdscrunascredential" && prop.Replace(" ", "") != "\"\r" && prop.Split('=')[0].Trim().ToLower() != "dependson")
                        {
                            string propName = prop.Split('=')[0].Trim();
                            string propValue = prop.Split('=')[1].Trim();
                            rowIndex = -1;
                            for (short i = 0; i < shape.RowCount[customProps]; i++)
                            {
                                if (shape.CellsSRC[customProps, i, (short)Visio.VisCellIndices.visCustPropsLabel].Formula.Replace("\"", "") == propName)
                                    rowIndex = i;
                            }
                            propValue = propValue.Replace(";", "");
                            
                            if (propValue.StartsWith("$"))
                                propValue = propValue.Replace("$", "").ToUpper();
                            try
                            {
                                if(rowIndex >= 0)
                                    shape.CellsSRC[customProps, rowIndex, (short)Visio.VisCellIndices.visCustPropsValue].FormulaU = propValue;
                            }
                            catch(Exception ex)
                            {
                                if (ex.Message == "\n\n#NAME?")
                                { }
                                else if (ex.Message == "\n\nCannot create object.")
                                { }
                                else if (ex.Message == "\n\nBad or missing section index.")
                                { }
                                else
                                    throw new Exception("Unexpected Error occured");
                            }
                        }
                    }
                }

                start = this.contentDSC.IndexOf(shapeName + " ", end);
                //shape.Text = "";
            }
        }

        private void ReverseAnalyze()
        {
            object status = "";
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                status = "Warming Up...";
                bgDeployLocally.ReportProgress(20, status);
                status = "Extracting Current State...";
                bgDeployLocally.ReportProgress(40, status);
                
                GenerateDiagram();
            }
        }

        private void bgDeployLocally_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ReverseAnalyze();
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

        private void bgDeployLocally_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (!e.UserState.ToString().StartsWith("***"))
                lblStatus.Text = e.UserState.ToString();
            else
                lblDetails.Text = e.UserState.ToString().Replace("***", "");
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            currentForm.Close();
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
    }
}

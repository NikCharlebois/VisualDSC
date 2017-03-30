namespace VisualDSC
{    partial class AddinRibbonComponent : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public AddinRibbonComponent()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddinRibbonComponent));
            this.Tab1 = this.Factory.CreateRibbonTab();
            this.Group1 = this.Factory.CreateRibbonGroup();
            this.Command1 = this.Factory.CreateRibbonButton();
            this.btnAzure = this.Factory.CreateRibbonButton();
            this.btnInspect = this.Factory.CreateRibbonButton();
            this.button2 = this.Factory.CreateRibbonButton();
            this.btnImport = this.Factory.CreateRibbonButton();
            this.Tab1.SuspendLayout();
            this.Group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tab1
            // 
            this.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.Tab1.ControlId.OfficeId = "TabHome";
            this.Tab1.Groups.Add(this.Group1);
            this.Tab1.Label = "TabHome";
            this.Tab1.Name = "Tab1";
            // 
            // Group1
            // 
            this.Group1.Items.Add(this.Command1);
            this.Group1.Items.Add(this.btnAzure);
            this.Group1.Items.Add(this.btnInspect);
            this.Group1.Items.Add(this.button2);
            this.Group1.Items.Add(this.btnImport);
            this.Group1.Label = "Desired State";
            this.Group1.Name = "Group1";
            // 
            // Command1
            // 
            this.Command1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.Command1.Image = global::ExtendedVisioAddin1.Properties.Resources.Command1;
            this.Command1.Label = "Export";
            this.Command1.Name = "Command1";
            this.Command1.ShowImage = true;
            this.Command1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonExport_Click);
            // 
            // btnAzure
            // 
            this.btnAzure.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAzure.Image = global::ExtendedVisioAddin1.Properties.Resources.Azure;
            this.btnAzure.Label = "Publish to Azure";
            this.btnAzure.Name = "btnAzure";
            this.btnAzure.ShowImage = true;
            this.btnAzure.SuperTip = "Current Disabled in Preview";
            this.btnAzure.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAzure_Click);
            // 
            // btnInspect
            // 
            this.btnInspect.Image = ((System.Drawing.Image)(resources.GetObject("btnInspect.Image")));
            this.btnInspect.Label = "Inspect";
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.ShowImage = true;
            this.btnInspect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnInspect_Click);
            // 
            // button2
            // 
            this.button2.Image = global::ExtendedVisioAddin1.Properties.Resources.PackageIcon;
            this.button2.Label = "Deploy Locally";
            this.button2.Name = "button2";
            this.button2.ShowImage = true;
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button2_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = global::ExtendedVisioAddin1.Properties.Resources.Import;
            this.btnImport.Label = "Import";
            this.btnImport.Name = "btnImport";
            this.btnImport.ShowImage = true;
            this.btnImport.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnReverseDSC_Click);
            // 
            // AddinRibbonComponent
            // 
            this.Name = "AddinRibbonComponent";
            this.RibbonType = "Microsoft.Visio.Drawing";
            this.Tabs.Add(this.Tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.AddinRibbonComponent_Load);
            this.Tab1.ResumeLayout(false);
            this.Tab1.PerformLayout();
            this.Group1.ResumeLayout(false);
            this.Group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab Tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Command1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAzure;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnInspect;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnImport;
    }

    partial class ThisRibbonCollection
    {
        internal AddinRibbonComponent Ribbon
        {
            get { return this.GetRibbon<AddinRibbonComponent>(); }
        }
    }
}

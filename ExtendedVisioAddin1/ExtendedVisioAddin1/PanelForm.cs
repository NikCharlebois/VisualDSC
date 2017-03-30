using System.Windows.Forms;
using Visio = Microsoft.Office.Interop.Visio;

namespace VisualDSC
{
    public partial class PanelForm : Form
    {
        private readonly Visio.Window _window;
        private ThisAddIn _addinInstance;

        /// <summary>
        /// Form constructor, receives parent Visio diagram window
        /// </summary>
        /// <param name="window">Parent Visio diagram window</param>
        public PanelForm(Visio.Window window, ThisAddIn addinInstance)
        {
            _window = window;
            _addinInstance = addinInstance;
            InitializeComponent();
            RefreshData();
        }

        private void timerCheckResize_Tick(object sender, System.EventArgs e)
        {
            this.Width = this.Width;
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Visio.Shape shape in _window.Selection.ContainingShape.Shapes)
            {
                sb.AppendLine(_addinInstance.GetDSCBlockContent(shape));
            }

            txtDSC.Text = sb.ToString().Replace("        ", "");
        }
    }
}

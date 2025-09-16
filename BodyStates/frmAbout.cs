using System.Windows.Forms;

namespace BodyStates
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            lbVerNum.Text = $"{ProductVersion}";
        }
    }
}

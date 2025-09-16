using BodyStates.Properties;
using M64MM.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BodyStates
{
    public partial class mainForm : Form
    {
        string IsLatestVersion = "Unknown";
        string LatestVersion = "Unknown";
        static string CreatorName = "vazhka-dolya";
        static string AddonLinkName = "bodystates";

        static frmAbout about = new frmAbout();

        public mainForm()
        {
            InitializeComponent();

            this.Load += mainForm_Load;
            this.Text = $"BodyStates {ProductVersion}";

            // Upscale button icons for higher DPI
            // Because WinForms doesn't want to do it by itself
            if (this.DeviceDpi > 96f)
            {
                foreach (var button in GetButtonsWithImages(this))
                {
                    Bitmap originalIcon = new Bitmap(button.Image);
                    float scaleFactor = (float)(this.DeviceDpi - 96) / Math.Abs(96) + 1;
                    int newWidth = (int)(originalIcon.Width * scaleFactor);
                    int newHeight = (int)(originalIcon.Height * scaleFactor);

                    Bitmap scaled = new Bitmap(newWidth, newHeight);
                    using (Graphics g = Graphics.FromImage(scaled))
                    {
                        // NearestNeighbor looks ugly with DPIs that are
                        // increments of 25% but not of 50% (e. g. 125%, 175% etc.),
                        // and not scaling them up at all also looks bad,
                        // so we'll set HighQualityBicubic for those.
                        float remainder = scaleFactor % 1.0f;
                        if (Math.Abs(remainder - 0.25f) < 0.01f || Math.Abs(remainder - 0.75f) < 0.01f)
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                        g.DrawImage(originalIcon, 0, 0, newWidth, newHeight);
                    }

                    button.Image = scaled;
                }
            }
        }

        private IEnumerable<Button> GetButtonsWithImages(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button button && button.Image != null)
                    yield return button;

                foreach (var childbutton in GetButtonsWithImages(control))
                    yield return childbutton;
            }
        }

        private async void mainForm_Load(object sender, EventArgs e)
        {
            await CheckForUpdates();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Hide the add-on instead of closing it
            e.Cancel = true;
            Hide();
        }

        private byte[] GetOneByteAsArray(int address)
        {
            return new byte[] { Core.ReadBytes(Core.BaseAddress + address, 1)[0] };
        }

        private void UpdatesButtonPress()
        {
            switch (IsLatestVersion)
            {
                case "True":
                    Process.Start($"https://github.com/{CreatorName}/{AddonLinkName}/releases");
                    break;
                case "False":
                    Process.Start($"https://github.com/{CreatorName}/{AddonLinkName}/releases/latest");
                    break;
                default:
                    DialogResult result = MessageBox.Show(
                        Resources.updates_unknown_elaborate,
                        Resources.updates_unknown_string,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        Process.Start($"https://github.com/{CreatorName}/{AddonLinkName}/releases/latest");
                    }
                    break;
            }
        }

        private async Task CheckForUpdates()
        {
            updatesToolStripMenuItem.Image = Resources.updates_unknown;
            updatesToolStripMenuItem.Text = Resources.updates_checking_string;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "BodyStates")
;
                    var LatestResponse = await client.GetStringAsync($"https://api.github.com/repos/{CreatorName}/{AddonLinkName}/releases/latest");

                    JObject json = JObject.Parse(LatestResponse);
                    LatestVersion = (string)json["name"];

                    if ("BodyStates v" + ProductVersion == LatestVersion)
                        IsLatestVersion = "True";
                    else IsLatestVersion = "False";
                }
            }
            catch
            {
                IsLatestVersion = "Unknown";
                LatestVersion = "Unknown";
            }

            switch (IsLatestVersion)
            {
                case "True":
                    updatesToolStripMenuItem.Image = Resources.updates_latest;
                    updatesToolStripMenuItem.Text = Resources.updates_latest_string;
                    break;
                case "False":
                    updatesToolStripMenuItem.Image = Resources.updates_outdated;
                    updatesToolStripMenuItem.Text = Resources.updates_outdated_string;
                    break;
                default:
                    updatesToolStripMenuItem.Image = Resources.updates_unknown;
                    updatesToolStripMenuItem.Text = Resources.updates_unknown_string;
                    break;
            }
        }

        private void CheckFixCheckBox()
        {
            switch (chAutoFixBodyStateReset.Checked)
            {
                case true:
                    FixResetBodyState(); // Automatically fix body state reset if the checkbox for that is on and one of the buttons is pressed
                    break;
                default:
                    break;
            }
        }

        public static void FixResetBodyState()
        {
            Core.WriteBytes(Core.BaseAddress + 0x254338, BitConverter.GetBytes(0x27BDFFF8));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 4, BitConverter.GetBytes(0x8C8E0098));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 8, BitConverter.GetBytes(0xAFAE0004));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 12, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 16, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 20, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 24, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 28, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 32, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 36, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 40, BitConverter.GetBytes(0x8FAF0004));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 44, BitConverter.GetBytes(0xA5E00008));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 48, BitConverter.GetBytes(0x8FB80004));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 52, BitConverter.GetBytes(0xA3000007));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 56, BitConverter.GetBytes(0x8C990004));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 60, BitConverter.GetBytes(0x2401FFBF));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 64, BitConverter.GetBytes(0x03216024));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 68, BitConverter.GetBytes(0xAC884004));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 72, BitConverter.GetBytes(0x10000001));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 76, BitConverter.GetBytes(0x00000000));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 80, BitConverter.GetBytes(0x03E00008));
            Core.WriteBytes(Core.BaseAddress + 0x254338 + 84, BitConverter.GetBytes(0x27BD0008));
        }

        public void SetEyesState(int value)
        {
            CheckFixCheckBox();
            byte[] data = BitConverter.GetBytes(value);
            byte[] CapNMState = GetOneByteAsArray(0x33B3B7); // Get the Mormal/Wing cap state
            Core.WriteBytes(Core.BaseAddress + 0x33B3B6, data);
            Core.WriteBytes(Core.BaseAddress + 0x33B3B7, CapNMState); // Reapply the Normal/Wing cap state because changing hands/eyes reset that for some reason
        }

        public void SetHandsState(int value)
        {
            CheckFixCheckBox();
            byte[] data = BitConverter.GetBytes(value);
            byte[] CapNMState = GetOneByteAsArray(0x33B3B7); // Get the Normal/Wing cap state
            byte[] EyesState = GetOneByteAsArray(0x33B3B6); // Get the Eyes' state
            Core.WriteBytes(Core.BaseAddress + 0x33B3B5, data);

            // Reapply the Eyes' and Normal/Wing cap states because they seem to reset for some reason
            Core.WriteBytes(Core.BaseAddress + 0x33B3B6, EyesState);
            Core.WriteBytes(Core.BaseAddress + 0x33B3B7, CapNMState);
        }

        public void SetCapVMState(int value)
        {
            CheckFixCheckBox(); // Even though the Vanish/Metal cap variable doesn't get reset, there is still the issue that Mario's cap may for some reason disappear if there is no reset fix
            byte[] data = BitConverter.GetBytes(value * 2);
            Core.WriteBytes(Core.BaseAddress + 0x33B174, data);
        }

        public void SetCapNWState(int value)
        {
            byte[] CapVMState = GetOneByteAsArray(0x33B174);
            switch (CapVMState[0]) // If CapVMState is not 0–6, then it's possible that trying to change the Normal/Wing cap will do nothing
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                default:
                    Core.WriteBytes(Core.BaseAddress + 0x33B174, BitConverter.GetBytes(0x00));
                    break;
            }

            CheckFixCheckBox();
            byte[] data = BitConverter.GetBytes(value);
            Core.WriteBytes(Core.BaseAddress + 0x33B3B7, data);
        }

        // All of the following is just methods for the UI

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about.ShowDialog();
        }

        private void btnFixBodyStateReset_Click(object sender, EventArgs e)
        {
            FixResetBodyState();
        }

        private void btnEyes1_Click(object sender, EventArgs e)
        {
            SetEyesState(0);
        }

        private void btnEyes2_Click(object sender, EventArgs e)
        {
            SetEyesState(1);
        }

        private void btnEyes3_Click(object sender, EventArgs e)
        {
            SetEyesState(2);
        }

        private void btnEyes4_Click(object sender, EventArgs e)
        {
            SetEyesState(3);
        }

        private void btnEyes5_Click(object sender, EventArgs e)
        {
            SetEyesState(4);
        }

        private void btnEyes6_Click(object sender, EventArgs e)
        {
            SetEyesState(5);
        }

        private void btnEyes7_Click(object sender, EventArgs e)
        {
            SetEyesState(6);
        }

        private void btnEyes8_Click(object sender, EventArgs e)
        {
            SetEyesState(7);
        }

        private void btnEyes9_Click(object sender, EventArgs e)
        {
            SetEyesState(8);
        }

        private void btnHands1_Click(object sender, EventArgs e)
        {
            SetHandsState(0);
        }

        private void btnHands2_Click(object sender, EventArgs e)
        {
            SetHandsState(1);
        }

        private void btnHands3_Click(object sender, EventArgs e)
        {
            SetHandsState(2);
        }

        private void btnHands4_Click(object sender, EventArgs e)
        {
            SetHandsState(3);
        }

        private void btnHands5_Click(object sender, EventArgs e)
        {
            SetHandsState(4);
        }

        private void btnHands6_Click(object sender, EventArgs e)
        {
            SetHandsState(5);
        }

        private void btnCapVM1_Click(object sender, EventArgs e)
        {
            SetCapVMState(0);
        }

        private void btnCapVM2_Click(object sender, EventArgs e)
        {
            SetCapVMState(1);
        }

        private void btnCapVM3_Click(object sender, EventArgs e)
        {
            SetCapVMState(2);
        }

        private void btnCapVM4_Click(object sender, EventArgs e)
        {
            SetCapVMState(3);
        }

        private void btnCapNW1_Click(object sender, EventArgs e)
        {
            SetCapNWState(0);
        }

        private void btnCapNW2_Click(object sender, EventArgs e)
        {
            SetCapNWState(1);
        }

        private void btnCapNW3_Click(object sender, EventArgs e)
        {
            SetCapNWState(2);
        }

        private void btnCapNW4_Click(object sender, EventArgs e)
        {
            SetCapNWState(3);
        }

        private async void updatesrefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await CheckForUpdates();
        }

        private void updatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdatesButtonPress();
        }
    }
}
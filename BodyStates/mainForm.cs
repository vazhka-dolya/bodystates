using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using M64MM.Utils;

namespace BodyStates
{
    public partial class mainForm : Form
    {

        static frmAbout about = new frmAbout();
        public mainForm()
        {
            InitializeComponent();
        }

        private byte[] GetOneByteAsArray(int address)
        {
            return new byte[] { Core.ReadBytes(Core.BaseAddress + address, 1)[0] };
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
            Core.WriteBytes(Core.BaseAddress + 0x33B3B6, EyesState); // Reapply the Eyes' state because they seem to reset for some reason
            Core.WriteBytes(Core.BaseAddress + 0x33B3B7, CapNMState); // Reapply the Normal/Wing cap state because changing hands/eyes also reset that for some reason
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
            switch (CapVMState[0]) // If this number is not 0–6, then it's possible that trying to change the Normal/Wing cap will do nothing
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
    }
}
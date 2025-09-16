using System;
using System.Collections.Generic;
using System.Drawing;
using M64MM.Additions;
using BodyStates.Properties;

namespace BodyStates
{
    public class Module : IModule
    {
        static mainForm frmMain = new mainForm();

        public string SafeName => "BodyStates";

        public string Description => Resources.m64mm_desc;

        public Image AddonIcon => Resources.bodystates_logo;

        public void Close(EventArgs e)
        {

        }

        public List<ToolCommand> GetCommands()
        {
            List<ToolCommand> tcl = new List<ToolCommand>();
            ToolCommand tcOpen = new ToolCommand(Resources.m64mm_open);
            tcOpen.Summoned += (a, b) => openForm();
            tcl.Add(tcOpen);
            return tcl;
        }

        public void openForm()
        {
            if (frmMain == null || frmMain.IsDisposed)
            {
                frmMain = new mainForm();
            }
            frmMain.Show();
        }

        public void Initialize()
        {
        }

        public void OnBaseAddressFound()
        {
        }

        public void OnBaseAddressZero()
        {
        }

        public void Reset()
        {
        }

        public void Update()
        {
        }

        public void OnCoreEntAddressChange(uint addr)
        {
            // :P
        }
    }
}

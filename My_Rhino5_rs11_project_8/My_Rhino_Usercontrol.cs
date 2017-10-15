using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace My_Rhino5_rs11_project_8
{
    public partial class My_Rhino_Usercontrol : UserControl
    {
        public My_Rhino_Usercontrol()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Import_blue_object", false);
            //Rhino.RhinoApp.RunScript("Import_my_object", false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Get_object_name", false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Rhino.RhinoApp.RunScript("Move_object", false);
            Rhino.RhinoApp.RunScript("Move_multi_pins", false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Get_mesh", false);
            Rhino.RhinoApp.RunScript("_PrintDisplay _State=On _Color=Display", false);
            Rhino.RhinoApp.RunScript("\b", false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Rhino.RhinoApp.RunScript("Link_pins", false);
            Rhino.RhinoApp.RunScript("Link_multi_pins", false);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Rhino.RhinoApp.RunScript("Delete_object", false);
            Rhino.RhinoApp.RunScript("Delete_multi_pins", false);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Rotate_multi_pins", false);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Import_purple_object", false);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Rhino.RhinoApp.RunScript("Import_red_object", false);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MyRhino5rs11project8PlugIn.Instance.LoadUI_SerialPort();
        }
    }
}

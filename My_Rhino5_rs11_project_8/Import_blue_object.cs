using System;
using System.Drawing;
using System.IO;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("6520e9a1-94c0-41cf-ace1-3fb85e402434")]
    public class Import_blue_object : Command
    {
        static Import_blue_object _instance;
        public Import_blue_object()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Import_object_4pins command.</summary>
        public static Import_blue_object Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Import_blue_object"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            Brep my_brep = My_object_functions.Initialize("D:/Desktop/MyObject_4pins.3dm");
            My_object_functions.SetName(my_brep, "my_object_4_pins");
            My_object_functions.SetColor(my_brep, Color.Blue);
            My_object_functions.SetPosition(my_brep, new Point3d(0, 0, -5.0));
            My_object_functions.SetPinQuantity(my_brep, 4);
            My_object_functions.SetPinCoordination(my_brep, 0, new Point3d(7.5, 5.0, 0.0));
            My_object_functions.SetPinCoordination(my_brep, 1, new Point3d(-7.5, 5.0, 0.0));
            My_object_functions.SetPinCoordination(my_brep, 2, new Point3d(7.5, -5.0, 0.0));
            My_object_functions.SetPinCoordination(my_brep, 3, new Point3d(-7.5, -5.0, 0.0));
            for(int i = 0; i < 4; i++) { My_object_functions.SetPinGuid(my_brep, i, Guid.NewGuid()); }

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.ObjectColor = My_object_functions.GetColor(my_brep);

            /*
            Point3d position = My_object_functions.GetPosition(my_brep);
            Vector3d d_x = My_object_functions.GetX(my_brep);
            Vector3d d_y = My_object_functions.GetY(my_brep);
            Vector3d d_z = My_object_functions.GetZ(my_brep);
            Line l_x = new Line(position, 10 * d_x);
            Line l_y = new Line(position, 10 * d_y);
            Line l_z = new Line(position, 10 * d_z);
            

            ObjectAttributes path_attributes = new ObjectAttributes();
            path_attributes.ObjectColor = Color.Yellow;
            path_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            path_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            path_attributes.PlotWeight = 2.0;

            
            doc.Objects.AddLine(l_x, path_attributes);
            doc.Objects.AddLine(l_y, path_attributes);
            doc.Objects.AddLine(l_z, path_attributes);
            */
            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            p.my_objects_list.Add(my_brep);

            doc.Objects.AddBrep(my_brep, my_attributes);
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}

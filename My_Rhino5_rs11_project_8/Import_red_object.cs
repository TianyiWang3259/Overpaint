using System;
using System.Drawing;
using System.IO;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("f0494d0a-fb63-4709-931c-3475f71bdb5c")]
    public class Import_red_object : Command
    {
        static Import_red_object _instance;
        public Import_red_object()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Import_red_object command.</summary>
        public static Import_red_object Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Import_red_object"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            Brep my_brep = My_object_functions.Initialize("D:/Desktop/MyObject_1pin.3dm");
            My_object_functions.SetName(my_brep, "my_object_1_pin");
            My_object_functions.SetColor(my_brep, Color.Red);
            My_object_functions.SetPosition(my_brep, new Point3d(0, 10.0, 0));
            My_object_functions.SetZ(my_brep, new Vector3d(0, -1.0, 0));
            My_object_functions.SetY(my_brep, new Vector3d(1.0, 0, 0));
            My_object_functions.SetX(my_brep, new Vector3d(0.0, 0, 1.0));

            My_object_functions.SetPinQuantity(my_brep, 1);
            My_object_functions.SetPinCoordination(my_brep, 0, new Point3d(0, 0, 0));
            

            for (int i = 0; i < 1; i++) { My_object_functions.SetPinGuid(my_brep, i, Guid.NewGuid()); }

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.ObjectColor = My_object_functions.GetColor(my_brep);

            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            p.my_objects_list.Add(my_brep);

            doc.Objects.AddBrep(my_brep, my_attributes);
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}

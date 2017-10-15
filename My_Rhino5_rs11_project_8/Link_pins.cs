using System;
using System.Drawing;
using System.Diagnostics;
using Rhino;
using Rhino.Geometry;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("37364f62-802d-4014-801b-4e16ea738dea")]
    public class Link_pins : Command
    {
        static Link_pins _instance;
        public Link_pins()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Link_pins command.</summary>
        public static Link_pins Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Link_pins"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.

            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;

            if (p.if_painted_object_set_ == false)
            {
                RhinoApp.WriteLine("No mesh");
                return Result.Failure;
            }
            Mesh my_mesh = p.painted_object_;

            DijkstraGraph my_graph = p.graph;

            GetObject gbrep1 = new GetObject();
            gbrep1.SetCommandPrompt("get the brep");
            gbrep1.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            gbrep1.SubObjectSelect = false;
            gbrep1.Get();
            if (gbrep1.CommandResult() != Rhino.Commands.Result.Success)
                return gbrep1.CommandResult();
            Rhino.DocObjects.ObjRef my_objref1 = gbrep1.Object(0);
            Rhino.DocObjects.RhinoObject my_obj1 = my_objref1.Object();
            if (my_obj1 == null)
                return Rhino.Commands.Result.Failure;
            Brep brep1 = my_objref1.Brep();
            if (brep1 == null)
                return Result.Failure;
            my_obj1.Select(false);

            GetObject gbrep2 = new GetObject();
            gbrep2.SetCommandPrompt("get the brep");
            gbrep2.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            gbrep2.SubObjectSelect = false;
            gbrep2.Get();
            if (gbrep2.CommandResult() != Rhino.Commands.Result.Success)
                return gbrep2.CommandResult();
            Rhino.DocObjects.ObjRef my_objref2 = gbrep2.Object(0);
            Rhino.DocObjects.RhinoObject my_obj2 = my_objref2.Object();
            if (my_obj2 == null)
                return Rhino.Commands.Result.Failure;
            Brep brep2 = my_objref2.Brep();
            if (brep2 == null)
                return Result.Failure;
            my_obj2.Select(false);

            Point3d pin_1_position = brep1.UserDictionary.GetPoint3d("CurrentPosition");
            Point3d pin_2_position = brep2.UserDictionary.GetPoint3d("CurrentPosition");
            Guid pin_1_id = brep1.UserDictionary.GetGuid("PinID");
            Guid pin_2_id = brep2.UserDictionary.GetGuid("PinID");

            MeshPoint pin_1_meshpoint = my_mesh.ClosestMeshPoint(pin_1_position, 0);
            MeshPoint pin_2_meshpoint = my_mesh.ClosestMeshPoint(pin_2_position, 0);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            NurbsCurve d_path = my_graph.DijkstraPath_Add(pin_1_meshpoint, pin_1_id, pin_2_meshpoint, pin_2_id);
            watch.Stop();
            if(d_path == null)
            {
                return Result.Success;
            }
            RhinoApp.WriteLine("link time: {0}", watch.Elapsed);
            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = Color.Yellow;
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            my_attributes.PlotWeight = 2.0;

            doc.Objects.AddCurve(d_path, my_attributes);
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}

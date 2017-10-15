using System;
using System.Drawing;
using System.Collections.Generic;
using Rhino;
using Rhino.Geometry;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("cdf839f4-a92f-499f-9844-9a5c4821b973")]
    public class Delete_object : Command
    {
        static Delete_object _instance;
        public Delete_object()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Delete_object command.</summary>
        public static Delete_object Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Delete_object"; }
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

            Guid pin_1_id = brep1.UserDictionary.GetGuid("PinID");

            //delete all current path
            IEnumerable<RhinoObject> path_objref = doc.Objects.GetObjectList(ObjectType.Curve);
            foreach (RhinoObject path in path_objref)
            {
                doc.Objects.Delete(path, true);
            }

            doc.Objects.Delete(my_objref1, true);

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = Color.Yellow;
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            my_attributes.PlotWeight = 2.0;

            List<NurbsCurve> new_path_list = p.graph.DijkstraPath_DeletePin(pin_1_id);
            for(int i = 0; i < new_path_list.Count; i++)
            {
                doc.Objects.Add(new_path_list[i], my_attributes);
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}

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
    [System.Runtime.InteropServices.Guid("f2af6ab8-df14-4426-a379-40beba0b7d0e")]
    public class Delete_path : Command
    {
        static Delete_path _instance;
        public Delete_path()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Delete_path command.</summary>
        public static Delete_path Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Delete_path"; }
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
            GetObject gcurve = new GetObject();
            gcurve.SetCommandPrompt("Get nurbscurve");
            gcurve.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            gcurve.SubObjectSelect = false;
            gcurve.Get();
            if (gcurve.CommandResult() != Result.Success)
                return gcurve.CommandResult();
            ObjRef curve_objref = gcurve.Object(0);
            RhinoObject curve_obj = curve_objref.Object();
            Curve selected_curve = curve_objref.Curve();
            NurbsCurve nurbs_curve = selected_curve.ToNurbsCurve();
            Guid path_id = selected_curve.UserDictionary.GetGuid("PathID");
            IEnumerable<RhinoObject> path_objref = doc.Objects.GetObjectList(ObjectType.Curve);
            foreach (RhinoObject path in path_objref)
            {
                doc.Objects.Delete(path, true);
            }
            List<NurbsCurve> new_path_list = new List<NurbsCurve>();
            new_path_list = my_graph.DijkstraPath_DeletePath(path_id);

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = Color.Yellow;
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            my_attributes.PlotWeight = 2.0;

            //doc.Objects.Delete(my_objref1, true);

            for (int i = 0; i < new_path_list.Count; i++)
            {
                Guid PathID = new_path_list[i].UserDictionary.GetGuid("PathID");
                my_attributes.ObjectId = PathID;
                doc.Objects.Add(new_path_list[i], my_attributes);
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}

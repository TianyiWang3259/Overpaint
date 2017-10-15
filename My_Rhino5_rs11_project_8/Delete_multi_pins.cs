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
    [System.Runtime.InteropServices.Guid("dde4332b-9b99-4b46-9c19-77944d6a33ca")]
    public class Delete_multi_pins : Command
    {
        static Delete_multi_pins _instance;
        public Delete_multi_pins()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Delete_multi_objects command.</summary>
        public static Delete_multi_pins Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Delete_multi_pins"; }
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

            for(int i = 0; i < p.my_objects_list.Count; i++)
            {
                Guid brep1_id = My_object_functions.GetComponentID(brep1);
                Guid my_object_id = My_object_functions.GetComponentID(p.my_objects_list[i]);
                if(brep1_id == my_object_id)
                {
                    p.my_objects_list.RemoveAt(i);
                }
            }

            IEnumerable<RhinoObject> path_objref = doc.Objects.GetObjectList(ObjectType.Curve);
            foreach (RhinoObject path in path_objref)
            {
                doc.Objects.Delete(path, true);
            }
            
            List<NurbsCurve> new_path_list = new List<NurbsCurve>();
            int pin_number = My_object_functions.GetPinQuantity(brep1);
            for (int i = 0; i < pin_number; i++)
            {
                Guid pin_id = My_object_functions.GetPinGuid(brep1, i);
                new_path_list = p.graph.DijkstraPath_DeletePin(pin_id);
            }

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = Color.Yellow;
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            my_attributes.PlotWeight = 2.0;

            doc.Objects.Delete(my_objref1, true);

            for (int i = 0; i < new_path_list.Count; i++)
            {
                Guid path_id = new_path_list[i].UserDictionary.GetGuid("PathID");
                my_attributes.ObjectId = path_id;
                doc.Objects.Add(new_path_list[i], my_attributes);
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}

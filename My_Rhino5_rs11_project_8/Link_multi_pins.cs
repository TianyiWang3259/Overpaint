using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using Rhino;
using Rhino.Geometry;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;


namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("24d9625c-2ff6-473b-9205-8ec0004d4393")]
    public class Link_multi_pins : Command
    {
        static Link_multi_pins _instance;

        //private List<Guid> pin_ball_id_list_1;


        public Link_multi_pins()
        {
            _instance = this;
            //pin_ball_id_list_1 = new List<Guid>();
        }

        ///<summary>The only instance of the Link_multi_pins command.</summary>
        public static Link_multi_pins Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Link_multi_pins"; }
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

            GetObject gbrep = new GetObject();
            gbrep.SetCommandPrompt("get the brep");
            gbrep.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            gbrep.SubObjectSelect = false;
            gbrep.Get();
            if (gbrep.CommandResult() != Rhino.Commands.Result.Success)
                return gbrep.CommandResult();
            Rhino.DocObjects.ObjRef my_objref = gbrep.Object(0);
            Rhino.DocObjects.RhinoObject my_obj = my_objref.Object();
            if (my_obj == null)
                return Rhino.Commands.Result.Failure;
            Brep brep = my_objref.Brep();
            if (brep == null)
                return Result.Failure;
            my_obj.Select(false);

            int pin_number_1 = My_object_functions.GetPinQuantity(brep);
            List<Guid> pin_ball_guid_list_1 = new List<Guid>();
            
            for (int i = 0; i < pin_number_1; i++)
            {
                ObjectAttributes green_ball_attributes = new ObjectAttributes();
                green_ball_attributes.ObjectColor = Color.Green;
                green_ball_attributes.ColorSource = ObjectColorSource.ColorFromObject;
                
                Point3d pin_position = My_object_functions.GetPinPosition(brep, i);
                Guid pin_id = My_object_functions.GetPinGuid(brep, i);
                green_ball_attributes.ObjectId = pin_id;
                green_ball_attributes.UserDictionary.Set("isPin", true);
                Sphere pin_ball = new Sphere(pin_position, 2);
                //Brep pin_ball_brep = pin_ball.ToBrep();
                doc.Objects.AddSphere(pin_ball, green_ball_attributes);
                pin_ball_guid_list_1.Add(pin_id);
            }
            doc.Views.Redraw();

            GetObject g_pinball = new GetObject();
            g_pinball.SetCommandPrompt("choose the pin");
            g_pinball.GeometryFilter = ObjectType.Surface;
            //g_pinball.SetCustomGeometryFilter(PinBallGeometryFilter);
            //g_pinball.DisablePreSelect();
            g_pinball.SubObjectSelect = false;
            g_pinball.Get();
            if(g_pinball.CommandResult() != Result.Success) { return g_pinball.CommandResult(); }
            if(g_pinball.Object(0).Brep() == null) { return Result.Failure; }

            RhinoObject selected_pin_ball = g_pinball.Object(0).Object();
            Guid selected_pin_ball_id = selected_pin_ball.Id;
            for(int i = 0; i < pin_ball_guid_list_1.Count; i++) { doc.Objects.Delete(pin_ball_guid_list_1[i], true); }

            ObjectAttributes greenyellow_ball_attributes = new ObjectAttributes();
            greenyellow_ball_attributes.ObjectColor = Color.GreenYellow;
            greenyellow_ball_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            greenyellow_ball_attributes.ObjectId = selected_pin_ball_id;
            greenyellow_ball_attributes.UserDictionary.Set("isPin", true);
            int pin_number = My_object_functions.FindPinNumber(brep, selected_pin_ball_id);
            Point3d selected_pin_position = My_object_functions.GetPinPosition(brep, pin_number);
            Sphere pin_ball_new = new Sphere(selected_pin_position, 2);
            doc.Objects.AddSphere(pin_ball_new, greenyellow_ball_attributes);
            

            doc.Views.Redraw();

            //MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            if(p.my_objects_list.Count <= 1)
            {
                RhinoApp.WriteLine("not enough objects");
                return Result.Failure;
            }

            List<Guid> pin_ball_guid_list_2 = new List<Guid>();

            for(int i = 0; i < p.my_objects_list.Count; i++)
            {
                Brep an_object = p.my_objects_list[i];
                if(My_object_functions.GetComponentID(brep) != My_object_functions.GetComponentID(an_object))
                {
                    int pin_quantity = My_object_functions.GetPinQuantity(an_object);
                    for(int j = 0; j < pin_quantity; j++)
                    {
                        ObjectAttributes green_ball_attributes = new ObjectAttributes();
                        green_ball_attributes.ObjectColor = Color.Green;
                        green_ball_attributes.ColorSource = ObjectColorSource.ColorFromObject;
                        Point3d pin_position = My_object_functions.GetPinPosition(an_object, j);
                        Guid pin_id = My_object_functions.GetPinGuid(an_object, j);
                        green_ball_attributes.ObjectId = pin_id;
                        green_ball_attributes.UserDictionary.Set("isPin", true);
                        Sphere pin_ball = new Sphere(pin_position, 2);
                        doc.Objects.AddSphere(pin_ball, green_ball_attributes);
                        pin_ball_guid_list_2.Add(pin_id);
                    }
                }
            }
            doc.Views.Redraw();

            GetObject g_pinball_2 = new GetObject();
            g_pinball_2.SetCommandPrompt("choose the pin");
            g_pinball_2.GeometryFilter = ObjectType.Surface;
            //g_pinball.SetCustomGeometryFilter(PinBallGeometryFilter);
            //g_pinball.DisablePreSelect();
            g_pinball_2.SubObjectSelect = false;
            g_pinball_2.Get();
            if (g_pinball_2.CommandResult() != Result.Success) { return g_pinball_2.CommandResult(); }
            if (g_pinball_2.Object(0).Brep() == null) { return Result.Failure; }

            RhinoObject selected_pin_ball_2 = g_pinball_2.Object(0).Object();
            Guid selected_pin_ball_id_2 = selected_pin_ball_2.Id;
            Brep brep_2 = null; 
            for (int i = 0; i < p.my_objects_list.Count; i++)
            {
                Brep an_object = p.my_objects_list[i];
                int pin_quantity = My_object_functions.GetPinQuantity(an_object);
                bool mark = false;
                for (int j = 0; j < pin_quantity; j++)
                {
                    if(My_object_functions.GetPinGuid(an_object, j) == selected_pin_ball_id_2)
                    {
                        mark = true;
                        break;
                    }
                }
                if(mark)
                {
                    brep_2 = an_object;
                    break;
                }
            }
            if (brep_2 == null) { return Result.Failure; }

            int selected_pin_number_2 = My_object_functions.FindPinNumber(brep_2,selected_pin_ball_id_2);

            for (int i = 0; i < pin_ball_guid_list_2.Count; i++) { doc.Objects.Delete(pin_ball_guid_list_2[i], true); }

            Point3d selected_pin_position_2 = My_object_functions.GetPinPosition(brep_2, selected_pin_number_2);

            Sphere pin_ball_new_2 = new Sphere(selected_pin_position_2, 2);

            greenyellow_ball_attributes.ObjectId = selected_pin_ball_id_2;

            doc.Objects.AddSphere(pin_ball_new_2, greenyellow_ball_attributes);

            doc.Views.Redraw();

            MeshPoint pin_1_meshpoint = my_mesh.ClosestMeshPoint(selected_pin_position, 0);
            MeshPoint pin_2_meshpoint = my_mesh.ClosestMeshPoint(selected_pin_position_2, 0);
            NurbsCurve d_path = my_graph.DijkstraPath_Add(pin_1_meshpoint, selected_pin_ball_id, pin_2_meshpoint, selected_pin_ball_id_2);


            if(d_path != null)
            {
                ObjectAttributes path_attributes = new ObjectAttributes();
                path_attributes.ObjectColor = Color.Yellow;
                path_attributes.ColorSource = ObjectColorSource.ColorFromObject;
                path_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
                path_attributes.PlotWeight = 2.0;
                path_attributes.ObjectId = d_path.UserDictionary.GetGuid("PathID");
                doc.Objects.AddCurve(d_path, path_attributes);
            }
            


            doc.Objects.Delete(selected_pin_ball_id, true);
            doc.Objects.Delete(selected_pin_ball_id_2, true);

            doc.Views.Redraw();

            

            return Result.Success;
        }


        /*
        private static bool PinBallGeometryFilter(RhinoObject rhObject, GeometryBase geometry, ComponentIndex component_index)
        {
            bool isPin = false;
            Sphere sphere = new Sphere();
            if(geometry is Brep)
            {
                Brep brep = Brep.TryConvertBrep(geometry);
                if(brep.IsSurface)
                {
                    Surface surface = brep.Surfaces[0];
                    if(surface.TryGetSphere(out sphere))
                    {
                        isPin = true;
                    }
                }
                //isPin = sphere.Radius == 2;
                //isPin = true;
            }
            return isPin;
        }
        */
    }
}

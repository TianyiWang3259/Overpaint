using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("c4823aee-b8fe-41e6-a223-0b5378c408fd")]
    public class Move_multi_pins : Command
    {
        static Move_multi_pins _instance;
        public Move_multi_pins()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Move_object_new command.</summary>
        public static Move_multi_pins Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Move_multi_pins"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            if (p.if_painted_object_set_ == false)
            {
                RhinoApp.WriteLine("No mesh");
                return Result.Failure;
            }
            Mesh my_mesh = p.painted_object_;

            GetObject gbrep = new GetObject();
            gbrep.SetCommandPrompt("get the brep");
            gbrep.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            gbrep.SubObjectSelect = false;
            gbrep.Get();
            if (gbrep.CommandResult() != Rhino.Commands.Result.Success)
                return gbrep.CommandResult();
            ObjRef my_objref = gbrep.Object(0);
            RhinoObject my_obj = my_objref.Object();
            if (my_obj == null)
                return Rhino.Commands.Result.Failure;
            Brep brep = my_objref.Brep();
            if (brep == null)
                return Result.Failure;
            my_obj.Select(false);

            int brep_number = 0;
            for(int i = 0; i < p.my_objects_list.Count; i++)
            {
                Brep an_object = p.my_objects_list[i];
                if( My_object_functions.GetComponentID(brep) == My_object_functions.GetComponentID(an_object) )
                {
                    brep_number = i;
                    break;
                }
            }
            
            GetComponentPosition gp = new GetComponentPosition(brep, my_mesh);
            gp.SetCommandPrompt("Get the object position on mesh: ");
            gp.Constrain(my_mesh, false);
            gp.Get();
            if (gp.CommandResult() != Result.Success)
                return gp.CommandResult();

            Point3d n_p = gp.Point();
            List<Sphere> pin_ball_list;
            Brep new_brep = GetComponentPosition.MoveBrep(brep, my_mesh, n_p, out pin_ball_list);

            /*
            Point3d position = My_object_functions.GetPosition(new_brep);
            Vector3d d_x = My_object_functions.GetX(new_brep);
            Vector3d d_y = My_object_functions.GetY(new_brep);
            Vector3d d_z = My_object_functions.GetZ(new_brep);
            Line l_x = new Line(position, 10 * d_x);
            Line l_y = new Line(position, 10 * d_y);
            Line l_z = new Line(position, 10 * d_z);
            */

            ObjectAttributes path_attributes = new ObjectAttributes();
            path_attributes.ObjectColor = Color.Yellow;
            path_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            path_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            path_attributes.PlotWeight = 2.0;

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = My_object_functions.GetColor(new_brep);
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;

            int new_pin_number = My_object_functions.GetPinQuantity(new_brep);
            List<NurbsCurve> new_path_list = new List<NurbsCurve>();
            for(int i = 0; i < new_pin_number; i++)
            {
                Guid pin_id = My_object_functions.GetPinGuid(new_brep, i);
                Point3d pin_position = My_object_functions.GetPinPosition(new_brep, i);
                MeshPoint pin_on_mesh = my_mesh.ClosestMeshPoint(pin_position, 0);
                new_path_list = p.graph.DijkstraPath_Change(pin_id, pin_on_mesh);
            }

            IEnumerable<RhinoObject> path_objref = doc.Objects.GetObjectList(ObjectType.Curve);
            foreach (RhinoObject path in path_objref)
            {
                doc.Objects.Delete(path, true);
            }

            for (int i = 0; i < new_path_list.Count; i++)
            {
                Guid path_id = new_path_list[i].UserDictionary.GetGuid("PathID");
                path_attributes.ObjectId = path_id;
                doc.Objects.AddCurve(new_path_list[i], path_attributes);
            }

            doc.Objects.Delete(my_objref, true);
            
            /*
            IEnumerable<RhinoObject> rhino_objects = doc.Objects.GetObjectList(ObjectType.Brep);
            foreach (RhinoObject r_o in rhino_objects) { doc.Objects.Delete(r_o, true); }

            IEnumerable<RhinoObject> lines = doc.Objects.GetObjectList(ObjectType.Curve);
            foreach (RhinoObject r_o in lines) { doc.Objects.Delete(r_o, true); }
            */
            doc.Objects.AddBrep(new_brep, my_attributes);
            p.my_objects_list[brep_number] = new_brep;
            /*
            foreach (Sphere s in pin_ball_list)
            { doc.Objects.AddSphere(s); }
            doc.Objects.AddLine(l_x, path_attributes);
            doc.Objects.AddLine(l_y, path_attributes);
            doc.Objects.AddLine(l_z, path_attributes);
            */
            doc.Views.Redraw();

            return Result.Success;
        }
    }

    public class GetComponentPosition : GetPoint
    {
        private Brep new_brep;
        private Mesh base_mesh;
        public GetComponentPosition(Brep brep, Mesh mesh)
        {
            new_brep = brep;
            base_mesh = mesh;
        }
        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            

            Point3d current_point = e.CurrentPoint;

            List<Sphere> pin_ball_list;

            Brep new_new_brep = MoveBrep(new_brep, base_mesh, current_point, out pin_ball_list);

            Color color = My_object_functions.GetColor(new_new_brep);
            for(int i = 0; i < pin_ball_list.Count; i++)
            {
                e.Display.DrawSphere(pin_ball_list[i], System.Drawing.Color.Red);
            }
            e.Display.DrawBrepWires(new_new_brep, color);
            Rhino.Display.DisplayMaterial material = new Rhino.Display.DisplayMaterial(color);
            e.Display.DrawBrepShaded(new_new_brep, material);
        }

        public static Brep MoveBrep(Brep brep, Mesh base_mesh, Point3d current_point, out List<Sphere> pin_ball_list)
        {
            Brep new_brep = brep.DuplicateBrep();
            Point3d original_position = My_object_functions.GetPosition(new_brep);
            Vector3d original_direction = My_object_functions.GetZ(new_brep);
            Vector3d normal_on_mesh = base_mesh.NormalAt(base_mesh.ClosestMeshPoint(current_point, 0.1));

            My_object_functions.RotateVerticallyTo(new_brep, normal_on_mesh);
            My_object_functions.TranslateTo(new_brep, current_point);

            int pin_quantity = My_object_functions.GetPinQuantity(new_brep);
            List<double> pin_to_mesh_distance = new List<double>();
            List<Vector3d> pin_to_mesh_direction = new List<Vector3d>();
            List<Sphere> pin_ball = new List<Sphere>();
            for (int i = 0; i < pin_quantity; i++)
            {
                Point3d pin_position = My_object_functions.GetPinPosition(new_brep, i);
                //Sphere pin_ball_i = new Sphere(pin_position, 2);
                //pin_ball.Add(pin_ball_i);
                Line line1 = new Line(pin_position, -20 * normal_on_mesh);
                Line line2 = new Line(pin_position, 10 * normal_on_mesh);
                int[] faceids;
                Point3d[] i_p_1 = Intersection.MeshLine(base_mesh, line1, out faceids);
                Point3d[] i_p_2 = Intersection.MeshLine(base_mesh, line2, out faceids);
                foreach (Point3d p_1 in i_p_1)
                {
                    pin_to_mesh_distance.Add(pin_position.DistanceTo(p_1));
                    pin_to_mesh_direction.Add(p_1 - pin_position);
                }
                foreach (Point3d p_2 in i_p_2)
                {
                    pin_to_mesh_distance.Add(pin_position.DistanceTo(p_2));
                    pin_to_mesh_direction.Add(p_2 - pin_position);
                }
            }
            int min = 0;
            double min_d = double.MaxValue;
            for (int i = 0; i < pin_to_mesh_distance.Count; i++)
            {
                if (min_d > pin_to_mesh_distance[i])
                {
                    min_d = pin_to_mesh_distance[i];
                    min = i;
                }
            }
            original_position = My_object_functions.GetPosition(new_brep);
            My_object_functions.TranslateTo(new_brep, original_position + pin_to_mesh_direction[min]);
            /*
            for (int i = 0; i < pin_quantity; i ++)
            {
                Point3d pin_position = My_object_functions.GetPinPosition(new_brep, i);
                Sphere pin_ball_i = new Sphere(pin_position, 2);
                pin_ball.Add(pin_ball_i);
            }
            */
            pin_ball_list = pin_ball;
            return new_brep;
        }

    }
}

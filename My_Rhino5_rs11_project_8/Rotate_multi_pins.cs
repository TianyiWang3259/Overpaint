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
    [System.Runtime.InteropServices.Guid("1be84038-a186-4959-bbbe-1fa796e9ca89")]
    public class Rotate_multi_pins : Command
    {
        static Rotate_multi_pins _instance;
        public Rotate_multi_pins()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Rotate_multi_pins command.</summary>
        public static Rotate_multi_pins Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Rotate_multi_pins"; }
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
            for (int i = 0; i < p.my_objects_list.Count; i++)
            {
                Brep an_object = p.my_objects_list[i];
                if (My_object_functions.GetComponentID(brep) == My_object_functions.GetComponentID(an_object))
                {
                    brep_number = i;
                    break;
                }
            }

            Point3d position = My_object_functions.GetPosition(brep);
            Vector3d normal_direction = My_object_functions.GetZ(brep);
            Plane plane = new Plane(position, normal_direction);

            /*
            GetPoint gp = new GetPoint();
            gp.Constrain(plane, false);
            gp.Get();
            if (gp.CommandResult() != Result.Success)
                return gp.CommandResult();
            var start_point = gp.Point();
            if (start_point == Point3d.Unset)
                return Result.Failure;
            */

            Vector3d horizontal_direction = My_object_functions.GetY(brep);
            Point3d start_point = position + 10 * horizontal_direction;

            GetRotationAngle gr = new GetRotationAngle(brep, my_mesh, start_point);
            gr.SetCommandPrompt("Get the rotation angle");
            gr.Constrain(plane, false);
            gr.Get();
            if (gr.CommandResult() != Result.Success)
                return gr.CommandResult();

            Point3d end_point = gr.Point();
            Brep new_brep = GetRotationAngle.RotateBrep(brep, my_mesh, end_point, start_point);

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
            for (int i = 0; i < new_pin_number; i++)
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
            doc.Objects.AddBrep(new_brep, my_attributes);
            p.my_objects_list[brep_number] = new_brep;
            doc.Views.Redraw();


            return Result.Success;
        }
    }

    public class GetRotationAngle : GetPoint
    {
        private Brep new_brep;
        private Mesh base_mesh;
        private Point3d start_point;
        private Point3d center_point;
        private Plane plane;
        public GetRotationAngle(Brep b, Mesh m, Point3d s_p)
        {
            new_brep = b;
            base_mesh = m;
            start_point = s_p;
            center_point = My_object_functions.GetPosition(new_brep);
            Vector3d normal_direction = My_object_functions.GetZ(new_brep);
            plane = new Plane(center_point, normal_direction);
        }
        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);
            Point3d current_point = e.CurrentPoint;
            Brep new_new_brep = RotateBrep(new_brep, base_mesh, current_point, start_point);
            Color color = My_object_functions.GetColor(new_new_brep);
            double radius = current_point.DistanceTo(center_point);
            Circle circle = new Circle(plane, center_point, radius);
            Line line1 = new Line(current_point, center_point);
            Line line2 = new Line(center_point, start_point - center_point, radius);

            e.Display.DrawBrepWires(new_new_brep, color);
            Rhino.Display.DisplayMaterial material = new Rhino.Display.DisplayMaterial(color);
            e.Display.DrawBrepShaded(new_new_brep, material);

            e.Display.DrawCircle(circle, System.Drawing.Color.Black);
            e.Display.DrawLine(line1, System.Drawing.Color.Black);
            //e.Display.DrawLine(line2, System.Drawing.Color.Black);
            
        }

        public static Brep RotateBrep(Brep brep, Mesh base_mesh, Point3d current_point, Point3d start_point)
        {
            Brep new_brep = brep.DuplicateBrep();
            Point3d position = My_object_functions.GetPosition(new_brep);
            Vector3d start_direction = start_point - position;
            Vector3d end_direction = current_point - position;
            double rotate_angle = Vector3d.VectorAngle(end_direction, start_direction);
            //My_object_functions.RotateHorizontally(new_brep, rotate_angle);
            My_object_functions.RotateHorizontallyTo(new_brep, end_direction);

            Vector3d normal_on_mesh = My_object_functions.GetZ(new_brep);
            int pin_quantity = My_object_functions.GetPinQuantity(new_brep);
            List<double> pin_to_mesh_distance = new List<double>();
            List<Vector3d> pin_to_mesh_direction = new List<Vector3d>();
            for (int i = 0; i < pin_quantity; i++)
            {
                Point3d pin_position = My_object_functions.GetPinPosition(new_brep, i);
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
            position = My_object_functions.GetPosition(new_brep);
            My_object_functions.TranslateTo(new_brep, position + pin_to_mesh_direction[min]);
            
            return new_brep;
        }


    }


}

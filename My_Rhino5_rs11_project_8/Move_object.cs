using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;
using Rhino.DocObjects;


namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("0f7b98e5-3faa-402a-aa5c-b4b8dd709d51")]
    public class Move_object : Command
    {
        static Move_object _instance;
        public Move_object()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Move_object command.</summary>
        public static Move_object Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Move_object"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            if(p.if_painted_object_set_ == false)
            {
                RhinoApp.WriteLine("No mesh");
                return Result.Failure;
            }
            Mesh my_mesh = p.painted_object_;

            Stopwatch watch = new Stopwatch();

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

            GetObjectPosition gp = new GetObjectPosition(brep, my_mesh);
            gp.SetCommandPrompt("Get the object position on mesh: ");
            gp.Constrain(my_mesh, false);
            gp.Get();
            if (gp.CommandResult() != Result.Success)
                return gp.CommandResult();

            Brep moved_brep = brep.DuplicateBrep();
            Point3d Origin = brep.UserDictionary.GetPoint3d("CurrentPosition");
            Vector3d OriginVector = brep.UserDictionary.GetVector3d("CurrentDirection");
            Point3d new_position = gp.Point();
            Vector3d normal_on_mesh = my_mesh.NormalAt(my_mesh.ClosestMeshPoint(new_position, 0));

            if (OriginVector.IsParallelTo(normal_on_mesh) == 0)
            {
                double RotationAngle = Vector3d.VectorAngle(OriginVector, normal_on_mesh);
                Vector3d RoationAxis = Vector3d.CrossProduct(OriginVector, normal_on_mesh);
                moved_brep.Rotate(RotationAngle, RoationAxis, Origin);
            }

            moved_brep.Translate(new_position - Origin);
            moved_brep.UserDictionary.Set("CurrentPosition", new_position);
            moved_brep.UserDictionary.Set("CurrentDirection", normal_on_mesh);

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = Color.FromName(moved_brep.UserDictionary.GetString("Color"));
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;

            watch.Start();
            //delete all old paths
            IEnumerable<RhinoObject> path_objref = doc.Objects.GetObjectList(ObjectType.Curve);
            foreach (RhinoObject path in path_objref)
            {
                doc.Objects.Delete(path, true);
            }
            watch.Stop();
            RhinoApp.WriteLine("time 1: {0}", watch.Elapsed);
            
            ObjectAttributes path_attributes = new ObjectAttributes();
            path_attributes.ObjectColor = Color.Yellow;
            path_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            path_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            path_attributes.PlotWeight = 2.0;

            
            Guid pin_1_id = moved_brep.UserDictionary.GetGuid("PinID");
            MeshPoint current_meshpoint = my_mesh.ClosestMeshPoint(new_position, 0.0);
            watch.Restart();
            List<NurbsCurve> new_path_list = p.graph.DijkstraPath_Change(pin_1_id, current_meshpoint);
            watch.Stop();
            RhinoApp.WriteLine("time 2: {0}", watch.Elapsed);
            watch.Restart();
            for (int i = 0; i < new_path_list.Count; i++)
            {
                doc.Objects.Add(new_path_list[i], path_attributes);
            }

            doc.Objects.Delete(my_objref, true);
            brep.Dispose();
            doc.Objects.AddBrep(moved_brep,my_attributes);
            doc.Views.Redraw();
            watch.Stop();
            RhinoApp.WriteLine("time 3: {0}", watch.Elapsed);

            return Result.Success;
        }
    }

    public class GetObjectPosition : GetPoint
    {
        private Brep new_brep;
        private Mesh base_mesh;
        public GetObjectPosition(Brep brep, Mesh mesh)
        {
            new_brep = brep.DuplicateBrep();
            base_mesh = mesh;
        }
        protected override void OnDynamicDraw(GetPointDrawEventArgs e)
        {
            base.OnDynamicDraw(e);

            Point3d Origin = new_brep.UserDictionary.GetPoint3d("CurrentPosition");
            Vector3d OriginVector = new_brep.UserDictionary.GetVector3d("CurrentDirection");

            Point3d currentpoint = e.CurrentPoint;

            Vector3d normal_on_mesh = base_mesh.NormalAt(base_mesh.ClosestMeshPoint(currentpoint,0));

            Brep moved_brep = new_brep.DuplicateBrep();

            if(OriginVector.IsParallelTo(normal_on_mesh) == 0)
            {
                double RotationAngle = Vector3d.VectorAngle(OriginVector, normal_on_mesh);
                Vector3d RoationAxis = Vector3d.CrossProduct(OriginVector, normal_on_mesh);
                moved_brep.Rotate(RotationAngle, RoationAxis, Origin);
            }

            moved_brep.Translate(currentpoint - Origin);
            e.Display.DrawBrepWires(moved_brep, System.Drawing.Color.Red);
            Rhino.Display.DisplayMaterial material = new Rhino.Display.DisplayMaterial(System.Drawing.Color.Red);
            e.Display.DrawBrepShaded(moved_brep,material);
            moved_brep.Dispose();
        }
    }
}

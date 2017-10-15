using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("f858fc92-3aaa-47f8-8ec8-c33a89f6c3ce")]
    public class Get_mesh : Command
    {
        static Get_mesh _instance;
        public Get_mesh()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Get_mesh command.</summary>
        public static Get_mesh Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Get_mesh"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);

            Rhino.Input.Custom.GetObject gmesh = new Rhino.Input.Custom.GetObject();
            gmesh.SetCommandPrompt("Get the Mesh");
            gmesh.GeometryFilter = Rhino.DocObjects.ObjectType.Mesh;
            gmesh.SubObjectSelect = true;
            gmesh.Get();
            if (gmesh.CommandResult() != Rhino.Commands.Result.Success)
                return gmesh.CommandResult();
            Rhino.DocObjects.ObjRef objref = gmesh.Object(0);
            Rhino.DocObjects.RhinoObject obj = objref.Object();
            if (obj == null)
                return Rhino.Commands.Result.Failure;
            Rhino.Geometry.Mesh mesh = objref.Mesh();
            if (mesh == null)
                return Rhino.Commands.Result.Failure;
            obj.Select(false);

            MeshTextureCoordinateList texture_list = mesh.TextureCoordinates;
            for (int i = 0; i<texture_list.Count-1; i++)
            {
                Point2f f1 = texture_list[i];
                Point2f f2 = texture_list[i+1];
                Point3d t1 = new Point3d(f1.X, f1.Y, 0);
                Point3d t2 = new Point3d(f2.X, f2.Y, 0);
                Line l = new Line(t1, t2);
                doc.Objects.AddLine(l);
                RhinoApp.WriteLine("Line added");
            }
            doc.Views.Redraw();
            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            p.painted_object_ = mesh;
            p.if_painted_object_set_ = true;
            mesh.UserDictionary.Set("name", "myMesh");
            mesh.UserDictionary.Set("isMovable", false);
            p.graph = new DijkstraGraph(10);
            RhinoApp.WriteLine("Mesh Got");
            return Result.Success;
        }
    }
}

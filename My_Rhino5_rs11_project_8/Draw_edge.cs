using System;
using System.Collections.Generic;
using System.Drawing;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Geometry.Collections;

namespace My_Rhino5_rs11_project_8
{
    [System.Runtime.InteropServices.Guid("c54589d9-b297-4302-bb82-c4a41a2d6b03")]
    public class Draw_edge : Command
    {
        static Draw_edge _instance;
        public Draw_edge()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Draw_edge command.</summary>
        public static Draw_edge Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Draw_edge"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.
            //DijkstraGraph graph = new DijkstraGraph(50);
            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            Mesh my_mesh = p.painted_object_;

            int vertex_list_count = p.graph.GetVertexListCount();
            RhinoApp.WriteLine("the total number of vertices is {0}", vertex_list_count);

            GetPoint gp_start = new GetPoint();
            gp_start.SetCommandPrompt("Get the begin point on mesh: ");
            gp_start.Constrain(my_mesh, false);
            gp_start.Get();
            if (gp_start.CommandResult() != Result.Success)
                return gp_start.CommandResult();

            Point3d p_start = gp_start.Point();

            GetPoint gp_end = new GetPoint();
            gp_end.SetCommandPrompt("Get the end point on mesh: ");
            gp_end.Constrain(my_mesh, false);
            gp_end.Get();
            if (gp_end.CommandResult() != Result.Success)
                return gp_end.CommandResult();

            Point3d p_end = gp_end.Point();

            int begin_num = p.graph.NearVertexOnMesh(my_mesh.ClosestMeshPoint(p_start,0).FaceIndex);
            int end_num = p.graph.NearVertexOnMesh(my_mesh.ClosestMeshPoint(p_end, 0).FaceIndex);

            /*
            GetInteger g_begin = new GetInteger();
            g_begin.SetCommandPrompt("Type in the number of beginning vertex");
            g_begin.AcceptNumber(true, true);
            g_begin.Get();
            int begin_num = g_begin.Number();
            GetInteger g_end = new GetInteger();
            g_end.SetCommandPrompt("Type in the number of the ending vertex");
            g_end.AcceptNumber(true, true);
            g_end.Get();
            int end_num = g_end.Number();
            */
            NurbsCurve d_path = p.graph.GetDijkstraPath(begin_num, end_num, true).path;
            

            ObjectAttributes my_attributes = new ObjectAttributes();
            my_attributes.ObjectColor = Color.Yellow;
            my_attributes.ColorSource = ObjectColorSource.ColorFromObject;
            my_attributes.PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromObject;
            my_attributes.PlotWeight = 2.0;

            doc.Objects.AddCurve(d_path, my_attributes);
            doc.Objects.AddPoint(my_mesh.ClosestMeshPoint(p_start, 0).Point);
            doc.Objects.AddPoint(my_mesh.ClosestMeshPoint(p_end, 0).Point);          
            
            doc.Views.Redraw();
            return Result.Success;
        }
    }
}

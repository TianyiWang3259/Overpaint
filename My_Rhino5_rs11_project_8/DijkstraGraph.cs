using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace My_Rhino5_rs11_project_8
{
   public class DijkstraGraph
    {
        private Mesh original_mesh;
        private MeshFaceList face_list;
        private List<Point3d> face_center_list;
        private MeshVertexList original_vertex_list;
        private double precision;
        private List<int> weight_list;
        private List<bool> is_weight_set;
        private int weight_on_path;
        private int weight_on_end;
        private List<int> original_vertex_num_list;  //the index in vertex_list of original vertices
        private int face_count;
        private int original_vertex_count;
        private List<Vertex> vertex_list;
        private List<Edge> edge_list;
        private int current_face_num;
        private int max_value = Int32.MaxValue;
        private List<DijkstraPath> dijkstrapath_list;
        private List<DijkstraPathVertex> dijkstrapath_vertex_list;
        private int shift_count = 0;     //used for calculating compute complexity
        double average_edge;
        //min heap
        private int[] min_heap_array;
        private int min_heap_current_size;


        //private List<int> face_on_path;   //store the faces on path temporarily


        /// <summary>
        /// this mat is to record wether an edge of the mesh has been visited. 
        ///If an edge AB is visited, the vertice on this edge, not including AB, will form a link list.
        ///original_edge_mat[A,B] is the index of the first vertex in this link list.
        ///original_edge_mat[B,A] is same as [A,B], -1 means there is no edge between AB or the edge AB is not visited
        /// </summary>
        //private int[,] original_edge_mat;
        private SparseMatrix original_edge_mat;

        public DijkstraGraph(int r)
        {
            MyRhino5rs11project8PlugIn p = MyRhino5rs11project8PlugIn.Instance;
            if (p.if_painted_object_set_ == false)
            {
                Rhino.RhinoApp.WriteLine("There is no mesh");
                return;
            }
            original_mesh = p.painted_object_;

            face_list = original_mesh.Faces;
            if (face_list.QuadCount > 0) { face_list.ConvertQuadsToTriangles(); }
            original_vertex_list = original_mesh.Vertices;
            
            face_count = face_list.Count;
            original_vertex_count = original_vertex_list.Count;
            weight_list = new List<int>(face_count);
            is_weight_set = new List<bool>(face_count);
            face_center_list = new List<Point3d>(face_count);
            original_vertex_num_list = new List<int>(original_vertex_count);
            vertex_list = new List<Vertex>();
            edge_list = new List<Edge>();
            dijkstrapath_list = new List<DijkstraPath>();
            dijkstrapath_vertex_list = new List<DijkstraPathVertex>();
            current_face_num = 0;
            original_edge_mat = new SparseMatrix(original_vertex_count, original_vertex_count);
            //double longest_edge = 0;
            average_edge = 0;
            //face_on_path = new List<int>();

            //calculate the longest edge and set the precesion
            /*
            for (int i = 0; i < face_count; i++)
            {
                MeshFace face = face_list[i];
                Point3f ori_vertex_A = original_vertex_list[face.A];
                Point3f ori_vertex_B = original_vertex_list[face.B];
                Point3f ori_vertex_C = original_vertex_list[face.C];
                double AB = ori_vertex_A.DistanceTo(ori_vertex_B);
                double AC = ori_vertex_A.DistanceTo(ori_vertex_C);
                double BC = ori_vertex_B.DistanceTo(ori_vertex_C);
                double long_edge = AB;
                if (AC > long_edge) { long_edge = AC; }
                if (BC > long_edge) { long_edge = BC; }
                if (long_edge > longest_edge) { longest_edge = long_edge; }
            }
            precision = longest_edge / (r*1.0);
            */
            for (int i = 0; i < face_count; i++)
            {
                MeshFace face = face_list[i];
                Point3d ori_vertex_A = original_vertex_list[face.A];
                Point3d ori_vertex_B = original_vertex_list[face.B];
                Point3d ori_vertex_C = original_vertex_list[face.C];
                Point3d center = (ori_vertex_A + ori_vertex_B + ori_vertex_C) / 3;
                face_center_list.Add(center);
                double AB = ori_vertex_A.DistanceTo(ori_vertex_B);
                double AC = ori_vertex_A.DistanceTo(ori_vertex_C);
                double BC = ori_vertex_B.DistanceTo(ori_vertex_C);
                double average = (AB + BC + AC) / 3;
                average_edge += average;
            }
            average_edge = average_edge / (face_list.Count);
            precision = average_edge / (r * 1.0);

            for (int i = 0; i < face_count; i++) { weight_list.Add(1); is_weight_set.Add(false); }
            weight_on_path = 32768;
            weight_on_end = 4096;

            for (int i = 0; i < original_vertex_count; i++)
            {     
                original_vertex_num_list.Add( -1);
                //for (int j = 0; j < original_vertex_count; j++)
                //{
                //    original_edge_mat[i, j] = -1;
                //}
            }

            //generating vertices and edges for the graph
            for (int i = 0; i < face_count; i++)
            {
                MeshFace face = face_list[i];
                current_face_num = i;
                
                //handle all the three vertices
                Point3f ori_vertex_A_pos = original_vertex_list[face.A];
                int ori_vertex_A_num = original_vertex_num_list[face.A];
                if (ori_vertex_A_num < 0)
                {
                    Vertex v_A = new Vertex(ori_vertex_A_pos);
                    v_A.is_original_vertex = true;
                    vertex_list.Add(v_A);
                    int v_A_num = vertex_list.Count - 1;
                    original_vertex_num_list[face.A] = v_A_num;
                    ori_vertex_A_num = v_A_num;
                }
                Point3f ori_vertex_B_pos = original_vertex_list[face.B];
                int ori_vertex_B_num = original_vertex_num_list[face.B];
                if (ori_vertex_B_num < 0)
                {
                    Vertex v_B = new Vertex(ori_vertex_B_pos);
                    v_B.is_original_vertex = true;
                    vertex_list.Add(v_B);
                    int v_B_num = vertex_list.Count - 1;
                    original_vertex_num_list[face.B] = v_B_num;
                    ori_vertex_B_num = v_B_num;
                }
                Point3f ori_vertex_C_pos = original_vertex_list[face.C];
                int ori_vertex_C_num = original_vertex_num_list[face.C];
                if (ori_vertex_C_num < 0)
                { 
                    Vertex v_C = new Vertex(ori_vertex_C_pos);
                    v_C.is_original_vertex = true;
                    vertex_list.Add(v_C);
                    int v_C_num = vertex_list.Count - 1;
                    original_vertex_num_list[face.C] = v_C_num;
                    ori_vertex_C_num = v_C_num;
                }

                Vector3f unit_AB = ori_vertex_B_pos - ori_vertex_A_pos; unit_AB.Unitize();
                Vector3f unit_BC = ori_vertex_C_pos - ori_vertex_B_pos; unit_BC.Unitize();
                Vector3f unit_CA = ori_vertex_A_pos - ori_vertex_C_pos; unit_CA.Unitize();

                //1. 
                //For each edge, get all the points on the edge
                List<int> steiner_point_list_AB = GetVerticeOnMeshEdge(face.A, face.B);
                List<int> steiner_point_list_BC = GetVerticeOnMeshEdge(face.B, face.C);
                List<int> steiner_point_list_CA = GetVerticeOnMeshEdge(face.C, face.A);


                //2.
                //Use the points list achieved above to link them in the face
                List<int> steiner_point_list_BC_CA = new List<int>();
                for (int k = 0; k < steiner_point_list_BC.Count; k++) { steiner_point_list_BC_CA.Add(steiner_point_list_BC[k]); }
                steiner_point_list_BC_CA.Add(ori_vertex_C_num);
                for (int k = 0; k < steiner_point_list_CA.Count; k++) { steiner_point_list_BC_CA.Add(steiner_point_list_CA[k]); }

                List<int> steiner_point_list_CA_AB = new List<int>();
                for (int k = 0; k < steiner_point_list_CA.Count; k++) { steiner_point_list_CA_AB.Add(steiner_point_list_CA[k]); }
                steiner_point_list_CA_AB.Add(ori_vertex_A_num);
                for (int k = 0; k < steiner_point_list_AB.Count; k++) { steiner_point_list_CA_AB.Add(steiner_point_list_AB[k]); }

                List<int> steiner_point_list_AB_BC = new List<int>();
                for (int k = 0; k < steiner_point_list_AB.Count; k++) { steiner_point_list_AB_BC.Add(steiner_point_list_AB[k]); }
                steiner_point_list_AB_BC.Add(ori_vertex_A_num);
                for (int k = 0; k < steiner_point_list_BC.Count; k++) { steiner_point_list_AB_BC.Add(steiner_point_list_BC[k]); }

                
                for (int j = 1; j < steiner_point_list_AB.Count - 1; j++)
                {
                    int[] vertex_num_array = new int[] { -1, -1, -1, -1, -1, -1 };
                    double[] distance_array = new double[] { max_value, max_value, max_value, max_value, max_value, max_value };
                    int v1_num = steiner_point_list_AB[j];
                    for (int k = 0; k < steiner_point_list_BC_CA.Count; k++)
                    {
                        int v2_num = steiner_point_list_BC_CA[k];
                        Point3d v1_position = vertex_list[v1_num].position;
                        Point3d v2_position = vertex_list[v2_num].position;
                        Vector3d unit_v1_v2 = (v2_position - v1_position);
                        unit_v1_v2.Unitize();
                        double dotproduct = unit_AB.X * unit_v1_v2.X + unit_AB.Y * unit_v1_v2.Y + unit_AB.Z * unit_v1_v2.Z;
                        //if (dotproduct > 1) { dotproduct = 1; }
                        //if (dotproduct < -1) { dotproduct = -1; }
                        double angle = Math.Acos(dotproduct) * 180.0 / Math.PI;
                        if (angle > 180) { angle = 180; }
                        if (angle < 0) { angle = 0; }
                        double distance_v1_v2 = v1_position.DistanceTo(v2_position);
                        int index = (int)(angle / 30);
                        if (distance_v1_v2 < distance_array[index])
                        {
                            distance_array[index] = distance_v1_v2;
                            vertex_num_array[index] = v2_num;
                        }
                    }
                    for (int k = 0; k < 6; k++)
                    {
                        SetFaceNumToEdge(Link(v1_num, vertex_num_array[k]), current_face_num);
                    }
                }

                for (int j = 1; j < steiner_point_list_BC.Count - 1; j++)
                {
                    int[] vertex_num_array = new int[] { -1, -1, -1, -1, -1, -1 };
                    double[] distance_array = new double[] { max_value, max_value, max_value, max_value, max_value, max_value };
                    int v1_num = steiner_point_list_BC[j];
                    for (int k = 0; k < steiner_point_list_CA_AB.Count; k++)
                    {
                        int v2_num = steiner_point_list_CA_AB[k];
                        Point3d v1_position = vertex_list[v1_num].position;
                        Point3d v2_position = vertex_list[v2_num].position;
                        Vector3d unit_v1_v2 = (v2_position - v1_position);
                        unit_v1_v2.Unitize();
                        double dotproduct = unit_BC.X * unit_v1_v2.X + unit_BC.Y * unit_v1_v2.Y + unit_BC.Z * unit_v1_v2.Z;
                        double angle = Math.Acos(dotproduct) * 180.0 / Math.PI;
                        if (angle > 180) { angle = 180; }
                        if (angle < 0) { angle = 0; }
                        double distance_v1_v2 = v1_position.DistanceTo(v2_position);
                        int index = (int)(angle / 30);
                        if (distance_v1_v2 < distance_array[index])
                        {
                            distance_array[index] = distance_v1_v2;
                            vertex_num_array[index] = v2_num;
                        }
                    }
                    for (int k = 0; k < 6; k++)
                    {
                        SetFaceNumToEdge(Link(v1_num, vertex_num_array[k]), current_face_num);
                    }
                }

                for (int j = 1; j < steiner_point_list_CA.Count - 1; j++)
                {
                    int[] vertex_num_array = new int[] { -1, -1, -1, -1, -1, -1 };
                    double[] distance_array = new double[] { max_value, max_value, max_value, max_value, max_value, max_value };
                    int v1_num = steiner_point_list_CA[j];
                    for (int k = 0; k < steiner_point_list_AB_BC.Count; k++)
                    {
                        int v2_num = steiner_point_list_AB_BC[k];
                        Point3d v1_position = vertex_list[v1_num].position;
                        Point3d v2_position = vertex_list[v2_num].position;
                        Vector3d unit_v1_v2 = (v2_position - v1_position);
                        unit_v1_v2.Unitize();
                        double dotproduct = unit_CA.X * unit_v1_v2.X + unit_CA.Y * unit_v1_v2.Y + unit_CA.Z * unit_v1_v2.Z;
                        if (dotproduct > 1) { dotproduct = 1; }
                        if (dotproduct < -1) { dotproduct = -1; }
                        double angle = Math.Acos(dotproduct) * 180.0 / Math.PI;
                        if (angle > 180) { angle = 180; }
                        if (angle < 0) { angle = 0; }
                        double distance_v1_v2 = v1_position.DistanceTo(v2_position);
                        int index = (int)(angle / 30);
                        if (distance_v1_v2 < distance_array[index])
                        {
                            distance_array[index] = distance_v1_v2;
                            vertex_num_array[index] = v2_num;
                        }
                    }
                    for (int k = 0; k < 6; k++)
                    {
                        SetFaceNumToEdge(Link(v1_num, vertex_num_array[k]), current_face_num);
                    }
                }
            }
        }
        
        //link vertex A and vertex B, create the edge AB, and return the index of edge AB
        private int Link(int v_A_num, int v_B_num)
        {
            if (v_B_num < 0 || v_A_num < 0 || v_A_num > vertex_list.Count || v_B_num > vertex_list.Count) { return -1; }
            Vertex v_A = vertex_list[v_A_num];
            Vertex v_B = vertex_list[v_B_num];

            //check if there is an edge between A and B
            List<int> v_A_edge_list = GetEdgeList(v_A_num);
            for(int k = 0; k < v_A_edge_list.Count; k++)
            {
                int e = v_A_edge_list[k];
                int v = edge_list[e].AnotherEnd(v_A_num);
                if(v == v_B_num) { return e; }
            }

            //if there is no edge between A and B, build a new edge
            Edge e_AB = new Edge(v_A_num, v_B_num, v_A.position.DistanceTo(v_B.position));
            edge_list.Add(e_AB);
            int e_AB_num = edge_list.Count - 1;
            if (v_A.first_edge_num < 0) { vertex_list[v_A_num].first_edge_num = e_AB_num; }
            else
            {
                int edge = v_A.first_edge_num;
                while(edge_list[edge].NextEdge(v_A_num) > 0)
                {
                    edge = edge_list[edge].NextEdge(v_A_num);
                }
                edge_list[edge].SetNextEdge(v_A_num,e_AB_num);
            }
            if (v_B.first_edge_num < 0) { vertex_list[v_B_num].first_edge_num = e_AB_num; }
            else
            {
                int edge = v_B.first_edge_num;
                while (edge_list[edge].NextEdge(v_B_num) > 0)
                {
                    edge = edge_list[edge].NextEdge(v_B_num);
                }
                edge_list[edge].SetNextEdge(v_B_num, e_AB_num);
            }
            return e_AB_num;
        }

        //Add face index to edge
        private bool SetFaceNumToEdge(int edge_num, int face_num)
        {
            if (edge_num < 0) { return false; }
            if (edge_list[edge_num].face_1_num < 0) {
                edge_list[edge_num].face_1_num = face_num;
                return true;
            }
            else if (edge_list[edge_num].face_2_num < 0) {
                edge_list[edge_num].face_2_num = face_num;
                return true;
            }
            else { return false; }
        }

        //get weight number for edge
        private int GetEdgeWeight(int edge_num)
        {
            int weight = 1;
            if (edge_list[edge_num].face_1_num > 0)
            {
                weight = weight_list[edge_list[edge_num].face_1_num];
            }
            if (edge_list[edge_num].face_2_num > 0)
            {
                if (weight < weight_list[edge_list[edge_num].face_2_num])
                {
                    weight = weight_list[edge_list[edge_num].face_2_num];
                }
            }
            return weight;
        }
        
        /// <summary>
        /// get the list of index of edges of a vertex
        /// </summary>
        private List<int> GetEdgeList(int ver_num)
        {
            List<int> ver_edge_list = new List<int>();
            Vertex v = vertex_list[ver_num];
            int edge_num = v.first_edge_num;
            while(edge_num >= 0)
            {
                ver_edge_list.Add(edge_num);
                edge_num = edge_list[edge_num].NextEdge(ver_num);
            }
            return ver_edge_list;
        }
        
        /// <summary>
        /// get the list of neighboor of a vertex
        /// </summary>
        private List<int> GetNeighboorList(int ver_num)
        {
            List<int> ver_edge_list = GetEdgeList(ver_num);
            List<int> neighboor_list = new List<int>();
            for(int i = 0; i < ver_edge_list.Count; i++)
            {
                Edge ver_edge = edge_list[ver_edge_list[i]];
                int neighboor = ver_edge.AnotherEnd(ver_num);
                neighboor_list.Add(neighboor);
            }
            return neighboor_list;
        }

        /// <summary>
        /// return the weight length of the edge between v1 and v2, if there is no edge, return max_value
        /// </summary>
        private double GetWeightedDistance(int v1, int v2)
        {
            if (v1 < 0 || v2 < 0 || v1 > vertex_list.Count || v2 > vertex_list.Count) { return 0; }
            else
            {
                List<int> v_A_edge_list = GetEdgeList(v1);
                for (int k = 0; k < v_A_edge_list.Count; k++)
                {
                    int e = v_A_edge_list[k];
                    int v = edge_list[e].AnotherEnd(v1);
                    if (v == v2)
                    {
                        int weight = GetEdgeWeight(e);
                        double distance = edge_list[e].length;
                        double weighted_distance = weight * distance;
                        return weighted_distance;
                    }
                }
                return double.MaxValue;
            }
        }
    
        /// <summary>
        /// Return the list of vertices on a mesh edge, the parameters must be the index of original vertices in original_vertex_num_list
        /// </summary>
        private List<int> GetVerticeOnMeshEdge(int ori_ver_1, int ori_ver_2)
        {
            int first_ver_on_edge = original_edge_mat[ori_ver_1, ori_ver_2];
            int v1_num = original_vertex_num_list[ori_ver_1];
            int v2_num = original_vertex_num_list[ori_ver_2];
            if(first_ver_on_edge < 0) //this edge is not visited
            {
                Vertex v1 = vertex_list[v1_num];
                Vertex v2 = vertex_list[v2_num];
                double distance = v1.position.DistanceTo(v2.position);
                int points_count = (int)(distance / precision) + 1;
                float interval = (float)(distance / (points_count + 1));
                Vector3d unit = v2.position - v1.position;
                unit.Unitize();
                List<int> steiner_point_list = new List<int>();
                for (int j = 0; j < points_count; j++)
                {
                    Point3d point = v1.position + (unit * (j + 1) * interval);
                    Vertex steiner_point = new Vertex(point);
                    vertex_list.Add(steiner_point);
                    steiner_point_list.Add(vertex_list.Count - 1);
                }
                for (int j = 0; j < steiner_point_list.Count - 1; j++)
                {
                    int v_1 = steiner_point_list[j];
                    int v_2 = steiner_point_list[j + 1];
                    SetFaceNumToEdge(Link(v_1, v_2), current_face_num);
                    vertex_list[v_1].next_vertex_on_original_edge = v_2;
                }
                SetFaceNumToEdge(Link(v1_num, steiner_point_list[0]), current_face_num);
                SetFaceNumToEdge(Link(v2_num, steiner_point_list[steiner_point_list.Count - 1]), current_face_num);
                original_edge_mat[ori_ver_1, ori_ver_2] = steiner_point_list[0];
                original_edge_mat[ori_ver_2, ori_ver_1] = steiner_point_list[0];
                return steiner_point_list;
            }
            else     //this edge is visited
            {
                List<int> steiner_point_list = new List<int>();
                int ver_on_edge = first_ver_on_edge;
                while(ver_on_edge > 0)
                {
                    steiner_point_list.Add(ver_on_edge);
                    ver_on_edge = vertex_list[ver_on_edge].next_vertex_on_original_edge;
                }
                return steiner_point_list;
            }
        }

        private void ChangeFaceWeight(int edge_num, int weight, out int face_num, out List<int> face_num_list)
        {
            Edge e = edge_list[edge_num];
            face_num = -1;
            face_num_list = new List<int>();
            double distance_range = average_edge * 3.0;
            if(e.face_1_num > 0)
            {
                face_num = e.face_1_num;
                Point3d face_center = face_center_list[face_num];
                for(int i = 0; i < face_count; i++)
                {
                    if(face_center_list[i].DistanceTo(face_center) < distance_range)
                    { weight_list[i] = weight; face_num_list.Add(i); }
                }
                //weight_list[e.face_1_num] = weight;
                //int[] adjacentfaces1 = face_list.AdjacentFaces(e.face_1_num);
                //foreach (int face in adjacentfaces1) { weight_list[face] = weight; }
            }
            if(e.face_2_num > 0)
            {
                face_num = e.face_2_num;
                Point3d face_center = face_center_list[face_num];
                for (int i = 0; i < face_count; i++)
                {
                    if (face_center_list[i].DistanceTo(face_center) < distance_range)
                    { weight_list[i] = weight; face_num_list.Add(i); }
                }
                //weight_list[e.face_2_num] = weight;
                //int[] adjacentfaces2 = face_list.AdjacentFaces(e.face_2_num);
                //foreach (int face in adjacentfaces2) { weight_list[face] = weight; }
            }
        }

        private void ChangeFaceWeight(NurbsCurve n_curve, int weight, out List<int> weight_changed_faces, bool if_change)
        {
            double distance_range = average_edge * 3.0;
            List<int> changed_faces = new List<int>();
            for(int i = 0; i < face_list.Count; i++)
            {
                //if ( is_weight_set[i] == false)
                //{
                double t = 0;
                if (n_curve.ClosestPoint(face_center_list[i], out t, distance_range))
                {
                    if(if_change)
                    {
                        weight_list[i] = weight;
                        is_weight_set[i] = true;
                    }
                    changed_faces.Add(i);
                } 
                //}
            }
            weight_changed_faces = changed_faces;
        }

        private void MinHeap_AddVertex(int ver_num)
        {
            if(min_heap_current_size == 0)
            {
                min_heap_array[0] = ver_num;
                vertex_list[ver_num].min_heap_position = 0;
                vertex_list[ver_num].is_in_minheap = true;
                min_heap_current_size++;
            }
            else
            {
                min_heap_array[min_heap_current_size] = ver_num;
                vertex_list[ver_num].min_heap_position = min_heap_current_size;
                MinHeap_ShiftUp(min_heap_current_size);
                min_heap_current_size++;
            }
        }
        
        private int MinHeap_RemoveMin()
        {
            int min_ver_num = min_heap_array[0];
            min_heap_array[0] = min_heap_array[min_heap_current_size - 1];
            vertex_list[min_heap_array[0]].min_heap_position = 0;
            min_heap_current_size--;
            MinHeap_ShiftDown(0, min_heap_current_size - 1);
            vertex_list[min_ver_num].is_in_minheap = false;
            return min_ver_num;
        }

        private void MinHeap_DecreaseDist(int ver_num, double new_dist)
        {
            vertex_list[ver_num].dist = new_dist;
            int start = vertex_list[ver_num].min_heap_position;
            MinHeap_ShiftUp(start);
        }

        private void MinHeap_ShiftUp(int start)
        {
            int j = start;
            int i = (j - 1) / 2;
            int temp_ver_num = min_heap_array[j];
            double temp_ver_dist = MinHeap_GetValue(j);
            while( j > 0)
            {
                if (MinHeap_GetValue(i) <= temp_ver_dist) { break; }
                else
                {
                    //vertex_list[min_heap_array[j]].min_heap_position = i;
                    vertex_list[min_heap_array[i]].min_heap_position = j;
                    min_heap_array[j] = min_heap_array[i];
                    j = i;
                    i = (i - 1) / 2;
                    shift_count++;
                }
            }
            min_heap_array[j] = temp_ver_num;
            vertex_list[temp_ver_num].min_heap_position = j;
        }

        private void MinHeap_ShiftDown(int start, int m)
        {
            int i = start;
            int j = 2 * i + 1;
            int temp_ver_num = min_heap_array[i];
            double temp_ver_dist = MinHeap_GetValue(i);
            while(j <= m)
            {
                if(j < m && MinHeap_GetValue(j) > MinHeap_GetValue(j+1)) { j++; }
                if(temp_ver_dist <= MinHeap_GetValue(j)) { break; }
                else
                {
                    vertex_list[min_heap_array[j]].min_heap_position = i;
                    min_heap_array[i] = min_heap_array[j];
                    i = j;
                    j = 2 * j + 1;
                    shift_count++;
                }
            }
            min_heap_array[i] = temp_ver_num;
            vertex_list[temp_ver_num].min_heap_position = i;
        }

        private double MinHeap_GetValue(int min_heap_index)
        {
            if (min_heap_index > min_heap_current_size) { return 0; }
            int ver_num = min_heap_array[min_heap_index];
            return vertex_list[ver_num].dist;
        }
        
        private int AddNewVertexOnMesh(MeshPoint mesh_point)
        {
            Point3d mesh_point_position = mesh_point.Point;
            Vertex new_ver = new Vertex(mesh_point_position);
            vertex_list.Add(new_ver);
            int new_ver_num = vertex_list.Count - 1;
            int face_num = mesh_point.FaceIndex;
            MeshFace face = face_list[face_num];
            List<int> steiner_point_list_AB = GetVerticeOnMeshEdge(face.A, face.B);
            List<int> steiner_point_list_BC = GetVerticeOnMeshEdge(face.B, face.C);
            List<int> steiner_point_list_CA = GetVerticeOnMeshEdge(face.C, face.A);
            int A_num = original_vertex_num_list[face.A];
            int B_num = original_vertex_num_list[face.B];
            int C_num = original_vertex_num_list[face.C];
            SetFaceNumToEdge(Link(new_ver_num, A_num), face_num);
            SetFaceNumToEdge(Link(new_ver_num, B_num), face_num);
            SetFaceNumToEdge(Link(new_ver_num, C_num), face_num);
            for (int i = 0; i < steiner_point_list_AB.Count; i++) { SetFaceNumToEdge(Link(new_ver_num, steiner_point_list_AB[i]), face_num); }
            for (int i = 0; i < steiner_point_list_BC.Count; i++) { SetFaceNumToEdge(Link(new_ver_num, steiner_point_list_BC[i]), face_num); }
            for (int i = 0; i < steiner_point_list_CA.Count; i++) { SetFaceNumToEdge(Link(new_ver_num, steiner_point_list_CA[i]), face_num); }
            return new_ver_num;
        }

        private bool CheckConnection(DijkstraPathVertex v1, DijkstraPathVertex v2)
        {
            if(v1 == v2)
            {
                //for (int i = 0; i < dijkstrapath_vertex_list.Count; i++) { dijkstrapath_vertex_list[i].is_visited = false; }
                return true;
            }
            else
            {
                v1.is_visited = true;
                for (int i = 0; i < v1.neighboor_list.Count; i++)
                {
                    if(v1.neighboor_list[i].is_visited == false)
                    {
                        if (CheckConnection(v1.neighboor_list[i], v2))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //convert all edges to a Rhino.Geometery.Line and put them in a list
        public List<Line> GetLineList()
        {
            List<Line> line_list = new List<Line>();
            for (int i = 0; i < edge_list.Count; i++)
            {
                Edge e = edge_list[i];
                Vertex v1 = vertex_list[e.vertex_1_num];
                Vertex v2 = vertex_list[e.vertex_2_num];
                Point3d p1 = new Point3d(v1.position);
                Point3d p2 = new Point3d(v2.position);
                Line l = new Line(p1, p2);
                line_list.Add(l);
            }
            return line_list;
        }

        public DijkstraPath GetDijkstraPath(int start_num, int terminal_num, bool if_change_weight)
        {
            shift_count = 0;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            //face_on_path.Clear();
            min_heap_array = new int[vertex_list.Count];
            for (int i = 0; i < vertex_list.Count; i++)
            {
                min_heap_array[i] = -1;
            }
            min_heap_current_size = 0;

            int p = 1;
            for (int i = 0; i < vertex_list.Count; i++)
            {
                if(i != start_num)
                {
                    vertex_list[i].min_heap_position = p;
                    min_heap_array[p] = i;
                    vertex_list[i].prev = -1;
                    vertex_list[i].dist = Double.MaxValue;
                    vertex_list[i].is_in_minheap = true;
                    p++;
                }
            }
            vertex_list[start_num].dist = 0;
            vertex_list[start_num].min_heap_position = 0;
            vertex_list[start_num].prev = -1;
            min_heap_array[0] = start_num;
            min_heap_current_size = vertex_list.Count;
            while(min_heap_current_size > 0)
            {
                int min_vertex_num = MinHeap_RemoveMin();
                if (min_vertex_num == terminal_num) { break; }
                List<int> neighboor_of_min_vertex = GetNeighboorList(min_vertex_num);
                for (int i = 0; i < neighboor_of_min_vertex.Count; i++)
                {
                    Vertex v = vertex_list[neighboor_of_min_vertex[i]];
                    if (v.is_in_minheap)
                    {
                        double length_v = GetWeightedDistance(min_vertex_num, neighboor_of_min_vertex[i]);
                        double alt = vertex_list[min_vertex_num].dist + length_v;
                        if (alt < v.dist)
                        {
                            MinHeap_DecreaseDist(neighboor_of_min_vertex[i], alt);
                            vertex_list[neighboor_of_min_vertex[i]].prev = min_vertex_num;
                        }
                    }
                }
            }

            DijkstraPath new_dijkstra_path = new DijkstraPath();
            //new_dijkstra_path.ver_1_num = start_num;
            //new_dijkstra_path.ver_2_num = terminal_num;

            List<int> ver_on_path = new List<int>();
            int ver_on_path_num = terminal_num;
            while(vertex_list[ver_on_path_num].prev > 0)
            {
                ver_on_path.Add(ver_on_path_num);
                new_dijkstra_path.vertex_on_path.Add(ver_on_path_num);
                ver_on_path_num = vertex_list[ver_on_path_num].prev;
            }
            ver_on_path.Add(ver_on_path_num);
            new_dijkstra_path.vertex_on_path.Add(ver_on_path_num);

            Polyline d_path = new Polyline();
            for(int i = 0; i<ver_on_path.Count - 1; i++)
            {
                int edge_num = Link(ver_on_path[i], ver_on_path[i + 1]);
                new_dijkstra_path.edge_on_path.Add(edge_num);
                Edge e = edge_list[edge_num];
                if (e.face_1_num > 0) { new_dijkstra_path.face_on_path.Add(e.face_1_num); }
                if (e.face_2_num > 0) { new_dijkstra_path.face_on_path.Add(e.face_2_num); }
                //ChangeFaceWeight(edge_num, 16384);
            }
            

            for (int i = 0; i < ver_on_path.Count; i++)
            {
                Point3d p_f = vertex_list[ver_on_path[i]].position;
                Point3d p_d = new Point3d(p_f);
                d_path.Add(p_d);
            }
            //d_path.CopyTo(new_dijkstra_path.path_polyline);
            new_dijkstra_path.path_polyline = d_path;
            

            d_path.Smooth(1);
            NurbsCurve d_nurbscurve = d_path.ToNurbsCurve();
            d_nurbscurve.PullToMesh(original_mesh, 0);
            new_dijkstra_path.path = d_nurbscurve;
            ChangeFaceWeight(d_nurbscurve, weight_on_path, out new_dijkstra_path.weight_changed_faces, if_change_weight);
            if(if_change_weight)
            {
                List<int> face_change_weight_on_terminal_1, face_change_weight_on_terminal_2;
                ChangeFaceWeight(Link(ver_on_path[0], ver_on_path[1]), weight_on_end, out new_dijkstra_path.start_face_num, out face_change_weight_on_terminal_1);
                ChangeFaceWeight(Link(ver_on_path[ver_on_path.Count - 2], ver_on_path[ver_on_path.Count - 1]), weight_on_end, out new_dijkstra_path.end_face_num, out face_change_weight_on_terminal_2);
                for(int j = 0; j < face_change_weight_on_terminal_2.Count; j++)
                {
                    face_change_weight_on_terminal_1.Add(face_change_weight_on_terminal_2[j]);
                }
                new_dijkstra_path.weight_changed_faces_on_terminal = face_change_weight_on_terminal_1;
            }
            watch.Stop();
            RhinoApp.WriteLine("Get DijkstraPath Time: {0}; Shift Count is {1}", watch.Elapsed,shift_count);
            
            return new_dijkstra_path;

        }

        public DijkstraPath GetDijkstraPath(DijkstraPathVertex v1, DijkstraPathVertex v2, Guid path_id, bool if_change_weight)
        {
            DijkstraPath d_path = GetDijkstraPath(v1.ver_num, v2.ver_num, if_change_weight);
            d_path.d_ver_1 = v1;
            d_path.d_ver_2 = v2;
            //d_path.path_id = path_id;
            d_path.SetPathID(path_id);
            return d_path;
        }
        
        public int GetVertexListCount() { return vertex_list.Count; }
        
        public int NearVertexOnMesh(int face_index)
        {
            int ori_ver_num = face_list[face_index].A;
            return original_vertex_num_list[ori_ver_num];
        }

        public NurbsCurve DijkstraPath_Add(MeshPoint p1, Guid pin_id_1, MeshPoint p2, Guid pin_id_2)
        {
            int ver_1_num = -1;
            int ver_2_num = -1;
            /*
            for(int i = 0; i < dijkstrapath_list.Count; i++)
            {
                int pin_num = dijkstrapath_list[i].GetPinNum(pin_id_1);
                if(pin_num > 0) { ver_1_num = pin_num; break; }
            }
            for (int i = 0; i < dijkstrapath_list.Count; i++)
            {
                int pin_num = dijkstrapath_list[i].GetPinNum(pin_id_2);
                if (pin_num > 0) { ver_2_num = pin_num; break; }
            }
            if (ver_1_num < 0) { ver_1_num = AddNewVertexOnMesh(p1); }
            if (ver_2_num < 0) { ver_2_num = AddNewVertexOnMesh(p2); }
            */
            bool is_ver_1_exist = false;
            DijkstraPathVertex d_ver_1 = null;
            bool is_ver_2_exist = false;
            DijkstraPathVertex d_ver_2 = null;

            for(int i = 0; i < dijkstrapath_vertex_list.Count; i++)
            {
                if(dijkstrapath_vertex_list[i].pin_id == pin_id_1)
                {
                    is_ver_1_exist = true;
                    d_ver_1 = dijkstrapath_vertex_list[i];
                    break;
                }
            }
            for (int i = 0; i < dijkstrapath_vertex_list.Count; i++)
            {
                if (dijkstrapath_vertex_list[i].pin_id == pin_id_2)
                {
                    is_ver_2_exist = true;
                    d_ver_2 = dijkstrapath_vertex_list[i];
                    break;
                }
            }
            if(d_ver_1 == null)
            {
                ver_1_num = AddNewVertexOnMesh(p1);
                d_ver_1 = new DijkstraPathVertex(pin_id_1, ver_1_num);
                dijkstrapath_vertex_list.Add(d_ver_1);
            }
            if (d_ver_2 == null)
            {
                ver_2_num = AddNewVertexOnMesh(p2);
                d_ver_2 = new DijkstraPathVertex(pin_id_2, ver_2_num);
                dijkstrapath_vertex_list.Add(d_ver_2);
            }
            DijkstraPath new_d_path = null;
            if ( (!is_ver_1_exist) || (!is_ver_2_exist))
            {
                new_d_path = GetDijkstraPath(d_ver_1.ver_num, d_ver_2.ver_num, true);
                d_ver_1.d_edge_list.Add(new_d_path);
                d_ver_2.d_edge_list.Add(new_d_path);
                d_ver_1.neighboor_list.Add(d_ver_2);
                d_ver_2.neighboor_list.Add(d_ver_1);
                new_d_path.d_ver_1 = d_ver_1;
                new_d_path.d_ver_2 = d_ver_2;

                Guid path_id = Guid.NewGuid();
                //new_d_path.pin_1_id = pin_id_1;
                //new_d_path.pin_2_id = pin_id_2;
                new_d_path.SetPathID(path_id);
                dijkstrapath_list.Add(new_d_path);
            }
            else
            {
                if(CheckConnection(d_ver_1, d_ver_2))
                {
                    for (int i = 0; i < dijkstrapath_vertex_list.Count; i++) { dijkstrapath_vertex_list[i].is_visited = false; }
                    RhinoApp.WriteLine("These two pins are already connected!");
                    return null;
                }
                else
                {
                    for (int i = 0; i < dijkstrapath_vertex_list.Count; i++) { dijkstrapath_vertex_list[i].is_visited = false; }
                    new_d_path = GetDijkstraPath(d_ver_1.ver_num, d_ver_2.ver_num, true);
                    d_ver_1.d_edge_list.Add(new_d_path);
                    d_ver_2.d_edge_list.Add(new_d_path);
                    d_ver_1.neighboor_list.Add(d_ver_2);
                    d_ver_2.neighboor_list.Add(d_ver_1);
                    new_d_path.d_ver_1 = d_ver_1;
                    new_d_path.d_ver_2 = d_ver_2;

                    Guid path_id = Guid.NewGuid();
                    //new_d_path.pin_1_id = pin_id_1;
                    //new_d_path.pin_2_id = pin_id_2;
                    new_d_path.SetPathID(path_id);
                    dijkstrapath_list.Add(new_d_path);
                }
            }
            
            return new_d_path.path;
        }

        public List<NurbsCurve> DijkstraPath_Change(Guid pin_id, MeshPoint p)
        {
            for (int i = 0; i < face_count; i++) { weight_list[i] = 1; }
            for (int i = 0; i < dijkstrapath_list.Count; i++) { dijkstrapath_list[i].is_being_changed = false; }
            int ver_num = AddNewVertexOnMesh(p);
            List<NurbsCurve> d_path_list = new List<NurbsCurve>();

            DijkstraPathVertex d_ver_changed = null;
            for (int i = 0; i < dijkstrapath_vertex_list.Count; i++)
            {
                if (dijkstrapath_vertex_list[i].pin_id == pin_id)
                {
                    d_ver_changed = dijkstrapath_vertex_list[i];
                    break;
                }
            }
            if(d_ver_changed == null)
            {
                d_ver_changed = new DijkstraPathVertex(pin_id, ver_num);
            }
            for (int i = 0; i < d_ver_changed.d_edge_list.Count; i++)
            {
                d_ver_changed.d_edge_list[i].is_being_changed = true;
            }

            for (int i = 0; i < dijkstrapath_list.Count; i++)
            {
                if(dijkstrapath_list[i].is_being_changed == false)
                {
                    d_path_list.Add(dijkstrapath_list[i].path);
                    List<int> weight_changed_faces = dijkstrapath_list[i].weight_changed_faces;
                    List<int> weight_changed_faces_on_terminal = dijkstrapath_list[i].weight_changed_faces_on_terminal;
                    for (int j = 0; j < weight_changed_faces.Count; j++)
                    {
                        weight_list[weight_changed_faces[j]] = weight_on_path;
                    }
                    for (int j = 0; j < weight_changed_faces_on_terminal.Count; j++)
                    {
                        weight_list[weight_changed_faces_on_terminal[j]] = weight_on_end;
                    }
                }
            }

            d_ver_changed.ver_num = ver_num;

            for (int i = 0; i < d_ver_changed.d_edge_list.Count; i++)
            {
                DijkstraPath d_edge = d_ver_changed.d_edge_list[i];
                Guid Path_id = d_edge.path_id;
                DijkstraPathVertex d_ver_neighboor = d_edge.GetNeighboor(d_ver_changed);
                DijkstraPath new_path = GetDijkstraPath(d_ver_changed, d_ver_neighboor, Path_id, true);
                d_ver_changed.d_edge_list[i].DeepCopy(new_path);  
                //d_edge = null;
                d_path_list.Add(new_path.path);
            }

                /*
                for (int i = 0; i < dijkstrapath_list.Count; i++)
                {
                    dijkstrapath_list[i].SetPinNum(pin_id, ver_num);
                    DijkstraPath new_d_path = GetDijkstraPath(dijkstrapath_list[i].ver_1_num, dijkstrapath_list[i].ver_2_num, true);
                    new_d_path.path_id = dijkstrapath_list[i].path_id;
                    new_d_path.pin_1_id = dijkstrapath_list[i].pin_1_id;
                    new_d_path.pin_2_id = dijkstrapath_list[i].pin_2_id;
                    new_d_path.path.UserDictionary.Set("path_number", i);
                    dijkstrapath_list[i] = new_d_path;
                    d_path_list.Add(dijkstrapath_list[i].path);
                }*/
             return d_path_list;
        }
        
        public List<NurbsCurve> DijkstraPath_DeletePin(Guid pin_id)
        {
            for (int i = 0; i < face_count; i++) { weight_list[i] = 1; }
            List<NurbsCurve> d_path_list = new List<NurbsCurve>();
            if(dijkstrapath_list.Count == 0) { return d_path_list; }
            DijkstraPathVertex d_ver_changed = null;
            for (int i = 0; i < dijkstrapath_vertex_list.Count; i++)
            {
                if (dijkstrapath_vertex_list[i].pin_id == pin_id)
                {
                    d_ver_changed = dijkstrapath_vertex_list[i];
                    break;
                }
            }

            if (ReferenceEquals(d_ver_changed, null))
            {
                for (int i = 0; i < dijkstrapath_list.Count; i++)
                {
                    d_path_list.Add(dijkstrapath_list[i].path);
                    return d_path_list;
                }
            }

            for (int i = 0; i < d_ver_changed.d_edge_list.Count; i++)
            {
                d_ver_changed.d_edge_list[i].is_being_changed = true;
            }
            for (int i = 0; i < dijkstrapath_list.Count; i++)
            {
                if (dijkstrapath_list[i].is_being_changed == false)
                {
                    d_path_list.Add(dijkstrapath_list[i].path);
                    List<int> weight_changed_faces = dijkstrapath_list[i].weight_changed_faces;
                    List<int> weight_changed_faces_on_terminal = dijkstrapath_list[i].weight_changed_faces_on_terminal;
                    for (int j = 0; j < weight_changed_faces.Count; j++)
                    {
                        weight_list[weight_changed_faces[j]] = weight_on_path;
                    }
                    for (int j = 0; j < weight_changed_faces_on_terminal.Count; j++)
                    {
                        weight_list[weight_changed_faces_on_terminal[j]] = weight_on_end;
                    }
                }
            }
            for(int i = dijkstrapath_list.Count - 1; i >= 0; i--)
            {
                if (dijkstrapath_list[i].is_being_changed)
                {
                    dijkstrapath_list.RemoveAt(i);
                }
            }
            for(int i = 0; i < d_ver_changed.neighboor_list.Count; i++)
            {
                DijkstraPathVertex d_ver = d_ver_changed.neighboor_list[i];
                for(int j = 0; j < d_ver.neighboor_list.Count; j++)
                {
                    if(d_ver.neighboor_list[j] == d_ver_changed)
                    {
                        d_ver.neighboor_list.RemoveAt(j);
                    }
                }
                for(int j = d_ver.d_edge_list.Count - 1; j >= 0; j--)
                {
                    if(d_ver.d_edge_list[j].is_being_changed)
                    {
                        d_ver.d_edge_list[j] = null;
                        d_ver.d_edge_list.RemoveAt(j);
                    }
                }
            }
            d_ver_changed = null;
            
            /*
            
            List<int> num_to_be_deleted = new List<int>();
            for (int i = 0; i < dijkstrapath_list.Count; i++)
            {
                if (dijkstrapath_list[i].GetPinNum(pin_id) > 0) { num_to_be_deleted.Add(i); }
            }
            for(int i = num_to_be_deleted.Count - 1; i >= 0; i--)
            {
                dijkstrapath_list.RemoveAt(num_to_be_deleted[i]);
            }
            List<NurbsCurve> d_path_list = new List<NurbsCurve>();
            for (int i = 0; i < dijkstrapath_list.Count; i++)
            {
                //DijkstraPath new_d_path = GetDijkstraPath(dijkstrapath_list[i].ver_1_num, dijkstrapath_list[i].ver_2_num);
                //NurbsCurve d_path = new_d_path.path;
                //dijkstrapath_list[i].path = d_path;
                //dijkstrapath_list[i].SetPathID(dijkstrapath_list[i].path_id);
                //d_path_list.Add(dijkstrapath_list[i].path);
                d_path_list.Add(dijkstrapath_list[i].path);
            }
            */
            return d_path_list;
        }

        //if pin_id is Guid.Empty, then all the path ids will be returned 
        public List<Guid> DigstraPath_GetPathIDFromPin(Guid pin_id)
        {
            List<Guid> path_id_list = new List<Guid>();
            if(pin_id == Guid.Empty)
            {
                for (int i = 0; i < dijkstrapath_list.Count; i++)
                {
                    Guid pathid = dijkstrapath_list[i].path_id;
                    path_id_list.Add(pathid);
                }
                return path_id_list;
            }
            else
            {
                for (int i = 0; i < dijkstrapath_list.Count; i++)
                {
                    Guid pathid = dijkstrapath_list[i].GetPathID(pin_id);
                    if (pathid != Guid.Empty) { path_id_list.Add(pathid); }
                }
                return path_id_list;
            }
        }  
    }
    

    class Vertex
    {
        public Point3d position;
        public int first_edge_num;
        public bool is_original_vertex;
        public int next_vertex_on_original_edge;
        public bool is_in_queue;
        public double dist;
        public int prev;
        public int min_heap_position;
        public bool is_in_minheap;

        public Vertex(Point3d p)
        {
            position = p;
            is_original_vertex = false;
            next_vertex_on_original_edge = -1;
            first_edge_num = -1;

            prev = -1;
            dist = Double.MaxValue;
            is_in_queue = true;

            min_heap_position = -1;
            is_in_minheap = true;
        }

    }
    
    class Edge
    {
        public int vertex_1_num;
        public int vertex_2_num;
        public double length;
        public int face_1_num;
        public int face_2_num;
        public int next_edge_v1_num;
        public int next_edge_v2_num;

        public Edge(int v1, int v2, double l)
        {
            vertex_1_num = v1;
            vertex_2_num = v2;
            length = l;
            next_edge_v1_num = -1;
            next_edge_v2_num = -1;
            face_1_num = -1;
            face_2_num = -1;
        }
        public int NextEdge(int v)
        {
            if( v == vertex_1_num ) { return next_edge_v1_num; }
            else if ( v == vertex_2_num ) { return next_edge_v2_num; }
            else { return -1; }
        }

        /// <summary>
        /// Must be sure that thisedge.NextEdge(v) == -1
        /// </summary>
        public void SetNextEdge(int v, int e)  
        {
            if (v == vertex_1_num) { next_edge_v1_num = e; }
            else if (v == vertex_2_num) { next_edge_v2_num = e; }
        }
        public int AnotherEnd(int v)
        {
            if (v == vertex_1_num) { return vertex_2_num; }
            else if (v == vertex_2_num) { return vertex_1_num; }
            else { return -1; }
        }
    }
    
    
}

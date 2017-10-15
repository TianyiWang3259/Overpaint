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
    public class DijkstraPath
    {
        //public int ver_1_num;
        //public int ver_2_num;
        //public Guid pin_1_id;
        //public Guid pin_2_id;
        public Guid path_id;
        public Polyline path_polyline;
        public List<int> face_on_path;
        public List<int> edge_on_path;
        public List<int> vertex_on_path;
        public List<int> weight_changed_faces;
        public List<int> weight_changed_faces_on_terminal;
        public int start_face_num;
        public int end_face_num;
        public NurbsCurve path;
        public DijkstraPathVertex d_ver_1;
        public DijkstraPathVertex d_ver_2;
        public bool is_being_changed;
        public DijkstraPath()
        {
            //ver_1_num = -1;
            //ver_2_num = -1;
            //pin_1_id = Guid.Empty;
            //pin_2_id = Guid.Empty;
            is_being_changed = false;
            d_ver_2 = null;
            d_ver_1 = null;
            path_id = Guid.Empty;
            face_on_path = new List<int>();
            edge_on_path = new List<int>();
            vertex_on_path = new List<int>();
            weight_changed_faces = new List<int>();
            weight_changed_faces_on_terminal = new List<int>();
        }
        public void DeepCopy(DijkstraPath d_p_new)
        {
            path_id = d_p_new.path_id;
            path_polyline = d_p_new.path_polyline;
            face_on_path = d_p_new.face_on_path;
            edge_on_path = d_p_new.edge_on_path;
            vertex_on_path = d_p_new.vertex_on_path;
            weight_changed_faces = d_p_new.weight_changed_faces;
            weight_changed_faces_on_terminal = d_p_new.weight_changed_faces_on_terminal;
            start_face_num = d_p_new.start_face_num;
            end_face_num = d_p_new.end_face_num;
            path = d_p_new.path;
            d_ver_1 = d_p_new.d_ver_1;
            d_ver_2 = d_p_new.d_ver_2;
            is_being_changed = d_p_new.is_being_changed;
        }

        /*
        public DijkstraPath(int v1, Guid id1, int v2, Guid id2, NurbsCurve p)
        {
            ver_1_num = v1;
            ver_2_num = v2;
            pin_1_id = id1;
            pin_2_id = id2;
            path = new NurbsCurve(p);
        }
        */

        public DijkstraPathVertex GetNeighboor(DijkstraPathVertex d_ver)
        {
            if (d_ver == d_ver_1) { return d_ver_2; }
            else if (d_ver == d_ver_2) { return d_ver_1; }
            else { return null; }
        }

        public int GetPinNum(Guid id)
        {
            if (id == d_ver_1.pin_id) { return d_ver_1.ver_num; }
            else if (id == d_ver_2.pin_id) { return d_ver_2.ver_num; }
            else { return -1; }
        }
        public bool SetPinNum(Guid id, int v)
        {
            if (id == d_ver_1.pin_id) { d_ver_1.ver_num = v; return true; }
            else if (id == d_ver_2.pin_id) { d_ver_2.ver_num = v; return true; }
            else { return false; }
        }

        public bool SetPathID(Guid id)
        {
            path_id = id;
            path.UserDictionary.Set("PathID", id);
            return true;
        }

        public Guid GetPathID(Guid pin_id)
        {
            if (pin_id == d_ver_1.pin_id || pin_id == d_ver_2.pin_id)
            {
                return path_id;
            }
            else { return Guid.Empty; }
        }

    }

    public class DijkstraPathVertex
    {
        public Guid pin_id;
        public int ver_num;
        public List<DijkstraPathVertex> neighboor_list;
        public List<DijkstraPath> d_edge_list;
        public bool is_visited;
        public DijkstraPathVertex(Guid id, int v)
        {
            pin_id = id;
            ver_num = v;
            neighboor_list = new List<DijkstraPathVertex>();
            d_edge_list = new List<DijkstraPath>();
            is_visited = false;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_Rhino5_rs11_project_8
{
    public class SparseMatrix
    {
        private List<LinkedList<SparseMatrixNode>> sparse_matrix;
        private int height;
        private int width;

        public SparseMatrix(int h, int w)
        {
            height = h;
            width = w;
            sparse_matrix = new List<LinkedList<SparseMatrixNode>>(h);
            for(int i = 0; i < h; i++)
            {
                LinkedList<SparseMatrixNode> ll = new LinkedList<SparseMatrixNode>();
                sparse_matrix.Add(ll);
            }
        }

        public int this [int row, int col]
        {
            get { return GetValue(row,col); }
            set { SetValue(row, col, value); }
        }

        private int GetValue(int row, int col)
        {
            if((row > height) || (col > width)) { return -1; }
            SparseMatrixNode n = new SparseMatrixNode(row, col, 0);
            if(sparse_matrix[row].Find(n) == null) { return -1; }
            SparseMatrixNode m = sparse_matrix[row].Find(n).Value;
            if(m == null) { return -1; }
            return m.value;
        }
        private bool SetValue(int row, int col, int value)
        {
            if ((row > height) || (col > width)) { return false; }
            SparseMatrixNode n = new SparseMatrixNode(row, col, value);
            if (sparse_matrix[row].Find(n) == null)  { sparse_matrix[row].AddLast(n); return true; }
            else { sparse_matrix[row].Find(n).Value.value = value; return true; }
        }


    }

    public class SparseMatrixNode
    {
        public int col;
        public int row;
        public int value;

        public SparseMatrixNode(int r, int c, int v)
        {
            col = c;
            row = r;
            value = v;
        }

        public override bool Equals(object obj)
        {
            if ( obj == null ) { return false; }
            SparseMatrixNode n = obj as SparseMatrixNode;
            if ( n == null ) { return false; }
            return (col == n.col) && (row == n.row);
        }

        public bool Equals(SparseMatrixNode n)
        {
            if ( n == null) { return false; }
            return (col == n.col) && (row == n.row);
        }

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}_{2}", col, row, value).GetHashCode();
        }

        public static bool operator == (SparseMatrixNode n1, SparseMatrixNode n2)
        {
            if (ReferenceEquals(n1, n2)) { return true; }
            if (ReferenceEquals(n1, null) && !ReferenceEquals(n2, null)) { return false; }
            if (!ReferenceEquals(n1, null) && ReferenceEquals(n2, null)) { return false; }
            if (ReferenceEquals(n1, null) && ReferenceEquals(n2, null)) { return true; }
            return (n1.col == n2.col) && (n1.row == n2.row);
        }
        public static bool operator != (SparseMatrixNode n1, SparseMatrixNode n2)
        {
            return !(n1 == n2);
        }
    }
}

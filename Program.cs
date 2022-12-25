using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphs
{
    public static class Calc
    {
        public static double degtorad(double deg)
        {
            return deg * Math.PI / 180;
        }
        public static double radtodeg(double rad)
        {
            return rad / Math.PI * 180;
        }
        public static double lengthdir_x(double len, double dir)
        {
            return len * Math.Cos(degtorad(dir));
        }
        public static double lengthdir_y(double len, double dir)
        {
            return len * Math.Sin(degtorad(dir)) * (-1);
        }
        public static double point_direction(int x1, int y1, int x2, int y2)
        {
            return 180 - radtodeg(Math.Atan2(y1 - y2, x1 - x2));
        }
        public static double point_distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }
    public class Graph
    {
        public List<Pair> graph;
        public int nodes;
        public List<Node> n;
        public float angle;
        public int x = 0;
        public int y = 0;
        public int size = 20;

        public Graph()
        {
            graph = new List<Pair>();
            this.nodes = 0;
            n = new List<Node>();
        }
        public class Node
        {
            public int index;
            public int degree;
            public bool used;
            public int x;
            public int y;

            public Node(int index)
            {
                this.index = index;
                this.used = false;
            }
        }

        public class Pair
        {
            public Node a;
            public Node b;

            public Pair()
            {
            }

            public Pair(Node a, Node b)
            {
                this.a = a;
                this.b = b;
            }

            public bool Contains(int index)
            {
                if (this.a.index == index || this.b.index == index) return true;
                else return false;
            }
        }

        public void StringToEdge(string s)
        {
            string[] pair = s.Split('-');
            bool flag_a = false;
            bool flag_b = false;
            Node a=new Node(Convert.ToInt32(pair[0]));
            Node b= new Node(Convert.ToInt32(pair[1]));
            Pair edge = new Pair();
            foreach (Pair p in graph)
            {
                if (p.Contains(Convert.ToInt32(pair[0]))) { flag_a = true;
                    if (p.a.index == Convert.ToInt32(pair[0])) edge.a = p.a;
                    if (p.b.index == Convert.ToInt32(pair[0])) edge.a = p.b;
                }
                if (p.Contains(Convert.ToInt32(pair[1]))) { flag_b = true;
                    if (p.a.index == Convert.ToInt32(pair[1])) edge.b = p.a;
                    if (p.b.index == Convert.ToInt32(pair[1])) edge.b = p.b;
                }
            }
            if (flag_a == false) { a = new Node(Convert.ToInt32(pair[0])); a.x = this.x; a.y = this.y; edge.a = a; }
            if (flag_b == false) { b = new Node(Convert.ToInt32(pair[1])); b.x = this.x; b.y = this.y; edge.b = b; }
            graph.Add(edge);
        }

        public int FindDegree(int a)
        {
            int degree = 0;
            for (int i=0; i<graph.Count; i++)
            {
                if (graph[i].Contains(a)) degree++;
            }
            return degree;
        }

        public void DSF_Degrees(Node a)
        {
            if (a.used == true) return;
            a.used = true;
            a.degree = FindDegree(a.index);
            foreach (Pair p in graph)
            {
                if (p.a==a) DSF_Degrees(p.b);
            }
        }

        public void CountNodes()
        {
            n = new List<Node>();
            foreach (Pair p in graph)
            {
                if (!n.Contains(p.a)) n.Add(p.a);
                if (!n.Contains(p.b)) n.Add(p.b);
            }
            nodes = n.Count;
            Debug.WriteLine(n.Count);
            angle = 360 / nodes;
        }
        public void BuildGraph(string s)
        {
            string[] edges = s.Split(' ');
            foreach (string e in edges)
            {
                this.StringToEdge(e);
            }
            this.CountNodes();
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

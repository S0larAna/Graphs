using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Graphs
{
    public partial class Form1 : Form
    {
        public Graph Gr = new Graph();
        public int drag = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Gr.n.Count; i++)
            {
                for (int j = 0; j < Gr.n.Count; j++)
                {
                    if (i != j)
                    {
                        double dist = Calc.point_distance(Gr.n[i].x, Gr.n[i].y, Gr.n[j].x, Gr.n[j].y);
                        int add = 10;
                        if (dist <= Gr.size+add)
                        {
                            var rand = new Random();
                            if (Gr.n[i].x == Gr.n[j].x)
                            {
                                if (rand.Next(2) == 1)
                                    Gr.n[i].x += 1;
                                else
                                    Gr.n[i].x -= 1;
                            }
                            if (Gr.n[i].y == Gr.n[j].y)
                            {
                                if (rand.Next(2) == 1)
                                    Gr.n[i].y += 1;
                                else
                                    Gr.n[i].y -= 1;
                            }
                            if (Gr.n[i].x < Gr.n[j].x)
                            {
                                Gr.n[i].x -= (int)(Gr.size + add - dist);
                                Gr.n[j].x += (int)(Gr.size + add - dist);
                            }
                            else
                            {
                                Gr.n[i].x += (int)(Gr.size + add - dist);
                                Gr.n[j].x -= (int)(Gr.size + add - dist);
                            }
                            if (Gr.n[i].y < Gr.n[j].y)
                            {
                                Gr.n[i].y -= (int)(Gr.size + add - dist);
                                Gr.n[j].y += (int)(Gr.size + add - dist);
                            }
                            else
                            {
                                Gr.n[i].y += (int)(Gr.size + add - dist);
                                Gr.n[j].y -= (int)(Gr.size + add - dist);
                            }
                        }
                    }
                }
                if (Gr.n[i].x - Gr.size / 2 < 0) Gr.n[i].x = Gr.size / 2;
                if (Gr.n[i].y - Gr.size / 2 < 0) Gr.n[i].y = Gr.size / 2;
                if (Gr.n[i].x + Gr.size / 2 > pictureBox1.Width) Gr.n[i].x = pictureBox1.Width - Gr.size / 2 - 1;
                if (Gr.n[i].y + Gr.size / 2 > pictureBox1.Height) Gr.n[i].y = pictureBox1.Height - Gr.size / 2 - 1;
            }
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
                Gr.x = pictureBox1.Width / 4;
                Gr.y = pictureBox1.Height / 4;
                Bitmap buffer = new Bitmap(Width, Height);
                Graphics gfx = Graphics.FromImage(buffer);
                Pen myPen = new Pen(Color.Indigo);
            myPen.Width = 5;
                gfx.Clear(Color.White);

                foreach (Graph.Pair pair in Gr.graph)
                {
                    double angle = Calc.point_direction(pair.a.x, pair.a.y, pair.b.x, pair.b.y);
                    double dist = Calc.point_distance(pair.a.x, pair.a.y, pair.b.x, pair.b.y);
                    gfx.DrawLine(myPen,
                                new Point(pair.a.x + (int)Calc.lengthdir_x(Gr.size / 2, angle), pair.a.y + (int)Calc.lengthdir_y(Gr.size / 2, angle)),
                                new Point(pair.a.x + (int)Calc.lengthdir_x(dist - (Gr.size / 2), angle),
                                pair.a.y + (int)Calc.lengthdir_y(dist - (Gr.size / 2), angle)));
                }
                foreach (Graph.Node n in Gr.n)
                {
                SolidBrush myBrush = new SolidBrush(Color.MediumSlateBlue);
                    gfx.FillEllipse(myBrush, new Rectangle(n.x - Gr.size / 2, n.y - Gr.size / 2, Gr.size, Gr.size));
                    gfx.DrawEllipse(myPen, new Rectangle(n.x - Gr.size / 2, n.y - Gr.size / 2, Gr.size, Gr.size));
                    myBrush.Color = Color.White;
                    gfx.DrawString(Convert.ToString(n.index), new Font("Arial", 10, FontStyle.Regular), myBrush, new PointF(n.x - Gr.size / 3, n.y - 10));
                    SolidBrush myBrush1 = new SolidBrush(Color.Black);
                myBrush.Dispose();
                gfx.DrawString(Convert.ToString(n.degree), new Font("Arial", 10, FontStyle.Regular), myBrush1, new PointF(n.x - Gr.size / 3, n.y - 30));
                myBrush1.Dispose();
            }
            pictureBox1.Image = buffer;
                myPen.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gr = new Graph();
            Gr.BuildGraph(textBox1.Text);
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (drag == -1)
                {
                    foreach (Graph.Node n in Gr.n)
                    {
                        if (Calc.point_distance(n.x, n.y, e.X, e.Y) < Gr.size / 2)
                        {
                            drag = n.index;
                            n.x = e.X;
                            n.y = e.Y;
                            break;
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (drag != -1)
            {
                foreach (Graph.Node n in Gr.n)
                {
                    if (drag == n.index)
                    {
                        n.x = e.X;
                        n.y = e.Y;
                        break;
                    }
                }
            }
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                drag = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Graph.Node n in Gr.n)
            {
                Gr.DSF_Degrees(n);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(@"C:\Users\PC\source\repos\Graphs\Graph\G.grf");
            StreamWriter writer = file.CreateText();
            writer.WriteLine(textBox1.Text);
            writer.Close();
        }
    }
}

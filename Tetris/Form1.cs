using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Figure currentFigure;
        int size;
        int[,] map = new int[16, 8];
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Initialize()
        {
            size = 25;

            currentFigure = new Figure(3, 0);

            timer1.Interval = 500;
            timer1.Tick += new EventHandler(update);
            timer1.Start();

            Invalidate();
        }

        private void update(object sender, EventArgs e)
        {
            ResetArea();
            currentFigure.MoveDown();
            Merge();
            Invalidate();
        }

        public void Merge()
        {
            for(int i = currentFigure.y; i < currentFigure.y + 3; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + 3; j++)
                {
                    map[i, j] = currentFigure.form[i - currentFigure.y, j - currentFigure.x];
                }
            }
        }
        public void ResetArea()
        {
            for (int i = currentFigure.y; i < currentFigure.y + 3; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + 3; j++)
                {
                    map[i, j] = 0;
                }
            }
        }
        public void DrawFigure(Graphics e)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(map[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public void DrawPole(Graphics g)
        {
            for (int i = 0; i <= 16; i++)
            {
                g.DrawLine(Pens.Black, new Point(50, 50 + i * size), new Point(50 + 8 * size, 50 + i * size));
            }
            for (int i = 0; i <= 8; i++)
            {
                g.DrawLine(Pens.Black, new Point(50 + i * size, 50), new Point(50 + i * size, 50 + 16 * size));
            }
        }

        private void OnPoint(object sender, PaintEventArgs e)
        {
            DrawPole(e.Graphics);
            DrawFigure(e.Graphics);
        }
    }
}

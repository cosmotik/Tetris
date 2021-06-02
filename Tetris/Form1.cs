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
        int RemovedLines;
        int score;
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
            score = 0;
            RemovedLines = 0;

            currentFigure = new Figure(3, 0);

            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + RemovedLines;

            this.KeyUp += new KeyEventHandler(keyFunc);

            timer1.Interval = 200;
            timer1.Tick += new EventHandler(update);
            timer1.Start();

            Invalidate();
        }

        private void keyFunc(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Space:
                    break;
                case Keys.Right:
                    if(!CollideHorizont(1))
                    {
                        ResetArea();
                        currentFigure.MoveRight();
                        Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Left:
                    if (!CollideHorizont(-1))
                    {
                        ResetArea();
                        currentFigure.MoveLeft();
                        Merge();
                        Invalidate();
                    }
                    break;
            }
        }

        private void update(object sender, EventArgs e)
        {
            ResetArea();
            if(!Collide())
            {
                currentFigure.MoveDown();
            }
            else
            {
                Merge();
                DeleteLine();
                currentFigure = new Figure(3, 0);
            }
            Merge();
            Invalidate();
        }

        public void DeleteLine()
        {
            int count = 0;
            int CurrentRemovedLines = 0;
            for (int i = 0; i < 16; i++)
            {
                count = 0;
                for (int j = 0; j < 8; j++)
                {
                    if(map[i, j] != 0)
                    {
                        count++;
                    }
                }
                if(count == 8)
                {
                    CurrentRemovedLines++;
                    for (int k = i; k >= 1; k--)
                    {
                        for (int o = 0; o < 8; o++)
                        {
                            map[k, o] = map[k - 1, o];
                        }
                    }
                }
            }
            for (int i = 0; i < CurrentRemovedLines; i++)
            {
                score += 10 * (i + 1);
            }
            RemovedLines += CurrentRemovedLines;

            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + RemovedLines;
        }

        public void Merge()
        {
            for(int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if(currentFigure.form[i - currentFigure.y, j - currentFigure.x] != 0)
                    map[i, j] = currentFigure.form[i - currentFigure.y, j - currentFigure.x];
                }
            }
        }
        public bool Collide()
        {
            for (int i = currentFigure.y + currentFigure.sizeMatrix - 1; i >= currentFigure.y; i--)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if(currentFigure.form[i - currentFigure.y, j - currentFigure.x] != 0)
                    {
                        if (i + 1 == 16)
                            return true;
                        if(map[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CollideHorizont(int dir)
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if (currentFigure.form[i - currentFigure.y, j - currentFigure.x] != 0)
                    {
                        if (j + 1 * dir > 7 || j + 1 * dir < 0)
                            return true;

                        if(map[i, j + 1 * dir] != 0)
                        {
                            if(j - currentFigure.x + 1 * dir >= currentFigure.sizeMatrix || j - currentFigure.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (currentFigure.form[i - currentFigure.y, j - currentFigure.x + 1 * dir] == 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ResetArea()
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.sizeMatrix; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.sizeMatrix; j++)
                {
                    if(i >= 0 && j >= 0 && i < 16 && j < 8)
                    {
                        if(currentFigure.form[i - currentFigure.y, j - currentFigure.x] != 0) 
                        { 
                            map[i, j] = 0;
                        }

                    }
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

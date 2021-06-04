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
        int Interval;
        public Form1()
        {
            InitializeComponent();
            this.KeyUp += new KeyEventHandler(keyFunc); // handling keystrokes
            Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Initialize() // initialize variables
        {
            size = 25;
            score = 0;
            RemovedLines = 0;

            currentFigure = new Figure(3, 0);

            Interval = 500;

            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + RemovedLines;

            timer1.Interval = Interval;
            timer1.Tick += new EventHandler(update); // timer processing
            timer1.Start();

            Invalidate(); // Invalidate calls the Paint method.
        }

        private void keyFunc(object sender, KeyEventArgs e) // check which button is pressed
        {
            switch(e.KeyCode)
            {
                case Keys.A:
                    if(!IsIntersects())
                    {
                        ResetArea();
                        currentFigure.RotateFigure();
                        Merge();
                        Invalidate();
                    }
                    break;

                case Keys.Space:
                    timer1.Interval = 10;
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

        public void ShowNextFigure(Graphics e) // shows the next figure from the side
        {
            for (int i = 0; i < currentFigure.NextFigureSize; i++)
            {
                for (int j = 0; j < currentFigure.NextFigureSize; j++)
                {
                    if (currentFigure.NextFigure[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.NextFigure[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.NextFigure[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.NextFigure[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(330 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (currentFigure.NextFigure[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(350 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        private void update(object sender, EventArgs e) // when the timer ticks this function is triggered
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
                timer1.Interval = Interval;
                currentFigure.ResetFigure(3, 0);

                if(Collide()) // if the figure collide when spawning, then we have lost
                {
                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            map[i, j] = 0;
                        }
                    }
                    timer1.Tick -= new EventHandler(update);
                    timer1.Stop();
                    Initialize();
                }
            }
            Merge();
            Invalidate();
        }

        public void DeleteLine() // if the line(lines) is full then the function deletes it
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
            for (int i = 0; i < CurrentRemovedLines; i++) // score: the increase in points multiplied by 10 and multiplied by the number of lines removed.
            {
                score += 10 * (i + 1);
            }
            RemovedLines += CurrentRemovedLines;

            if(RemovedLines % 5 == 0) // speedUp game
            {
                if(Interval > 50)
                Interval -= 10;
            }

            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + RemovedLines;
        }

        public bool IsIntersects() // checks if rotation is possible now, if superimposed on another shape, then rotation is impossible
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.FigureSize; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.FigureSize; j++)
                {
                    if(j >= 0 && j <= 7)
                    {
                        if (map[i, j] != 0 && currentFigure.figure[i - currentFigure.y, j - currentFigure.x] == 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public void Merge() // sync figure with the map
        {
            for(int i = currentFigure.y; i < currentFigure.y + currentFigure.FigureSize; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.FigureSize; j++)
                {
                    if(currentFigure.figure[i - currentFigure.y, j - currentFigure.x] != 0)
                    map[i, j] = currentFigure.figure[i - currentFigure.y, j - currentFigure.x];
                }
            }
        }

        public bool Collide() // checks if a piece is out of bounds of the map, and then see if there are any objects below.
        {
            for (int i = currentFigure.y + currentFigure.FigureSize - 1; i >= currentFigure.y; i--)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.FigureSize; j++)
                {
                    if (currentFigure.figure[i - currentFigure.y, j - currentFigure.x] != 0)
                    {
                        if (i + 1 == 16)
                            return true;
                        if (map[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CollideHorizont(int dir) // go over each element of the figure
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.FigureSize; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.FigureSize; j++)
                {
                    if (currentFigure.figure[i - currentFigure.y, j - currentFigure.x] != 0)
                    {
                        if (j + 1 * dir > 7 || j + 1 * dir < 0)
                            return true;

                        if(map[i, j + 1 * dir] != 0)
                        {
                            if(j - currentFigure.x + 1 * dir >= currentFigure.FigureSize || j - currentFigure.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (currentFigure.figure[i - currentFigure.y, j - currentFigure.x + 1 * dir] == 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public void ResetArea() // will restart the part of the map on which the figure is located, so that the figure retains its shape
        {
            for (int i = currentFigure.y; i < currentFigure.y + currentFigure.FigureSize; i++)
            {
                for (int j = currentFigure.x; j < currentFigure.x + currentFigure.FigureSize; j++)
                {
                    if(i >= 0 && j >= 0 && i < 16 && j < 8)
                    {
                        if(currentFigure.figure[i - currentFigure.y, j - currentFigure.x] != 0) 
                        { 
                            map[i, j] = 0;
                        }

                    }
                }
            }
        }
        public void DrawFigure(Graphics e) // draws figures on the map
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(map[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public void DrawPole(Graphics g) // playing field
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

        private void OnPoint(object sender, PaintEventArgs e) // calling functions for drawing on the form
        {
            DrawPole(e.Graphics);
            DrawFigure(e.Graphics);
            ShowNextFigure(e.Graphics);
        }
    }
}

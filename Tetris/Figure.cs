using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Figure
    {
        public int x;
        public int y;
        public int[,] figure;
        public int[,] NextFigure;
        public int FigureSize;
        public int NextFigureSize;

        public int[,] tetr1 = new int[4, 4]
        {
                {0, 0, 1, 0},
                {0, 0, 1, 0},
                {0, 0, 1, 0},
                {0, 0, 1, 0},
        };

        public int[,] tetr2 = new int[3, 3]
        {
                {0, 2, 0},
                {0, 2, 2},
                {0, 0, 2},
        };

        public int[,] tetr3 = new int[3, 3]
        {
                {0, 0, 0},
                {3, 3, 3},
                {0, 3, 0},
        };

        public int[,] tetr4 = new int[3, 3]
        {
                {4, 0, 0},
                {4, 0, 0},
                {4, 4, 0},
        };

        public int[,] tetr5 = new int[2, 2]
        {
                {5, 5},
                {5, 5},
        };

        public Figure(int coord_x, int coord_y)
        {
            x = coord_x;
            y = coord_y;
            figure = GenerateFigure();
            FigureSize = (int)Math.Sqrt(figure.Length); // calculating the size of the figure
            NextFigure = GenerateFigure();
            NextFigureSize = (int)Math.Sqrt(NextFigure.Length); // calculating the size of the next figure
        }
        public void ResetFigure(int coord_x, int coord_y) // the function resets all values
        {
            x = coord_x;
            y = coord_y;
            figure = NextFigure;
            FigureSize = (int)Math.Sqrt(figure.Length);
            NextFigure = GenerateFigure();
            NextFigureSize = (int)Math.Sqrt(NextFigure.Length);
        }

        public int[,] GenerateFigure() // figures are randomly generated
        {
            int[,] form = tetr1;
            Random r = new Random();
            switch(r.Next(1, 6))
            {
                case 1:
                    form = tetr1;
                    break;
                case 2:
                    form = tetr2;
                    break;
                case 3:
                    form = tetr3;
                    break;
                case 4:
                    form = tetr4;
                    break;
                case 5:
                    form = tetr5;
                    break;
            }
            return form;
        }

        public void RotateFigure() // function to scroll the figure
        {
            int[,] tempFigure = new int[FigureSize, FigureSize];

            for (int i = 0; i < FigureSize; i++)
            {
                for (int j = 0; j < FigureSize; j++)
                {
                    tempFigure[i, j] = figure[j, (FigureSize - 1) - i];
                }
            }
            figure = tempFigure;

            int offset1 = (10 - (x + FigureSize)); // when turning against the wall of the map, it does not throw an exception.
            if (offset1 < 0)
            {
                for (int i = 0; i < Math.Abs(offset1); i++)
                    MoveLeft();
            }
            if (x < 0)
            {
                for (int i = 0; i < Math.Abs(x) + 1; i++)
                    MoveRight();
            }
        }
        public void MoveDown()
        {
            y++;
        }
        public void MoveRight()
        {
            x++;
        }
        public void MoveLeft()
        {
            x--;
        }
    }
}

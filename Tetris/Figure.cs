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
        public int[,] form;

        public Figure(int coord_x, int coord_y)
        {
            x = coord_x;
            y = coord_y;
            form = new int[3, 3]
            {
                {0, 1, 0 },
                {0, 1, 1 },
                {0, 0, 1 },
            };
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

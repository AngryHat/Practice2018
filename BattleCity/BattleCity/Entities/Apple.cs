using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity.Entities
{
    public class Apple : GameObject
    {
        public Apple()
        {
            direction = Direction.right;
            posX = 96;
            posY = 96;
            image = new Bitmap(Properties.Resources.apple, size);
        }

        public Apple(int x, int y) : this()
        {
            posX = x;
            posY = y;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity.Entities
{
    public class Wall : GameObject
    {
        public Wall()
        {
            direction = Direction.right;
            posX = 0;
            posY = 200;
            image = new Bitmap(Properties.Resources.wall, size);
        }

        public Wall(int x, int y) : this()
        {
            posX = x;
            posY = y;
        }
        public Wall(int x, int y, bool spriteSizeBuild) : this(x, y)
        {
            posX = x * size.Width;
            posY = y * size.Height;
        }
    }

}

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
        public bool breakable;
        public int hitsTaken;

        public Wall()
        {
            hitsTaken = 0;
            breakable = true;
            direction = Direction.right;
            posX = 0;
            posY = 200;
            GetWallImage();
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
        public Wall(int x, int y, bool isBreakable, bool spriteSizeBuild) : this(x, y)
        {
            breakable = isBreakable;
            posX = x * size.Width;
            posY = y * size.Height;
        }

        public void GetWallImage()
        {
            if (breakable == true)
            {
                if (hitsTaken == 0)
                {
                    image = new Bitmap(Properties.Resources.wall, size);
                }
                else if (hitsTaken == 1)
                {
                    image = new Bitmap(Properties.Resources.wall2, size);
                }
                else { image =  new Bitmap(Properties.Resources.wall3, size); }
            }
            else image =  new Bitmap(Properties.Resources.wall, size);
        }

    }
}

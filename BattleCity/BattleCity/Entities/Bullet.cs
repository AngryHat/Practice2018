using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity.Entities
{
    public class Bullet : GameObject
    {
        public Bullet()
        {
            size = new Size(12, 12);
            image = new Bitmap(Properties.Resources.bullet, size);

            dynamicImagesArr = new Image[4];
            dynamicImagesArr[0] = new Bitmap(RotateImage(image, 270));
            dynamicImagesArr[1] = new Bitmap(RotateImage(image, 180));
            dynamicImagesArr[2] = new Bitmap(RotateImage(image, 90));
            dynamicImagesArr[3] = new Bitmap(image);
        }
        public Bullet(GameObject shooter) : this()
        {
            direction = shooter.direction;
            if (shooter.direction == Direction.up)
            {
                posY = shooter.posY;
                posX = shooter.posX + (shooter.image.Width / 2 - image.Width / 2);
            }
            else if (shooter.direction == Direction.down)
            {
                posY = shooter.posY + (shooter.image.Width);
                posX = shooter.posX + (shooter.image.Width / 2 - image.Width / 2);
            }
            else if (shooter.direction == Direction.left)
            {
                posX = shooter.posX;
                posY = shooter.posY + (shooter.image.Width / 2 - image.Width / 2);
            }
            else if (shooter.direction == Direction.right)
            {
                posX = shooter.posX + (shooter.image.Width);
                posY = shooter.posY + (shooter.image.Width / 2 - image.Width / 2);
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity
{
    public abstract class GameObject
    {
        public enum Direction { right, left, up, down };
        public Direction direction;

        public Size size = new Size(48,48);
        public Image image;

        public Image dynamicImage
        {
            get
            {
                switch (direction)
                {
                    case Direction.right:
                        return RotateImage(image, 270);

                    case Direction.up:
                        return RotateImage(image, 180);

                    case Direction.left:
                        return RotateImage(image, 90);

                    default:
                        return image;
                }
            }
        }

        public Point pos
        {
            get
            {
                return new Point(posX, posY);
            }
        }

        public int posX;
        public int posY;

        public int topBorder
        {
            get { return posY; }
        }
        public int leftBorder
        {
            get { return posX; }
        }
        public int rightBorder
        {
            get { return (posX + image.Size.Height); }
        }
        public int bottomBorder
        {
            get { return (posY + image.Size.Height); }
        }

        //turn 180 degrees
        public void TurnAround()
        {
            switch (direction)
            {
                case Direction.down:
                    direction = Direction.up;
                    break;
                case Direction.up:
                    direction = Direction.down;
                    break;
                case Direction.left:
                    direction = Direction.right;
                    break;
                case Direction.right:
                    direction = Direction.left;
                    break;
            }
        }

        //ROTATOR
        public static Image RotateImage(Image image, float angle)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
                g.DrawImage(image, new Point(0, 0));
            }
            return result;
        }
    }

    // playes class
    public class Player : GameObject
    {
        public Player()
        {
            direction = Direction.up;
            posX = 48;
            posY = 360;
            image = new Bitmap(Properties.Resources.player, size);
        }
    }

    // enemy class
    public class Enemy : GameObject
    {
        public int directionStep;
        public Enemy()
        {
            direction = Direction.right;
            posX = 48;
            posY = 48;
            image = new Bitmap(Properties.Resources.enemy, size);
        }
        public Enemy(int x, int y) : this()
        {
            posX = x;
            posY = y;
        }
    }

    //BULLETS
    public class Bullet : GameObject
    {
        public Bullet()
        {
            image = new Bitmap(Properties.Resources.bullet, size);
        }
        public Bullet(GameObject stooter) :this()
        {
            direction = stooter.direction;
            posX = stooter.posX;
            posY = stooter.posY;
        }
    }

    // obstacle classes
    public class Wall : GameObject
    {
        public Wall()
        {
            direction = Direction.right;
            posX = 0;
            posY = 200;
            image = new Bitmap(Properties.Resources.wall, size);
        }

        public Wall(int x, int y) :this()
        {
            posX = x;
            posY = y;
        }
    }
}

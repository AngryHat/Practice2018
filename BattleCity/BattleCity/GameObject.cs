using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity
{
    public abstract class GameObject
    {
        public enum Direction { right, left, up, down };
        public Direction direction;
        public Rectangle collider {
            get
            {

                return new Rectangle(posX, posY, image.Width, image.Height);
            }
        }

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
            get
            {
                if (direction == Direction.up)
                {
                    return (posY - 5);
                }
                else return posY;
            }
        }
        public int leftBorder
        {
            get
            {
                if (direction == Direction.left)
                {
                    return (posX - 5);
                }
                else return posX;
            }
        }
        public int rightBorder
        {
            get
            {
                if (direction == Direction.right)
                {
                    return (posX + image.Size.Width + 5);
                }
                else return (posX + image.Size.Width);
            }
        }
        public int bottomBorder
        {
            get
            {
                if (direction == Direction.down)
                {
                    return (posY + image.Size.Height + 5);
                }
                else return (posY + image.Size.Height);
            }
        }

        //turn 180 degrees
        public virtual void TurnAround()
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
            direction = Direction.right;
            posX = 48;
            posY = 720;
            image = new Bitmap(Properties.Resources.player, size);
        }
    }

    // enemy class
    public class Enemy : GameObject
    {
        //own features
        public int directionStep;
        public Timer turnaroundTimer;
        public bool turnaroundReady;


        public Enemy()
        {
            direction = Direction.right;
            //default position
            posX = 96;
            posY = 96;
            image = new Bitmap(Properties.Resources.enemy, size);

            //testing value 
            turnaroundTimer = new Timer();
            turnaroundTimer.Interval = 200;
            turnaroundTimer.Start();
            turnaroundTimer.Tick += resetTurnaroundCooldown;
            turnaroundReady = true;
        }
        public Enemy(int x, int y) : this()
        {
            posX = x;
            posY = y;
        }

        public void resetTurnaroundCooldown(object sender, EventArgs e)
        {
            turnaroundReady = true;
        }

        public override void TurnAround()
        {
            if (turnaroundReady)
            {
                switch (direction)
                {
                    case Direction.down:
                        direction = Direction.up;
                        turnaroundReady = false;
                        turnaroundTimer.Start();
                        break;
                    case Direction.up:
                        direction = Direction.down;
                        turnaroundReady = false;
                        turnaroundTimer.Start();
                        break;
                    case Direction.left:
                        direction = Direction.right;
                        turnaroundReady = false;
                        turnaroundTimer.Start();
                        break;
                    case Direction.right:
                        direction = Direction.left;
                        turnaroundReady = false;
                        turnaroundTimer.Start();
                        break;
                }
            }
        }
    }

    //BULLETS
    public class Bullet : GameObject
    {
        public Bullet()
        {
            size = new Size(12, 12);
            image = new Bitmap(Properties.Resources.bullet, size);
        }
        public Bullet(GameObject shooter) :this()
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

    // apple classes
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
        public Wall(int x, int y, bool spriteSizeBuild) : this(x,y)
        {
            posX = x * size.Width;
            posY = y * size.Height;
        }
    }
}

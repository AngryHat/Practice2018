﻿using System;
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



    //BULLETS
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity.Entities
{
    public class Tank : GameObject
    {
        //own features
        public int directionStep;
        public Timer turnaroundTimer;
        public Timer bulletCooldownTimer;
        int bulletCooldownTimerTick;
        int turnaroundCooldownTimerTick;
        public bool turnaroundReady;
        public List<Bullet> bullets;


        public Tank()
        {
            direction = Direction.right;

            posX = 96;
            posY = 96;
            image = new Bitmap(Properties.Resources.enemy, size);
            
            turnaroundReady = true;

            bullets = new List<Bullet>();

            dynamicImagesArr = new Image[4];
            dynamicImagesArr[0] = new Bitmap(RotateImage(image, 270));
            dynamicImagesArr[1] = new Bitmap(RotateImage(image, 180));
            dynamicImagesArr[2] = new Bitmap(RotateImage(image, 90));
            dynamicImagesArr[3] = new Bitmap(image);
        }
        public Tank(int x, int y) : this()
        {
            posX = x;
            posY = y;
        }

        public void bulletCooldownCheck()
        {
            bulletCooldownTimerTick++;
            if (bulletCooldownTimerTick > 400 / MainForm.GameSpeed)
            {
                Shoot();
            }
        }
        public void turnaroundCooldownCheck()
        {
            turnaroundCooldownTimerTick++;
            if (turnaroundCooldownTimerTick > 20)
            {
                turnaroundReady = true;
            }
        }
        public void Shoot()
        {
            bulletCooldownTimerTick = 0;
            bullets.Add(new Bullet(this));
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
                        turnaroundCooldownTimerTick = 0;
                        break;
                    case Direction.up:
                        direction = Direction.down;
                        turnaroundReady = false;
                        turnaroundCooldownTimerTick = 0;
                        break;
                    case Direction.left:
                        direction = Direction.right;
                        turnaroundReady = false;
                        turnaroundCooldownTimerTick = 0;
                        break;
                    case Direction.right:
                        direction = Direction.left;
                        turnaroundReady = false;
                        turnaroundCooldownTimerTick = 0;
                        break;
                }
            }
        }
        public void Move()
        {
            switch (direction)
            {
                case GameObject.Direction.right:
                    posX += MainForm.GameSpeed;
                    break;

                case GameObject.Direction.left:
                    posX -= MainForm.GameSpeed;
                    break;

                case GameObject.Direction.up:
                    posY -= MainForm.GameSpeed;
                    break;

                case GameObject.Direction.down:
                    posY += MainForm.GameSpeed;
                    break;
            }
        }
        public void CheckLevelBounds()
        {
            if (leftBorder < MainForm.spriteSize)
            {
                posX = MainForm.spriteSize;
                TurnAround();
            }
            else if (rightBorder > MainForm.mainFrame.Width - MainForm.spriteSize)
            {
                posX = MainForm.mainFrame.Width - image.Width - MainForm.spriteSize;
                TurnAround();
            }
            else if (topBorder < 0 + MainForm.spriteSize)
            {
                posY = 0 + MainForm.spriteSize;
                TurnAround();
            }
            else if (bottomBorder > MainForm.mainFrame.Height - MainForm.spriteSize)
            {
                posY = MainForm.mainFrame.Height - image.Height - MainForm.spriteSize;
                TurnAround();
            }
        }
    }
}

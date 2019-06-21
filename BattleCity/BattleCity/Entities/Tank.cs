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
        public bool turnaroundReady;
        public List<Bullet> bullets;


        public Tank()
        {
            direction = Direction.right;

            posX = 96;
            posY = 96;
            image = new Bitmap(Properties.Resources.enemy, size);

            turnaroundTimer = new Timer();
            turnaroundTimer.Interval = 200;
            turnaroundTimer.Start();
            turnaroundTimer.Tick += resetTurnaroundCooldown;
            turnaroundReady = true;

            bullets = new List<Bullet>();
            bulletCooldownTimer = new Timer();
            bulletCooldownTimer.Interval = 4000 / MainForm.GameSpeed;
            bulletCooldownTimer.Tick += Shoot;
            bulletCooldownTimer.Start();

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

        public void resetTurnaroundCooldown(object sender, EventArgs e)
        {
            turnaroundReady = true;
            turnaroundTimer.Start();

        }
        public void Shoot(object sender, EventArgs e)
        {
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

}

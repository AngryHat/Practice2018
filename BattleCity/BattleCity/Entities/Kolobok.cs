using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity.Entities
{
    public class Kolobok : GameObject
    {
        public List<Bullet> bullets;
        public bool bulletReady;
        int bulletCooldownTickCount;



        public Kolobok()
        {
            bullets = new List<Bullet>();
            bulletReady = true;
            bulletCooldownTickCount = 0;


            direction = Direction.right;
            posX = 48;
            posY = 720;
            image = new Bitmap(Properties.Resources.player, size);

            dynamicImagesArr = new Image[4];
            dynamicImagesArr[0] = new Bitmap(RotateImage(image, 270));
            dynamicImagesArr[1] = new Bitmap(RotateImage(image, 180));
            dynamicImagesArr[2] = new Bitmap(RotateImage(image, 90));
            dynamicImagesArr[3] = new Bitmap(image);
        }

        public void Shoot()
        {
            if (bulletReady)
            {
                bulletCooldownTickCount = 0;
                bullets.Add(new Bullet(this));
                bulletReady = false;
            }
        }
        public void checkShootingCooldown(object sender, EventArgs e)
        {
            bulletCooldownTickCount++;
            if(bulletCooldownTickCount > 50 / MainForm.GameSpeed)
            {
                bulletReady = true;
            }
        }
        public void ChangeDirection(string input)
        {
            switch (input)
            {
                case "d":
                    direction = GameObject.Direction.right;
                    break;

                case "a":
                    direction = GameObject.Direction.left;
                    break;

                case "w":
                    direction = GameObject.Direction.up;
                    break;

                case "s":
                    direction = GameObject.Direction.down;
                    break;
                case " ":
                    Shoot();
                    break;
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
            if (leftBorder < 0)
            {
                posX = 0;
                TurnAround();
            }
            else if (rightBorder > MainForm.mainFrame.Width)
            {
                posX = MainForm.mainFrame.Width - image.Width;
                TurnAround();
            }
            else if (topBorder < 0)
            {
                posY = 0;
                TurnAround();
            }
            else if (bottomBorder > MainForm.mainFrame.Height)
            {
                posY = MainForm.mainFrame.Height - image.Height;
                TurnAround();
            }
        }
    }

}

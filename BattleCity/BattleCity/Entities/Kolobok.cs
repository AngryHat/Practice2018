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
        public Timer bulletCooldownTimer;
        public List<Bullet> bullets;
        public bool bulletReady;

        
        public Kolobok()
        {
            bullets = new List<Bullet>();
            bulletCooldownTimer = new Timer();
            bulletCooldownTimer.Interval = 1000 / MainForm.GameSpeed;
            bulletCooldownTimer.Tick += resetShootingCooldown;
            bulletCooldownTimer.Start();
            bulletReady = true;

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
                bullets.Add(new Bullet(this));
                bulletCooldownTimer.Start();
                bulletReady = false;
            }
        }
        public void resetShootingCooldown(object sender, EventArgs e)
        {
            bulletReady = true;
            bulletCooldownTimer.Start();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity.Entities
{
    class Explosion
    {
        public static int spriteSize = 48;
        public Size size = new Size(48, 48);
        public int posX;
        public int posY;
        public Point pos
        {
            get
            {
                return new Point(posX, posY);
            }
        }
        public static Image spriteImage = Properties.Resources.explosion;
        public Image image
        {
            get
            {
                if (currentFrame < animationFrames.Count)
                {
                    return animationFrames[currentFrame];
                }
                else return animationFrames[animationFrames.Count - 1];
            }
        }
        public List<Image> animationFrames = GetAnimationFrames();
        int currentFrame;



        public Explosion()
        {
            
            currentFrame = 0;
            posX = -48;
            posY = -48;
        }
        public Explosion(int x, int y) :this()
        {
            posX = x;
            posY = y;
        }

        public void showExolosionAnimation()
        {
            if (currentFrame < animationFrames.Count)
            {
                currentFrame++;
            }
            else
            {
                posX = -50;
                posY = -50;
            }
        }

        public static List<Image> GetAnimationFrames()
        {
            List<Image> result = new List<Image>();

            for (int i = 0; i < spriteImage.Width / spriteSize; i++)
            {
                Rectangle rect = new Rectangle(i * spriteSize, 0, spriteSize, spriteSize);
                result.Add(GameObject.cropImage(spriteImage, rect));
            }
            for (int i = 0; i < spriteImage.Width / spriteSize; i++)
            {
                Rectangle rect = new Rectangle(i * spriteSize, spriteSize, spriteSize, spriteSize);
                result.Add(GameObject.cropImage(spriteImage, rect));
            }

            return result;
        }
    }
}

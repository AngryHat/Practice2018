using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity
{
    public partial class MainForm : Form
    {
        private PictureBox mainFrame;

        int levelWidth = 16;
        int levelHeight = 16;
        int spriteSize = 48;
        int enemiesMax = 5;

        static int speed = 2;

        public Player pl;
        public List<GameObject> enemies;
        public List<GameObject> walls;
        public List<GameObject> playerBullets;
        public List<GameObject> enemyBullets;

        public int directionStep;
        public bool playerBulletReady;
        public bool enemyBulletReady;

        //not using yet
        bool GameOver;

        public MainForm()
        {
            mainFrame = new PictureBox();

            Timer timer = new Timer();
            timer.Interval = 10;
            Timer bulletCooldown = new Timer();
            bulletCooldown.Interval = 1500;
            Timer enemiesRespawn = new Timer();
            bulletCooldown.Interval = 2000;

            GenerateLeveBackGround();
            pl = new Player();
            enemies = new List<GameObject>();


            //creating wall around level
            walls = new List<GameObject>();
            for (int i = 0; i < levelWidth; i++)
            {
                walls.Add(new Wall(i * spriteSize, 0));
                walls.Add(new Wall(i * spriteSize, levelHeight * spriteSize - spriteSize));
            }
            for (int i = 0; i < levelHeight; i++)
            {
                walls.Add(new Wall(0, i * spriteSize));
                walls.Add(new Wall(levelHeight * spriteSize - spriteSize, i * spriteSize));
            }

            playerBullets = new List<GameObject>();

            GameOver = false;

            mainFrame.Paint += new PaintEventHandler(PaintPlayer);
            mainFrame.Paint += new PaintEventHandler(PaintEnemy);
            mainFrame.Paint += new PaintEventHandler(PaintWalls);
            mainFrame.Paint += new PaintEventHandler(PaintBullets);

            Controls.Add(mainFrame);
            InitializeComponent();

            timer.Start();
            timer.Tick += new EventHandler(_update);
            bulletCooldown.Start();
            bulletCooldown.Tick += new EventHandler(resetCooldown);
            enemiesRespawn.Start();
            bulletCooldown.Tick += new EventHandler(GenerateEnemies);
        }

        //Creating field woth backgorund and size
        void GenerateLeveBackGround()
        {
            mainFrame.BackgroundImage = new Bitmap(Properties.Resources.grass);
            mainFrame.Width = spriteSize * levelWidth;
            mainFrame.Height = spriteSize * levelHeight;
        }

        void GenerateEnemies(object sender, EventArgs e)
        {
            if (enemies.Count < enemiesMax)
            {
                Random rnd = new Random();
                enemies.Add(new Enemy((rnd.Next(levelWidth * spriteSize - 100) + 50), (rnd.Next(levelHeight * spriteSize) - 100) + 50));
            }
        }

        //Painting player
        private void PaintPlayer(object sender, PaintEventArgs e)
        {
            if (pl != null)
            {
                e.Graphics.DrawImage(pl.dynamicImage, pl.pos);
            }
        }
        //Painting enemies
        private void PaintEnemy(object sender, PaintEventArgs e)
        {
            if (enemies != null)
            {
                foreach (Enemy en in enemies)
                {
                    e.Graphics.DrawImage(en.dynamicImage, en.pos);
                }
            }
        }
        //Painting bullets
        private void PaintBullets(object sender, PaintEventArgs e)
        {
            if (playerBullets != null)
            {
                foreach (Bullet bullet in playerBullets)
                {
                    e.Graphics.DrawImage(bullet.dynamicImage, bullet.pos);
                }
            }
            if (enemyBullets != null)
            {
                foreach (Bullet bullet in enemyBullets)
                {
                    e.Graphics.DrawImage(bullet.dynamicImage, bullet.pos);
                }
            }
        }
        //Painting walls
        private void PaintWalls(object sender, PaintEventArgs e)
        {
            if (walls != null)
            {
                foreach (Wall wall in walls)
                {
                    e.Graphics.DrawImage(wall.image, wall.pos);
                }
            }
        }

        void PlayerShoot()
        {
            if (playerBulletReady)
            {
                playerBullets.Add(new Bullet(pl));
                playerBulletReady = false;
            }
        }

        //bullet cooldown
        void resetCooldown(object sender, EventArgs e)
        {
            playerBulletReady = true;

        }
        //update func
        void _update(object sender, EventArgs e)
        {
            GenerateEnemies(sender, e);
            //player movement
            switch (pl.direction)
            {
                case GameObject.Direction.right:
                    pl.posX += speed;
                    break;

                case GameObject.Direction.left:
                    pl.posX -= speed;
                    break;

                case GameObject.Direction.up:
                    pl.posY -= speed;
                    break;

                case GameObject.Direction.down:
                    pl.posY += speed;
                    break;
            }
            //player collides level boundaries
            if ((pl.leftBorder == 0) || (pl.rightBorder == mainFrame.Width) || (pl.topBorder == 0) || (pl.bottomBorder == mainFrame.Height))
            {
                pl.TurnAround();
            }
            //bullet movement
            foreach (Bullet bullet in playerBullets)
            {
                int bulletSpeed = 2 * speed;
                switch (bullet.direction)
                {
                    case GameObject.Direction.right:
                        bullet.posX += bulletSpeed;
                        break;

                    case GameObject.Direction.left:
                        bullet.posX -= bulletSpeed;
                        break;

                    case GameObject.Direction.up:
                        bullet.posY -= bulletSpeed;
                        break;

                    case GameObject.Direction.down:
                        bullet.posY += bulletSpeed;
                        break;
                }
            }
            //removing bullets
            for (int i = 0; i < playerBullets.Count; i++)
            {
                if (playerBullets[i].posX == levelWidth || playerBullets[i].posX == 0 || playerBullets[i].posY == levelHeight || playerBullets[i].posY == 0)
                {
                    playerBullets.Remove(playerBullets[i]);
                }
            }


            //enemy movement
            foreach (Enemy en in enemies)
            {
                en.directionStep += speed;
                switch (en.direction)
                {
                    case GameObject.Direction.right:
                        en.posX += speed;
                        break;

                    case GameObject.Direction.left:
                        en.posX -= speed;
                        break;

                    case GameObject.Direction.up:
                        en.posY -= speed;
                        break;

                    case GameObject.Direction.down:
                        en.posY += speed;
                        break;
                }
            }
            //enemy random direction
            foreach (Enemy en in enemies)
            {
                if (en.directionStep >= en.image.Width)
                {
                    en.directionStep = 0;

                    Random rnd = new Random();
                    int newDirection = rnd.Next(1, 6);
                    switch (newDirection)
                    {
                        case 1:
                            en.direction = GameObject.Direction.down;
                            break;

                        case 2:
                            en.direction = GameObject.Direction.up;
                            break;

                        case 3:
                            en.direction = GameObject.Direction.left;
                            break;

                        case 4:
                            en.direction = GameObject.Direction.right;
                            break;
                        default:
                            break;
                    }
                }
            }

            //enemy collides level boundaries
            foreach (Enemy en in enemies)
            {
                if ((en.leftBorder == 0) || (en.rightBorder == mainFrame.Width) || (en.topBorder == 0) || (en.bottomBorder == mainFrame.Height))
                {
                    en.TurnAround();
                }
            }
                
            //colliders at work
            for (int i = 0; i < playerBullets.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j ++)
                {
                    if (boxCollides(enemies[j], playerBullets[i]))
                    {
                        enemies.Remove(enemies[j]);
                        //enemy down
                        //boom animation must be here
                    }
                }
            }
            foreach (Enemy en in enemies)
            {
                foreach (Enemy en2 in enemies)
                {
                    if (boxCollides(en, en2))
                    {
                        en.TurnAround();
                        en2.TurnAround();
                    }
                }
            }

            //end of update func
            mainFrame.Refresh();
        }


        //player WASD control
        private void PlayerDirectionControl(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar.ToString())
            {
                case "d":
                    pl.direction = GameObject.Direction.right;
                    break;

                case "a":
                    pl.direction = GameObject.Direction.left;
                    break;

                case "w":
                    pl.direction = GameObject.Direction.up;
                    break;

                case "s":
                    pl.direction = GameObject.Direction.down;
                    break;
                case " ":
                    PlayerShoot();
                    break;
            }
        }

        //collisions
        bool collides(int leftBorder, int topBorder, int rightBorder, int bottomBorder, int leftBorder2, int topBorder2, int rightBorder2, int bottomBorder2)
        {
            return !(rightBorder <= leftBorder2 || leftBorder > rightBorder2 || bottomBorder <= topBorder2 || topBorder > bottomBorder2);
        }

        bool boxCollides(GameObject obj1, GameObject obj2)
        {
            Point pos1 = obj1.pos;
            Point pos2 = obj2.pos;
            Image image1 = obj1.image;
            Image image2 = obj2.image;

            return collides(pos1.X, pos1.Y, pos1.X + image1.Width, pos1.Y + image1.Height,
                            pos2.X, pos2.Y, pos2.X + image1.Width, pos2.Y + image1.Height);
        }
    }
}

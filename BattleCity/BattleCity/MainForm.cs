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

        int levelWidth = 14;
        int levelHeight = 14;
        int spriteSize = 48;
        int enemiesMax = 6;
        int applesMax = 3;

        static int speed = 2;

        public Player pl;
        public List<GameObject> enemies;
        public List<GameObject> walls;
        public List<GameObject> playerBullets;
        public List<GameObject> apples;

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
            bulletCooldown.Interval = 1000 / speed;
            Timer enemiesRespawn = new Timer();
            enemiesRespawn.Interval = 1000;

            GenerateLeveBackGround();
            pl = new Player();
            enemies = new List<GameObject>();
            walls = new List<GameObject>();
            apples = new List<GameObject>();
            
            walls.Add(new Wall(100, 100));
            walls.Add(new Wall(200, 200));
            walls.Add(new Wall(300, 300));

            //creating wall around level
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
            mainFrame.Paint += new PaintEventHandler(PaintApples);

            Controls.Add(mainFrame);
            InitializeComponent();

            timer.Start();
            timer.Tick += new EventHandler(_update);
            bulletCooldown.Start();
            bulletCooldown.Tick += new EventHandler(resetCooldown);
            enemiesRespawn.Start();
            enemiesRespawn.Tick += new EventHandler(GenerateEnemies);
        }
        
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
                enemies.Add(new Enemy((rnd.Next(mainFrame.Width - spriteSize * 3) + spriteSize),
                                      (rnd.Next(mainFrame.Height - spriteSize * 3) + spriteSize)));
            }
        }
        void GenerateApples(object sender, EventArgs e)
        {
            if (apples.Count < applesMax)
            {
                Random rnd = new Random();
                apples.Add(new Apple((rnd.Next(mainFrame.Width - spriteSize * 3) + spriteSize),
                                      (rnd.Next(mainFrame.Height - spriteSize * 3) + spriteSize)));
            }
        }

        //Painting section
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
        //Painting apples
        private void PaintApples(object sender, PaintEventArgs e)
        {
            if (apples != null)
            {
                foreach (Apple apple in apples)
                {
                    e.Graphics.DrawImage(apple.image, apple.pos);
                }
            }
        }

        //shooting section
        void PlayerShoot()
        {
            if (playerBulletReady)
            {
                playerBullets.Add(new Bullet(pl));
                playerBulletReady = false;
            }
        }
        void resetCooldown(object sender, EventArgs e)
        {
            playerBulletReady = true;
        }


        //UPDATE func
        void _update(object sender, EventArgs e)
        {
            GenerateEnemies(sender, e);
            GenerateApples(sender, e);

            //MOVEMENT SECTION

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

            //PLAYER SECTION

            //player reach level boundaries !WORKS
            if (pl.leftBorder < 0)
            {
                pl.posX = 0;
                pl.TurnAround();
            }
            else if (pl.rightBorder > mainFrame.Width)
            {
                pl.posX = mainFrame.Width - pl.image.Width;
                pl.TurnAround();
            }
            else if (pl.topBorder < 0)
            {
                pl.posY = 0;
                pl.TurnAround();
            }
            else if (pl.bottomBorder > mainFrame.Height)
            {
                pl.posY = mainFrame.Height - pl.image.Height;
                pl.TurnAround();
            }
            //player - wall collision !WORKS
            foreach (Wall wall in walls)
            {
                if (boxCollides(pl, wall))
                {
                    if (pl.direction == GameObject.Direction.left)
                    {
                        pl.posX = wall.rightBorder;
                    }
                    else if (pl.direction == GameObject.Direction.right)
                    {
                        pl.posX = wall.leftBorder - pl.image.Width;
                    }
                    if (pl.direction == GameObject.Direction.up)
                    {
                        pl.posY = wall.bottomBorder;
                    }
                    else if (pl.direction == GameObject.Direction.down)
                    {
                        pl.posY = wall.topBorder - pl.image.Height;
                    }
                    pl.TurnAround();
                }
            }
            //player - apple collision //SCORE+
            for (int i = 0; i < apples.Count; i++)
            {
                if (boxCollides(pl, apples[i]))
                {
                    apples.Remove(apples[i]);
                    //NOT FINISHED YET
                }
            }

            //ENEMY SECTION

            //enemy reach level boundaries !WORKS
            foreach (Enemy en in enemies)
            {
                if (en.leftBorder < 0)
                {
                    en.posX = 0;
                    en.TurnAround();
                }
                else if (en.rightBorder > mainFrame.Width)
                {
                    en.posX = mainFrame.Width - en.image.Width;
                    en.TurnAround();
                }
                else if (en.topBorder < 0)
                {
                    en.posY = 0;
                    en.TurnAround();
                }
                else if (en.bottomBorder > mainFrame.Height)
                {
                    en.posY = mainFrame.Height - en.image.Height;
                    en.TurnAround();
                }
            }
            //enemy random direction !!!CHECK NEED
            foreach (Enemy en in enemies)
            {
                if (en.directionStep >= en.image.Width)
                {
                    en.directionStep = 0;

                    Random rnd = new Random();
                    int newDirection = rnd.Next(1, 7);
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
            //enemy - wall colission !WORKS
            foreach (Enemy en in enemies)
            {
                foreach (Wall wall in walls)
                {
                    if (boxCollides(en, wall))
                    {
                        en.TurnAround();
                    }
                }
            }
            //enemy - enemy collision ???
            foreach (Enemy en in enemies)
            {
                foreach (Enemy en2 in enemies)
                {
                    if (boxCollides(en, en2) && en != en2)
                    {
                        en.TurnAround();
                    }
                }
            }



            //BULLETS SECTION

            // removing bullets !WORKS
            for (int i = 0; i < playerBullets.Count; i++)
            {
                if (playerBullets[i].posX == levelWidth || playerBullets[i].posX == 0 || playerBullets[i].posY == levelHeight || playerBullets[i].posY == 0)
                {
                    playerBullets.Remove(playerBullets[i]);
                }
            }
            // bullet - enemy collision !WORKS
            for (int i = 0; i < playerBullets.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j ++)
                {
                    if (boxCollides(enemies[j], playerBullets[i]))
                    {
                        enemies.Remove(enemies[j]);
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

        //COLLISIOONS SECTION

        //VER1
        //bool collides(int leftBorder, int topBorder, int rightBorder, int bottomBorder, int leftBorder2, int topBorder2, int rightBorder2, int bottomBorder2)
        //{
        //    return !(rightBorder <= leftBorder2 || leftBorder > rightBorder2 || bottomBorder <= topBorder2 || topBorder > bottomBorder2);
        //}

        //bool boxCollides(GameObject obj1, GameObject obj2)
        //{
        //    Point pos1 = obj1.pos;
        //    Point pos2 = obj2.pos;
        //    Image image1 = obj1.image;
        //    Image image2 = obj2.image;

        //    return collides(obj1.leftBorder, obj1.topBorder, obj1.rightBorder, obj1.bottomBorder, obj2.leftBorder, obj2.topBorder,obj2.rightBorder,obj2.bottomBorder);
        //}

        //VER2
        bool boxCollides(GameObject obj1, GameObject obj2)
        {
            if (obj1.collider.IntersectsWith(obj2.collider))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

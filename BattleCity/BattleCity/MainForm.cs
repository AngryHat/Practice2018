using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public partial class MainForm : Form
    {
        private PictureBox mainFrame;
        PictureBox menuBox;
        PictureBox startButton;

        int levelWidth = 17;
        int levelHeight = 17;
        int spriteSize = 48;
        int enemiesMax = 5;
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

        Timer mainTimer;
        Timer bulletCooldown;
        Timer enemiesRespawn;

        //not using yet
        bool GameOver;

        public MainForm()
        {
            mainFrame = new PictureBox();

            mainTimer = new Timer();
            mainTimer.Interval = 10;
            bulletCooldown = new Timer();
            bulletCooldown.Interval = 1000 / speed;
            enemiesRespawn = new Timer();
            enemiesRespawn.Interval = 1000;


            ResetGameObjects();

            LoadLevel();

            LoadStartMenu();

            PaintGameObjects();

            Controls.Add(mainFrame);

            InitializeComponent();

            //Game start
            //mainTimer.Start();
            //mainTimer.Tick += new EventHandler(_update);
            //bulletCooldown.Start();
            //bulletCooldown.Tick += new EventHandler(resetCooldown);
            //enemiesRespawn.Start();
            //enemiesRespawn.Tick += new EventHandler(GenerateEnemies);
        }

        void StartGame(object sender, EventArgs e)
        {
            GameOver = false;
            Controls.Remove(startButton);
            Controls.Remove(menuBox);

            ResetGameObjects();
            LoadLevel();

            mainTimer.Start();
            mainTimer.Tick += new EventHandler(_update);
            bulletCooldown.Start();
            bulletCooldown.Tick += new EventHandler(resetCooldown);
            enemiesRespawn.Start();
            enemiesRespawn.Tick += new EventHandler(GenerateEnemies);
        }
        void PaintGameObjects()
        {
            mainFrame.Paint += new PaintEventHandler(PaintPlayer);
            mainFrame.Paint += new PaintEventHandler(PaintEnemy);
            mainFrame.Paint += new PaintEventHandler(PaintWalls);
            mainFrame.Paint += new PaintEventHandler(PaintBullets);
            mainFrame.Paint += new PaintEventHandler(PaintApples);
        }
        void ResetGameObjects()
        {
            pl = new Player();
            enemies = new List<GameObject>();
            walls = new List<GameObject>();
            apples = new List<GameObject>();
            playerBullets = new List<GameObject>();
        }

            void LoadStartMenu()
        {
            menuBox = new PictureBox();
            menuBox.Width = 400;
            menuBox.Height = 300;
            menuBox.Location = new Point((mainFrame.Width / 2) - (menuBox.Width / 2), 250);
            menuBox.Image = Properties.Resources.menu_bg;
            
            startButton = new PictureBox();
            startButton.Parent = menuBox;
            startButton.Width = 204;
            startButton.Height = 64;
            startButton.Image = Properties.Resources.start_button;
            startButton.Location = new Point((mainFrame.Width / 2) - (menuBox.Width / 2) + 98, 250 + 196);

            Controls.Add(startButton);
            Controls.Add(menuBox);

            startButton.Click += new EventHandler(StartGame);
        }

        void LoadRestartMenu()
        {
            //idk hwo to fix it
            Controls.Remove(mainFrame);
            LoadStartMenu();
            Controls.Add(mainFrame);
            startButton.Image = Properties.Resources.restart_button;
            menuBox.Image = Properties.Resources.menu_restart_bg;
            mainTimer.Tick -= new EventHandler(_update);

        }

        void LoadLevel()
        {
            GenerateLevelBackGround();

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
            //LEVEL WALLS
            walls.Add(new Wall(1, 4, true));
            walls.Add(new Wall(2, 4, true));
            walls.Add(new Wall(3, 4, true));
            walls.Add(new Wall(4, 4, true));

            walls.Add(new Wall(15, 4, true));
            walls.Add(new Wall(14, 4, true));
            walls.Add(new Wall(13, 4, true));
            walls.Add(new Wall(12, 4, true));

            walls.Add(new Wall(8, 4, true));
            walls.Add(new Wall(8, 5, true));
            walls.Add(new Wall(8, 6, true));
            walls.Add(new Wall(8, 7, true));
            walls.Add(new Wall(7, 6, true));
            walls.Add(new Wall(9, 6, true));

            walls.Add(new Wall(5, 1, true));
            walls.Add(new Wall(6, 1, true));

            walls.Add(new Wall(11, 1, true));
            walls.Add(new Wall(12, 1, true));

            walls.Add(new Wall(4, 8, true));
            walls.Add(new Wall(4, 9, true));
            walls.Add(new Wall(5, 9, true));

            walls.Add(new Wall(13, 8, true));
            walls.Add(new Wall(13, 9, true));
            walls.Add(new Wall(12, 9, true));

            walls.Add(new Wall(6, 12, true));
            walls.Add(new Wall(7, 12, true));
            walls.Add(new Wall(8, 12, true));
            walls.Add(new Wall(9, 12, true));
            walls.Add(new Wall(10, 12, true));
            walls.Add(new Wall(6, 13, true));
            walls.Add(new Wall(10, 13, true));
            walls.Add(new Wall(10, 13, true));
            walls.Add(new Wall(8, 11, true));
            walls.Add(new Wall(8, 10, true));

            walls.Add(new Wall(1, 13, true));
            walls.Add(new Wall(2, 13, true));

            walls.Add(new Wall(15, 13, true));
            walls.Add(new Wall(14, 13, true));
        }

        void GenerateLevelBackGround()
        {
            mainFrame.BackgroundImage = new Bitmap(Properties.Resources.grass);
            mainFrame.Width = spriteSize * levelWidth;
            mainFrame.Height = spriteSize * levelHeight;
        }

        void GenerateEnemies(object sender, EventArgs e)
        {
            if (enemies.Count < enemiesMax)
            {
                int randomX = 0;
                int randomY = 0;

                while (randomX == 0 && randomY == 0)
                {
                    Random rnd = new Random();
                    randomX = rnd.Next(mainFrame.Width - spriteSize * 3) + spriteSize;
                    randomY = rnd.Next(mainFrame.Height - spriteSize * 3) + spriteSize;

                    Rectangle enlargedPlayerCollider = new Rectangle(pl.posX - spriteSize * 2, pl.posY - spriteSize * 2, spriteSize * 5, spriteSize * 5);

                    if (enemies.Count == 0)
                    {
                        break;
                    }

                    if (boxCollides(randomX, randomY, enlargedPlayerCollider))
                    {
                        randomX = 0;
                        randomY = 0;
                        continue;
                    }

                    foreach (Enemy en in enemies)
                    {
                        if (boxCollides(randomX, randomY, en))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }

                    foreach (Wall wall in walls)
                    {
                        if (boxCollides(randomX, randomY, wall))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }
                }
                enemies.Add(new Enemy(randomX, randomY));
            }
        }

        void GenerateApples(object sender, EventArgs e)
        {
            if (apples.Count < applesMax)
            {
                int randomX = 0;
                int randomY = 0;

                while (randomX == 0 && randomY == 0)
                {
                    Random rnd = new Random();
                    randomX = rnd.Next(mainFrame.Width - spriteSize * 3) + spriteSize;
                    randomY = rnd.Next(mainFrame.Height - spriteSize * 3) + spriteSize;

                    Rectangle enlargedPlayerCollider = new Rectangle(pl.posX - spriteSize * 2, pl.posY - spriteSize * 2, spriteSize * 5, spriteSize * 5);

                    if (apples.Count == 0)
                    {
                        break;
                    }

                    if (boxCollides(randomX, randomY, enlargedPlayerCollider))
                    {
                        randomX = 0;
                        randomY = 0;
                        continue;
                    }

                    foreach (Apple apple in apples)
                    {
                        if (boxCollides(randomX, randomY, apple))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }

                    foreach (Wall wall in walls)
                    {
                        if (boxCollides(randomX, randomY, wall))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }
                }
                apples.Add(new Apple(randomX, randomY));
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
            if (GameOver)
            {
                mainTimer.Stop();
                bulletCooldown.Stop();
                enemiesRespawn.Stop();

                LoadRestartMenu();
            }

            //GenerateEnemies(sender, e);
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
                }
            }
            //player - enemy collision //SCORE+
            for (int i = 0; i < enemies.Count; i++)
            {
                if (boxCollides(pl, enemies[i]))
                {
                    GameOver = true;
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
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (boxCollides(playerBullets[i], enemies[j]))
                    {
                        enemies.Remove(enemies[j]);
                        playerBullets[i].posX = -50;
                        playerBullets[i].posY = -50;
                    }
                }
            }
            for (int i = 0; i < playerBullets.Count; i++)
            {
                for (int j = 0; j < walls.Count; j++)
                {
                    if (boxCollides(playerBullets[i], walls[j]))
                    {
                        playerBullets[i].posX = -50;
                        playerBullets[i].posY = -50;
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
        //GameObjects collider
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
        //collider for new objects
        bool boxCollides(int x, int y, GameObject obj2)
        {
            Rectangle newObjectPlace = new Rectangle(x, y, spriteSize, spriteSize);
            if (newObjectPlace.IntersectsWith(obj2.collider))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //collider for new obj and rect (using for player)
        bool boxCollides(int x, int y, Rectangle rect)
        {
            Rectangle newObjectPlace = new Rectangle(x, y, spriteSize, spriteSize);
            if (newObjectPlace.IntersectsWith(rect))
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

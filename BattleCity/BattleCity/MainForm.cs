using BattleCity.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace BattleCity
{
    public partial class MainForm : Form
    {
        PictureBox mainFrame;
        PictureBox menuBox;
        PictureBox startButton;
        Label scoreLable;
        Label objFormButton;

        int levelWidth = Convert.ToInt32(ConfigurationManager.AppSettings["LevelWidth"]);
        int levelHeight = Convert.ToInt32(ConfigurationManager.AppSettings["LevelHeight"]);
        int spriteSize = 48;
        int enemiesMax = Convert.ToInt32(ConfigurationManager.AppSettings["EnemiesMax"]);
        int applesMax = Convert.ToInt32(ConfigurationManager.AppSettings["ApplesMax"]);
        int score = 0;

        public static int GameSpeed = 2;

        public Kolobok pl = GameObjectsStorage.pl;
        public List<GameObject> enemies = GameObjectsStorage.enemies;
        public List<GameObject> walls = GameObjectsStorage.walls;
        public List<GameObject> apples = GameObjectsStorage.apples;

        Timer mainTimer;
        Timer enemiesRespawnTimer;

        //not using yet
        bool GameOver;

        public MainForm()
        {
            mainFrame = new PictureBox();
            
            AddScoreLabel();
            AddObjectsFormButton();
            objFormButton.Click += objFormButtonClick;

            ResetGameObjects();
            LoadLevel();
            LoadStartMenu();
            PaintGameObjects();

            Controls.Add(mainFrame);

            InitializeComponent();
        }

        void StartGame(object sender, EventArgs e)
        {
            mainTimer = new Timer();
            mainTimer.Interval = 10;
            enemiesRespawnTimer = new Timer();
            enemiesRespawnTimer.Interval = 1000;
            
            GameOver = false;
            score = 0;
            Controls.Remove(startButton);
            Controls.Remove(menuBox);

            ResetGameObjects();
            LoadLevel();

            mainTimer.Start();
            mainTimer.Tick += new EventHandler(_update);
            enemiesRespawnTimer.Start();
            enemiesRespawnTimer.Tick += new EventHandler(GenerateEnemies);
        }
        void PaintGameObjects()
        {
            mainFrame.Paint += PaintFrame;
        }
        void ResetGameObjects()
        {
            GameObjectsStorage.pl = new Kolobok();
            GameObjectsStorage.enemies = new List<GameObject>();
            GameObjectsStorage.walls = new List<GameObject>();
            GameObjectsStorage.apples = new List<GameObject>();

            pl = GameObjectsStorage.pl;
            enemies = GameObjectsStorage.enemies;
            walls = GameObjectsStorage.walls;
            apples = GameObjectsStorage.apples;
        }

        void AddScoreLabel()
        {
            scoreLable = new Label();
            scoreLable.Location = new Point(spriteSize, spriteSize / 4);
            scoreLable.Text = "SCORE: " + score.ToString();
            scoreLable.Width = spriteSize * 3;
            scoreLable.Height = spriteSize / 2;
            scoreLable.TextAlign = ContentAlignment.MiddleCenter;
            scoreLable.Font = new Font("Arial", 10, FontStyle.Bold);
            scoreLable.ForeColor = Color.White;
            scoreLable.BackColor = Color.FromArgb(255, 32, 31, 30);

            Controls.Add(scoreLable);
        }
        void AddObjectsFormButton()
        {
            objFormButton = new Label();
            objFormButton.Location = new Point(spriteSize * (levelWidth - 4), 6);
            objFormButton.Text = "Show all GameObjects";
            objFormButton.Width = spriteSize * 3;
            objFormButton.Height = 36;
            objFormButton.TextAlign = ContentAlignment.MiddleCenter;
            objFormButton.Font = new Font("Arial", 9, FontStyle.Bold);
            objFormButton.ForeColor = Color.White;
            objFormButton.BackColor = Color.FromArgb(255, 32, 31, 30);

            Controls.Add(objFormButton);
        }
        private void objFormButtonClick(object sender, EventArgs e)
        {
            ObjectsViewForm obvForm = new ObjectsViewForm();
            obvForm.Show();
            this.Focus();
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

                    if (PackmanController.boxCollides(randomX, randomY, enlargedPlayerCollider))
                    {
                        randomX = 0;
                        randomY = 0;
                        continue;
                    }

                    foreach (Tank en in enemies)
                    {
                        if (PackmanController.boxCollides(randomX, randomY, en))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }

                    foreach (Wall wall in walls)
                    {
                        if (PackmanController.boxCollides(randomX, randomY, wall))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }
                }
                enemies.Add(new Tank(randomX, randomY));
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

                    if (PackmanController.boxCollides(randomX, randomY, enlargedPlayerCollider))
                    {
                        randomX = 0;
                        randomY = 0;
                        continue;
                    }

                    foreach (Apple apple in apples)
                    {
                        if (PackmanController.boxCollides(randomX, randomY, apple))
                        {
                            randomX = 0;
                            randomY = 0;
                            break;
                        }
                    }

                    foreach (Wall wall in walls)
                    {
                        if (PackmanController.boxCollides(randomX, randomY, wall))
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

        
        private void PaintFrame(object sender, PaintEventArgs e)
        {
            //Painting player
            if (pl != null)
            {
                e.Graphics.DrawImage(pl.dynamicImage, pl.pos);
            }
            //Painting enemies
            if (enemies != null)
            {
                foreach (Tank en in enemies)
                {
                    e.Graphics.DrawImage(en.dynamicImage, en.pos);
                }
            }
            //Painting bullets
            if (pl.bullets != null)
            {
                foreach (Bullet bullet in pl.bullets)
                {
                    e.Graphics.DrawImage(bullet.dynamicImage, bullet.pos);
                }
            }
            foreach (Tank en in enemies)
            {
                foreach (Bullet bullet in en.bullets)
                {
                    e.Graphics.DrawImage(bullet.dynamicImage, bullet.pos);
                }
            }
            //Painting walls
            if (walls != null)
            {
                foreach (Wall wall in walls)
                {
                    e.Graphics.DrawImage(wall.image, wall.pos);
                }
            }
            //Painting apples
            if (apples != null)
            {
                foreach (Apple apple in apples)
                {
                    e.Graphics.DrawImage(apple.image, apple.pos);
                }
            }
        }

        //UPDATE func
        void _update(object sender, EventArgs e)
        {
            if (GameOver)
            {
                mainTimer.Stop();
                enemiesRespawnTimer.Stop();

                LoadRestartMenu();
            }

            GenerateApples(sender, e);
            //updating scores
            scoreLable.Text = "SCORES: " + score.ToString();

            //MOVEMENT SECTION
            //bullet movement
            foreach (Bullet bullet in pl.bullets)
            {
                int bulletSpeed = 2 * GameSpeed;
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
            foreach (Tank en in enemies)
            {
                foreach (Bullet bullet in en.bullets)
                {
                    int bulletSpeed = 2 * GameSpeed;
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
            }
            //player movement
            switch (pl.direction)
            {
                case GameObject.Direction.right:
                    pl.posX += GameSpeed;
                    break;

                case GameObject.Direction.left:
                    pl.posX -= GameSpeed;
                    break;

                case GameObject.Direction.up:
                    pl.posY -= GameSpeed;
                    break;

                case GameObject.Direction.down:
                    pl.posY += GameSpeed;
                    break;
            }
            //enemy movement
            foreach (Tank en in enemies)
            {
                en.directionStep += GameSpeed;
                switch (en.direction)
                {
                    case GameObject.Direction.right:
                        en.posX += GameSpeed;
                        break;

                    case GameObject.Direction.left:
                        en.posX -= GameSpeed;
                        break;

                    case GameObject.Direction.up:
                        en.posY -= GameSpeed;
                        break;

                    case GameObject.Direction.down:
                        en.posY += GameSpeed;
                        break;
                }
            }

            //PLAYER SECTION
            //player reach level boundaries
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
            //player - wall collision
            foreach (Wall wall in walls)
            {
                PackmanController.PlayerWall_Collide(pl, wall);
            }
            //player - apple collision
            for (int i = 0; i < apples.Count; i++)
            {
                if (PackmanController.PlayerApple_Collide(pl, apples[i] as Apple))
                {
                    score++;
                }
            }
            //player - enemy collision
            for (int i = 0; i < enemies.Count; i++)
            {
                if (PackmanController.PlayerEnemy_Collide(pl, enemies[i] as Tank))
                {
                    GameOver = true;
                }
            }

            //ENEMY SECTION
            //enemy reach level boundaries
            foreach (Tank en in enemies)
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
            //enemy random direction
            foreach (Tank en in enemies)
            {
                PackmanController.EnemyRandomDirection(en);
            }
            //enemy - wall colission
            foreach (Tank en in enemies)
            {
                foreach (Wall wall in walls)
                {
                    PackmanController.EnemyWall_Collide(en, wall);
                }
            }
            //enemy - enemy collision
            foreach (Tank en in enemies)
            {
                foreach (Tank en2 in enemies)
                {
                    PackmanController.EnemyEnemy_Collide(en, en2);
                }
            }



            //BULLETS SECTION
            //player bullets
            // removing player bullets
            for (int i = 0; i < pl.bullets.Count; i++)
            {
                if (pl.bullets[i].posX == levelWidth || pl.bullets[i].posX == 0 || pl.bullets[i].posY == levelHeight || pl.bullets[i].posY == 0)
                {
                    pl.bullets.Remove(pl.bullets[i]);
                }
            }
            // player bullet enemy collision
            for (int i = 0; i < pl.bullets.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    PackmanController.BulletEnemy_Collide(pl, i, enemies[j] as Tank);
                }
            }
            for (int i = 0; i < pl.bullets.Count; i++)
            {
                for (int j = 0; j < walls.Count; j++)
                {
                    PackmanController.BulletWall_Collide(pl, i, walls[j] as Wall);
                }
            }

            // enemy bullet - wall collision
            foreach (Tank en in enemies)
            {
                for (int i = 0; i < en.bullets.Count; i++)
                {
                    for (int j = 0; j < walls.Count; j++)
                    {
                        PackmanController.BulletWall_Collide(en, i, walls[j] as Wall);
                    }
                }
            }
            foreach (Tank en in enemies)
            {
                for (int i = 0; i < en.bullets.Count; i++)
                {
                    if (PackmanController.BulletPlayer_Collide(en, i, pl))
                    {
                        GameOver = true;
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
                    pl.Shoot();
                    break;
            }
        }
        
    }
}

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
        private PictureBox player;
        enum playerDirection {up, down, left, right }

        private PictureBox bullet;
        private PictureBox enemy;
        private PictureBox apple;
        private PictureBox level;

        int levelWidth = 10;
        int levelHeight = 10;
        int spriteSize = 48;
        int speed = 48;
        string bgImagePath = @"C:\Users\Elijah\Desktop\EPAM .NET\[.NET] Internal\BattleCity TEST\BattleCity\BattleCity\Resources\grass.png";
        string playerImagePath = @"C:\Users\Elijah\Desktop\EPAM .NET\[.NET] Internal\BattleCity TEST\BattleCity\BattleCity\Resources\player.png";
        string bulletImagePath = @"C:\Users\Elijah\Desktop\EPAM .NET\[.NET] Internal\BattleCity TEST\BattleCity\BattleCity\Resources\enemy.png";

        public MainForm()
        {
            player = new PictureBox();
            Timer timer = new Timer();
            timer.Interval = 10;

            addPlayer(player);
            generateLevelBG();

            InitializeComponent();

            timer.Start();
            timer.Tick += new EventHandler(_update);
        }

        void generateLevelBG()
        {
            for (int i = 0; i < levelHeight; i++)
            {
                for (int j = 0; j < levelWidth; j++)
                {
                    PictureBox bg = new PictureBox();
                    bg.BackgroundImage = new Bitmap(bgImagePath);
                    bg.Location = new Point(spriteSize * j, spriteSize * i);
                    bg.Size = new Size(spriteSize, spriteSize);
                    Controls.Add(bg);
                }
            }
        }

        void addPlayer(PictureBox pl)
        {
            pl.Image = new Bitmap(playerImagePath);
            pl.Location = new Point(spriteSize * 0, spriteSize * levelHeight - 1);
            pl.Size = new Size(spriteSize, spriteSize);
            Controls.Add(player);
        }

        void Shoot()
        {
            bullet = new PictureBox();
            bullet.Image = new Bitmap(bulletImagePath);
            bullet.Location = new Point(player.Location.X + 32, player.Location.Y);
            bullet.Size = new Size(spriteSize, spriteSize);
            Controls.Add(bullet);
        }

        void _update(object sender, EventArgs e)
        {
            if (bullet != null && bullet.Created)
            {
                bullet.Location = new Point(bullet.Location.X + 10, bullet.Location.Y);
            }

        }

        //player move
        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            int Speed = speed;
            //MessageBox.Show(e.KeyChar.ToString());

            switch (e.KeyChar.ToString())
            {
                case "d":
                    player.Location = new Point(player.Location.X + Speed, player.Location.Y);
                    break;

                case "a":
                    player.Location = new Point(player.Location.X - Speed, player.Location.Y);
                    break;

                case "w":
                    player.Location = new Point(player.Location.X, player.Location.Y - Speed);
                    break;

                case "s":
                    player.Location = new Point(player.Location.X, player.Location.Y + Speed);
                    break;
                case " ":
                    Shoot();
                    break;
            }
        }
    }
}

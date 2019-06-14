using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity
{
    class GameObject : PictureBox
    {
        int spriteSize = 48;
        string bgImagePath = @"C:\Users\Elijah\Desktop\EPAM .NET\[.NET] Internal\BattleCity TEST\BattleCity\BattleCity\Resources\grass.png";
        string playerImagePath = @"C:\Users\Elijah\Desktop\EPAM .NET\[.NET] Internal\BattleCity TEST\BattleCity\BattleCity\Resources\player.png";

        public GameObject()
        {
            this.BackgroundImage = new Bitmap(playerImagePath);
            this.Location = new Point(0, 0);
            this.Size = new Size(spriteSize, spriteSize);
        }
    }
}

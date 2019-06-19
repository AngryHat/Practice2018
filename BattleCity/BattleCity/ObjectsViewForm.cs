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
    public partial class ObjectsViewForm : Form
    {
        public ObjectsViewForm()
        {
            Location = new Point(200, 200);

            InitializeComponent();
            
            ObjectsInformation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ObjectsInformation.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ObjectsInformation.RowHeadersVisible = false;
            ObjectsInformation.AutoGenerateColumns = false;

            Timer updateTimer = new Timer();
            updateTimer.Interval = 500;
            updateTimer.Tick += UpdateObjectsInformation;
            updateTimer.Start();

            ObjectsInformation.DataSource = GameObjectsStorage.ObjectViewFormList;
        }

        private void UpdateObjectsInformation(object sender, EventArgs e)
        {
            ObjectsInformation.DataSource = null;
            ObjectsInformation.DataSource = GameObjectsStorage.ObjectViewFormList;
        }
    }
}

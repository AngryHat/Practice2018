namespace BattleCity
{
    partial class ObjectsViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ObjectsInformation = new System.Windows.Forms.DataGridView();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.Collider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectsInformation)).BeginInit();
            this.SuspendLayout();
            // 
            // ObjectsInformation
            // 
            this.ObjectsInformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ObjectsInformation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Position,
            this.Image,
            this.Collider,
            this.rb,
            this.Column2,
            this.Column4,
            this.Column3});
            this.ObjectsInformation.Location = new System.Drawing.Point(12, 12);
            this.ObjectsInformation.Name = "ObjectsInformation";
            this.ObjectsInformation.Size = new System.Drawing.Size(308, 593);
            this.ObjectsInformation.TabIndex = 0;
            // 
            // Position
            // 
            this.Position.DataPropertyName = "pos";
            this.Position.HeaderText = "Position";
            this.Position.Name = "Position";
            // 
            // Image
            // 
            this.Image.DataPropertyName = "dynamicImage";
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            // 
            // Collider
            // 
            this.Collider.DataPropertyName = "collider";
            this.Collider.HeaderText = "Collider Points";
            this.Collider.Name = "Collider";
            this.Collider.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Collider.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // rb
            // 
            this.rb.DataPropertyName = "rightBorder";
            this.rb.HeaderText = "rb";
            this.rb.Name = "rb";
            this.rb.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "leftBorder";
            this.Column2.HeaderText = "lb";
            this.Column2.Name = "Column2";
            this.Column2.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "bottomBorder";
            this.Column4.HeaderText = "bb";
            this.Column4.Name = "Column4";
            this.Column4.Visible = false;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "topBorder";
            this.Column3.HeaderText = "tb";
            this.Column3.Name = "Column3";
            this.Column3.Visible = false;
            // 
            // ObjectsViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(332, 617);
            this.Controls.Add(this.ObjectsInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ObjectsViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ObjectsViewForm";
            ((System.ComponentModel.ISupportInitialize)(this.ObjectsInformation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ObjectsInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn Collider;
        private System.Windows.Forms.DataGridViewTextBoxColumn rb;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}
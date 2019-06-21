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
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectsInformation)).BeginInit();
            this.SuspendLayout();
            // 
            // ObjectsInformation
            // 
            this.ObjectsInformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ObjectsInformation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.X,
            this.Y});
            this.ObjectsInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectsInformation.Location = new System.Drawing.Point(0, 0);
            this.ObjectsInformation.Name = "ObjectsInformation";
            this.ObjectsInformation.Size = new System.Drawing.Size(332, 617);
            this.ObjectsInformation.TabIndex = 0;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "TypeString";
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Type.Width = 128;
            // 
            // X
            // 
            this.X.DataPropertyName = "leftBorder";
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Y
            // 
            this.Y.DataPropertyName = "topBorder";
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ObjectsViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(332, 617);
            this.Controls.Add(this.ObjectsInformation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectsViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "All GameObjects";
            ((System.ComponentModel.ISupportInitialize)(this.ObjectsInformation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ObjectsInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
    }
}
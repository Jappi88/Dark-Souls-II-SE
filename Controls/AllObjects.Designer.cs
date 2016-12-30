namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class AllObjects
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radListView1 = new Telerik.WinControls.UI.RadListView();
            this.radContextMenu1 = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.radContextMenuManager1 = new Telerik.WinControls.UI.RadContextMenuManager();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radListView1
            // 
            this.radListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radListView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radListView1.Location = new System.Drawing.Point(0, 23);
            this.radListView1.Name = "radListView1";
            this.radContextMenuManager1.SetRadContextMenu(this.radListView1, this.radContextMenu1);
            this.radListView1.SelectLastAddedItem = false;
            this.radListView1.ShowGridLines = true;
            this.radListView1.Size = new System.Drawing.Size(488, 346);
            this.radListView1.TabIndex = 0;
            this.radListView1.ThemeName = "TelerikMetroBlue";
            // 
            // radContextMenu1
            // 
            this.radContextMenu1.ThemeName = "TelerikMetroBlue";
            this.radContextMenu1.DropDownOpening += new System.ComponentModel.CancelEventHandler(this.radContextMenu1_DropDownOpening);
            // 
            // radTextBox1
            // 
            this.radTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTextBox1.Location = new System.Drawing.Point(0, 0);
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.Size = new System.Drawing.Size(488, 23);
            this.radTextBox1.TabIndex = 1;
            this.radTextBox1.Text = "Search Entry";
            this.radTextBox1.ThemeName = "TelerikMetroBlue";
            this.radTextBox1.TextChanged += new System.EventHandler(this.radTextBox1_TextChanged);
            // 
            // AllObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radListView1);
            this.Controls.Add(this.radTextBox1);
            this.DoubleBuffered = true;
            this.Name = "AllObjects";
            this.Size = new System.Drawing.Size(488, 369);
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadListView radListView1;
        private Telerik.WinControls.UI.RadContextMenuManager radContextMenuManager1;
        private Telerik.WinControls.UI.RadContextMenu radContextMenu1;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
    }
}

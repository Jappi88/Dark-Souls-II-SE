namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class PopupContainer
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
            this.radTitleBar1 = new Telerik.WinControls.UI.RadTitleBar();
            this.xcontrolcontainer = new Telerik.WinControls.UI.RadScrollablePanel();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcontrolcontainer)).BeginInit();
            this.xcontrolcontainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // radTitleBar1
            // 
            this.radTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTitleBar1.Location = new System.Drawing.Point(0, 0);
            this.radTitleBar1.Name = "radTitleBar1";
            this.radTitleBar1.Size = new System.Drawing.Size(318, 23);
            this.radTitleBar1.TabIndex = 0;
            this.radTitleBar1.TabStop = false;
            this.radTitleBar1.ThemeName = "VisualStudio2012Dark";
            this.radTitleBar1.Close += new Telerik.WinControls.UI.TitleBarSystemEventHandler(this.radTitleBar1_Close);
            this.radTitleBar1.Minimize += new Telerik.WinControls.UI.TitleBarSystemEventHandler(this.radTitleBar1_Minimize);
            this.radTitleBar1.DoubleClick += new System.EventHandler(this.radTitleBar1_Minimize);
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTitleBar1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // xcontrolcontainer
            // 
            this.xcontrolcontainer.BackColor = System.Drawing.Color.Transparent;
            this.xcontrolcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xcontrolcontainer.Location = new System.Drawing.Point(0, 23);
            this.xcontrolcontainer.Name = "xcontrolcontainer";
            // 
            // xcontrolcontainer.PanelContainer
            // 
            this.xcontrolcontainer.PanelContainer.Size = new System.Drawing.Size(316, 234);
            this.xcontrolcontainer.Size = new System.Drawing.Size(318, 236);
            this.xcontrolcontainer.TabIndex = 1;
            this.xcontrolcontainer.Text = "radScrollablePanel1";
            this.xcontrolcontainer.ThemeName = "VisualStudio2012Dark";
            // 
            // PopupContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.xcontrolcontainer);
            this.Controls.Add(this.radTitleBar1);
            this.DoubleBuffered = true;
            this.Name = "PopupContainer";
            this.Size = new System.Drawing.Size(318, 259);
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xcontrolcontainer)).EndInit();
            this.xcontrolcontainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar radTitleBar1;
        private Telerik.WinControls.UI.RadScrollablePanel xcontrolcontainer;
    }
}

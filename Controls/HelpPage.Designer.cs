using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    partial class HelpPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpPage));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.richTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.richTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(862, 446);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.richTextBox1);
            this.radPanel1.Controls.Add(this.radTextBox1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(53, 53);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(756, 340);
            this.radPanel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.AutoSize = false;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 122);
            this.richTextBox1.MinimumSize = new System.Drawing.Size(0, 24);
            this.richTextBox1.Multiline = true;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            // 
            // 
            // 
            this.richTextBox1.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.richTextBox1.Size = new System.Drawing.Size(756, 218);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.ThemeName = "VisualStudio2012Dark";
            // 
            // radTextBox1
            // 
            this.radTextBox1.AutoSize = false;
            this.radTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radTextBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTextBox1.ForeColor = System.Drawing.Color.Red;
            this.radTextBox1.Location = new System.Drawing.Point(0, 0);
            this.radTextBox1.MinimumSize = new System.Drawing.Size(0, 24);
            this.radTextBox1.Multiline = true;
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.ReadOnly = true;
            // 
            // 
            // 
            this.radTextBox1.RootElement.MinSize = new System.Drawing.Size(0, 24);
            this.radTextBox1.Size = new System.Drawing.Size(756, 122);
            this.radTextBox1.TabIndex = 1;
            this.radTextBox1.Text = resources.GetString("radTextBox1.Text");
            this.radTextBox1.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBox1.GetChildAt(0))).Text = resources.GetString("resource.Text");
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBox1.GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(177)))), ((int)(((byte)(0)))));
            // 
            // HelpPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "HelpPage";
            this.Size = new System.Drawing.Size(862, 446);
            this.SizeChanged += new System.EventHandler(this.MainPage_SizeChanged);
            this.Resize += new System.EventHandler(this.MainPage_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.richTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RadTextBox richTextBox1;
        private RadPanel radPanel1;
        private RadTextBox radTextBox1;
    }
}

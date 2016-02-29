namespace TileMapManager
{
    partial class TileMapViewCtrl
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
            this.mapBrowserContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.�Ŵ�ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.��СToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.�ƶ�ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.��λToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ˢ��ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.LinesMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PolysMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapPicBox = new System.Windows.Forms.PictureBox();
            this.mapBrowserContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mapBrowserContextMenuStrip
            // 
            this.mapBrowserContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.�Ŵ�ToolStripMenuItem,
            this.��СToolStripMenuItem,
            this.�ƶ�ToolStripMenuItem,
            this.toolStripSeparator1,
            this.��λToolStripMenuItem,
            this.toolStripSeparator2,
            this.ˢ��ToolStripMenuItem,
            this.toolStripSeparator3,
            this.LinesMeasureToolStripMenuItem,
            this.PolysMeasureToolStripMenuItem});
            this.mapBrowserContextMenuStrip.Name = "mapBrowserContextMenuStrip";
            this.mapBrowserContextMenuStrip.Size = new System.Drawing.Size(157, 198);
            // 
            // �Ŵ�ToolStripMenuItem
            // 
            this.�Ŵ�ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuZoomIn;
            this.�Ŵ�ToolStripMenuItem.Name = "�Ŵ�ToolStripMenuItem";
            this.�Ŵ�ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.�Ŵ�ToolStripMenuItem.Text = "�Ŵ󴰿�";
            this.�Ŵ�ToolStripMenuItem.Click += new System.EventHandler(this.�Ŵ�ToolStripMenuItem_Click);
            // 
            // ��СToolStripMenuItem
            // 
            this.��СToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuZoomOut;
            this.��СToolStripMenuItem.Name = "��СToolStripMenuItem";
            this.��СToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.��СToolStripMenuItem.Text = "��С����";
            this.��СToolStripMenuItem.Click += new System.EventHandler(this.��СToolStripMenuItem_Click);
            // 
            // �ƶ�ToolStripMenuItem
            // 
            this.�ƶ�ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuHander;
            this.�ƶ�ToolStripMenuItem.Name = "�ƶ�ToolStripMenuItem";
            this.�ƶ�ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.�ƶ�ToolStripMenuItem.Text = "�ƶ�����";
            this.�ƶ�ToolStripMenuItem.Click += new System.EventHandler(this.�ƶ�ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // ��λToolStripMenuItem
            // 
            this.��λToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuRestore;
            this.��λToolStripMenuItem.Name = "��λToolStripMenuItem";
            this.��λToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.��λToolStripMenuItem.Text = "��λ����";
            this.��λToolStripMenuItem.Click += new System.EventHandler(this.��λToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
            // 
            // ˢ��ToolStripMenuItem
            // 
            this.ˢ��ToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.MenuRefresh;
            this.ˢ��ToolStripMenuItem.Name = "ˢ��ToolStripMenuItem";
            this.ˢ��ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.ˢ��ToolStripMenuItem.Text = "ˢ�´���";
            this.ˢ��ToolStripMenuItem.Click += new System.EventHandler(this.ˢ��ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(153, 6);
            // 
            // LinesMeasureToolStripMenuItem
            // 
            this.LinesMeasureToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.GetPloyLenghth;
            this.LinesMeasureToolStripMenuItem.Name = "LinesMeasureToolStripMenuItem";
            this.LinesMeasureToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.LinesMeasureToolStripMenuItem.Text = "��������(��ʼ)";
            this.LinesMeasureToolStripMenuItem.Click += new System.EventHandler(this.��������ToolStripMenuItem_Click);
            // 
            // PolysMeasureToolStripMenuItem
            // 
            this.PolysMeasureToolStripMenuItem.Image = global::TileMapManager.Properties.Resources.GetPolyArea;
            this.PolysMeasureToolStripMenuItem.Name = "PolysMeasureToolStripMenuItem";
            this.PolysMeasureToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.PolysMeasureToolStripMenuItem.Text = "�������(��ʼ)";
            this.PolysMeasureToolStripMenuItem.Click += new System.EventHandler(this.�������ToolStripMenuItem_Click);
            // 
            // mapPicBox
            // 
            this.mapPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.mapPicBox.ContextMenuStrip = this.mapBrowserContextMenuStrip;
            this.mapPicBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPicBox.Location = new System.Drawing.Point(0, 0);
            this.mapPicBox.Name = "mapPicBox";
            this.mapPicBox.Size = new System.Drawing.Size(400, 400);
            this.mapPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mapPicBox.TabIndex = 1;
            this.mapPicBox.TabStop = false;
            this.mapPicBox.SizeChanged += new System.EventHandler(this.TileMapPictureBox_SizeChanged);
            this.mapPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TileMapPictureBox_MouseDown);
            this.mapPicBox.MouseEnter += new System.EventHandler(this.mapPicBox_MouseEnter);
            this.mapPicBox.MouseLeave += new System.EventHandler(this.mapPicBox_MouseLeave);
            this.mapPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TileMapPictureBox_MouseMove);
            this.mapPicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TileMapPictureBox_MouseUp);
            // 
            // TileMapViewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.mapPicBox);
            this.Name = "TileMapViewCtrl";
            this.Size = new System.Drawing.Size(400, 400);
            this.mapBrowserContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapPicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mapBrowserContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem �Ŵ�ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ��СToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem �ƶ�ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ˢ��ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ��λToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem LinesMeasureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PolysMeasureToolStripMenuItem;
        public System.Windows.Forms.PictureBox mapPicBox;
    }
}

namespace Tool_Project
{
    partial class MapEditor
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GridTypeList = new System.Windows.Forms.FlowLayoutPanel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Grid = new System.Windows.Forms.Panel();
            this.update = new System.Windows.Forms.Timer(this.components);
            this.newClass = new System.Windows.Forms.Button();
            this.GridListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.GridListEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.GridListDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.GridListMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(832, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // GridTypeList
            // 
            this.GridTypeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridTypeList.Location = new System.Drawing.Point(621, 30);
            this.GridTypeList.Name = "GridTypeList";
            this.GridTypeList.Size = new System.Drawing.Size(200, 571);
            this.GridTypeList.TabIndex = 2;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(60, 60);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Grid
            // 
            this.Grid.Location = new System.Drawing.Point(9, 30);
            this.Grid.Margin = new System.Windows.Forms.Padding(0);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(600, 600);
            this.Grid.TabIndex = 3;
            // 
            // update
            // 
            this.update.Enabled = true;
            this.update.Interval = 10;
            this.update.Tick += new System.EventHandler(this.Update_Tick);
            // 
            // newClass
            // 
            this.newClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newClass.Location = new System.Drawing.Point(621, 604);
            this.newClass.Name = "newClass";
            this.newClass.Size = new System.Drawing.Size(199, 23);
            this.newClass.TabIndex = 0;
            this.newClass.Text = "New Class";
            this.newClass.UseVisualStyleBackColor = true;
            this.newClass.Click += new System.EventHandler(this.newClass_Click);
            // 
            // GridListMenu
            // 
            this.GridListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GridListEdit,
            this.GridListDelete});
            this.GridListMenu.Name = "GridListMenu";
            this.GridListMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // GridListEdit
            // 
            this.GridListEdit.Name = "GridListEdit";
            this.GridListEdit.Size = new System.Drawing.Size(107, 22);
            this.GridListEdit.Text = "Edit";
            this.GridListEdit.Click += new System.EventHandler(this.GridListEdit_Click);
            // 
            // GridListDelete
            // 
            this.GridListDelete.Name = "GridListDelete";
            this.GridListDelete.Size = new System.Drawing.Size(107, 22);
            this.GridListDelete.Text = "Delete";
            this.GridListDelete.Click += new System.EventHandler(this.GridListDelete_Click);
            // 
            // MapEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 639);
            this.Controls.Add(this.newClass);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.GridTypeList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MapEditor";
            this.Text = "Map Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapEditor_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MapEditor_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MapEditor_DragEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.GridListMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.FlowLayoutPanel GridTypeList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel Grid;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Timer update;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Button newClass;
        private System.Windows.Forms.ContextMenuStrip GridListMenu;
        private System.Windows.Forms.ToolStripMenuItem GridListEdit;
        private System.Windows.Forms.ToolStripMenuItem GridListDelete;
    }
}


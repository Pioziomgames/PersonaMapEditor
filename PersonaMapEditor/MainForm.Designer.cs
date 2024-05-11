namespace PersonaMapEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            showGridToolStripMenuItem = new ToolStripMenuItem();
            drawGridBehindTilesToolStripMenuItem = new ToolStripMenuItem();
            displayCoordinatesToolStripMenuItem = new ToolStripMenuItem();
            backgroundColorToolStripMenuItem = new ToolStripMenuItem();
            gridBackgroundColorToolStripMenuItem = new ToolStripMenuItem();
            gridColorToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            Display = new MapDisplay();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(704, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(186, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(186, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(186, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(186, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.Click += SaveAsToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showGridToolStripMenuItem, drawGridBehindTilesToolStripMenuItem, displayCoordinatesToolStripMenuItem, backgroundColorToolStripMenuItem, gridBackgroundColorToolStripMenuItem, gridColorToolStripMenuItem, resetToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // showGridToolStripMenuItem
            // 
            showGridToolStripMenuItem.Checked = true;
            showGridToolStripMenuItem.CheckState = CheckState.Checked;
            showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            showGridToolStripMenuItem.Size = new Size(195, 22);
            showGridToolStripMenuItem.Text = "Display Grid";
            showGridToolStripMenuItem.Click += ShowGridToolStripMenuItem_Click;
            // 
            // drawGridBehindTilesToolStripMenuItem
            // 
            drawGridBehindTilesToolStripMenuItem.Checked = true;
            drawGridBehindTilesToolStripMenuItem.CheckState = CheckState.Checked;
            drawGridBehindTilesToolStripMenuItem.Name = "drawGridBehindTilesToolStripMenuItem";
            drawGridBehindTilesToolStripMenuItem.Size = new Size(195, 22);
            drawGridBehindTilesToolStripMenuItem.Text = "Draw Grid Behind Tiles";
            drawGridBehindTilesToolStripMenuItem.Click += DrawGridBehindTilesToolStripMenuItem_Click;
            // 
            // displayCoordinatesToolStripMenuItem
            // 
            displayCoordinatesToolStripMenuItem.Checked = true;
            displayCoordinatesToolStripMenuItem.CheckState = CheckState.Checked;
            displayCoordinatesToolStripMenuItem.Name = "displayCoordinatesToolStripMenuItem";
            displayCoordinatesToolStripMenuItem.Size = new Size(195, 22);
            displayCoordinatesToolStripMenuItem.Text = "Display Coordinates";
            displayCoordinatesToolStripMenuItem.Click += DisplayCoordinatesToolStripMenuItem_Click;
            // 
            // backgroundColorToolStripMenuItem
            // 
            backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            backgroundColorToolStripMenuItem.Size = new Size(195, 22);
            backgroundColorToolStripMenuItem.Text = "Background Color";
            backgroundColorToolStripMenuItem.Click += BackgroundColorToolStripMenuItem_Click;
            // 
            // gridBackgroundColorToolStripMenuItem
            // 
            gridBackgroundColorToolStripMenuItem.Name = "gridBackgroundColorToolStripMenuItem";
            gridBackgroundColorToolStripMenuItem.Size = new Size(195, 22);
            gridBackgroundColorToolStripMenuItem.Text = "Grid Background Color";
            gridBackgroundColorToolStripMenuItem.Click += GridBackgroundColorToolStripMenuItem_Click;
            // 
            // gridColorToolStripMenuItem
            // 
            gridColorToolStripMenuItem.Name = "gridColorToolStripMenuItem";
            gridColorToolStripMenuItem.Size = new Size(195, 22);
            gridColorToolStripMenuItem.Text = "Grid Color";
            gridColorToolStripMenuItem.Click += GridColorToolStripMenuItem_Click;
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(195, 22);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += ResetToolStripMenuItem_Click;
            // 
            // Display
            // 
            Display.Dock = DockStyle.Fill;
            Display.Location = new Point(0, 24);
            Display.Name = "Display";
            Display.Size = new Size(704, 426);
            Display.TabIndex = 2;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 450);
            Controls.Add(Display);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Persona Map Editor";
            DragDrop += MainForm_DragDrop;
            DragEnter += MainForm_DragEnter;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem showGridToolStripMenuItem;
        private ToolStripMenuItem backgroundColorToolStripMenuItem;
        private ToolStripMenuItem gridBackgroundColorToolStripMenuItem;
        private ToolStripMenuItem gridColorToolStripMenuItem;
        private MapDisplay Display;
        private ToolStripMenuItem drawGridBehindTilesToolStripMenuItem;
        private ToolStripMenuItem displayCoordinatesToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
    }
}
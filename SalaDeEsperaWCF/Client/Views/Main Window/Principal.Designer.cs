namespace Client
{
    partial class Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Geral", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("TV", System.Windows.Forms.HorizontalAlignment.Left);
            this.listViewComponents = new System.Windows.Forms.ListView();
            this.imageListComponents = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxComponents = new System.Windows.Forms.GroupBox();
            this.panelBuilder = new System.Windows.Forms.Panel();
            this.contextMenuStripBackground = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propriedadesBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propriedadesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ficheiroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.novoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ferramentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.computadoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxPC = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonFechar = new System.Windows.Forms.Button();
            this.buttonPlayer = new System.Windows.Forms.Button();
            this.listViewDisplays = new System.Windows.Forms.ListView();
            this.treeViewRede = new System.Windows.Forms.TreeView();
            this.groupBoxRede = new System.Windows.Forms.GroupBox();
            this.buttonPause = new System.Windows.Forms.Button();
            this.groupBoxBuilder = new System.Windows.Forms.GroupBox();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.listViewPlayerStatus = new System.Windows.Forms.ListView();
            this.columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxComponents.SuspendLayout();
            this.contextMenuStripBackground.SuspendLayout();
            this.contextMenuStripComponents.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBoxPC.SuspendLayout();
            this.groupBoxRede.SuspendLayout();
            this.groupBoxBuilder.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewComponents
            // 
            this.listViewComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewComponents.Location = new System.Drawing.Point(3, 16);
            this.listViewComponents.Name = "listViewComponents";
            this.listViewComponents.Size = new System.Drawing.Size(241, 346);
            this.listViewComponents.SmallImageList = this.imageListComponents;
            this.listViewComponents.TabIndex = 0;
            this.listViewComponents.UseCompatibleStateImageBehavior = false;
            this.listViewComponents.View = System.Windows.Forms.View.List;
            this.listViewComponents.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewComponents_ItemDrag);
            this.listViewComponents.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewComponents_DragDrop);
            this.listViewComponents.DoubleClick += new System.EventHandler(this.listViewComponents_DoubleClick);
            // 
            // imageListComponents
            // 
            this.imageListComponents.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListComponents.ImageStream")));
            this.imageListComponents.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListComponents.Images.SetKeyName(0, "DateTime");
            this.imageListComponents.Images.SetKeyName(1, "PriceList");
            this.imageListComponents.Images.SetKeyName(2, "Video");
            this.imageListComponents.Images.SetKeyName(3, "SlideShow");
            this.imageListComponents.Images.SetKeyName(4, "Image");
            this.imageListComponents.Images.SetKeyName(5, "TV");
            this.imageListComponents.Images.SetKeyName(6, "WaitList");
            this.imageListComponents.Images.SetKeyName(7, "Footer");
            this.imageListComponents.Images.SetKeyName(8, "Weather");
            this.imageListComponents.Images.SetKeyName(9, "Computer");
            this.imageListComponents.Images.SetKeyName(10, "Monitor");
            // 
            // groupBoxComponents
            // 
            this.groupBoxComponents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxComponents.Controls.Add(this.listViewComponents);
            this.groupBoxComponents.Location = new System.Drawing.Point(250, 158);
            this.groupBoxComponents.Name = "groupBoxComponents";
            this.groupBoxComponents.Size = new System.Drawing.Size(247, 365);
            this.groupBoxComponents.TabIndex = 1;
            this.groupBoxComponents.TabStop = false;
            this.groupBoxComponents.Text = "Componentes";
            // 
            // panelBuilder
            // 
            this.panelBuilder.AllowDrop = true;
            this.panelBuilder.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelBuilder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBuilder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBuilder.ContextMenuStrip = this.contextMenuStripBackground;
            this.panelBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBuilder.Location = new System.Drawing.Point(3, 16);
            this.panelBuilder.Name = "panelBuilder";
            this.panelBuilder.Size = new System.Drawing.Size(629, 477);
            this.panelBuilder.TabIndex = 2;
            this.panelBuilder.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelConfigurador_DragDrop);
            this.panelBuilder.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelConfigurador_DragEnter);
            this.panelBuilder.MouseEnter += new System.EventHandler(this.panelBuilder_MouseEnter);
            this.panelBuilder.MouseLeave += new System.EventHandler(this.panelBuilder_MouseLeave);
            // 
            // contextMenuStripBackground
            // 
            this.contextMenuStripBackground.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propriedadesBackgroundToolStripMenuItem});
            this.contextMenuStripBackground.Name = "contextMenuStripBackground";
            this.contextMenuStripBackground.Size = new System.Drawing.Size(144, 26);
            // 
            // propriedadesBackgroundToolStripMenuItem
            // 
            this.propriedadesBackgroundToolStripMenuItem.Name = "propriedadesBackgroundToolStripMenuItem";
            this.propriedadesBackgroundToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.propriedadesBackgroundToolStripMenuItem.Text = "Propriedades";
            this.propriedadesBackgroundToolStripMenuItem.Click += new System.EventHandler(this.propriedadesBackgroundToolStripMenuItem_Click);
            // 
            // contextMenuStripComponents
            // 
            this.contextMenuStripComponents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removerToolStripMenuItem,
            this.propriedadesToolStripMenuItem});
            this.contextMenuStripComponents.Name = "contextMenuStripComponents";
            this.contextMenuStripComponents.Size = new System.Drawing.Size(144, 48);
            // 
            // removerToolStripMenuItem
            // 
            this.removerToolStripMenuItem.Name = "removerToolStripMenuItem";
            this.removerToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.removerToolStripMenuItem.Text = "Remover";
            this.removerToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // propriedadesToolStripMenuItem
            // 
            this.propriedadesToolStripMenuItem.Name = "propriedadesToolStripMenuItem";
            this.propriedadesToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.propriedadesToolStripMenuItem.Text = "Propriedades";
            this.propriedadesToolStripMenuItem.Click += new System.EventHandler(this.propriedadesToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ficheiroToolStripMenuItem,
            this.ferramentasToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1150, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ficheiroToolStripMenuItem
            // 
            this.ficheiroToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novoToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.toolStripSeparator1,
            this.guardarToolStripMenuItem,
            this.guardarComoToolStripMenuItem,
            this.toolStripSeparator2,
            this.sairToolStripMenuItem});
            this.ficheiroToolStripMenuItem.Name = "ficheiroToolStripMenuItem";
            this.ficheiroToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.ficheiroToolStripMenuItem.Text = "Ficheiro";
            // 
            // novoToolStripMenuItem
            // 
            this.novoToolStripMenuItem.Name = "novoToolStripMenuItem";
            this.novoToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.novoToolStripMenuItem.Text = "Novo";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // guardarComoToolStripMenuItem
            // 
            this.guardarComoToolStripMenuItem.Name = "guardarComoToolStripMenuItem";
            this.guardarComoToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.guardarComoToolStripMenuItem.Text = "Guardar como...";
            this.guardarComoToolStripMenuItem.Click += new System.EventHandler(this.guardarComoToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // ferramentasToolStripMenuItem
            // 
            this.ferramentasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.computadoresToolStripMenuItem,
            this.abrirPlayerToolStripMenuItem});
            this.ferramentasToolStripMenuItem.Name = "ferramentasToolStripMenuItem";
            this.ferramentasToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.ferramentasToolStripMenuItem.Text = "Ferramentas";
            // 
            // computadoresToolStripMenuItem
            // 
            this.computadoresToolStripMenuItem.Name = "computadoresToolStripMenuItem";
            this.computadoresToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.computadoresToolStripMenuItem.Text = "Computadores";
            this.computadoresToolStripMenuItem.Click += new System.EventHandler(this.computadoresToolStripMenuItem_Click);
            // 
            // abrirPlayerToolStripMenuItem
            // 
            this.abrirPlayerToolStripMenuItem.Name = "abrirPlayerToolStripMenuItem";
            this.abrirPlayerToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.abrirPlayerToolStripMenuItem.Text = "Abrir Player";
            // 
            // groupBoxPC
            // 
            this.groupBoxPC.Controls.Add(this.button2);
            this.groupBoxPC.Controls.Add(this.buttonFechar);
            this.groupBoxPC.Controls.Add(this.buttonPlayer);
            this.groupBoxPC.Controls.Add(this.listViewDisplays);
            this.groupBoxPC.Location = new System.Drawing.Point(253, 27);
            this.groupBoxPC.Name = "groupBoxPC";
            this.groupBoxPC.Size = new System.Drawing.Size(244, 125);
            this.groupBoxPC.TabIndex = 7;
            this.groupBoxPC.TabStop = false;
            this.groupBoxPC.Text = "Ligado a: nenhum";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Pause";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonPause_click);
            // 
            // buttonFechar
            // 
            this.buttonFechar.Location = new System.Drawing.Point(162, 96);
            this.buttonFechar.Name = "buttonFechar";
            this.buttonFechar.Size = new System.Drawing.Size(75, 23);
            this.buttonFechar.TabIndex = 8;
            this.buttonFechar.Text = "Fechar Player";
            this.buttonFechar.UseVisualStyleBackColor = true;
            this.buttonFechar.Click += new System.EventHandler(this.buttonFechar_Click);
            // 
            // buttonPlayer
            // 
            this.buttonPlayer.Location = new System.Drawing.Point(84, 96);
            this.buttonPlayer.Name = "buttonPlayer";
            this.buttonPlayer.Size = new System.Drawing.Size(75, 23);
            this.buttonPlayer.TabIndex = 7;
            this.buttonPlayer.Text = "Abrir Player";
            this.buttonPlayer.UseVisualStyleBackColor = true;
            this.buttonPlayer.Click += new System.EventHandler(this.buttonPlayer_Click);
            // 
            // listViewDisplays
            // 
            this.listViewDisplays.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewDisplays.Location = new System.Drawing.Point(3, 16);
            this.listViewDisplays.Name = "listViewDisplays";
            this.listViewDisplays.ShowItemToolTips = true;
            this.listViewDisplays.Size = new System.Drawing.Size(238, 77);
            this.listViewDisplays.TabIndex = 0;
            this.listViewDisplays.UseCompatibleStateImageBehavior = false;
            this.listViewDisplays.SelectedIndexChanged += new System.EventHandler(this.listViewDisplays_SelectedIndexChanged);
            // 
            // treeViewRede
            // 
            this.treeViewRede.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewRede.CheckBoxes = true;
            this.treeViewRede.FullRowSelect = true;
            this.treeViewRede.ImageIndex = 0;
            this.treeViewRede.ImageList = this.imageListComponents;
            this.treeViewRede.Indent = 15;
            this.treeViewRede.Location = new System.Drawing.Point(3, 16);
            this.treeViewRede.Name = "treeViewRede";
            this.treeViewRede.SelectedImageIndex = 0;
            this.treeViewRede.ShowNodeToolTips = true;
            this.treeViewRede.Size = new System.Drawing.Size(229, 171);
            this.treeViewRede.TabIndex = 8;
            this.treeViewRede.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewRede_AfterCheck);
            // 
            // groupBoxRede
            // 
            this.groupBoxRede.Controls.Add(this.buttonPause);
            this.groupBoxRede.Controls.Add(this.treeViewRede);
            this.groupBoxRede.Location = new System.Drawing.Point(12, 27);
            this.groupBoxRede.Name = "groupBoxRede";
            this.groupBoxRede.Size = new System.Drawing.Size(235, 222);
            this.groupBoxRede.TabIndex = 9;
            this.groupBoxRede.TabStop = false;
            this.groupBoxRede.Text = "Rede";
            // 
            // buttonPause
            // 
            this.buttonPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPause.Location = new System.Drawing.Point(154, 193);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 9;
            this.buttonPause.Text = "Scan";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // groupBoxBuilder
            // 
            this.groupBoxBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBuilder.Controls.Add(this.panelBuilder);
            this.groupBoxBuilder.Location = new System.Drawing.Point(503, 27);
            this.groupBoxBuilder.Name = "groupBoxBuilder";
            this.groupBoxBuilder.Size = new System.Drawing.Size(635, 496);
            this.groupBoxBuilder.TabIndex = 10;
            this.groupBoxBuilder.TabStop = false;
            this.groupBoxBuilder.Text = "Builder";
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.listViewPlayerStatus);
            this.groupBoxStatus.Location = new System.Drawing.Point(15, 255);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(226, 265);
            this.groupBoxStatus.TabIndex = 11;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Status";
            // 
            // listViewPlayerStatus
            // 
            this.listViewPlayerStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTitle,
            this.columnValue});
            this.listViewPlayerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPlayerStatus.FullRowSelect = true;
            listViewGroup1.Header = "Geral";
            listViewGroup1.Name = "LVGroupGeneral";
            listViewGroup2.Header = "TV";
            listViewGroup2.Name = "LVGroupTV";
            this.listViewPlayerStatus.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listViewPlayerStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPlayerStatus.Location = new System.Drawing.Point(3, 16);
            this.listViewPlayerStatus.Name = "listViewPlayerStatus";
            this.listViewPlayerStatus.Size = new System.Drawing.Size(220, 246);
            this.listViewPlayerStatus.TabIndex = 0;
            this.listViewPlayerStatus.UseCompatibleStateImageBehavior = false;
            this.listViewPlayerStatus.View = System.Windows.Forms.View.Details;
            this.listViewPlayerStatus.SelectedIndexChanged += new System.EventHandler(this.listViewPlayerStatus_SelectedIndexChanged);
            // 
            // columnTitle
            // 
            this.columnTitle.Text = "";
            this.columnTitle.Width = 96;
            // 
            // columnValue
            // 
            this.columnValue.Text = "";
            this.columnValue.Width = 120;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 535);
            this.Controls.Add(this.groupBoxStatus);
            this.Controls.Add(this.groupBoxBuilder);
            this.Controls.Add(this.groupBoxRede);
            this.Controls.Add(this.groupBoxPC);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBoxComponents);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "Principal";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Principal_Load);
            this.groupBoxComponents.ResumeLayout(false);
            this.contextMenuStripBackground.ResumeLayout(false);
            this.contextMenuStripComponents.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxPC.ResumeLayout(false);
            this.groupBoxRede.ResumeLayout(false);
            this.groupBoxBuilder.ResumeLayout(false);
            this.groupBoxStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewComponents;
        private System.Windows.Forms.GroupBox groupBoxComponents;
        private System.Windows.Forms.ImageList imageListComponents;
        private System.Windows.Forms.Panel panelBuilder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripComponents;
        private System.Windows.Forms.ToolStripMenuItem removerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propriedadesToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ferramentasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem computadoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirPlayerToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxPC;
        private System.Windows.Forms.ListView listViewDisplays;
        private System.Windows.Forms.TreeView treeViewRede;
        private System.Windows.Forms.GroupBox groupBoxRede;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonFechar;
        private System.Windows.Forms.Button buttonPlayer;
        private System.Windows.Forms.GroupBox groupBoxBuilder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBackground;
        private System.Windows.Forms.ToolStripMenuItem propriedadesBackgroundToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.ListView listViewPlayerStatus;
        private System.Windows.Forms.ColumnHeader columnTitle;
        private System.Windows.Forms.ColumnHeader columnValue;
        private System.Windows.Forms.ToolStripMenuItem ficheiroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
    }
}


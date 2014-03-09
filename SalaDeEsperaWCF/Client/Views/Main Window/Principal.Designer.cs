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
            this.listViewComponents = new System.Windows.Forms.ListView();
            this.imageListComponents = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxComponents = new System.Windows.Forms.GroupBox();
            this.panelBuilder = new System.Windows.Forms.Panel();
            this.contextMenuStripBackground = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propriedadesBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propriedadesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPrincipal = new System.Windows.Forms.MenuStrip();
            this.ficheiroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.novoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ferramentasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.treeViewRede = new System.Windows.Forms.TreeView();
            this.contextMenuStripTreeViewRede = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ligarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxRede = new System.Windows.Forms.GroupBox();
            this.progressBarScan = new System.Windows.Forms.ProgressBar();
            this.buttonScan = new System.Windows.Forms.Button();
            this.groupBoxBuilder = new System.Windows.Forms.GroupBox();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.listViewPlayerStatus = new System.Windows.Forms.ListView();
            this.listViewStatusColAtribute = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewStatusColValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainerGeral = new System.Windows.Forms.SplitContainer();
            this.splitContainerRedeStatus = new System.Windows.Forms.SplitContainer();
            this.groupBoxComponents.SuspendLayout();
            this.contextMenuStripBackground.SuspendLayout();
            this.contextMenuStripComponents.SuspendLayout();
            this.menuStripPrincipal.SuspendLayout();
            this.contextMenuStripTreeViewRede.SuspendLayout();
            this.groupBoxRede.SuspendLayout();
            this.groupBoxBuilder.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGeral)).BeginInit();
            this.splitContainerGeral.Panel1.SuspendLayout();
            this.splitContainerGeral.Panel2.SuspendLayout();
            this.splitContainerGeral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRedeStatus)).BeginInit();
            this.splitContainerRedeStatus.Panel1.SuspendLayout();
            this.splitContainerRedeStatus.Panel2.SuspendLayout();
            this.splitContainerRedeStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewComponents
            // 
            this.listViewComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewComponents.Location = new System.Drawing.Point(3, 18);
            this.listViewComponents.Name = "listViewComponents";
            this.listViewComponents.Size = new System.Drawing.Size(152, 471);
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
            this.imageListComponents.Images.SetKeyName(10, "ComputerError.png");
            this.imageListComponents.Images.SetKeyName(11, "Monitor");
            this.imageListComponents.Images.SetKeyName(12, "Clinic");
            // 
            // groupBoxComponents
            // 
            this.groupBoxComponents.Controls.Add(this.listViewComponents);
            this.groupBoxComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxComponents.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxComponents.Location = new System.Drawing.Point(0, 0);
            this.groupBoxComponents.Name = "groupBoxComponents";
            this.groupBoxComponents.Size = new System.Drawing.Size(158, 492);
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
            this.panelBuilder.Location = new System.Drawing.Point(3, 18);
            this.panelBuilder.Name = "panelBuilder";
            this.panelBuilder.Size = new System.Drawing.Size(629, 471);
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
            // menuStripPrincipal
            // 
            this.menuStripPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ficheiroToolStripMenuItem,
            this.ferramentasToolStripMenuItem});
            this.menuStripPrincipal.Location = new System.Drawing.Point(0, 0);
            this.menuStripPrincipal.Name = "menuStripPrincipal";
            this.menuStripPrincipal.Size = new System.Drawing.Size(1150, 24);
            this.menuStripPrincipal.TabIndex = 4;
            this.menuStripPrincipal.Text = "menuStrip1";
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
            this.abrirPlayerToolStripMenuItem});
            this.ferramentasToolStripMenuItem.Name = "ferramentasToolStripMenuItem";
            this.ferramentasToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.ferramentasToolStripMenuItem.Text = "Ferramentas";
            // 
            // abrirPlayerToolStripMenuItem
            // 
            this.abrirPlayerToolStripMenuItem.Name = "abrirPlayerToolStripMenuItem";
            this.abrirPlayerToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.abrirPlayerToolStripMenuItem.Text = "Abrir Player";
            this.abrirPlayerToolStripMenuItem.Click += new System.EventHandler(this.abrirPlayerToolStripMenuItem_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Location = new System.Drawing.Point(87, 197);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Fechar Player";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonFechar_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonConnect.Location = new System.Drawing.Point(6, 197);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "Abrir Player";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonPlayer_Click);
            // 
            // treeViewRede
            // 
            this.treeViewRede.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewRede.ContextMenuStrip = this.contextMenuStripTreeViewRede;
            this.treeViewRede.FullRowSelect = true;
            this.treeViewRede.ImageIndex = 0;
            this.treeViewRede.ImageList = this.imageListComponents;
            this.treeViewRede.Indent = 15;
            this.treeViewRede.Location = new System.Drawing.Point(3, 18);
            this.treeViewRede.Name = "treeViewRede";
            this.treeViewRede.SelectedImageIndex = 0;
            this.treeViewRede.ShowNodeToolTips = true;
            this.treeViewRede.Size = new System.Drawing.Size(316, 174);
            this.treeViewRede.TabIndex = 8;
            this.treeViewRede.DoubleClick += new System.EventHandler(this.treeViewRede_DoubleClick);
            // 
            // contextMenuStripTreeViewRede
            // 
            this.contextMenuStripTreeViewRede.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ligarToolStripMenuItem});
            this.contextMenuStripTreeViewRede.Name = "contextMenuStripTreeViewRede";
            this.contextMenuStripTreeViewRede.Size = new System.Drawing.Size(123, 26);
            this.contextMenuStripTreeViewRede.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripTreeViewRede_Opening);
            // 
            // ligarToolStripMenuItem
            // 
            this.ligarToolStripMenuItem.Name = "ligarToolStripMenuItem";
            this.ligarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.ligarToolStripMenuItem.Text = "Conectar";
            this.ligarToolStripMenuItem.Click += new System.EventHandler(this.ligarToolStripMenuItem_Click);
            // 
            // groupBoxRede
            // 
            this.groupBoxRede.Controls.Add(this.progressBarScan);
            this.groupBoxRede.Controls.Add(this.buttonClose);
            this.groupBoxRede.Controls.Add(this.buttonScan);
            this.groupBoxRede.Controls.Add(this.buttonConnect);
            this.groupBoxRede.Controls.Add(this.treeViewRede);
            this.groupBoxRede.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRede.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxRede.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRede.Name = "groupBoxRede";
            this.groupBoxRede.Size = new System.Drawing.Size(322, 226);
            this.groupBoxRede.TabIndex = 9;
            this.groupBoxRede.TabStop = false;
            this.groupBoxRede.Text = "Rede";
            // 
            // progressBarScan
            // 
            this.progressBarScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarScan.Location = new System.Drawing.Point(3, 182);
            this.progressBarScan.Name = "progressBarScan";
            this.progressBarScan.Size = new System.Drawing.Size(316, 10);
            this.progressBarScan.TabIndex = 13;
            this.progressBarScan.Visible = false;
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(241, 197);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 9;
            this.buttonScan.Text = "Procurar";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // groupBoxBuilder
            // 
            this.groupBoxBuilder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBoxBuilder.Controls.Add(this.panelBuilder);
            this.groupBoxBuilder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBuilder.Location = new System.Drawing.Point(503, 27);
            this.groupBoxBuilder.Name = "groupBoxBuilder";
            this.groupBoxBuilder.Size = new System.Drawing.Size(635, 492);
            this.groupBoxBuilder.TabIndex = 10;
            this.groupBoxBuilder.TabStop = false;
            this.groupBoxBuilder.Text = "Builder";
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.listViewPlayerStatus);
            this.groupBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(322, 261);
            this.groupBoxStatus.TabIndex = 11;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Status";
            // 
            // listViewPlayerStatus
            // 
            this.listViewPlayerStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listViewStatusColAtribute,
            this.listViewStatusColValue});
            this.listViewPlayerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPlayerStatus.FullRowSelect = true;
            this.listViewPlayerStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPlayerStatus.Location = new System.Drawing.Point(3, 18);
            this.listViewPlayerStatus.MultiSelect = false;
            this.listViewPlayerStatus.Name = "listViewPlayerStatus";
            this.listViewPlayerStatus.ShowItemToolTips = true;
            this.listViewPlayerStatus.Size = new System.Drawing.Size(316, 240);
            this.listViewPlayerStatus.TabIndex = 0;
            this.listViewPlayerStatus.UseCompatibleStateImageBehavior = false;
            this.listViewPlayerStatus.View = System.Windows.Forms.View.Details;
            this.listViewPlayerStatus.SelectedIndexChanged += new System.EventHandler(this.listViewPlayerStatus_SelectedIndexChanged);
            // 
            // listViewStatusColAtribute
            // 
            this.listViewStatusColAtribute.Text = "Atributo";
            this.listViewStatusColAtribute.Width = 100;
            // 
            // listViewStatusColValue
            // 
            this.listViewStatusColValue.Text = "Valor";
            this.listViewStatusColValue.Width = 90;
            // 
            // splitContainerGeral
            // 
            this.splitContainerGeral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainerGeral.Location = new System.Drawing.Point(12, 27);
            this.splitContainerGeral.Name = "splitContainerGeral";
            // 
            // splitContainerGeral.Panel1
            // 
            this.splitContainerGeral.Panel1.Controls.Add(this.splitContainerRedeStatus);
            this.splitContainerGeral.Panel1MinSize = 247;
            // 
            // splitContainerGeral.Panel2
            // 
            this.splitContainerGeral.Panel2.Controls.Add(this.groupBoxComponents);
            this.splitContainerGeral.Size = new System.Drawing.Size(485, 492);
            this.splitContainerGeral.SplitterDistance = 322;
            this.splitContainerGeral.SplitterWidth = 5;
            this.splitContainerGeral.TabIndex = 12;
            // 
            // splitContainerRedeStatus
            // 
            this.splitContainerRedeStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerRedeStatus.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRedeStatus.Name = "splitContainerRedeStatus";
            this.splitContainerRedeStatus.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRedeStatus.Panel1
            // 
            this.splitContainerRedeStatus.Panel1.Controls.Add(this.groupBoxRede);
            // 
            // splitContainerRedeStatus.Panel2
            // 
            this.splitContainerRedeStatus.Panel2.Controls.Add(this.groupBoxStatus);
            this.splitContainerRedeStatus.Size = new System.Drawing.Size(322, 492);
            this.splitContainerRedeStatus.SplitterDistance = 226;
            this.splitContainerRedeStatus.SplitterWidth = 5;
            this.splitContainerRedeStatus.TabIndex = 0;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1150, 534);
            this.Controls.Add(this.splitContainerGeral);
            this.Controls.Add(this.groupBoxBuilder);
            this.Controls.Add(this.menuStripPrincipal);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStripPrincipal;
            this.MinimumSize = new System.Drawing.Size(1166, 572);
            this.Name = "Principal";
            this.Text = "Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Principal_FormClosing);
            this.Load += new System.EventHandler(this.Principal_Load);
            this.groupBoxComponents.ResumeLayout(false);
            this.contextMenuStripBackground.ResumeLayout(false);
            this.contextMenuStripComponents.ResumeLayout(false);
            this.menuStripPrincipal.ResumeLayout(false);
            this.menuStripPrincipal.PerformLayout();
            this.contextMenuStripTreeViewRede.ResumeLayout(false);
            this.groupBoxRede.ResumeLayout(false);
            this.groupBoxBuilder.ResumeLayout(false);
            this.groupBoxStatus.ResumeLayout(false);
            this.splitContainerGeral.Panel1.ResumeLayout(false);
            this.splitContainerGeral.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGeral)).EndInit();
            this.splitContainerGeral.ResumeLayout(false);
            this.splitContainerRedeStatus.Panel1.ResumeLayout(false);
            this.splitContainerRedeStatus.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRedeStatus)).EndInit();
            this.splitContainerRedeStatus.ResumeLayout(false);
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
        private System.Windows.Forms.MenuStrip menuStripPrincipal;
        private System.Windows.Forms.ToolStripMenuItem ferramentasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirPlayerToolStripMenuItem;
        private System.Windows.Forms.TreeView treeViewRede;
        private System.Windows.Forms.GroupBox groupBoxRede;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.GroupBox groupBoxBuilder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBackground;
        private System.Windows.Forms.ToolStripMenuItem propriedadesBackgroundToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.ListView listViewPlayerStatus;
        private System.Windows.Forms.ColumnHeader listViewStatusColAtribute;
        private System.Windows.Forms.ColumnHeader listViewStatusColValue;
        private System.Windows.Forms.ToolStripMenuItem ficheiroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem novoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeViewRede;
        private System.Windows.Forms.ToolStripMenuItem ligarToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerGeral;
        private System.Windows.Forms.SplitContainer splitContainerRedeStatus;
        private System.Windows.Forms.ProgressBar progressBarScan;
    }
}


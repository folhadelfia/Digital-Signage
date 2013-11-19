namespace Assemblies.Options.OptionsGeneral
{
    partial class FormOptionsBuilder
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Janela");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptionsBuilder));
            this.treeViewItems = new System.Windows.Forms.TreeView();
            this.imageListComponents = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeViewItems
            // 
            this.treeViewItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewItems.ImageIndex = 0;
            this.treeViewItems.ImageList = this.imageListComponents;
            this.treeViewItems.Location = new System.Drawing.Point(12, 12);
            this.treeViewItems.Name = "treeViewItems";
            treeNode1.ImageKey = "Display";
            treeNode1.Name = "Background";
            treeNode1.SelectedImageKey = "Display";
            treeNode1.Text = "Janela";
            this.treeViewItems.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeViewItems.SelectedImageIndex = 0;
            this.treeViewItems.Size = new System.Drawing.Size(186, 483);
            this.treeViewItems.TabIndex = 0;
            this.treeViewItems.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewItems_BeforeSelect);
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
            this.imageListComponents.Images.SetKeyName(9, "Display");
            // 
            // FormOptionsBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 507);
            this.Controls.Add(this.treeViewItems);
            this.Name = "FormOptionsBuilder";
            this.Text = "FormOptionsBuilder";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewItems;
        private System.Windows.Forms.ImageList imageListComponents;
    }
}
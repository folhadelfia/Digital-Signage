using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.PlayerComponents;

/*
 * Arranjar melhor forma de associar os nós com as configurações
 */

namespace Assemblies.Options.OptionsGeneral
{
    public partial class FormOptionsBuilder : Form
    {
        Dictionary<int, ItemConfiguration> configsAlteradas = new Dictionary<int, ItemConfiguration>();
        OptionsView visibleView = null;

        private List<ComposerComponent> componentList = new List<ComposerComponent>();

        public List<ComposerComponent> ComponentList
        {
            get { return componentList; }
        }

        public FormOptionsBuilder(IEnumerable<ComposerComponent> componentList)
        {
            InitializeComponent();



            #region OLD
            int nodeIndex, typeindex = 0;

            List<Type> types = new List<Type>();

            foreach (var item in componentList)
                if (!types.Contains(item.GetType())) types.Add(item.GetType());

            foreach (Type T in types)
            {
                OptionsView view = OptionsBuilderToolkit.GetViewFor(T);

                view.Dock = DockStyle.Right;
                view.Left = treeViewItems.Right + treeViewItems.Margin.Right;
                view.Visible = false;

                this.Controls.Add(view);

                foreach (var item in componentList.Where(x => x.GetType() == T))
                {
                    nodeIndex = treeViewItems.Nodes[0].Nodes.Add(OptionsBuilderToolkit.GetNodeFor(item, ++typeindex));

                    configsAlteradas.Add(nodeIndex, item.Configuration);
                    this.componentList.Add(item);
                }
            } 
            #endregion
        }

        private void treeViewItems_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == treeViewItems.Nodes[0]) return;

            OptionsView view;

            if (e.Node.Tag is MarkeeConfiguration)
            {
                view = this.Controls.Cast<Control>().Single(x => x is FooterOptionsView) as FooterOptionsView;
            }
            else if (e.Node.Tag is TVConfiguration)
            {
                //tal tal tal
                return;
            }
            else return;

            if(visibleView != null)
                configsAlteradas[treeViewItems.SelectedNode.Index] = visibleView.Configuration;

            view.Configuration = configsAlteradas[e.Node.Index];

            if (visibleView != null && visibleView.GetType() != view.GetType())
            {
                foreach (var item in this.Controls.Cast<Control>().Where(x => x is OptionsView))
                    item.Visible = false;
            }

            view.Visible = true;

            visibleView = view;
        }
    }

    public static class OptionsBuilderToolkit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">Item usado para distinguir o tipo de nó</param>
        /// <param name="index">Index do item, em relaçao a todos os do seu tipo</param>
        /// <returns></returns>
        public static TreeNode GetNodeFor(ComposerComponent item, int index)
        {
            if (item is Markee)
            {
                return new TreeNode
                {
                    Text = string.Format("Rodapé {0}", index),
                    ImageKey = "Footer",
                    SelectedImageKey = "Footer",
                    Tag = item.Configuration

                };
            }
            else if (item is TVComposer)
            {
                return new TreeNode
                {
                    Text = string.Format("TV {0}", index),
                    ImageKey = "TV",
                    SelectedImageKey = "TV",
                    Tag = null //ENFIAR aqui um IOptionsView

                };
            }
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">Item usado para distinguir o tipo de vista</param>
        /// <param name="id">Preferencialmente o index do nó correspondente, para se poder ligar o nó à vista</param>
        /// <returns></returns>
        public static OptionsView GetViewFor(Type T)
        {
            {
                if (T == typeof(Markee))
                {
                    return new FooterOptionsView();
                }
                else if (T == typeof(TVComposer))
                {
                    return null; //ENFIAR aqui um IOptionsView
                }
                else return null;
            }
        }
    }
}

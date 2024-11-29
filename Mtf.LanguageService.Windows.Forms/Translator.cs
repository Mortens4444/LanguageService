using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mtf.LanguageService.Windows.Forms
{
    public static class Translator
    {
        /// <summary>
        /// Translates a Windows.Forms.Form and all of its children.
        /// </summary>
        /// <param name="form">The Form to be translated.</param>
        /// <param name="toolTip">The tooltip to be translated.</param>
        public static void Translate(Form form, ToolTip toolTip = null)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            form.Text = Lng.Elem(form.Text);
            Translate(form.Controls, toolTip);
        }

        /// <summary>
        /// Translates a UserControl and all of its children.
        /// </summary>
        /// <param name="userControl">The custom user control</param>
        /// <param name="toolTip">The tooltip to be translated.</param>
        public static void Translate(UserControl userControl, ToolTip toolTip = null)
        {
            if (userControl == null)
            {
                throw new ArgumentNullException(nameof(userControl));
            }

            userControl.Text = Lng.Elem(userControl.Text);
            Translate(userControl.Controls, toolTip);
        }

        /// <summary>
        /// Translates a ControlCollection.
        /// </summary>
        /// <param name="controls">The ControlCollection to be translated.</param>
        /// <param name="toolTip">The tooltip to be translated.</param>
        public static void Translate(Control.ControlCollection controls, ToolTip toolTip = null)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            foreach (var control in controls)
            {
                if (control is WebBrowser)
                {
                    continue;
                }

                if (control is Control controlWithTextProperty)
                {
                    controlWithTextProperty.Text = Lng.Elem(controlWithTextProperty.Text);
                    Translate(controlWithTextProperty.Controls, toolTip);
                    TranslateTooltips(controlWithTextProperty, toolTip);
                    if (controlWithTextProperty.ContextMenuStrip != null)
                    {
                        Translate(controlWithTextProperty.ContextMenuStrip.Items, toolTip);
                    }
                }

                if (control is ListView listview)
                {
                    foreach (ListViewGroup listViewGroup in listview.Groups)
                    {
                        listViewGroup.Header = Lng.Elem(listViewGroup.Header);
                    }
                    foreach (ColumnHeader column in listview.Columns)
                    {
                        column.Text = Lng.Elem(column.Text);
                    }
                }
                else if (control is TreeView treeview)
                {
                    foreach (TreeNode node in treeview.Nodes)
                    {
                        Translate(node, toolTip);
                    }
                }
                else if (control is MenuStrip menuStrip)
                {
                    Translate(menuStrip.Items, toolTip);
                }
                else if (control is StatusStrip statusStrip)
                {
                    Translate(statusStrip.Items, toolTip);
                }
                else if (control is ToolStrip toolStrip)
                {
                    Translate(toolStrip.Items, toolTip);
                }
                else if (control is ComboBox comboBox)
                {
                    Translate(comboBox, comboBox.Items, toolTip);
                }
                else if (control is ContextMenuStrip contextMenuStrip)
                {
                    Translate(contextMenuStrip.Items, toolTip);
                }
#if NET481
                else if (control is ContextMenu contextMenu)
                {
                    Translate(contextMenu.MenuItems, toolTip);
                }
#endif
                else if (control is DataGridView dataGridView)
                {
                    Translate(dataGridView, toolTip);
                }
            }
        }

        private static void Translate(DataGridView dataGridView, ToolTip toolTip)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.HeaderText = Lng.Elem(column.HeaderText);
            }
        }

        private static void Translate(TreeNode node, ToolTip toolTip)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                Translate(childNode, toolTip);
            }
            node.Text = Lng.Elem(node.Text);
        }

#if NET481
        private static void Translate(Menu.MenuItemCollection items, ToolTip toolTip)
        {
            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    item.Text = Lng.Elem(item.Text);
                    Translate(item.MenuItems, toolTip);
                }
            }
        }
#endif

        private static void Translate(ComboBox comboBox, ComboBox.ObjectCollection items, ToolTip toolTip)
        {
            var result = new List<object>();
            for (var i = 0; i < items.Count; i++)
            {
                result.Add(items[i] is string text ? Lng.Elem(text) : items[i]);
            }
            comboBox.Items.Clear();
            comboBox.Items.AddRange(result.ToArray());
        }

        public static void Translate(ToolStripItemCollection toolStripItems, ToolTip toolTip)
        {
            if (toolStripItems == null)
            {
                throw new ArgumentNullException(nameof(toolStripItems));
            }

            foreach (ToolStripItem toolStripItem in toolStripItems)
            {
                toolStripItem.Text = Lng.Elem(toolStripItem.Text);
                TranslateTooltips(toolStripItem);
                if (toolStripItem is ToolStripMenuItem toolStripMenuItem)
                {
                    Translate(toolStripMenuItem.DropDownItems, toolTip);
                }
                else if (toolStripItem is ToolStripDropDownButton toolStripDropDownButton)
                {
                    Translate(toolStripDropDownButton.DropDownItems, toolTip);
                }
            }
        }

        private static void TranslateTooltips(Control control, ToolTip toolTip)
        {
            if (toolTip == null)
            {
                return;
            }

            // Check if the control has a ToolTip property and translate it
            if (control is Control controlWithTooltip)
            {
                var hint = toolTip.GetToolTip(controlWithTooltip);
                if (!String.IsNullOrEmpty(hint))
                {
                    toolTip.SetToolTip(controlWithTooltip, Lng.Elem(hint));
                }
            }
        }

        private static void TranslateTooltips(ToolStripItem toolStripItem)
        {
            if (toolStripItem is ToolStripButton button)
            {
                var hint = button.ToolTipText;
                if (!String.IsNullOrEmpty(hint))
                {
                    button.ToolTipText = Lng.Elem(hint);
                }
            }
            else if (toolStripItem is ToolStripMenuItem menuItem)
            {
                var hint = menuItem.ToolTipText;
                if (!String.IsNullOrEmpty(hint))
                {
                    menuItem.ToolTipText = Lng.Elem(hint);
                }
            }
            else if (toolStripItem is ToolStripLabel label)
            {
                var hint = label.ToolTipText;
                if (!String.IsNullOrEmpty(hint))
                {
                    label.ToolTipText = Lng.Elem(hint);
                }
            }
        }
    }
}

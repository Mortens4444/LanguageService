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
        /// <returns>Returns the original texts for the controls.</returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if form is null.</exception>
        public static Dictionary<object, string> Translate(Form form, ToolTip toolTip = null)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var originalTexts = new Dictionary<object, string>
            {
                { form, form.Text }
            };
            form.Text = Lng.Elem(form.Text);
            var originals = Translate(form.Controls, toolTip);
            foreach (var original in originals)
            {
                originalTexts.Add(original.Key, original.Value);
            }
            return originalTexts;
        }

        /// <summary>
        /// Translates a UserControl and all of its children.
        /// </summary>
        /// <param name="userControl">The custom user control</param>
        /// <param name="toolTip">The tooltip to be translated.</param>
        /// <returns>Returns the original texts for the controls.</returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if userControl is null.</exception>
        public static Dictionary<object, string> Translate(UserControl userControl, ToolTip toolTip = null)
        {
            if (userControl == null)
            {
                throw new ArgumentNullException(nameof(userControl));
            }

            var originalTexts = new Dictionary<object, string>();
            originalTexts.Add(userControl, userControl.Text);
            userControl.Text = Lng.Elem(userControl.Text);
            var originals = Translate(userControl.Controls, toolTip);
            foreach (var original in originals)
            {
                originalTexts.Add(original.Key, original.Value);
            }
            return originalTexts;
        }

        /// <summary>
        /// Translates a ControlCollection.
        /// </summary>
        /// <param name="controls">The ControlCollection to be translated.</param>
        /// <param name="toolTip">The tooltip to be translated.</param>
        /// <returns>Returns the original texts for the controls.</returns>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if controls is null.</exception>
        public static List<KeyValuePair<object, string>> Translate(Control.ControlCollection controls, ToolTip toolTip = null)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            var result = new List<KeyValuePair<object, string>>();
            foreach (var control in controls)
            {
                if (control is WebBrowser)
                {
                    continue;
                }

                if (control is Control controlWithTextProperty)
                {
                    result.Add(new KeyValuePair<object, string>(controlWithTextProperty, controlWithTextProperty.Text));
                    controlWithTextProperty.Text = Lng.Elem(controlWithTextProperty.Text);
                    var originals = Translate(controlWithTextProperty.Controls, toolTip);
                    foreach (var original in originals)
                    {
                        result.Add(original);
                    }
                    TranslateTooltips(controlWithTextProperty, toolTip);
                    if (controlWithTextProperty.ContextMenuStrip != null)
                    {
                        Translate(controlWithTextProperty.ContextMenuStrip.Items, toolTip, result);
                    }
                }

                if (control is ListView listview)
                {
                    foreach (ListViewGroup listViewGroup in listview.Groups)
                    {
                        result.Add(new KeyValuePair<object, string>(listViewGroup, listViewGroup.Header));
                        listViewGroup.Header = Lng.Elem(listViewGroup.Header);
                    }
                    foreach (ColumnHeader column in listview.Columns)
                    {
                        result.Add(new KeyValuePair<object, string>(column, column.Text));
                        column.Text = Lng.Elem(column.Text);
                    }
                }
                else if (control is TreeView treeview)
                {
                    foreach (TreeNode node in treeview.Nodes)
                    {
                        Translate(node, toolTip, result);
                    }
                }
                else if (control is MenuStrip menuStrip)
                {
                    Translate(menuStrip.Items, toolTip, result);
                }
                else if (control is StatusStrip statusStrip)
                {
                    Translate(statusStrip.Items, toolTip, result);
                }
                else if (control is ToolStrip toolStrip)
                {
                    Translate(toolStrip.Items, toolTip, result);
                }
                else if (control is ComboBox comboBox)
                {
                    Translate(comboBox, comboBox.Items, toolTip);
                }
                else if (control is ContextMenuStrip contextMenuStrip)
                {
                    Translate(contextMenuStrip.Items, toolTip, result);
                }
#if NET481
                else if (control is ContextMenu contextMenu)
                {
                    Translate(contextMenu.MenuItems, toolTip, result);
                }
#endif
                else if (control is DataGridView dataGridView)
                {
                    Translate(dataGridView, toolTip, result);
                }
            }

            return result;
        }

        public static void SetOriginalTexts(Dictionary<object, string> originalTexts)
        {
            if (originalTexts == null)
            {
                throw new ArgumentNullException(nameof(originalTexts));
            }

            foreach (var originalText in originalTexts)
            {
                if (originalText.Key is Control control)
                {
                    control.Text = originalText.Value;
                }
                else if(originalText.Key is DataGridViewColumn dataGridViewColumn)
                {
                    dataGridViewColumn.HeaderText = originalText.Value;
                }
                else if (originalText.Key is TreeNode treeNode)
                {
                    treeNode.Text = originalText.Value;
                }
#if NET481
                else if (originalText.Key is MenuItem menuItem)
                {
                    menuItem.Text = originalText.Value;
                }
#endif
                else if (originalText.Key is ToolStripItem toolStripItem)
                {
                    toolStripItem.Text = originalText.Value;
                }
                else if (originalText.Key is ToolStripMenuItem toolStripMenuItem)
                {
                    toolStripMenuItem.Text = originalText.Value;
                }
                else if (originalText.Key is ColumnHeader column)
                {
                    column.Text = originalText.Value;
                }
                else if (originalText.Key is ListViewGroup listViewGroup)
                {
                    listViewGroup.Header = originalText.Value;
                }
                
            }
        }

        private static void Translate(DataGridView dataGridView, ToolTip toolTip, List<KeyValuePair<object, string>> originalTexts)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                originalTexts.Add(new KeyValuePair<object, string>(column, column.HeaderText));
                column.HeaderText = Lng.Elem(column.HeaderText);
            }
        }

        private static void Translate(TreeNode node, ToolTip toolTip, List<KeyValuePair<object, string>> originalTexts)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                Translate(childNode, toolTip, originalTexts);
            }
            node.Text = Lng.Elem(node.Text);
        }

#if NET481
        private static void Translate(Menu.MenuItemCollection items, ToolTip toolTip, List<KeyValuePair<object, string>> originalTexts)
        {
            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    originalTexts.Add(new KeyValuePair<object, string>(item, item.Text));
                    item.Text = Lng.Elem(item.Text);
                    Translate(item.MenuItems, toolTip, originalTexts);
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

        public static void Translate(ToolStripItemCollection toolStripItems, ToolTip toolTip, List<KeyValuePair<object, string>> originalTexts)
        {
            if (toolStripItems == null)
            {
                throw new ArgumentNullException(nameof(toolStripItems));
            }

            foreach (ToolStripItem toolStripItem in toolStripItems)
            {
                originalTexts.Add(new KeyValuePair<object, string>(toolStripItem, toolStripItem.Text));
                toolStripItem.Text = Lng.Elem(toolStripItem.Text);
                TranslateTooltips(toolStripItem, originalTexts);
                if (toolStripItem is ToolStripMenuItem toolStripMenuItem)
                {
                    Translate(toolStripMenuItem.DropDownItems, toolTip, originalTexts);
                }
                else if (toolStripItem is ToolStripDropDownButton toolStripDropDownButton)
                {
                    Translate(toolStripDropDownButton.DropDownItems, toolTip, originalTexts);
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

        private static void TranslateTooltips(ToolStripItem toolStripItem, List<KeyValuePair<object, string>> originalTexts)
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

using System.Collections.Generic;
using System.Windows.Forms;

namespace LanguageService.Windows.Forms
{
	public static class Translator
	{
		/// <summary>
		/// Translates a Windows.Forms.Form.
		/// </summary>
		/// <param name="form">The Form to be translated.</param>
		public static void Translate(Form form)
		{
			form.Text = Lng.Elem(form.Text);
			Translate(form.Controls);
		}

		/// <summary>
		/// Translates a ControlCollection.
		/// </summary>
		/// <param name="controls">The ControlCollection to be translated.</param>
		public static void Translate(Control.ControlCollection controls)
		{
			foreach (var control in controls)
			{
				if (control is WebBrowser)
				{
					continue;
				}

				if (control is Control controlWithTextProperty)
				{
					controlWithTextProperty.Text = Lng.Elem(controlWithTextProperty.Text);
					Translate(controlWithTextProperty.Controls);
					if (controlWithTextProperty.ContextMenuStrip != null)
					{
						Translate(controlWithTextProperty.ContextMenuStrip.Items);
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
						Translate(node);
					}
				}
				else if (control is MenuStrip menuStrip)
				{
					Translate(menuStrip.Items);
				}
				else if (control is ComboBox comboBox)
				{
					Translate(comboBox, comboBox.Items);
				}
				else if (control is ContextMenuStrip contextMenuStrip)
				{
					Translate(contextMenuStrip.Items);
				}
				else if (control is ContextMenu contextMenu)
				{
					Translate(contextMenu.MenuItems);
				}
			}
		}

		private static void Translate(TreeNode node)
		{
			foreach (TreeNode childNode in node.Nodes)
			{
				Translate(childNode);
			}
			node.Text = Lng.Elem(node.Text);
		}

		private static void Translate(Menu.MenuItemCollection items)
		{
			if (items != null)
			{
				foreach (MenuItem item in items)
				{
					item.Text = Lng.Elem(item.Text);
					Translate(item.MenuItems);
				}
			}
		}

		private static void Translate(ComboBox comboBox, ComboBox.ObjectCollection items)
		{
			var result = new List<object>();
			for (int i = 0; i < items.Count; i++)
			{
				result.Add(items[i] is string text ? Lng.Elem(text) : items[i]);
			}
			comboBox.Items.Clear();
			comboBox.Items.AddRange(result.ToArray());
		}

		public static void Translate(ToolStripItemCollection toolStripItems)
		{
			foreach (ToolStripItem toolStripItem in toolStripItems)
			{
				toolStripItem.Text = Lng.Elem(toolStripItem.Text);
				if (toolStripItem is ToolStripMenuItem toolStripMenuItem)
				{
					Translate(toolStripMenuItem.DropDownItems);
				}
			}
		}
	}
}

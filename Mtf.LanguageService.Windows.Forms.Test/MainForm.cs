using System.Windows.Forms;

namespace Mtf.LanguageService.Windows.Forms.Test
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			Translator.Translate(this);
			treeView1.ExpandAll();
			comboBox1.SelectedIndex = 0;
		}
	}
}

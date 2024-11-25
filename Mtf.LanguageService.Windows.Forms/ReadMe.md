**Usage**

[GitHub URL](https://github.com/Mortens4444/LanguageService)
[Developer's URL](http://w3.hdsnet.hu/mortens/)
[Facebook Page](https://www.facebook.com/developersdream)

```
using Mtf.LanguageService;
using Mtf.LanguageService.Windows.Forms;

public partial class MainForm : Form
{
	public MainForm()
	{
		InitializeComponent();
		
		lv.ItemsSource = new[] { Lng.Elem("Users"), Lng.Elem("Groups") };
		rtb.Document.Blocks.Add(new Paragraph(new Run(Lng.Elem("File name"))));
		
		Translator.Translate(this);
	}
}
```

If you need to translate to Hungarian, just use this code snippet.
```
Lng.DefaultLanguage = Mtf.LanguageService.Language.Hungarian;
Translator.Translate(this);
```

You can translate from any language to any other language like this.
```
Lng.Translate(Language.Hungarian, "Ismétlődés", Language.English);
```
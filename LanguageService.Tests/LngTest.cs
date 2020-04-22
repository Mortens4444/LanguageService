using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LanguageService.Tests
{
	[TestClass]
	public class LngTest
	{
		[TestMethod]
		public void LngElemTest()
		{
			// Test translation to Hungarian
			Assert.AreEqual("Igen", Lng.Elem(Language.Hungarian, "Yes"));

			// Test translation to German
			Assert.AreEqual("Ja", Lng.Elem(Language.German, "Yes"));

			// Test some elements to ensure not deleting rows accidentally on sheets
			Assert.AreEqual("WMI Adattár", Lng.Elem(Language.Hungarian, "WMI Repository"));
			Assert.AreEqual("WMI 存储库", Lng.Elem(Language.Chinese, "WMI Repository"));
			Assert.AreEqual("WMI-Repository", Lng.Elem(Language.German, "WMI Repository"));

			// Test translation with non-existing translate
			Assert.AreEqual("Yes", Lng.Elem(Language.Albanian, "Yes"));

			// Test translation with non-existing language element
			Assert.AreEqual("Non-existing language element", Lng.Elem(Language.German, "Non-existing language element"));

			Lng.DefaultLanguage = Language.Hungarian;
			Assert.AreEqual("WMI Adattár", Lng.Elem("WMI Repository"));

			Lng.DefaultLanguage = Language.Chinese;
			Assert.AreEqual("WMI 存储库", Lng.Elem("WMI Repository"));
		}
	}
}

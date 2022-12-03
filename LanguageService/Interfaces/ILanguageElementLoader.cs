using System.Collections.Generic;

namespace LanguageService.Interfaces
{
	interface ILanguageElementLoader
	{
		Dictionary<(Language Language, string ElementIdentifier), List<string>> LoadElements(string filePath);
	}
}

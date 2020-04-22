using System.Collections.Generic;
using LanguageService.Enums;

namespace LanguageService.Interfaces
{
	interface ILanguageElementLoader
	{
		Dictionary<(Language Language, string ElementIdentifier), List<string>> LoadElements(string filePath);
	}
}

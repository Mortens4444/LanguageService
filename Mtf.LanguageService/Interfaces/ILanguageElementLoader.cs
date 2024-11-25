using Mtf.LanguageService.Enums;
using System.Collections.Generic;

namespace Mtf.LanguageService.Interfaces
{
    public interface ILanguageElementLoader
    {
        Dictionary<(Language Language, string ElementIdentifier), List<string>> LoadElements(string filePath);
    }
}

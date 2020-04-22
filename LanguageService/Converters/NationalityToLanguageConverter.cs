using System;
using System.Collections.Generic;
using System.Linq;
using LanguageService.Attributes;
using LanguageService.Enums;
using LanguageService.Extensions;
using LanguageService.Interfaces;

namespace LanguageService.Converters
{
	public class NationalityToLanguageConverter : INationalityToLanguageConverter
	{
		public Language Convert(Nationality nationality)
		{
			var nationalityEnum = nationality as Enum;
			var firstDefaultLanguageAttribute = nationalityEnum.GetSingleEnumAttribute<DefaultLanguagesAttribute>("Languages");
			if (firstDefaultLanguageAttribute is IEnumerable<Language> defaultLanguages)
			{
				return defaultLanguages.First();
			}
			return Language.English;
		}
	}
}

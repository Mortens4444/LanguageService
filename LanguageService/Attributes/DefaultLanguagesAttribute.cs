using System;
using System.Collections.Generic;
using LanguageService.Enums;

namespace LanguageService.Attributes
{
	class DefaultLanguagesAttribute : Attribute
	{
		public IEnumerable<Language> Languages { get; set; }

		public DefaultLanguagesAttribute(params Language[] languages)
		{
			Languages = languages;
		}
	}
}

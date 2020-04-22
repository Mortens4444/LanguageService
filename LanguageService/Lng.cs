using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using LanguageService.Interfaces;
using LanguageService.Ods;
using Application = System.Windows.Forms.Application;

namespace LanguageService
{
	public static class Lng
	{
		public static Language DefaultLanguage;

		public static readonly Dictionary<(Language Language, string ElementIdentifier), List<string>> AllLanguageElements;

		private static readonly ILanguageElementLoader languageElementLoader = new OdsLanguageElementLoader();

		static Lng()
		{
			SetDefaultLanguage();

			var languageFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, @"Languages.ods");
			if (languageFiles.Any())
			{
				AllLanguageElements = languageElementLoader.LoadElements(languageFiles.First());
			}
		}







		private static void SetDefaultLanguage()
		{
			var currentCulture = Application.CurrentCulture;
			var languageName = currentCulture.EnglishName.Split(' ').First();
			DefaultLanguage = (Language)Enum.Parse(typeof(Language), languageName);
		}

		public static string Elem(string elementIdentifier, int index = 0)
		{
			return Elem(DefaultLanguage, elementIdentifier, index);
		}

		/// <summary>
		/// Get a translation of an English expression.
		/// </summary>
		/// <param name="toLanguage">The language of the requested translation.</param>
		/// <param name="elementIdentifier">The requested element, which wanted to be translated.</param>
		/// <param name="index">Index of the specified translations. If not set, it will return the first translation.</param>
		/// <returns>Returns the translation or the requested element itself, if it is not present in the dictionary.</returns>
		public static string Elem(Language toLanguage, string elementIdentifier, int index = 0)
		{
			var result = GetLanguageElement(elementIdentifier, index, toLanguage);
			if (String.IsNullOrEmpty(result))
			{
				result = GetLanguageElement(elementIdentifier, index);
			}
			return String.IsNullOrEmpty(result) ? elementIdentifier : result;
		}

		private static string GetLanguageElement(string elementIdentifier, int index, Language language = Language.English)
		{
			var key = (language, elementIdentifier);
			return AllLanguageElements.ContainsKey(key) ? AllLanguageElements[key][index] : null;
		}
	}
}

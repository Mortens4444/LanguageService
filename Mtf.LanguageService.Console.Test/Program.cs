using System;

namespace LanguageService.ConsoleTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Lng.Elem(Language.Hungarian, "Yes"));

			//Console.WriteLine(Lng.Elem("Yes"));

			Console.ReadKey();
		}
	}
}

using LanguageService.Enums;

namespace LanguageService.Interfaces
{
	public interface INationalityToLanguageConverter
	{
		Language Convert(Nationality nationality);
	}
}

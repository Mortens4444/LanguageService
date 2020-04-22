using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageService.Extensions
{
	public static class EnumExtensions
	{
		public static IEnumerable<object> GetEnumAttribute<TAttribute>(this Enum value, string propertyName)
		{
			var type = typeof(TAttribute);
			var field = value.GetType().GetField(value.ToString());
			var attributes = field.GetCustomAttributes(type, false) as TAttribute[];
			return attributes.Select(attribute => type.GetProperty(propertyName).GetValue(attribute));
		}

		public static object GetSingleEnumAttribute<TAttribute>(this Enum value, string propertyName)
		{
			var attributes = value.GetEnumAttribute<TAttribute>(propertyName);
			if (attributes.Any())
			{
				return attributes.Single();
			}

			return null;
		}
	}
}

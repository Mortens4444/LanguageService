﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mtf.LanguageService.Attributes;
using Mtf.LanguageService.Enums;
using Mtf.LanguageService.Extensions;
using Mtf.LanguageService.Interfaces;

namespace Mtf.LanguageService.Converters
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

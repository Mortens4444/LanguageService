# `Lng` Class Documentation  

The `Lng` class is a static utility for managing multilingual translations of text elements in an application. It loads translations from a specified ODS file (`Languages.ods`) and provides methods to retrieve translations in different languages.  

## **Namespace**  
`Mtf.LanguageService`  

## **Dependencies**  
- `Mtf.LanguageService.Enums`
- `Mtf.LanguageService.Interfaces`
- `Mtf.LanguageService.Ods`
- System libraries: `System`, `System.Collections.Generic`, `System.Globalization`, `System.IO`, `System.Linq`

---

## **Fields**  

### `LanguageFile`  
- **Type:** `string`  
- **Description:** The filename of the ODS file containing language translations.  

### `DefaultLanguage`  
- **Type:** `Language` (enum)  
- **Description:** The default language for translations, determined by the system's current culture.  

### `AllLanguageElements`  
- **Type:** `Dictionary<(Language, string), List<string>>`  
- **Description:** A dictionary containing all language elements, keyed by language and element identifier.  

### `languageElementLoader`  
- **Type:** `ILanguageElementLoader`  
- **Description:** A loader for parsing the ODS file and retrieving language elements. Defaults to `OdsLanguageElementLoader`.  

---

## **Methods**  

### `Elem(string elementIdentifier, int index = 0)`  
- **Description:** Retrieves a translation for a given element identifier in the default language.  
- **Parameters:**  
  - `elementIdentifier` (string): The identifier of the element to translate.  
  - `index` (int): The index of the translation (default is 0).  
- **Returns:**  
  - `string`: The translated text or the element identifier itself if no translation exists.  

---

### `Elem(Language toLanguage, string elementIdentifier, int index = 0)`  
- **Description:** Retrieves a translation for a given element identifier in the specified language.  
- **Parameters:**  
  - `toLanguage` (Language): The target language.  
  - `elementIdentifier` (string): The identifier of the element to translate.  
  - `index` (int): The index of the translation (default is 0).  
- **Returns:**  
  - `string`: The translated text or the element identifier itself if no translation exists.  

---

### `Translate(Language fromLanguage, string languageElement, Language toLanguage)`  
- **Description:** Translates a specific text from one language to another.  
- **Parameters:**  
  - `fromLanguage` (Language): The source language of the element.  
  - `languageElement` (string): The text to be translated.  
  - `toLanguage` (Language): The target language.  
- **Returns:**  
  - `string`: The translated text or the original text if no translation exists.  

---

### `SetDefaultLanguage()`  
- **Description:** Sets the `DefaultLanguage` field based on the current system culture.  
- **Returns:**  
  - None  

---

### `GetLanguageElement(string elementIdentifier, int index, Language language = Language.English)`  
- **Description:** Retrieves a specific translation from the dictionary based on the identifier, index, and language.  
- **Parameters:**  
  - `elementIdentifier` (string): The identifier of the element.  
  - `index` (int): The index of the translation.  
  - `language` (Language): The target language (default is English).  
- **Returns:**  
  - `string`: The translated text or null if no translation exists.  

---

## **Exceptions**  
- **`InvalidOperationException`**  
  - Thrown if the `Languages.ods` file is not found in the application directory.  

---

## **Usage Example**  

```csharp
using Mtf.LanguageService;

class Program
{
    static void Main()
    {
        // Retrieve a translation in the default language
        string translatedText = Lng.Elem("HelloWorld");

        // Retrieve a translation in a specific language
        string translatedInSpanish = Lng.Elem(Language.Spanish, "HelloWorld");

        // Translate between languages
        string translated = Lng.Translate(Language.English, "Hello World", Language.French);
        
        Console.WriteLine(translatedText);
        Console.WriteLine(translatedInSpanish);
        Console.WriteLine(translated);
    }
}
```

---

## **Notes**  
- Ensure the `Languages.ods` file is located in the base directory of the application.  
- Update the `Languages.ods` file to include new translations as needed.  
- Default language is automatically determined by the system's culture but can be changed programmatically.  
public interface ILocalizationUISystem
{
    void UpdateUIText(LocalizationModes mode, string languageCode = "en");
    string GetCurrentLanguageText(string textKey);
}


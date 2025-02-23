using System.Collections.Generic;

public interface IGUISystem
{
    void SetUI(LocalizationModes mode, string languageCode = "en");
    string GetUIText(string textKey);
    bool ReadDialogJSON(string path);
    Dictionary<string, List<string>> GetDialog(string key);
}
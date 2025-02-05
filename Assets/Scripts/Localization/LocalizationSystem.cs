using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

internal class LocalizationSystem : MonoBehaviour
{
    public delegate void OnLanguageChanged();
    public static event OnLanguageChanged LanguageChanged;
    public static LocalizationSystem instance { get; private set; }
    public string CurrentLanguage { get; private set; }

    private Dictionary<string, string> _localizedText = null;
    private Dictionary<string, TextAsset> _localizationFiles = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        // загрузка языка предыдущего сеанса при создании объекта
        string startLanguage = PlayerPrefs.GetString("language", CultureInfo.InstalledUICulture.Name);
        LoadLocalizedText(LocalizationModes.SET, startLanguage);
        this.gameObject.SetActive(true);  
    }

    public void LoadLocalizedText(LocalizationModes mode, string languageCode = "en")
    {
        if (LoadLocalizationFilesAndIsEmpty()) return;
        if (!DetermineCurrentLanguageAndIsModeCorrect(mode, languageCode)) return;
        SettingPlayerLanguage();
        if (ReadingLanguageFileAndIsNull()) return;
        // обновление всех подписанных текстов
        LanguageChanged?.Invoke();
    }

    public string GetLocalizedText(string textKey)
    {
        if (_localizedText.TryGetValue(textKey, out string value)) return value;
        Debug.LogWarning($"LocalizationSystem: Invalid key - {textKey}");
        return textKey;
    }

    private bool LoadLocalizationFilesAndIsEmpty()
    {
        // заполнение словаря всех файлов локализации
        _localizationFiles = new Dictionary<string, TextAsset>();
        string language;
        TextAsset languageAsset = null;
        string localizationPath = "Assets/Resources/Localization/";
        string[] files = Directory.GetFiles(localizationPath, "*.txt");
        foreach (string filePath in files)
        {
            language = Path.GetFileNameWithoutExtension(filePath);
            languageAsset = Resources.Load($"Localization/{language}") as TextAsset;
            if (!_localizationFiles.TryAdd(language, languageAsset))
            {
                Debug.LogError($"LocalizationSystem: localization file was not added");
            }
        }

        if (_localizationFiles.Count <= 0)
        {
            Debug.LogError($"LocalizationSystem: localization files are missing");
            return true;
        }
        return false;
    }

    private bool DetermineCurrentLanguageAndIsModeCorrect(LocalizationModes mode, string languageCode)
    {
        if (mode == LocalizationModes.SWITCH_NEXT)
        {
            List<TextAsset> list = _localizationFiles.Values.ToList();
            int currentIndex = list.IndexOf(_localizationFiles.GetValueOrDefault(CurrentLanguage));
            int nextIndex = (currentIndex + 1) % _localizationFiles.Count;
            CurrentLanguage = _localizationFiles.ElementAt(nextIndex).Key;
        }
        else if (mode == LocalizationModes.SET)
        {
            CurrentLanguage = languageCode;
        }
        else
        {
            Debug.LogError($"LocalizationSystem: Invalid mode - {mode}");
            return false;
        }
        return true;
    }

    private bool ReadingLanguageFileAndIsNull()
    {
        TextAsset selectedFile = null;
        if (!_localizationFiles.TryGetValue(CurrentLanguage, out selectedFile))
        {
            Debug.LogError($"LocalizationSystem: Localization file not found for language - {CurrentLanguage}");
            return true;
        }
        // чтение ключ-значение
        _localizedText = new Dictionary<string, string>();
        string[] lines = selectedFile.text.Split('\n');
        foreach (string item in lines)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                string[] keyValue = item.Split('=');
                if (keyValue.Length == 2)
                {
                    _localizedText[keyValue[0].Trim()] = keyValue[1].Trim();
                    continue;
                }
                Debug.LogWarning($"LocalizationSystem: Invalid line - {item}");
            }
        }
        return false;
    }

    private void SettingPlayerLanguage()
    {
        PlayerPrefs.SetString("language", CurrentLanguage);
        PlayerPrefs.Save();
    }
}
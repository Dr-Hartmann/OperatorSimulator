using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

public class LocalizationSystem : MonoBehaviour
{
    public delegate void OnLanguageChanged();
    public static event OnLanguageChanged LanguageChanged;

    public static LocalizationSystem instance;
    public string CurrentLanguage { get; private set; }

    private Dictionary<string, string> _localizedText;
    private Dictionary<string, TextAsset> _localizationFiles;

    public void Awake()
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

        // загрузка языка предыдущего сеанса
        string startLanguage = PlayerPrefs.GetString("language", CultureInfo.InstalledUICulture.Name);
        LoadLocalizedText(LocalizationModes.SET, startLanguage);
    }

    private void LoadLocalizationFiles()
    {
        // заполнение словаря всех файлов локализации
        _localizationFiles = new Dictionary<string, TextAsset>();
        string language;
        TextAsset languageAsset;
        string localizationPath = "Assets/Resources/Localization/";
        string[] files = Directory.GetFiles(localizationPath, "*.txt");
        foreach (string filePath in files)
        {
            language = Path.GetFileNameWithoutExtension(filePath);
            languageAsset = Resources.Load("Localization/" + language) as TextAsset;
            _localizationFiles.TryAdd(language, languageAsset);
        }
    }

    public void LoadLocalizedText(LocalizationModes mode, string languageCode = "en")
    {
        LoadLocalizationFiles();

        if (_localizationFiles.Count <= 0)
        {
            Debug.LogError($"LocalizationSystem: localization files are missing");
            return;
        }

        // определение режима смены языка
        if (mode == LocalizationModes.SWITCH_NEXT)
        {
            var list = _localizationFiles.Values.ToList();
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
            return;
        }

        // установка значений по умолчанию
        PlayerPrefs.SetString("language", CurrentLanguage);
        PlayerPrefs.Save();

        // выгрузка данных нового языка
        TextAsset selectedFile = null;
        if (!_localizationFiles.TryGetValue(CurrentLanguage, out selectedFile))
        {
            Debug.LogError($"LocalizationSystem: Localization file not found for language - {CurrentLanguage}");
            return;
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
                }
                else
                {
                    Debug.LogWarning($"LocalizationSystem: Invalid line - {item}");
                }
            }
        }

        // обновление всех подписанных текстов
        LanguageChanged?.Invoke();
    }

    public string GetLocalizedText(string textKey)
    {
        if (_localizedText.TryGetValue(textKey, out string value))
        {
            return value;
        }

        Debug.LogWarning($"LocalizationSystem: Invalid key - {textKey}");
        return textKey;
    }
}
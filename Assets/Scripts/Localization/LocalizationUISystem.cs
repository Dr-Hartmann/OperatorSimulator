using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

internal class LocalizationUISystem : MonoBehaviour
{
    [SerializeField] private string _uiFolderName;

    public delegate void OnLanguageChanged();
    public static event OnLanguageChanged LanguageChanged;
    public static LocalizationUISystem instance { get; private set; }
    public string CurrentLanguage { get; private set; }

    private Dictionary<string, string> _languagePath = null;
    private Dictionary<string, string> _localizedText = null;

    private void Awake()
    {
        if (IsNullAndSetInstance()) return;
        PlayerPrefs.DeleteAll(); // TODO - удалить настройки пользователя, использовать log файлы

        string startLanguage = PlayerPrefs.GetString("language", CultureInfo.InstalledUICulture.TwoLetterISOLanguageName);
        if (CheckFilesAndIsEmpty()) return;
        if (startLanguage == "") startLanguage = "en";
        LoadUIText(LocalizationModes.SET, startLanguage);

        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        
    }

    private bool IsNullAndSetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return false;
        }
        else
        {
            Destroy(this.gameObject);
            return true;
        }
    }

    public void LoadUIText(LocalizationModes mode, string languageCode = "en")
    {
        if (!IsModeCorrectAndLoadText(mode, languageCode)) return;
        SetThisPlayerLanguage();
        if (ReadingFileAndIsNull()) return;
        // обновление всех подписанных текстов
        LanguageChanged?.Invoke();
    }

    public string GetText(string textKey)
    {
        if (_localizedText.TryGetValue(textKey, out string value)) return value;
        Debug.LogWarning($"LocalizationUISystem: Invalid key - {textKey}");
        return textKey;
    }

    private bool CheckFilesAndIsEmpty()
    {
        _languagePath = new Dictionary<string, string>();
        string localizationPath = Application.streamingAssetsPath + "/Localization/" + _uiFolderName;
        string[] filesPath = Directory.GetFiles(localizationPath, "*.txt");
        foreach (string path in filesPath)
        {
            _languagePath.Add(Path.GetFileNameWithoutExtension(path), path);
        }

        if (_languagePath.Count <= 0)
        {
            Debug.LogError($"LocalizationUISystem: localization files are missing");
            return true;
        }
        return false;
    }

    private bool IsModeCorrectAndLoadText(LocalizationModes mode, string languageCode)
    {
        if (mode == LocalizationModes.SWITCH_NEXT)
        {
            List<string> list = _languagePath.Values.ToList();
            int currentIndex = list.IndexOf(_languagePath.GetValueOrDefault(CurrentLanguage));
            int nextIndex = (currentIndex + 1) % _languagePath.Count;
            CurrentLanguage = _languagePath.ElementAt(nextIndex).Key;
        }
        else if (mode == LocalizationModes.SET)
        {
            CurrentLanguage = languageCode;
        }
        else
        {
            Debug.LogError($"LocalizationUISystem: Invalid mode - {mode}");
            return false;
        }
        return true;
    }

    private bool ReadingFileAndIsNull()
    {
        // чтение ключ-значение
        _localizedText = new Dictionary<string, string>();
        string[] lines = File.ReadAllText(_languagePath[CurrentLanguage]).Split("</END>");
        foreach (string item in lines)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                string[] keyValue = item.Split("===");
                if (keyValue.Length == 2)
                {
                    _localizedText[keyValue[0].Trim()] = keyValue[1].Trim();
                    continue;
                }
                Debug.LogWarning($"LocalizationUISystem: Invalid line - {item}");
            }
        }
        return false;
    }

    private void SetThisPlayerLanguage()
    {
        PlayerPrefs.SetString("language", CurrentLanguage);
        PlayerPrefs.Save();
    }
}
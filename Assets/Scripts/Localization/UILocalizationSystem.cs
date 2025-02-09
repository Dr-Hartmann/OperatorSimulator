using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

internal class UILocalizationSystem : MonoBehaviour
{
    [SerializeField] private string _uiPath;
    [SerializeField] private string _separator = "===";
    [SerializeField] private string _endLine = "</END>";

    public delegate void OnLanguageChanged();
    public static event OnLanguageChanged LanguageChanged;
    public static UILocalizationSystem instance { get; private set; }
    public string CurrentLanguage { get; private set; }

    private Dictionary<string, string> _languageFiles = null;
    private Dictionary<string, string> _localizedText = null;

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

        if (IsInstanceNull()) return;
        if (!GetLanguageFiles()) return;

        PlayerPrefs.DeleteAll(); // TODO - удалить настройки пользователя, использовать log файлы

        string currentCulture = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
        string startLanguage = PlayerPrefs.GetString("language", currentCulture);

        LoadUIText(UILocalizationModes.SET, startLanguage);

        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        
    }

    public void LoadUIText(UILocalizationModes mode, string languageCode = "en")
    {
        if (IsEmptyLocalizationFiles()) return;
        if(!DetermineAndSetLanguage(mode, languageCode)) return;
        if (!ReadCurrentLanguageFile()) return;

        // обновление всех подписанных текстов
        LanguageChanged?.Invoke();
    }

    public string GetText(string textKey)
    {
        if (IsEmptyLocalizationText()) return textKey;
        if (_localizedText.TryGetValue(textKey, out string value)) return value;
        SimulationUtilities.DisplayWarning($"Invalid key - {textKey}");
        return textKey;
    }

    public bool IsEmptyLocalizationFiles()
    {
        if (_languageFiles == null || _languageFiles.Count <= 0)
        {
            SimulationUtilities.DisplayError($"Localization files do not exist");
            return true;
        }
        return false;
    }
    public bool IsEmptyLocalizationText()
    {
        if (_localizedText == null || _localizedText.Count <= 0)
        {
            SimulationUtilities.DisplayError($"There is no text");
            return true;
        }
        return false;
    }
    public bool IsInstanceNull()
    {
        if (instance == null)
        {
            SimulationUtilities.DisplayError("Instance is null");
            return true;
        }
        return false;
    }
    public bool CheckLanguageAvailability(string languageCode)
    {
        bool check = _languageFiles.TryGetValue(languageCode, out var value);
        if (!check) SimulationUtilities.DisplayError("Language not found");
        return check;
    }

    private bool GetLanguageFiles()
    {
        _languageFiles = new Dictionary<string, string>();
        string localizationPath = Application.streamingAssetsPath + "/" + _uiPath;
        string[] filesPath = Directory.GetFiles(localizationPath, "*.txt");
        foreach (string path in filesPath)
        {
            _languageFiles.Add(Path.GetFileNameWithoutExtension(path), path);
        }

        if (IsEmptyLocalizationFiles()) return false;
        return true;
    }
    private bool DetermineAndSetLanguage(UILocalizationModes mode, string languageCode)
    {
        if(!CheckLanguageAvailability(languageCode)) return false;

        switch (mode)
        {
            case UILocalizationModes.SWITCH_NEXT:
                List<string> list = _languageFiles.Values.ToList();
                int currentIndex = list.IndexOf(_languageFiles.GetValueOrDefault(CurrentLanguage));
                int nextIndex = (currentIndex + 1) % _languageFiles.Count;
                CurrentLanguage = _languageFiles.ElementAt(nextIndex).Key;
                break;

            case UILocalizationModes.SET:
                CurrentLanguage = languageCode;
                break;

            default:
                SimulationUtilities.DisplayError($"Invalid mode - {mode}");
                break;
        }
        SetThisPlayerLanguage();

        return true;
    }
    private bool ReadCurrentLanguageFile()
    {
        _localizedText = new Dictionary<string, string>();
        string[] lines = File.ReadAllText(_languageFiles[CurrentLanguage]).Split(_endLine);
        foreach (string item in lines)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                string[] keyValue = item.Split(_separator);
                if (keyValue.Length == 2)
                {
                    _localizedText[keyValue[0].Trim()] = keyValue[1].Trim();
                    continue;
                }
                SimulationUtilities.DisplayWarning($"Invalid line - {item}");
            }
        }

        if(IsEmptyLocalizationText()) return false;
        return true;
    }
    private void SetThisPlayerLanguage()
    {
        PlayerPrefs.SetString("language", CurrentLanguage);
        PlayerPrefs.Save();
    }
}
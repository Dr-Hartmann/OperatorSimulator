using UnityEngine;
using System.Collections.Generic;
using System;

public class LocalizationSystem : MonoBehaviour
{
    [SerializeField] private TextAsset _localizationFileEN;
    [SerializeField] private TextAsset _localizationFileRU;

    public delegate void OnLanguageChanged();
    public static event OnLanguageChanged LanguageChanged;

    public static LocalizationSystem instance;
    public LocalizationLanguageEnum CurrentLanguage { get; private set; } = LocalizationLanguageEnum.RU;
    private int _count = Enum.GetNames(typeof(LocalizationLanguageEnum)).Length;

    private Dictionary<string, string> _localizedText;
    private string _currentLanguage;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _currentLanguage = PlayerPrefs.GetString("language", "ru");
        LoadLanguage(CurrentLanguage);
    }

    public void LoadLanguage(LocalizationLanguageEnum languageCode)
    {
        TextAsset selectedFile;

        CurrentLanguage = (LocalizationLanguageEnum)((CurrentLanguage.GetHashCode()+1) % _count);

        if (CurrentLanguage == LocalizationLanguageEnum.RU)
        {
            _currentLanguage = "ru";
            selectedFile = _localizationFileRU;
        }
        else if (CurrentLanguage == LocalizationLanguageEnum.EN)
        {
            _currentLanguage = "en";
            selectedFile = _localizationFileEN;
        }
        else
        {
            _currentLanguage = "en";
            selectedFile = _localizationFileEN;
        }

        PlayerPrefs.SetString("language", _currentLanguage);
        PlayerPrefs.Save();

        if (selectedFile == null)
        {
            Debug.LogError($"LocalizationSystem: Localization file not found for language - {languageCode}");
            return;
        }

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
                    Debug.LogWarning($"LocalizationSystem: Invalid line in localization file - {item}");
                }
            }
        }

        LanguageChanged?.Invoke();
    }

    public string GetLocalizedText(string key)
    {
        if (_localizedText.TryGetValue(key, out string value)) return value;

        Debug.LogWarning($"LocalizationSystem: Translation missing for key - {key}");
        return key;
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System;

// TODO - убрать првязку к StreamingAssets, Addressables?

namespace UserInterface
{
    /// <summary>
    /// Система локализации интерфейса.
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        [SerializeField] private UISettings _uiSettings;

        private string _uiPath, _uiSeparator, _uiEndLine;
        private Dictionary<string, string> _uiFiles;
        private Dictionary<string, string> _uiText;
        private Action LanguageChanged;
        
   
        public static string CurrentLanguage { get; private set; } = _defaultLanguage;
        public static void SubEventLanguageChanged(Action action)
        {
            Instance.LanguageChanged += action;
        }
        public static void UnsubEventLanguageChanged(Action action)
        {
            if (_instance) _instance.LanguageChanged -= action;
        }
        public static void SetUI(LocalizationModes mode, string languageCode = _defaultLanguage)
        {
            if (Instance.IsEmptyUIFiles()) return;

            if (!Instance._uiFiles.TryGetValue(languageCode, out var value))
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning("Language not found");
                languageCode = _defaultLanguage;
            }

            if (!Instance.SetLanguage(mode, languageCode)) return;
            if (!Instance.ReadUIFile()) return;

            Instance.LanguageChanged?.Invoke();
            Instance.SetPlayerPrefs();
        }
        public static string GetUIText(string key)
        {
            if (Instance.IsEmptyUIText()) return key;
            if (Instance._uiText.TryGetValue(key, out string value)) return value;
            GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid key - {key}");
            return key;
        }


        private void Awake()
        {
            if (!_uiSettings) return;
            _uiPath = _uiSettings.UiPath;
            _uiSeparator = _uiSettings.UiSeparator;
            _uiEndLine = _uiSettings.UiEndLine;

            if (!GetUIFiles()) return;
            string currentCulture = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
            string startLanguage = PlayerPrefs.GetString("language", currentCulture);
            SetUI(LocalizationModes.SET, startLanguage);
        }
        private bool SetLanguage(LocalizationModes mode, string languageCode)
        {
            switch (mode)
            {
                case LocalizationModes.SWITCH_NEXT:
                    List<string> list = _uiFiles.Values.ToList();
                    int currentIndex = list.IndexOf(_uiFiles.GetValueOrDefault(CurrentLanguage));
                    int nextIndex = (currentIndex + 1) % _uiFiles.Count;
                    CurrentLanguage = _uiFiles.ElementAt(nextIndex).Key;
                    break;

                case LocalizationModes.SET:
                    CurrentLanguage = languageCode;
                    break;

                default:
                    GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid mode - {mode}");
                    break;
            }
            return true;
        }
        private void SetPlayerPrefs()
        {
            PlayerPrefs.SetString("language", CurrentLanguage);
            PlayerPrefs.Save();
        }
        private bool ReadUIFile()
        {
            _uiText = new Dictionary<string, string>();
            string[] lines = File.ReadAllText(_uiFiles[CurrentLanguage]).Split(_uiEndLine);
            foreach (string item in lines)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    string[] keyValue = item.Split(_uiSeparator);
                    if (keyValue.Length == 2)
                    {
                        _uiText[keyValue[0].Trim()] = keyValue[1].Trim();
                        continue;
                    }
                    GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid line - {item}");
                }
            }
            if (IsEmptyUIText()) return false;
            return true;
        }
        private bool GetUIFiles()
        {
            _uiFiles = new Dictionary<string, string>();
            string localizationPath = Application.streamingAssetsPath + "/" + _uiPath;
            string[] filesPath = Directory.GetFiles(localizationPath, "*.txt");
            foreach (string path in filesPath)
            {
                _uiFiles.Add(Path.GetFileNameWithoutExtension(path), path);
            }
            if (IsEmptyUIFiles()) return false;
            return true;
        }
        private bool IsEmptyUIFiles()
        {
            if (_uiFiles == null || _uiFiles.Count <= 0)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"Localization files do not exist");
                return true;
            }
            return false;
        }
        private bool IsEmptyUIText()
        {
            if (_uiText == null || _uiText.Count <= 0)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"There is no text");
                return true;
            }
            return false;
        }


        private static UISystem Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindAnyObjectByType<UISystem>();
                    if (!_instance) return null;
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private static UISystem _instance;

        private const string _defaultLanguage = "en"; // TODO - извлечь

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoadMethod()
        {
            _instance = null;
        }
    }
}


//public static void SubEventMessageDisplayed(Action<string, float> action)
//{
//    Instance.MessageDisplayed += action;
//}
//public static void UnsubEventMessageDisplayed(Action<string, float> action)
//{
//    if (_instance) _instance.MessageDisplayed -= action;
//}
//public static void SubEventMessageDeleted(Action action)
//{
//    Instance.MessageDeleted += action;
//}
//public static void UnsubEventMessageDeleted(Action action)
//{
//    if (_instance) _instance.MessageDeleted -= action;
//}
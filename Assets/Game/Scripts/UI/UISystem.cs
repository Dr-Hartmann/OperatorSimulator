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
    public static class UISystem
    {
        private const string DEFAULT_LANGUAGE = "en";

        #region PUBLIC
        public static string CurrentLanguage { get; private set; }

        public static void UpdateLanguageUI(LocalizationModes mode)
        {
            if (IsEmptyFiles()) return;
            SetCurrentLanguage(mode);
            SetPlayerPrefs();
            ReadFileText();

            if (IsEmptyText()) return;
            LanguageChanged?.Invoke();
        }
        public static string GetText(string key)
        {
            if (IsEmptyText()) return key;
            if (Text.TryGetValue(key, out string value)) return value;
            GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid key - {key}");
            return key;
        }
        public static void SubEventLanguageChanged(Action action)
        {
            LanguageChanged += action;
        }
        public static void UnsubEventLanguageChanged(Action action)
        {
            LanguageChanged -= action;
        }
        #endregion

        #region CORE
        public static void Init(string uiPath, string uiSeparator, string uiEndLine)
        {
            Path = uiPath;
            Separator = uiSeparator;
            EndLine = uiEndLine;
            GetUIFiles();
        }
        public static void Begin()
        {
            UpdateLanguageUI(LocalizationModes.SET_DEFAULT);
        }
        #endregion

        #region handlers
        private static void SetCurrentLanguage(LocalizationModes mode, string languageCode = DEFAULT_LANGUAGE)
        {
            switch (mode)
            {
                case LocalizationModes.SWITCH_NEXT:
                    List<string> list = Files.Values.ToList();
                    int currentIndex = list.IndexOf(Files.GetValueOrDefault(CurrentLanguage));
                    int nextIndex = (currentIndex + 1) % Files.Count;
                    CurrentLanguage = Files.ElementAt(nextIndex).Key;
                    break;

                case LocalizationModes.SET:
                    CurrentLanguage = languageCode;
                    break;

                case LocalizationModes.SET_DEFAULT:
                    string currentCulture = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                    CurrentLanguage = PlayerPrefs.GetString("language", currentCulture);
                    if (!Files.TryGetValue(CurrentLanguage, out var value))
                    {
                        GameUtilities.Debug.GameUtilities.DisplayWarning("Language not found");
                        CurrentLanguage = DEFAULT_LANGUAGE;
                    }
                    break;

                default:
                    GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid mode - {mode}");
                    break;
            }
        }
        private static void ReadFileText()
        {
            Text = new Dictionary<string, string>();
            string[] lines = File
                .ReadAllText(Files[CurrentLanguage])
                .Split(EndLine);
            foreach (string item in lines)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    string[] keyValue = item.Split(Separator);
                    if (keyValue.Length == 2)
                    {
                        Text[keyValue[0].Trim()] = keyValue[1].Trim();
                        continue;
                    }
                    GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid line - {item}");
                }
            }
        }
        private static void GetUIFiles()
        {
            Files = new Dictionary<string, string>();
            string localizationPath = Application.streamingAssetsPath + "/" + Path;
            string[] filesPath = Directory.GetFiles(localizationPath, "*.txt");
            foreach (string path in filesPath)
            {
                Files.Add(System.IO.Path.GetFileNameWithoutExtension(path), path);
            }
        }
        private static void SetPlayerPrefs()
        {
            PlayerPrefs.SetString("language", CurrentLanguage);
            PlayerPrefs.Save();
        }
        private static bool IsEmptyFiles()
        {
            if (Files == null || Files.Count <= 0)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"Localization files do not exist");
                return true;
            }
            return false;
        }
        private static bool IsEmptyText()
        {
            if (Text == null || Text.Count <= 0)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"There is no text");
                return true;
            }
            return false;
        }
        #endregion

        #region variables
        private static string Path { get; set; }
        private static string Separator { get; set; }
        private static string EndLine { get; set; }
        private static Dictionary<string, string> Files { get; set; }
        private static Dictionary<string, string> Text { get; set; }
        private static Action LanguageChanged { get; set; }
        #endregion
    }
}
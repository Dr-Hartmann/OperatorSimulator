using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System;

// TODO - убрать првязку к StreamingAssets, Addressables?

/// <summary>
/// Система локализации интерфейса.
/// Организует чтение диалогов из .json-файла в класс <see cref="JSONDialog"/> и получение нужного набора по ключу.
/// 
/// </summary>
public sealed partial class GUISystem : MonoBehaviour, IGUISystem
{
    [SerializeField] private int _targetFrameRate = 30;

    public static IGUISystem Instance { get; private set; }
    public static Action LanguageChanged;
    public static Action<string, float> MessageDisplayed;
    public static Action MessageDeleted;

    private JSONDialog _dialog;
    private Dictionary<string, string> _uiFiles;
    private Dictionary<string, string> _uiText;

    private static int _tipsCounter = 0;

    public void SetUI(LocalizationModes mode, string languageCode = SimulationUtilities.DEFAULT_LANGUAGE)
    {
        if (IsEmptyUIFiles()) return;

        if (!_uiFiles.TryGetValue(languageCode, out var value))
        {
            SimulationUtilities.DisplayWarning("Language not found");
            languageCode = SimulationUtilities.DEFAULT_LANGUAGE;
        }

        if (!SetLanguage(mode, languageCode)) return;
        if (!ReadUIFile()) return;

        LanguageChanged?.Invoke();
        SetPlayerPrefs();
    }
    public string GetUIText(string key)
    {
        if (IsEmptyUIText()) return key;
        if (_uiText.TryGetValue(key, out string value)) return value;
        SimulationUtilities.DisplayWarning($"Invalid key - {key}");
        return key;
    }
    public bool ReadDialogJSON(string path)
    {
        _dialog = new();
        TextAsset json = Resources.Load(path) as TextAsset;
        _dialog = JsonConvert.DeserializeObject<JSONDialog>(json.text);
        if (_dialog.text.Count <= 0)
        {
            SimulationUtilities.DisplayWarning("Path is empty");
            return false;
        }
        return true;
    }
    public Dictionary<string, List<string>> GetDialog(string key)
    {
        _dialog.text.TryGetValue(key, out var dialog);
        return dialog;
    }


    private void Awake()
    {
        if (!SetInstance()) return;
        SetFrameRate(_targetFrameRate, 0);

        if (!GetUIFiles()) return;
        string currentCulture = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
        string startLanguage = PlayerPrefs.GetString("language", currentCulture);
        SetUI(LocalizationModes.SET, startLanguage);
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        MessageDisplayed += CreateTip;
        MessageDeleted += DeleteTip;
    }
    private void OnDisable()
    {
        MessageDisplayed -= CreateTip;
        MessageDeleted -= DeleteTip;
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    private void SetFrameRate(int frameRate, int vSyncCount)
    {
        QualitySettings.vSyncCount = vSyncCount;
        Application.targetFrameRate = frameRate;
    }
    private bool SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }


    [RuntimeInitializeOnLoadMethod]
    private static void InitializeOnLoadMethod()
    {
        LanguageChanged = null;
        MessageDisplayed = null;
        MessageDeleted = null;
        _tipsCounter = 0;
    }
}


public partial class GUISystem
{
    private bool SetLanguage(LocalizationModes mode, string languageCode)
    {
        switch (mode)
        {
            case LocalizationModes.SWITCH_NEXT:
                List<string> list = _uiFiles.Values.ToList();
                int currentIndex = list.IndexOf(_uiFiles.GetValueOrDefault(SimulationUtilities.CurrentLanguage));
                int nextIndex = (currentIndex + 1) % _uiFiles.Count;
                SimulationUtilities.CurrentLanguage = _uiFiles.ElementAt(nextIndex).Key;
                break;

            case LocalizationModes.SET:
                SimulationUtilities.CurrentLanguage = languageCode;
                break;

            default:
                SimulationUtilities.DisplayWarning($"Invalid mode - {mode}");
                break;
        }        
        return true;
    }
    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetString("language", SimulationUtilities.CurrentLanguage);
        PlayerPrefs.Save();
    }
}

public partial class GUISystem
{
    [Header("Tip settings")]
    [SerializeField] private GameObject _tipsPrefab;
    [SerializeField] private RectTransform _tipsContent;
    [SerializeField] private int _tipsMaxNumber = 20;
    //[SerializeField] private Animator _animator;


    private void CreateTip(string message, float lifetime)
    {
        if (_tipsCounter <= _tipsMaxNumber)
        {
            GameObject obj = Instantiate(_tipsPrefab, _tipsContent);
            Tip tip = obj.GetComponent<Tip>();
            tip.Index = ++_tipsCounter;
            tip.Lifetime = lifetime;
            tip.Message = message;
        }
    }
    private void DeleteTip()
    {
        --_tipsCounter;
    }
}

public partial class GUISystem
{
    [Header("UI settings")]
    [SerializeField] private string _uiPath;
    [SerializeField] private string _uiSeparator = "===";
    [SerializeField] private string _uiEndLine = "</END>";


    private bool ReadUIFile()
    {
        _uiText = new Dictionary<string, string>();
        string[] lines = File.ReadAllText(_uiFiles[SimulationUtilities.CurrentLanguage]).Split(_uiEndLine);
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
                SimulationUtilities.DisplayWarning($"Invalid line - {item}");
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
            SimulationUtilities.DisplayWarning($"Localization files do not exist");
            return true;
        }
        return false;
    }
    private bool IsEmptyUIText()
    {
        if (_uiText == null || _uiText.Count <= 0)
        {
            SimulationUtilities.DisplayWarning($"There is no text");
            return true;
        }
        return false;
    }
}
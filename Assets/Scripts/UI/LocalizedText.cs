using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;
    private TextMeshProUGUI _uiText;

    public void Awake()
    {
        _uiText = this.GetComponent<TextMeshProUGUI>();
        if (_uiText == null)
        {
            Debug.LogError("LocalizedText: Text component not found - " + key);
        }

        LocalizationSystem.LanguageChanged += UpdateText;
    }

    public void Start()
    {
        UpdateText();
    }

    public void OnDestroy()
    {
        LocalizationSystem.LanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (_uiText != null)
        {
            _uiText.SetText(LocalizationSystem.instance.GetLocalizedText(key));
            Debug.LogWarning($"обнова");
        }
    }
}
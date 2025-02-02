using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _textKey;
    private TextMeshProUGUI _thisText;

    public void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        if (_thisText == null)
        {
            Debug.LogError("LocalizedText: Text component not found - " + _textKey);
            return;
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
        if (_thisText != null)
        {
            _thisText.SetText(LocalizationSystem.instance.GetLocalizedText(_textKey));
        }
    }
}
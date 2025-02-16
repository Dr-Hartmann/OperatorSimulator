using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
internal class LocalizedUIText : MonoBehaviour
{
    [SerializeField] private string _textKey;
    [SerializeField] private RectTransform _shell;
    [SerializeField] private float _additionalHeight = 10f;
    private TextMeshProUGUI _thisText;

    private void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (IsNull()) return;
        LocalizationUISystem.LanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDestroy()
    {
        LocalizationUISystem.LanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (IsNull()) return;
        _thisText.SetText(LocalizationUISystem.instance.GetText(_textKey));
        AdjustWidth();
    }

    private void AdjustWidth()
    {
        _shell.sizeDelta = new Vector2(_thisText.preferredWidth, _thisText.preferredHeight + _additionalHeight);
    }

    private bool IsNull()
    {
        if (_thisText == null || _textKey == "" || _shell == null)
        {
            SimulationUtilities.DisplayError($"Invalid ui-text object - {_textKey}");
            return true;
        }
        return false;
    }
}
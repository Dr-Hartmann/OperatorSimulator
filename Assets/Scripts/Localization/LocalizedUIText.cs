using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
internal class LocalizedUIText : MonoBehaviour
{
    [SerializeField] private string _textKey;
    [SerializeField] private RectTransform _shell;
    private TextMeshProUGUI _thisText;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        if (_thisText == null || _textKey == "" || _shell == null)
        {
            Debug.LogError($"LocalizedText: Invalid ui-text object - {_textKey}");
            return;
        }

        LocalizationUISystem.LanguageChanged += UpdateTextAndAdjustWidth;
        UpdateTextAndAdjustWidth();
    }

    private void OnDestroy()
    {
        LocalizationUISystem.LanguageChanged -= UpdateTextAndAdjustWidth;
    }

    public void UpdateTextAndAdjustWidth()
    {
        if (_thisText == null || _textKey == "" || _shell == null)
        {
            Debug.LogError($"LocalizedText: Invalid ui-text object - {_textKey}");
            return;
        }
        _thisText.SetText(LocalizationUISystem.instance.GetText(_textKey));
        AdjustWidth();
    }

    private void AdjustWidth()
    {
        Vector2 _thisSizeDelta = _shell.sizeDelta;
        _shell.sizeDelta = new Vector2(_thisText.preferredWidth, _thisSizeDelta.y);

        // если pivot в центре
        /*
         * Vector2 _thisSizeDelta = this.GetComponent<RectTransform>().sizeDelta;
         * Vector2 _bufSizeDelta = new Vector2(_thisSizeDelta.x, _thisSizeDelta.y);
         * _thisSizeDelta = new Vector2(_thisText.preferredWidth, _thisSizeDelta.y);
         * float deltaX = _bufSizeDelta.x - _thisSizeDelta.x;
         * this.GetComponent<RectTransform>().sizeDelta = _thisSizeDelta;

         * Vector2 _thisAnchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
         * this.GetComponent<RectTransform>().anchoredPosition
         *    = new Vector2(_thisAnchoredPosition.x - deltaX / 2f, _thisAnchoredPosition.y);
        */
    }
}
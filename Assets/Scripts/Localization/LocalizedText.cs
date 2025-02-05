using TMPro;
using UnityEngine;

internal class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _textKey;
    [SerializeField] private RectTransform _shell;
    private TextMeshProUGUI _thisText;

    private void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        if (_thisText == null)
        {
            Debug.LogError($"LocalizedText: Text component not found - {_textKey}");
            return;
        }
        LocalizationSystem.LanguageChanged += UpdateText;
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnDestroy()
    {
        LocalizationSystem.LanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (_thisText == null)
        {
            Debug.LogError($"LocalizedText: Text component not found - {_textKey}");
            return;
        }
        _thisText.SetText(LocalizationSystem.instance.GetLocalizedText(_textKey));
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
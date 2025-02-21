using TMPro;
using UnityEngine;

/// <summary>
/// Организует подписку и отписку на событие <see cref="LocalizationUISystem.LanguageChanged"/>
/// и изменяет компонент <see cref="TextMeshProUGUI"/> в зависимости от ключа.
/// Язык явным образом не указывается.
/// Адаптирует размер родительского контейнера <see cref="RectTransform"/>
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedUIText : MonoBehaviour
{
    [SerializeField] private string _textKey;

    private TextMeshProUGUI _thisText;
    private RectTransform _parentContainer;


    private void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        _parentContainer = base.transform.parent.GetComponent<RectTransform>();
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


    /// <summary>
    /// Обновляет текст компонента <see cref="TextMeshProUGUI"/>,
    /// получая его от экземпляра <see cref="LocalizationUISystem"/>,
    /// и регулирует ширину и высоту родительского компонента <see cref="RectTransform"/>.
    /// </summary>
    public void UpdateText()
    {
        if (IsNull()) return;
        string newText = LocalizationUISystem.instance.GetCurrentLanguageText(_textKey);
        _thisText.SetText(newText);

        if (_parentContainer)
        {
            // TODO - это вообще надо?
            _parentContainer.sizeDelta
            = new Vector2(_thisText.preferredWidth + 60f, _thisText.fontSize + 50f);
        }
    }


    private bool IsNull()
    {
        if (_thisText == null || _textKey == "" || _parentContainer == null)
        {
            SimulationUtilities.DisplayError($"Invalid ui-text object - {_textKey}");
            return true;
        }
        return false;
    }
}
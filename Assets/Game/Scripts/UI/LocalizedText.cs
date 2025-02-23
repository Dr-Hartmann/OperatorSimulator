using TMPro;
using UnityEngine;

/// <summary>
/// Организует подписку и отписку на событие <see cref="GUISystem.LanguageChanged"/>
/// и изменяет компонент <see cref="TextMeshProUGUI"/> в зависимости от ключа.
/// Язык явным образом не указывается.
/// Адаптирует размер родительского контейнера <see cref="RectTransform"/>
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _key;

    private TextMeshProUGUI _text;
    private RectTransform _parent;
    private bool _isSubscribed = false;

    /// <summary>
    /// Обновляет текст и регулирует ширину и высоту родителя.
    /// </summary>
    public void UpdateText()
    {
        if (IsNull()) return;
        string newText = GUISystem.Instance.GetUIText(_key);
        _text.SetText(newText);

        if (_parent)
        {
            _parent.sizeDelta = new Vector2(_text.preferredWidth + 50f, _text.preferredHeight + 30f);
        }
    }


    private void Awake()
    {
        _text = this.GetComponent<TextMeshProUGUI>();
        _parent = this.transform.parent.GetComponent<RectTransform>();
    }
    private void Start()
    {
        if (IsNull()) return;
        GUISystem.LanguageChanged += UpdateText;
        _isSubscribed = true;
        UpdateText();
    }
    private void OnEnable()
    {  
        if(!_isSubscribed) GUISystem.LanguageChanged += UpdateText; 
    }
    private void OnDestroy()
    {
        GUISystem.LanguageChanged -= UpdateText;
    }
    private bool IsNull()
    {
        if (_text == null || _key == "" || _parent == null)
        {
            SimulationUtilities.DisplayWarning($"Invalid ui-text object - {_key}");
            return true;
        }
        return false;
    }
}
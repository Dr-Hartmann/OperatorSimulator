using TMPro;
using UnityEngine;
using UserInterface;

/// <summary>
/// Организует подписку и отписку на событие <see cref="UISystem.LanguageChanged"/> и изменяет компонент <see cref="TextMeshProUGUI"/> в зависимости от ключа.
/// Язык явным образом не указывается. Адаптирует размер родительского контейнера <see cref="RectTransform"/>
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _key;
    /// <summary>
    /// Обновляет текст и регулирует ширину и высоту родителя.
    /// </summary>
    public void UpdateText()
    {
        if (IsNull()) return;
        string newText = UISystem.GetUIText(_key);
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
        UpdateText();
    }
    private void OnEnable()
    {  
        UISystem.SubEventLanguageChanged(UpdateText); 
    }
    private void OnDestroy()
    {
        UISystem.UnsubEventLanguageChanged(UpdateText);
    }
    private bool IsNull()
    {
        if (_text == null || _key == "" || _parent == null)
        {
            GameUtilities.Debug.GameUtilities.DisplayWarning($"Invalid ui-text object - {_key}");
            return true;
        }
        return false;
    }
    private TextMeshProUGUI _text;
    private RectTransform _parent;
}
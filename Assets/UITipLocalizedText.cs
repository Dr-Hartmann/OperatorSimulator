using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITipLocalizedText : MonoBehaviour
{
    [SerializeField] private string _textKey;
    [SerializeField] private RectTransform _shell;

    private TextMeshProUGUI _thisText;

    private void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        if (_thisText.text == string.Empty) _thisText.text = $"Сообщение ";
    }



}

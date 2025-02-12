using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogLocalizedText : MonoBehaviour, IPointerClickHandler
    //, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _textKey;
    [SerializeField] private RectTransform _shell;
    [SerializeField] private Image _window;
    [SerializeField] private Color _enter;
    [SerializeField] private Color _exit;

    private TextMeshProUGUI _thisText;
    private List<string> _dialogText;

    public int TextListSize { get; private set; } = 0;
    public int CurrentIndexText { get; private set; } = 0;
    public string CurrentText { get; private set; }


    private void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        _dialogText = new List<string>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {   
        ReadDialogFile();
        UpdateTextAndAdjustWidth();
        //LocalizationUISystem.LanguageChanged += UpdateTextAndAdjustWidth;
    }

    private void OnDestroy()
    {
        //_thisButton.onClick.RemoveAllListeners();
        //LocalizationUISystem.LanguageChanged -= UpdateTextAndAdjustWidth;
    }

    private void ReadDialogFile()
    {
        _dialogText.Add("Тест1");
        _dialogText.Add("Тест2");
        _dialogText.Add("Тест3");
        //dialogText.Add();
        TextListSize = _dialogText.Count;
    }

    public void UpdateTextAndAdjustWidth()
    {
        if (CurrentIndexText < TextListSize)
        {
            _thisText.text = string.Empty;
            CurrentText = _dialogText[CurrentIndexText];
            StartCoroutine(GetText());
            CurrentIndexText = ++CurrentIndexText % TextListSize;
        }

        //_thisText.SetText(DialogSystem.instance.GetText(_textKey));
        AdjustWidth();
    }

    private IEnumerator GetText()
    {
        foreach (char c in CurrentText)
        {
            _thisText.text += c;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void AdjustWidth()
    {
        _shell.sizeDelta = new Vector2(_shell.sizeDelta.x, _thisText.preferredHeight + 10f);
    }




    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    _window.color = _enter;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    _window.color = _exit;
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateTextAndAdjustWidth();
    }

    private bool CheckReadValidity()
    {
        if (_thisText == null || _textKey == "" || _shell == null)
        {
            Debug.LogError($"Invalid ui-text object - {_textKey}");
            return false;
        }

        if (_dialogText == null || _dialogText.Count <= 0)
        {
            Debug.LogError($"Invalid text array");
            return false;
        }

        return true;
    }
}
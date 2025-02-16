using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

[RequireComponent(typeof(TextMeshProUGUI))]
internal class DialogLocalizedText : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float MAX_WIDTH = 1700f;
    [SerializeField] private DialogUI _shell;
    [SerializeField] private Image _window;

    private TextMeshProUGUI _thisText;

    public int TextListSize { get; private set; } = 0;
    public string CurrentText { get; private set; } = string.Empty;
    public int CurrentIndexText { get; private set; } = 0;
    public bool IsUpdating { get; private set; } = false;

    private Dictionary<string, string[]> _dialogText;
    private Coroutine _coroutineGetText;

    private void Awake()
    {
        _thisText = this.GetComponent<TextMeshProUGUI>();
        RectTransform _thisRectTransform = GetComponent<RectTransform>();
        _thisRectTransform.sizeDelta = new Vector2(MAX_WIDTH, _thisRectTransform.sizeDelta.y);
        _dialogText = new();
    }

    private void Start()
    {
        ReadDialogFile();
        UpdateText();
    }

    private void OnDestroy()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateText();
    }

    private void ReadDialogFile()
    {
        var a = DialogSystem.instance.GetText("Test");
        foreach (FieldInfo s in a.GetType().GetFields())
        {
            _dialogText.Add(s.Name, s.GetValue(a) as string[]);
        }
    }

    public void UpdateText()
    {
        if (!CheckReadValidity()) return;

        var list = _dialogText[SimulationUtilities.CurrentLanguage];
        TextListSize = list.Length;

        if (IsUpdating)
        {
            StopCoroutine(_coroutineGetText);
            _thisText.text = CurrentText;
            SetPreferredHeight();
            IsUpdating = false;
            return;
        }

        if (CurrentIndexText < TextListSize)
        {
            _thisText.text = string.Empty;
            CurrentText = list[CurrentIndexText].Trim();
            _coroutineGetText = StartCoroutine(GetText());
            SetCurrentIndexText();
            IsUpdating = true;
        }
    }

    private IEnumerator GetText()
    {
        foreach (char c in CurrentText)
        {
            _thisText.text += c;
            SetPreferredHeight();
            yield return new WaitForSeconds(.0001f);
        }
        IsUpdating = false;
    }

    private void SetPreferredHeight()
    {
        _shell.Height = _thisText.preferredHeight + _shell.AdditionalHeight(_thisText.preferredHeight);
    }

    private void SetCurrentIndexText()
    {
        CurrentIndexText = ++CurrentIndexText % TextListSize;
    }

    private bool CheckReadValidity()
    {
        if (_dialogText == null || _dialogText.Count <= 0)
        {
            Debug.LogError($"Invalid text array");
            return false;
        }
        return true;
    }
}
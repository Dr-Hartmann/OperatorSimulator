using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

// TODO - ускорить корутину
// TODO - максимальная ширина

public class Dialog : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float _maxWidth = 1700f;
    [SerializeField] private float _minHeight = 200f;
    [SerializeField] private float _maxHeight = 700f;
    [SerializeField] private float _additionalHeight = 100f;

    private Coroutine _routine;
    private TextMeshProUGUI _childText;
    private RectTransform _thisRectTransform;
    private Dictionary<string, List<string>> _dialogText;


    public string Key { get; private set; } = "test";
    public string Path { get; set; } = "Dialog/greeting";
    public bool IsUpdating { get; private set; } = false;
    public void StartDialog(string key, float width = 0)
    {
        if (width == 0) width = _maxWidth;
        //_childText.gameObject.GetComponent<RectTransform>()
        //    .sizeDelta = new Vector2(width, _this.sizeDelta.y);

        Key = key;
        if (!GUISystem.Instance.ReadDialogJSON(Path)) return;
        SwitchText();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentIndex >= _listSize)
        {
            Destroy(_thisRectTransform.gameObject);
            return;
        }
        SwitchText();
    }
    public void SetHeight(float value)
    {
        if (value < _minHeight) value = _minHeight;
        else if (value > _maxHeight) value = _maxHeight;
        _thisRectTransform.sizeDelta = new Vector2(_thisRectTransform.sizeDelta.x, value);
    }
    public void SetPreferredHeight()
    {
        float addHeight = _childText.preferredHeight < _minHeight - _additionalHeight ? 0 : _additionalHeight;
        SetHeight(_childText.preferredHeight + addHeight);
    }


    private void Awake()
    {
        _childText = this.GetComponentInChildren<TextMeshProUGUI>();
        _thisRectTransform = this.GetComponent<RectTransform>();

        // обводка
        _childText.outlineWidth = .5f;
        _childText.outlineColor = new Color32(0, 0, 0, 255);
        SetHeight(_minHeight);
        _dialogText = new();
    }
    private void SwitchText()
    {
        if (IsUpdating)
        {
            StopCoroutine(_routine);
            _childText.text = _text;
            SetPreferredHeight();
            IsUpdating = false;
            return;
        }

        if (!SetDialogDictionary()) return;
        List<string> list = new();
        if (!SetDialogList(ref list)) return;

        _listSize = list.Count;
        if (_currentIndex < _listSize)
        {
            _childText.text = string.Empty;
            _text = list[_currentIndex].Trim();
            _routine = StartCoroutine(PrintText());
            _currentIndex = ++_currentIndex;
            IsUpdating = true;
        }
    }
    private bool SetDialogDictionary()
    {
        var dialog = GUISystem.Instance?.GetDialog(Key);
        if (dialog == null)
        {
            GameUtilities.DisplayWarning($"Dictionary is empty - {Key}");
            dialog = new(GUISystem.Instance?.GetDialog("null"));
            if (dialog == null) return false;
        }
        _dialogText = new(dialog);
        return true;
    }
    private bool SetDialogList(ref List<string> list)
    {
        if (!_dialogText.TryGetValue(GameUtilities.CurrentLanguage, out list))
        {
            GameUtilities.DisplayWarning($"Language does not exist");
            return false;
        }
        if (list.Count <= 0)
        {
            GameUtilities.DisplayWarning($"Language list is empty");
            return false;
        }
        return true;
    }
    private IEnumerator PrintText()
    {
        foreach (char c in _text)
        {
            _childText.text += c;
            SetPreferredHeight();
            yield return new WaitForSecondsRealtime(.01f);
        }
        IsUpdating = false;
    }


    private int _listSize = 0;
    private int _currentIndex = 0;
    private string _text = string.Empty;
}
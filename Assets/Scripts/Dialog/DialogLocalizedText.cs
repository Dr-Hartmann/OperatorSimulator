using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

// TODO - ускорить корутину

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogLocalizedText : MonoBehaviour, IPointerClickHandler
{
    //[SerializeField] private float MAX_WIDTH = 1700f;
    [SerializeField] private float _minHeight = 200f;
    [SerializeField] private float _maxHeight = 700f;
    [SerializeField] private float _additionalHeight = 100f;

    public static DialogLocalizedText Instance { get; private set; } = null;

    private Coroutine _routine;
    private TextMeshProUGUI _thisText;
    private RectTransform _parent;
    private Dictionary<string, List<string>> _dialogText;


    public string Key { get; private set; } = "Test";
    public void StartDialog(string key)
    {
        Key = key;
        if (!DialogSystem.Instance.ReadJSON(TipPath)) return;
        SwitchText();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchText();
    }

    
    private string TipPath
    {
        get => _tipPath;
        set => _tipPath = "Tip/" + value;
    }
    private float SetParentHeight
    {
        get => _currentHeight;
        set
        {
            _currentHeight = value;
            if (_currentHeight < _minHeight) _currentHeight = _minHeight;
            else if (_currentHeight > _maxHeight) _currentHeight = _maxHeight;
            _parent.sizeDelta = new Vector2(_parent.sizeDelta.x, _currentHeight);
        }
    }


    private void Awake()
    {
        if (!SetInstance()) return;
        _thisText = this.GetComponent<TextMeshProUGUI>();
        _parent = GameObject.FindGameObjectWithTag("Dialog").GetComponentInParent<RectTransform>();
        //RectTransform _thisRectTransform = GetComponent<RectTransform>();
        //_thisRectTransform.sizeDelta = new Vector2(MAX_WIDTH, _thisRectTransform.sizeDelta.y);
        _dialogText = new();
        
    }
    private void Start()
    {
        SetParentHeight = _minHeight;
    }
    private void SwitchText()
    {
        if (_isUpdating)
        {
            StopCoroutine(_routine);
            _thisText.text = _currentText;
            SetPreferredHeight();
            _isUpdating = false;
            return;
        }

        SetDialogText();
        
        List<string> list = new();
        if (!GetLanguageTextAndCheck(ref list)) return;
        if (IsListEmpty(list)) return;

        _currentListSize = list.Count;
        if (_currentListIndex < _currentListSize)
        {
            _thisText.text = string.Empty;  
            _currentText = list[_currentListIndex].Trim();
            _routine = StartCoroutine(PrintText());
            NextIndexText();
            _isUpdating = true;
        }
    }
    private IEnumerator PrintText()
    {
        foreach (char c in _currentText)
        {
            _thisText.text += c;
            SetPreferredHeight();
            yield return new WaitForSecondsRealtime(.01f);
        }
        _isUpdating = false;
    }


    private void SetPreferredHeight()
    {
        float addHeight = _thisText.preferredHeight < _minHeight - _additionalHeight ? 0 : _additionalHeight;
        SetParentHeight = _thisText.preferredHeight + addHeight;
    }
    private void NextIndexText()
    {
        _currentListIndex = ++_currentListIndex % _currentListSize;
    }


    private void SetDialogText()
    {
        _dialogText = new(DialogSystem.Instance.GetDialog(Key));
        if (_dialogText.Count <= 0)
        {
            SimulationUtilities.DisplayError($"Dictionary is empty - {Key}");
            _dialogText = new(DialogSystem.Instance.GetDialog("null"));
        }
    }
    private bool GetLanguageTextAndCheck(ref List<string> list)
    {
        if (!_dialogText.TryGetValue(SimulationUtilities.CurrentLanguage, out list))
        {
            SimulationUtilities.DisplayWarning($"Language does not exist");
            return false;
        }
        return true;
    }
    private bool IsListEmpty(List<string> list)
    {
        if (list.Count <= 0)
        {
            SimulationUtilities.DisplayError($"Language list is empty");
            return true;
        }
        return false;
    }
    private bool SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }

    private float _currentHeight;
    private string _tipPath = "Tip/tip";
    private int _currentListSize;
    private int _currentListIndex = 0;
    private string _currentText = string.Empty;
    private bool _isUpdating = false;
}
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UserInterface;
using Dialogues.System;

// TODO - ускорить корутину
// TODO - максимальная ширина

namespace Dialogues.Dialogue
{
    public class Dialog : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private float _maxWidth = 1700f;
        [SerializeField] private float _minHeight = 200f;
        [SerializeField] private float _maxHeight = 700f;
        [SerializeField] private float _additionalHeight = 100f;
        [SerializeField] private TextMeshProUGUI _childText;

        private DialogSettings _dialogSettings;
        private Coroutine _routine;
        private RectTransform _thisRectTransform;
        private Dictionary<string, List<string>> _dialogText;


        
        public void StartDialog(string key, string path, DialogSettings settings, float width = 0)
        {
            if (width == 0) width = _maxWidth;
            //_childText.gameObject.GetComponent<RectTransform>()
            //    .sizeDelta = new Vector2(width, _this.sizeDelta.y);

            _key = key;
            _dialogSettings = settings;
            DialogSystem.Instance.ReadDialogJSON(path);

            StopAll();
            SwitchText();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isUpdating)
            {
                FillAll();
                return;
            }
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
                _isUpdating = true;
            }
        }
        private void StopAll()
        {
            StopAllCoroutines();
            _listSize = 0;
            _currentIndex = 0;
            _isUpdating = false;
            _text = string.Empty;
            SetPreferredHeight();
            _childText.text = _text;
            _isUpdating = false;
        }
        private void FillAll()
        {
            StopCoroutine(_routine);
            _childText.text = _text;
            SetPreferredHeight();
            _isUpdating = false;
        }
        private bool SetDialogDictionary()
        {
            var dialog = DialogSystem.Instance.GetDialog(_key);
            if (dialog == null)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"Dictionary is empty - {_key}");
                dialog = new(DialogSystem.Instance.GetDialog(_dialogSettings.DefaultKey));
                if (dialog == null) return false;
            }
            _dialogText = new(dialog);
            return true;
        }
        private bool SetDialogList(ref List<string> list)
        {
            if (!_dialogText.TryGetValue(UISystem.CurrentLanguage, out list))
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"Language does not exist");
                return false;
            }
            if (list.Count <= 0)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"Language list is empty");
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
            _isUpdating = false;
        }

        private string _key;
        private int _listSize;
        private int _currentIndex ;
        private bool _isUpdating;
        private string _text;
    }
}
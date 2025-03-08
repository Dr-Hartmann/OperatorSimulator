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
        [SerializeField] private TextMeshProUGUI _childText;

        private Coroutine _currentRoutine;
        private RectTransform _thisRectTransform;
        private Dictionary<string, List<string>> _dialogText;

        #region PUBLIC
        public void StartDialog(string key, float minHeight, float maxHeight, float additionalHeight)
        {
            _key = key;
            _minHeight = minHeight;
            _maxHeight = maxHeight;
            _additionalHeight = additionalHeight;

            StopAll();
            this.gameObject.SetActive(true);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isUpdating)
            {
                FillText();
                return;
            }
            if (_currentIndex >= _listSize)
            {
                this.gameObject.SetActive(false);
                return;
            }
            SwitchText();
        }
        #endregion

        #region CORE
        private void Awake()
        {
            _thisRectTransform = this.GetComponent<RectTransform>();
            _dialogText = new();

            // обводка
            _childText.outlineWidth = .5f;
            _childText.outlineColor = new Color32(0, 0, 0, 255);
        }
        private void OnEnable()
        {
            StopAll();
            SwitchText();
        }
        private void OnDisable()
        {
            StopAll();
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        #endregion

        #region handlers
        private void SwitchText()
        {
            List<string> list = new();
            if (SetDialogListAndIsEmpty(ref list)) return;

            _listSize = list.Count;
            _childText.text = string.Empty;
            _text = list[_currentIndex].Trim();
            _currentRoutine = StartCoroutine(PrintText());
            _currentIndex = ++_currentIndex;
            _isUpdating = true;
        }
        private void StopAll()
        {
            StopAllCoroutines();
            _isUpdating = false;
            _currentIndex = 0;
            _childText.text = string.Empty;
            //SetPreferredHeight();
        }
        private void FillText()
        {
            StopCoroutine(_currentRoutine);
            _isUpdating = false;
            _childText.text = _text;
            SetPreferredHeight();
        }
        private bool SetDialogListAndIsEmpty(ref List<string> list)
        {
            _dialogText = new(DialogSystem.GetDialog(_key));

            if (!_dialogText.TryGetValue(DialogSystem.CurrentLanguage, out list))
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning($"Language does not exist");
                return true;
            }

            if (list.Count > 0) return false;

            GameUtilities.Debug.GameUtilities.DisplayWarning($"Language list is empty");
            return true;
        }
        private IEnumerator PrintText()
        {
            foreach (char c in _text)
            {
                _childText.text += c;
                SetPreferredHeight();
                yield return null /*new WaitForSecondsRealtime(.01f)*/;
            }
            _isUpdating = false;
        }
        private void SetHeight(float value)
        {
            if (value < _minHeight) value = _minHeight;
            else if (value > _maxHeight) value = _maxHeight;
            _thisRectTransform.sizeDelta = new Vector2(_thisRectTransform.sizeDelta.x, value);
        }
        private void SetPreferredHeight()
        {
            float addHeight = _childText.preferredHeight < _minHeight - _additionalHeight ? 0 : _additionalHeight;
            SetHeight(_childText.preferredHeight + addHeight);
        }
        #endregion

        #region variables
        private string _key;
        private float _minHeight;
        private float _maxHeight;
        private float _additionalHeight;
        private int _listSize;
        private int _currentIndex;
        private bool _isUpdating;
        private string _text;
        #endregion
    }
}
using UnityEngine;
using System;

namespace Tips
{
    public class TipSystem : MonoBehaviour
    {
        [SerializeField] private TipSettings _tipSettings;
        [SerializeField] private RectTransform _tipsPlace;

        private Tip _tipsPrefab;
        private int _tipsMaxNumber;
        //[SerializeField] private Animator _animator;

        private Action<string, float> TipDisplayed;
        private Action TipDeleted;

        private void Awake()
        {
            _tipsPrefab = _tipSettings.TipPrefab;
            _tipsMaxNumber = _tipSettings.TipsMaxNumber;
        }
        private void OnEnable()
        {
            TipDisplayed += CreateTip;
            TipDeleted += DeleteTip;
        }
        private void OnDisable()
        {
            TipDisplayed -= CreateTip;
            TipDeleted -= DeleteTip;
        }
        public static void InvokeTipDeleted()
        {
            if (_instance) _instance?.TipDeleted.Invoke();
        }
        public static void InvokeTipDisplayed(string message, float lifetime)
        {
            Instance?.TipDisplayed.Invoke(message, lifetime);
        }
        private void CreateTip(string message, float lifetime)
        {
            if (_tipsCounter <= _tipsMaxNumber)
            {
                GameObject obj = Instantiate(_tipsPrefab.gameObject, _tipsPlace);
                Tip tip = obj.GetComponent<Tip>();
                tip.Index = ++_tipsCounter;
                tip.Lifetime = lifetime;
                tip.Message = message;
            }
        }
        private void DeleteTip()
        {
            --_tipsCounter;
        }

        private static TipSystem Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindAnyObjectByType<TipSystem>();
                    if (!_instance) return null;
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private static TipSystem _instance;
        private static int _tipsCounter;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoadMethod()
        {
            _instance = null;
            _tipsCounter = 0;
        }
    }
}
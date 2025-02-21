using System;
using UnityEngine;

public sealed class TipSystem : MonoBehaviour
{
    [SerializeField] private GameObject _prefabTipUI;
    [SerializeField] private RectTransform _content;
    [SerializeField] private int _maxNumberTips = 20;
    //[SerializeField] private Animator _animator;

    public static TipSystem instance { get; private set; }
    public Action<string, float> MessageDisplayed;
    public Action MessageDeleted;


    private void Awake()
    {
        if (!SetInstance()) return;
    }

    private void Start()
    {
        MessageDisplayed += TipCreate;
        MessageDeleted += TipDelete;
    }

    private void OnDestroy()
    {
        MessageDisplayed -= TipCreate;
        MessageDeleted -= TipDelete;
    }

    private void TipCreate(string message, float lifetime)
    {
        if (_currentCount <= _maxNumberTips)
        {
            GameObject obj = Instantiate(_prefabTipUI, _content);
            TipUI tip = obj.GetComponent<TipUI>();
            tip.Index = ++_currentCount;
            tip.Lifetime = lifetime;
            tip.Message = message;
        }
    }

    private void TipDelete()
    {
        --_currentCount;
    }

    private bool SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }

    private static int _currentCount = 0;
}

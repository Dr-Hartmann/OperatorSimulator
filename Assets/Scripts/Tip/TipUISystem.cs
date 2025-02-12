using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TipUISystem : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private float _lifetime = 10f;
    [SerializeField] private int _maxNumberTips = 10;
    //[SerializeField] private Animator _animator;

    public static TipUISystem instance { get; private set; }
    public int CurrentCount { get; private set; } = 0;
    public Action<string> MessageDisplayed;
    public Action MessageDeleted;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        MessageDisplayed += UITipCreate;
        MessageDeleted += UITipDelete;
    }

    private void OnDestroy()
    {
        MessageDisplayed -= UITipCreate;
        MessageDeleted -= UITipDelete;
    }

    private void UITipCreate(string message)
    {
        if (CurrentCount <= _maxNumberTips)
        {
            GameObject obj = Instantiate(_prefab, _parent);
            TipUI tip = obj.GetComponent<TipUI>();
            tip.Index = ++CurrentCount;
            tip.Lifetime = _lifetime;
            tip.Message = message;
        }
    }

    private void UITipDelete()
    {
        --CurrentCount;
    }
}

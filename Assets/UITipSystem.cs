using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UITipSystem : MonoBehaviour
{
    [SerializeField] private GameObject _thisPrefab;

    //[SerializeField] private float _lifetime = 2f;
    [SerializeField] private int MAX_NUMBER_TIPS = 10;
    //[SerializeField] private Animator _animator;

    public int Index { get; private set; } = 0;

    public static UITipSystem instance { get; private set; }
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
        MessageDisplayed += Create;
    }

    private void OnDestroy()
    {
        MessageDisplayed -= Create;
    }

    private void Create(string message)
    {
        if (++Index <= MAX_NUMBER_TIPS)
            Instantiate(_thisPrefab, Random.insideUnitSphere * Random.value, new Quaternion(0, 0, 0, 0)).transform.SetParent(this.transform, true);
    }
}

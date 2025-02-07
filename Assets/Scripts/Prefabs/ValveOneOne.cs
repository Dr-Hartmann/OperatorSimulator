using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO - сделать курву
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
internal class ValveOneOne : MonoBehaviour
{
    [Header("Valve sprites")]
    [SerializeField] private Sprite _spriteNeutral;
    [SerializeField] private Sprite _spriteClose;
    [SerializeField] private Sprite _spriteAverage;
    [SerializeField] private Sprite _spriteOpen;

    [Header("Valve state")]
    [SerializeField] public ValveStates _currentValveState = ValveStates.Close;

    [Header("Valve sensors")]
    [SerializeField] private TextMeshProUGUI _openingSensorText;
    [Range(0f, 100f)]
    [SerializeField] private float _openingPercentage = MIN_VALUE_OPENNES;

    [Header("Valve settings")]
    [SerializeField] private float _openingSpeed = 1f;
    [SerializeField] private float _closingSpeed = .8f;
    [SerializeField] private float _multiplierSoonToEndState = 5f;

    private const float MAX_VALUE_OPENNES = 100f;
    private const float NEAR_OPEN = 95f;
    private const float MIN_VALUE_OPENNES = 0f;
    private const float NEAR_CLOSE = 5f;

    private Image _thisImage;
    private Button _thisButton;

    public float OpenPercent
    {
        get => _openingPercentage;
        private set => _openingPercentage = value;
    }

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton = GetComponent<Button>();
        _thisImage = GetComponent<Image>();
        SimulationSystem.instance.TickPassed += UpdateReadingsSensor;
        _thisButton.onClick.AddListener(OnClick);

        if(IsClose()) OpenPercent = 0f; 
        else if(IsOpen())OpenPercent = 1f;
    }

    private void OnDestroy()
    {
        SimulationSystem.instance.TickPassed -= UpdateReadingsSensor;
        _thisButton.onClick.RemoveAllListeners();
    }

    public void OnClick()
    {
        if (IsOpen() || IsOpening())
        {
            Closing();
        }
        else if (IsClose() || IsClosing())
        {
            Opening();
        }
        else
        {
            Debug.LogError($"ValveOneOne: The valve is in an unknown state");
            return;
        }
    }

    private void UpdateReadingsSensor(float tick)
    {
        _openingSensorText.text = OpenPercent.ToString();
        if (IsOpen() || IsClose()) return;
        if (IsOpening()) Add(tick);
        else if (IsClosing()) Sub(tick);
    }

    private float MultiplierTime(params float[] array)
    {
        float result = 1f;
        foreach (float item in array) result *= item;
        return result;
    }

    private void Add(float tick)
    {
        AddSub
        (
            () => OpenPercent += MultiplierTime(tick, _openingSpeed, _multiplierSoonToEndState),
            () => OpenPercent += MultiplierTime(tick, _openingSpeed)
        );
    }

    private void Sub(float tick)
    {
        AddSub
        (
            () => OpenPercent -= MultiplierTime(tick, _closingSpeed, _multiplierSoonToEndState),
            () => OpenPercent -= MultiplierTime(tick, _closingSpeed)
        );
    }

    private void AddSub(Action near, Action other)
    {
        if (OpenPercent >= NEAR_OPEN || OpenPercent <= NEAR_CLOSE) near();
        else other();

        if (OpenPercent >= MAX_VALUE_OPENNES)
        {
            OpenPercent = MAX_VALUE_OPENNES;
            Open();
        }
        else if (OpenPercent <= MIN_VALUE_OPENNES)
        {
            OpenPercent = MIN_VALUE_OPENNES;
            Close();
        }
    }

    private bool IsOpen() => _currentValveState == ValveStates.Open;
    private bool IsClose() => _currentValveState == ValveStates.Close;
    private bool IsOpening() => _currentValveState == ValveStates.Opening;
    private bool IsClosing() => _currentValveState == ValveStates.Closing;

    private void Open() => ChangeStateAndSprite(ValveStates.Open, _spriteOpen);
    private void Close() => ChangeStateAndSprite(ValveStates.Close, _spriteClose);
    private void Opening() => ChangeStateAndSprite(ValveStates.Opening, _spriteAverage);
    private void Closing() => ChangeStateAndSprite(ValveStates.Closing, _spriteAverage);

    private void ChangeStateAndSprite(ValveStates state, Sprite sprite)
    {
        _currentValveState = state;
        _thisImage.sprite = sprite;
    }
}
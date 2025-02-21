using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO - ������� ����� ��� ��������� ����������� ���������� �������

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public sealed class ValveOneOne : MonoBehaviour, IValve
{
    [Header("Valve sprites")]
    [SerializeField] private Sprite _spriteNeutral;
    [SerializeField] private Sprite _spriteClose;
    [SerializeField] private Sprite _spriteAverage;
    [SerializeField] private Sprite _spriteOpen;

    [Header("Valve sensors")]
    [SerializeField] private TextMeshProUGUI _openingSensorText;
    [Range(MIN_VALUE_OPENNES, MAX_VALUE_OPENNES)]
    [SerializeField] private float _openingPercentage = MIN_VALUE_OPENNES;

    [Header("Valve settings")]
    [SerializeField] public ValveStates _currentValveState = ValveStates.Close;
    [SerializeField] private float _openingSpeed = 1f;
    [SerializeField] private float _closingSpeed = .8f;
    [SerializeField] private float _multiplierSoonToEndState = 5f;

    private Image _thisImage;
    private Button _thisButton;


    public float OpenPercent
    {
        get => _openingPercentage;
        private set => _openingPercentage = value;
    }
    public bool IsOpen
    {
        get => _currentValveState == ValveStates.Open;
    }
    public bool IsClose
    {
        get => _currentValveState == ValveStates.Close;
    }
    public bool IsOpening
    {
        get => _currentValveState == ValveStates.Opening;
    }
    public bool IsClosing
    {
        get => _currentValveState == ValveStates.Closing;
    }


    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisImage = GetComponent<Image>();
        _thisButton.onClick.AddListener(OnClick);

        if (IsClose) Close();
        else if (IsOpen) Open();
    }
    private void Start()
    {
        SimulationSystem.Instance.TickPassed += UpdateReadingsSensor;
    }
    private void OnDestroy()
    {
        SimulationSystem.Instance.TickPassed -= UpdateReadingsSensor;
        _thisButton.onClick.RemoveAllListeners();
    }

    private void OnClick()
    {
        if (IsOpen || IsOpening) ChangeStateAndSprite(ValveStates.Closing, _spriteAverage);
        else if (IsClose || IsClosing) ChangeStateAndSprite(ValveStates.Opening, _spriteAverage);
        else
        {
            SimulationUtilities.DisplayError($"Valve is in an unknown state");
            return;
        }
    }
    private void UpdateReadingsSensor(float tick)
    {
        _openingSensorText.text = ((int)OpenPercent).ToString();
        if (IsOpen || IsClose) return;
        if (IsOpening) Add(tick);
        else if (IsClosing) Sub(tick);

        if (OpenPercent >= MAX_VALUE_OPENNES) Open();
        else if (OpenPercent <= MIN_VALUE_OPENNES) Close();
    }


    private void Add(float tick)
    {
        AddSub
        (
            () => OpenPercent += IValve.MultiplierTime(tick, _openingSpeed, _multiplierSoonToEndState),
            () => OpenPercent += IValve.MultiplierTime(tick, _openingSpeed)
        );
    }
    private void Sub(float tick)
    {
        AddSub
        (
            () => OpenPercent -= IValve.MultiplierTime(tick, _closingSpeed, _multiplierSoonToEndState),
            () => OpenPercent -= IValve.MultiplierTime(tick, _closingSpeed)
        );
    }
    private void AddSub(Action near, Action other)
    {
        if (OpenPercent >= NEAR_OPEN || OpenPercent <= NEAR_CLOSE) near();
        else other();
    }


    private void Open()
    {
        OpenPercent = MAX_VALUE_OPENNES;
        ChangeStateAndSprite(ValveStates.Open, _spriteOpen);
    }
    private void Close()
    {
        OpenPercent = MIN_VALUE_OPENNES;
        ChangeStateAndSprite(ValveStates.Close, _spriteClose);
    }
    private void ChangeStateAndSprite(ValveStates state, Sprite sprite)
    {
        _currentValveState = state;
        _thisImage.sprite = sprite;
    }


    private const float MAX_VALUE_OPENNES = 100f;
    private const float NEAR_OPEN = 95f;
    private const float MIN_VALUE_OPENNES = 0f;
    private const float NEAR_CLOSE = 5f;
}
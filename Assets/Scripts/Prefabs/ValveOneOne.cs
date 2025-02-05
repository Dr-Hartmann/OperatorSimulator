using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO - сделать 
internal class ValveOneOne : MonoBehaviour
{
    [Header("Valve sprites")]
    [SerializeField] private Sprite _valveNeutral;
    [SerializeField] private Sprite _valveClose;
    [SerializeField] private Sprite _valveOpening;
    [SerializeField] private Sprite _valveOpen;

    [Header("Valve state")]
    [SerializeField] public ValveStates _currentValveState = ValveStates.Close;

    [Header("Valve sensors")]
    [SerializeField] private TextMeshProUGUI _openingSensorText;
    [Range(0f, 100f)] private float _openingPercentage = 0f;

    [Header("Valve settings")]
    [SerializeField] private float _openingSpeed = 1000f;
    [SerializeField] private float _closingSpeed = 800f;
    [SerializeField] private float _multiplierSoonToEndState = 5f;

    private Button _thisButton;

    public float OpeningPercentage
    {
        get => _openingPercentage;
        private set => _openingPercentage = value;
    }

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        SimulationSystem.instance.TickPassed += UpdateReadingsSensor;
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }

    public void OnClick()
    {
        if (IsOpen() || IsOpening())
        {
            Close();
        }
        else if (IsClose() || IsClosing())
        {
            Open();
        }
        else
        {
            Debug.LogError($"ValveOneOne: The valve is in an unknown state");
            return;
        }
    }

    private void UpdateReadingsSensor(float tick)
    {
        if (IsOpen() || IsClose()) return;

        if (IsOpening())
        {
            if (OpeningPercentage >= 95 || OpeningPercentage <= 5)
            {
                OpeningPercentage += TimeMultiplier(tick, _openingSpeed, _multiplierSoonToEndState);
            }
            else
            {
                OpeningPercentage += TimeMultiplier(tick, _openingSpeed);
            }
        }
        if (IsClosing())
        {

        }

        if(OpeningPercentage >= 100)
    }

    private float TimeMultiplier(params float[] array)
    {
        float result = 1;
        foreach (float item in array) result *= item;
        return result;
    }

    private bool IsOpen() => _currentValveState == ValveStates.Open;
    private bool IsClose() => _currentValveState == ValveStates.Close;
    private bool IsOpening() => _currentValveState == ValveStates.Opening;
    private bool IsClosing() => _currentValveState == ValveStates.Closing;

    private void Open() => _currentValveState = ValveStates.Opening;
    private void Close() => _currentValveState = ValveStates.Closing;
}

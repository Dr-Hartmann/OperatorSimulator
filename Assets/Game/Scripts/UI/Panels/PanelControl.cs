using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _playPause;
    [SerializeField] private Button _less;
    [SerializeField] private Button _more;

    [Header("Sprites")]
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;

    [Header("Speed")]
    [SerializeField] private TextMeshProUGUI _speedValue;

    private Image _playPauseImage;
    private bool _isSubscribed = false;


    private void Awake()
    {
        _playPauseImage = _playPause.GetComponent<Image>();
    }
    private void Update()
    {
        _speedValue.SetText(SimulationSystem.CurrentSpeed.ToString());
    }   
    private void Start()
    {
        SubscribeAll();
        _isSubscribed = true;
    }
    private void OnEnable()
    {
        if(!_isSubscribed) SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
        _isSubscribed = false;
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    private void ChangeSprite(bool _isPlayed)
    {
        if (_isPlayed) _playPauseImage.sprite = _pauseSprite;
        else _playPauseImage.sprite = _playSprite;
    }
    private void OnClickPlayPause()
    {
        SimulationSystem.Instance?.ReversePlayPause();
    }
    private void OnClickLess()
    {
        SimulationSystem.Instance?.SetSpeed(SimulationSpeedStates.Decrease);
    }
    private void OnClickMore()
    {
        SimulationSystem.Instance?.SetSpeed(SimulationSpeedStates.Increase);
    }
    private void SubscribeAll()
    {
        SimulationSystem.Played += ChangeSprite;
        _less.onClick.AddListener(OnClickLess);
        _more.onClick.AddListener(OnClickMore);
        _playPause.onClick.AddListener(OnClickPlayPause);
    }
    private void UnsubscribeAll()
    {
        SimulationSystem.Played -= ChangeSprite;
        _less.onClick.RemoveAllListeners();
        _more.onClick.RemoveAllListeners();
        _playPause.onClick.RemoveAllListeners();
    }
}

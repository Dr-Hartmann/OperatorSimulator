using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Simulation;

[RequireComponent(typeof(Image))]
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


    private void FixedUpdate()
    {
        _speedValue.SetText(SimulationSystem.CurrentSpeed.ToString());
    }
    private void ChangeSprite(bool _isPlayed)
    {
        if (_isPlayed) _playPauseImage.sprite = _pauseSprite;
        else _playPauseImage.sprite = _playSprite;
    }
    private void OnClickPlayPause()
    {
        SimulationSystem.ReversePlayPause();
    }
    private void OnClickLess()
    {
        SimulationSystem.SetSpeed(SimulationSpeedStates.Decrease);
    }
    private void OnClickMore()
    {
        SimulationSystem.SetSpeed(SimulationSpeedStates.Increase);
    }
    private void Awake()
    {
        _playPauseImage = _playPause.GetComponent<Image>();
    }
    private void OnEnable()
    {
        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    private void SubscribeAll()
    {
        UnsubscribeAll();
        SimulationSystem.SubEventEventPlayed(ChangeSprite);
        _less.onClick.AddListener(OnClickLess);
        _more.onClick.AddListener(OnClickMore);
        _playPause.onClick.AddListener(OnClickPlayPause);
    }
    private void UnsubscribeAll()
    {
        SimulationSystem.UnsubEventPlayed(ChangeSprite);
        _less.onClick.RemoveAllListeners();
        _more.onClick.RemoveAllListeners();
        _playPause.onClick.RemoveAllListeners();
    }
}
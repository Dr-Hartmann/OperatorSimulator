using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
internal class ControlButtonPlayPause : MonoBehaviour
{
    [SerializeField] private Image _stopPanel;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private bool _isPlaying = false;

    private Image _thisImage;
    private Button _thisButton;

    private void Awake()
    {
        _thisImage = GetComponent<Image>();
        _thisButton = GetComponent<Button>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(OnClick);
        if (_isPlaying) StartAndChangeSprite();
        else PauseAndChangeSprite();
    }

    public void OnClick()
    {
        if (_isPlaying) PauseAndChangeSprite();
        else StartAndChangeSprite();
        _stopPanel.gameObject.SetActive(_isPlaying);
        _isPlaying = !_isPlaying;
    }

    private void PauseAndChangeSprite()
    {
        _thisImage.sprite = _playSprite;
        SimulationSystem.instance.SimulationPause();
    }

    private void StartAndChangeSprite()
    {
        _thisImage.sprite = _pauseSprite;
        SimulationSystem.instance.SimulationStart();
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
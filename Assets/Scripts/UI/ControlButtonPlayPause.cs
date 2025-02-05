using UnityEngine;
using UnityEngine.UI;

internal class ControlButtonPlayPause : MonoBehaviour
{
    [SerializeField] private Image _controlPanel;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private bool _isPlaying = true;

    private Image _thisImage;
    private Button _thisButton;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisImage = GetComponent<Image>();
        _thisButton = GetComponent<Button>();
        _thisImage.sprite = _pauseSprite;
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (_isPlaying)
        {
            _thisImage.sprite = _playSprite;
            SimulationSystem.instance.SimulationPause();
        }
        else
        {
            _thisImage.sprite = _pauseSprite;
            SimulationSystem.instance.SimulationStart();
        }
        _controlPanel.gameObject.SetActive(_isPlaying);
        _isPlaying = !_isPlaying;
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
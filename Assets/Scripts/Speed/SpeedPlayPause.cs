using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class SpeedPlayPause : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;

    private Image _thisImage;
    private Button _thisButton;
    

    private void Awake()
    {
        _thisImage = GetComponent<Image>();
        _thisButton = GetComponent<Button>();  
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(OnClick);
        SimulationSystem.Instance.Played += SetSprite;
        SetSprite(SimulationSystem.Instance.IsPlayed);
    }

    private void OnClick()
    {
        SimulationSystem.Instance.ReversePlayPause();
    }

    private void SetSprite(bool _isPlayed)
    {
        if (_isPlayed) _thisImage.sprite = _pauseSprite;
        else _thisImage.sprite = _playSprite;
    }

    private void OnDestroy()
    {
        SimulationSystem.Instance.Played -= SetSprite;
        _thisButton.onClick.RemoveAllListeners();
    }
}
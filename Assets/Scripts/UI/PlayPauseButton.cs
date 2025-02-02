using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _pauseButton;
    [SerializeField] private UnityEngine.UI.Image _stopPanel;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private SimulationSystem _simulationSystem;

    private bool _isPlaying;

    public void Awake()
    {
        this.gameObject.SetActive(true);
        _stopPanel.gameObject.SetActive(false);// TODO - убрать отсюда
    }

    private void Start()
    {
        this.GetComponent<UnityEngine.UI.Image>().sprite = _pauseSprite;
        _pauseButton.onClick.AddListener(OnClick);
        _isPlaying = true;
    }

    public void OnClick()
    {
        if (_isPlaying)
        {
            this.GetComponent<UnityEngine.UI.Image>().sprite = _playSprite;
            _simulationSystem.SimulationPause();
        }
        else
        {
            this.GetComponent<UnityEngine.UI.Image>().sprite = _pauseSprite;
            _simulationSystem.SimulationStart();
        }
        _stopPanel.gameObject.SetActive(_isPlaying);
        _isPlaying = !_isPlaying;
    }
}
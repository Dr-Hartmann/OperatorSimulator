using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _pauseButton;
    [SerializeField] private UnityEngine.UI.Image _stop;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Simulation _simulationCore;

    private bool _isPlaying;

    public void Awake()
    {
        _isPlaying = true;
        _stop.gameObject.SetActive(false);
        GetComponent<UnityEngine.UI.Image>().sprite = _pauseSprite;
        _pauseButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (_isPlaying)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = _playSprite;
            _simulationCore.SimulationStop();
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().sprite = _pauseSprite;
            _simulationCore.SimulationStart();
        }
        _stop.gameObject.SetActive(_isPlaying);
        _isPlaying = !_isPlaying; 
    }
}
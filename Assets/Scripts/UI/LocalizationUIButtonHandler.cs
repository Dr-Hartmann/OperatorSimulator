using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LocalizationUIButtonHandler : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        _thisButton = this.gameObject.GetComponent<Button>();
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        LocalizationUISystem.instance.UpdateUIText(LocalizationModes.SWITCH_NEXT);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
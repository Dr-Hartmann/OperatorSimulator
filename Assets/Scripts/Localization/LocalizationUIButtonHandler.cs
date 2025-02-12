using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class LocalizationUIButtonHandler : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        _thisButton = this.gameObject.GetComponent<Button>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if(LocalizationUISystem.instance.IsInstanceNull()) return;
        LocalizationUISystem.instance.LoadUIText(LocalizationModes.SWITCH_NEXT);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
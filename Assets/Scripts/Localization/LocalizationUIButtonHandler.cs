using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class LocalizationUIButtonHandler : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton = this.gameObject.GetComponent<Button>();
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (LocalizationUISystem.instance == null)
        {
            Debug.LogError("LocalizationUIButtonHandler: LocalizationUISystem instance not found!");
            return;
        }
        LocalizationUISystem.instance.LoadUIText(LocalizationModes.SWITCH_NEXT);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
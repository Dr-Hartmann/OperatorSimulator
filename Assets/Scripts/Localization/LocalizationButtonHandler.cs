using UnityEngine;
using UnityEngine.UI;

internal class LocalizationButtonHandler : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(OnClick);
        this.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if (LocalizationSystem.instance == null)
        {
            Debug.LogError("LocalizationButtonHandler: LocalizationSystem instance not found!");
            return;
        }
        LocalizationSystem.instance.LoadLocalizedText(LocalizationModes.SWITCH_NEXT);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
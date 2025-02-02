using UnityEngine;
using UnityEngine.UI;

public class LocalizationButtonHandler : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        this.gameObject.SetActive(true);
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (LocalizationSystem.instance == null)
        {
            Debug.LogError("LocalizationSystem instance not found!");
            return;
        }
        LocalizationSystem.instance.LoadLocalizedText(LocalizationModes.SWITCH_NEXT);
    }
}
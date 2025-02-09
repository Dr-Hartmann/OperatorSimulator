using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
internal class UILocalizationButtonHandler : MonoBehaviour
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
        if(UILocalizationSystem.instance.IsInstanceNull()) return;
        UILocalizationSystem.instance.LoadUIText(UILocalizationModes.SWITCH_NEXT);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
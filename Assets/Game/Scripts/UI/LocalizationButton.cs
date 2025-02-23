using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LocalizationButton : MonoBehaviour
{
    private Button _thisButton;

    public void OnClick()
    {
        GUISystem.Instance?.SetUI(LocalizationModes.SWITCH_NEXT);
    }

    private void Awake()
    {
        _thisButton = this.gameObject.GetComponent<Button>();
    }
    private void OnEnable()
    {
        _thisButton.onClick.AddListener(OnClick);
    }
    private void OnDisable()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
    private void OnDestroy()
    {
        OnDisable();
    }
}
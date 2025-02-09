using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class TipButton : MonoBehaviour
{
    private Button _thisButton;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        _thisButton.onClick.AddListener(this.OnClick);
    }

    private void OnClick()
    {
        UITipSystem.instance.MessageDisplayed?.Invoke("Сообе");
    }
}

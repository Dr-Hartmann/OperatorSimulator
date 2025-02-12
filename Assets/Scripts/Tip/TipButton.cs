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
        TipUI.CreateTip("Какое-то сообщение ла ла ла" + Time.time.ToString());
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
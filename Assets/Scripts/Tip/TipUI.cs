using TMPro;
using UnityEngine;

public class TipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private RectTransform _thisRectTransform;
    private float CreationTime { get; set; }

    public float Lifetime { get; set; } = 20f;
    public int Index { get; set; }
    public string Message { get; set; } = "-";

    private void Awake()
    {
        _thisRectTransform = GetComponent<RectTransform>();
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        CreationTime = Time.time;
        _text.text = Message;
        AdjustWidth();
    }

    private void Update()
    {
        if (Time.time - CreationTime > Lifetime) Destroy(this.gameObject);
    }

    public static void CreateTip(string message)
    {
        TipUISystem.instance.MessageDisplayed?.Invoke(message);
    }

    private void OnDestroy()
    {
        TipUISystem.instance.MessageDeleted?.Invoke();
    }

    private void AdjustWidth()
    {
        _thisRectTransform.sizeDelta = new Vector2(_thisRectTransform.sizeDelta.x, _text.preferredHeight);
    }

}

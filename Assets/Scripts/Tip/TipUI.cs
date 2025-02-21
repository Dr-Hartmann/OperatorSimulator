using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TipUI : MonoBehaviour
{    
    public float Lifetime { get; set; } = 20f;
    public int Index { get; set; } = 0;
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            AdjustWidth();
        }
    }

    private float _creationTime;
    private string _message = "__message__";
    private RectTransform _thisRectTransform;
    private TextMeshProUGUI _text;
    

    private void Awake()
    {
        _thisRectTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _creationTime = Time.time;
    }

    private void Start()
    {
        _text.text = Message;
    }

    private void Update()
    {
        if (Time.time - _creationTime > Lifetime)
        {
            Destroy(this.gameObject);
        }
    }

    private static void OnDestroy()
    {
        TipSystem.instance.MessageDeleted?.Invoke();
    }

    private void AdjustWidth()
    {
        _thisRectTransform.sizeDelta
            = new Vector2(_thisRectTransform.sizeDelta.x, _text.preferredHeight);
    }
}
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
internal class DialogUI : MonoBehaviour
{
    [SerializeField] private float MIN_HEIGHT = 200f;
    [SerializeField] private float MAX_HEIGHT = 700f;
    [SerializeField] private float _additionalHeight = 100f;

    private RectTransform _thisRectTransform;
    private float _currentHeight;

    public float Height
    {
        get => _currentHeight;
        set
        {
            _currentHeight = value;
            if (_currentHeight < MIN_HEIGHT) _currentHeight = MIN_HEIGHT;
            else if (_currentHeight > MAX_HEIGHT) _currentHeight = MAX_HEIGHT;
            _thisRectTransform.sizeDelta = new Vector2(_thisRectTransform.sizeDelta.x, Height);
        }
    }

    private void Awake()
    {
        _thisRectTransform = this.GetComponent<RectTransform>();
        Height = MIN_HEIGHT;
    }

    public float AdditionalHeight(float height)
    {
        return height < MIN_HEIGHT - _additionalHeight ? 0 : _additionalHeight;
    }
}
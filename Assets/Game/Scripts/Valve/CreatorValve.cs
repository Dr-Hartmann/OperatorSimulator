using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CreatorValve : MonoBehaviour
{
    [SerializeField] private GameObject _valve;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private float _radius = 200f;

    private Button _thisButton;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
    }
    private void OnClick()
    {
        float parentX;
        float parentY;
        float x;
        float y;
        Vector3 pos;

        parentX = _parent.transform.position.x;
        parentY = _parent.transform.position.y;
        x = Random.Range(parentX - _radius, parentX + _radius);
        y = Random.Range(parentY - _radius, parentY + _radius);
        pos = new Vector3(x, y, 1f);
        GameObject obj = Instantiate(_valve, pos, Quaternion.identity, _parent);

        GUISystem.MessageDisplayed?.Invoke($"Создан клапан {Time.time.ToString()}", 30f);
    }
    private void OnEnable()
    {
        _thisButton.onClick.AddListener(this.OnClick);
    }
    private void OnDisable()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
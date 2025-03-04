using UnityEngine;
using UnityEngine.UI;
using Tips;

[RequireComponent(typeof(Button))]
public class CreatorValve : MonoBehaviour
{
    [SerializeField] private ValveOneOneSettings _valveSettings;
    [SerializeField] private RectTransform _placeForValves;

    private Button _thisButton;
    private ValveOneOne _valve;
    private float _radius;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _radius = _valveSettings.ValvesCreateRadius;
        _valve = _valveSettings.PrefabValve;
    }
    private void OnClick()
    {
        float parentX;
        float parentY;
        float x;
        float y;
        Vector3 pos;

        parentX = _placeForValves.position.x;
        parentY = _placeForValves.position.y;
        x = Random.Range(parentX - _radius, parentX + _radius);
        y = Random.Range(parentY - _radius, parentY + _radius);
        pos = new Vector3(x, y, 1f);
        GameObject obj = Instantiate(_valve.gameObject, pos, Quaternion.identity, _placeForValves);

        TipSystem.InvokeTipDisplayed($"Создан клапан {Time.time.ToString()}", 30f);
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
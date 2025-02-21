using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private float _radius = 200f;

    [SerializeField] private RectTransform _parentDialog;
    [SerializeField] private GameObject _prefabDialog;

    private bool _isDialog  = false;

    private Button _thisButton;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(this.OnClick);
    }

    private void OnClick()
    {
        CreateValve();
        if(!_isDialog)CreateDialog();
    }

    private void CreateDialog()
    {
        GameObject obj = Instantiate(_prefabDialog, _parentDialog);  
        DialogLocalizedText txt = obj.GetComponent<DialogLocalizedText>();
        txt.StartDialog("Test");
        _isDialog = true;
    }

    private void CreateValve()
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
        GameObject obj = Instantiate(_prefab, pos, Quaternion.identity, _parent);
        /*GameObject obj = */
        //ValveOneOne tip = obj.GetComponent<ValveOneOne>();

        TipSystem.instance.MessageDisplayed?.Invoke($"Создан клапан {Time.time.ToString()}", 30f);
    }

    private void OnDestroy()
    {
        _thisButton.onClick.RemoveAllListeners();
    }
}
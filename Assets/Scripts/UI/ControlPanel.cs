using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ControlPanel : MonoBehaviour
{
    private Image _thisImage;

    private void Awake()
    {
        _thisImage = GetComponent<Image>();
    }
}
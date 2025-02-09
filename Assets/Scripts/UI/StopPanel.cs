using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StopPanel : MonoBehaviour
{
    private Image _thisImage;

    private void Awake()
    {
        _thisImage = GetComponent<Image>();
        this.gameObject.SetActive(false);
    }
}
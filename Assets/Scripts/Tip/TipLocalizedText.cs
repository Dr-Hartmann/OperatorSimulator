using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TipLocalizedText : MonoBehaviour
{

    private void Awake()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "---";
    }
}

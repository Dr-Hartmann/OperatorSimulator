using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

internal class DialogSystem : MonoBehaviour
{
    [SerializeField] private string _tipPath;

    public static DialogSystem instance { get; private set; }

    private Dictionary<string, DialogText> _text;

    private void Awake()
    {
        if (!SetInstance()) return;
        if (!ReadJSON(_tipPath)) return;
    }

    private void Start()
    {
        if (IsInstanceNull()) return;
    }

    private bool ReadJSON(string path)
    {
        //string json = JsonConvert.SerializeObject(dialogLoaclizedJSON, Formatting.Indented);
        TextAsset json = Resources.Load(path) as TextAsset;
        if (json.text == "")
        {
            SimulationUtilities.DisplayError("Path is empty");
            return false;
        }
        _text = JsonConvert.DeserializeObject<Dictionary<string, DialogText>>(json.text);

        return true;
    }

    public DialogText GetText(string key)
    {
        DialogText text = null;
        _text.TryGetValue(key, out text);
        return text;
    }

    private bool SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }

    public bool IsInstanceNull()
    {
        if (instance == null)
        {
            SimulationUtilities.DisplayError("Instance is null");
            return true;
        }
        return false;
    }
}

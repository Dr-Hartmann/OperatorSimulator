using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Организует чтение .json-файла в класс <see cref="DialogJSON"/> и получение нужного набора по ключу.
/// </summary>
public class DialogSystem : MonoBehaviour, IDialogSystem
{
    public static IDialogSystem Instance { get; private set; }
    private DialogJSON _dialog;


    public bool ReadJSON(string path)
    {
        TextAsset json = Resources.Load(path) as TextAsset;
        if (json.text == "")
        {
            SimulationUtilities.DisplayError("Path is empty");
            return false;
        }
        _dialog = JsonConvert.DeserializeObject<DialogJSON>(json.text);
        return true;
    }
    public Dictionary<string, List<string>> GetDialog(string key)
    {
        return _dialog.text[key];
    }

    private void Awake()
    {
        if (!SetInstance()) return;
        _dialog = new();
    }

    public bool IsDialogEmpty()
    {
        if (_dialog.text.Count <= 0)
        {
            SimulationUtilities.DisplayError("Dialog is empty");
            return true;
        }
        return false;
    }
    private bool SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
        Destroy(this.gameObject);
        return false;
    }
}

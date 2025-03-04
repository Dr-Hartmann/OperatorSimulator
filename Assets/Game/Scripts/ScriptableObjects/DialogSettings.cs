using Dialogues.Dialogue;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogSettings", menuName = "Scriptable Objects/new DialogSettings")]
public class DialogSettings : ScriptableObject
{
    [SerializeField] private Dialog _dialogFull;
    [SerializeField] private string _defaultPath;
    [SerializeField] private string _defaultKey;

    public Dialog PrefabDialogFull
    {
        get => _dialogFull;
    }
    public string DefaultPath
    {
        get => _defaultPath;

    }
    public string DefaultKey
    {
        get => _defaultKey;
    }
}

[CreateAssetMenu(fileName = "TriggerDialog", menuName = "Dialogues/new TriggerDialog")]
public class TriggerDialog : DialogSettings
{
    [SerializeField] private string _dialogPathEnter;
    [SerializeField] private string _dialogKeyEnter;

    [SerializeField] private string _dialogPathExit;
    [SerializeField] private string _dialogKeyExit;

    public string DialogPathEnter
    {
        get => _dialogPathEnter;

    }
    public string DialogKeyEnter
    {
        get => _dialogKeyEnter;
    }
    public string DialogPathExit
    {
        get => _dialogPathExit;

    }
    public string DialogKeyExit
    {
        get => _dialogKeyExit;
    }
}
using System;

[Serializable]
public class DialogLoaclizedJSON
{
    public DialogText TipWarning;
    public DialogText TipError;
    public DialogText TipAction;
}

[Serializable]
public class DialogText
{
    public string[] ru, en, zh, uz, de, jp, it, es;
}
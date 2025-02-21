using System;
using System.Collections.Generic;

[Serializable]
public class DialogJSON
{
    public Dictionary<string, Dictionary<string, List<string>>> text { get; set; }
}
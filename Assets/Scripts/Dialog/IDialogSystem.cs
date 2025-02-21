using System.Collections.Generic;

public interface IDialogSystem
{
    bool ReadJSON(string path);
    Dictionary<string, List<string>> GetDialog(string key);
    bool IsDialogEmpty();
}
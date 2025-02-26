#if UNITY_EDITOR
using UnityEngine;
#endif

public static class GameUtilities
{
    public const string DEFAULT_LANGUAGE = "en";

    // TODO - null при первом запуске?
    public static string CurrentLanguage { get; set; }

    public static void DisplayLog(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
    public static void DisplayWarning(string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
    public static void DisplayError(string message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif
    }


    // TODO - существование приводит к работе смены языков на второй клик
    //[RuntimeInitializeOnLoadMethod]
    //private static void InitializeOnLoadMethod()
    //{
    //    CurrentLanguage = DEFAULT_LANGUAGE;
    //}
}
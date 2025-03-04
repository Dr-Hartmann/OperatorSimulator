namespace GameUtilities.Debug
{
#if UNITY_EDITOR
    using UnityEngine;
#endif
    public static class GameUtilities
    {
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
    }
}
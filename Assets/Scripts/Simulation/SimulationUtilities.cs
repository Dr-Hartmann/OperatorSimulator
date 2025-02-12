using UnityEngine;

public static class SimulationUtilities
{
    public static void DisplayLog(string message)
    {
        Debug.Log(message);
    }
    public static void DisplayWarning(string message)
    {
        Debug.LogWarning(message);
    }
    public static void DisplayError(string message)
    {
        Debug.LogError(message);
    }
}
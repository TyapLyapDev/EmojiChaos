#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class CustomLogger
{
    private const string White = "#ffffff";
    private const string Green = "#6bff4a";
    private const string Red = "#ff4242";
    private const string Blue = "#5c64ff";

    public static void ClearConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        System.Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method?.Invoke(new object(), null);
    }

    public static void LogWhite(string message) =>
        Debug.Log(MakeBold(WrapColor(White, message)));

    public static void LogGreen(string message) =>
        Debug.Log(MakeBold(WrapColor(Green, message)));

    public static void LogRed(string message) =>
        Debug.LogWarning(MakeBold(WrapColor(Red, message)));

    public static void LogBlue(string message) =>
        Debug.Log(MakeBold(WrapColor(Blue, message)));

    private static string WrapColor(string htmlColor, string text) =>
        $"<color={htmlColor}>{text}</color>";

    private static string MakeBold(string text) =>
        $"<b>{text}</b>";
}
#endif
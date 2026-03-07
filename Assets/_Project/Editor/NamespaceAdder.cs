using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class NamespaceAdder : EditorWindow
{
    [MenuItem("Tools/Add Namespaces")]
    static void AddNamespaces()
    {
        string scriptsPath = Application.dataPath + "/_Project/Scripts";
        ProcessDirectory(scriptsPath, "EmojiChaos");
        AssetDatabase.Refresh();
        Debug.Log("Namespaces added.");
    }

    static void ProcessDirectory(string path, string rootNamespace)
    {
        foreach (string file in Directory.GetFiles(path, "*.cs"))
        {
            ProcessFile(file, rootNamespace);
        }

        foreach (string subDir in Directory.GetDirectories(path))
        {
            ProcessDirectory(subDir, rootNamespace);
        }
    }

    static void ProcessFile(string filePath, string rootNamespace)
    {
        string relativePath = filePath.Replace(Application.dataPath + "/_Project/Scripts/", "").Replace('\\', '/');
        string folderPath = Path.GetDirectoryName(relativePath)?.Replace('\\', '/') ?? "";
        string ns = string.IsNullOrEmpty(folderPath)
            ? rootNamespace
            : rootNamespace + "." + folderPath.Replace('/', '.');

        string[] lines = File.ReadAllLines(filePath);

        // Пропускаем, если пространство имён уже добавлено
        foreach (string line in lines)
        {
            if (line.TrimStart().StartsWith("namespace ") && line.Contains(ns))
                return;
        }

        // Находим индекс последней using-директивы
        int lastUsingIndex = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            string trimmed = lines[i].TrimStart();
            if (trimmed.StartsWith("using ") && !trimmed.Contains("namespace"))
                lastUsingIndex = i;
        }

        List<string> newLines = new List<string>();

        if (lastUsingIndex != -1)
        {
            // Копируем всё до последнего using включительно
            for (int i = 0; i <= lastUsingIndex; i++)
                newLines.Add(lines[i]);

            // Пустая строка, открытие namespace
            newLines.Add("");
            newLines.Add($"namespace {ns}");
            newLines.Add("{");

            // Остальной код
            for (int i = lastUsingIndex + 1; i < lines.Length; i++)
                newLines.Add(lines[i]);

            // Закрывающая скобка
            newLines.Add("}");
        }
        else
        {
            // Нет using – ищем первую строку кода (не директиву препроцессора)
            int firstCodeLine = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].TrimStart();
                if (!string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith("#"))
                {
                    firstCodeLine = i;
                    break;
                }
            }

            // Всё до первой строки кода оставляем как есть
            for (int i = 0; i < firstCodeLine; i++)
                newLines.Add(lines[i]);

            newLines.Add($"namespace {ns}");
            newLines.Add("{");

            for (int i = firstCodeLine; i < lines.Length; i++)
                newLines.Add(lines[i]);

            newLines.Add("}");
        }

        File.WriteAllLines(filePath, newLines.ToArray());
    }
}
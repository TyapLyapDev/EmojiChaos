using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NamespaceAdder : EditorWindow
{
    [MenuItem("Tools/Add Namespaces")]
    static void AddNamespaces()
    {
        // Нормализуем путь к папке Scripts, заменяем обратные слеши на прямые
        string scriptsPath = Path.Combine(Application.dataPath, "_Project/Scripts").Replace('\\', '/');
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
        // Приводим путь к единому виду (прямые слеши)
        string normalizedPath = filePath.Replace('\\', '/');
        string scriptsPath = Path.Combine(Application.dataPath, "_Project/Scripts").Replace('\\', '/');

        // Убеждаемся, что файл действительно внутри папки Scripts
        if (!normalizedPath.StartsWith(scriptsPath))
            return;

        // Относительный путь от папки Scripts
        string relativePath = normalizedPath.Substring(scriptsPath.Length).TrimStart('/');
        // Папка, в которой лежит файл (без имени файла)
        string folderPath = Path.GetDirectoryName(relativePath)?.Replace('\\', '/') ?? "";

        // Формируем пространство имён: корень + (если есть подпапки) через точки
        string ns = string.IsNullOrEmpty(folderPath)
            ? rootNamespace
            : rootNamespace + "." + folderPath.Replace('/', '.');

        string[] lines = File.ReadAllLines(filePath);

        // Если такое пространство имён уже есть – пропускаем (простая проверка)
        foreach (string line in lines)
        {
            if (line.TrimStart().StartsWith("namespace ") && line.Contains(ns))
                return;
        }

        // Ищем последнюю using-директиву
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

            newLines.Add("");
            newLines.Add($"namespace {ns}");
            newLines.Add("{");

            // Остальной код
            for (int i = lastUsingIndex + 1; i < lines.Length; i++)
                newLines.Add(lines[i]);

            newLines.Add("}");
        }
        else
        {
            // Нет using – ищем первую строку кода (игнорируем директивы препроцессора)
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

            // Копируем всё до первой строки кода (директивы, пустые строки)
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
using System;
using System.Collections.Generic;

public static class Utils
{
    public static void Shuffle<T>(List<T> list)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));

        if (list.Count == 0)
            return;

        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T element = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = element;
        }
    }
}
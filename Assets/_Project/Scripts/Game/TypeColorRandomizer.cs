using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TypeColorRandomizer
{
    private readonly Dictionary<int, Color> _colorsMap = new();

    public TypeColorRandomizer(List<Color> colors, List<int> ids)
    {
        InitDictionary(colors, ids);
    }

    public bool TryGetColor(int id, out Color color) =>
        _colorsMap.TryGetValue(id, out color);

    private void InitDictionary(List<Color> colors, List<int> ids)
    {
        if (colors == null)
            throw new System.ArgumentNullException(nameof(colors));

        if (ids == null)
            throw new System.ArgumentNullException(nameof(ids));

        ids = ids.Distinct().ToList();

        if (colors.Count < ids.Count)
            throw new System.Exception($" оличество {nameof(colors)} должно быть не меньше количества {nameof(ids)}");

        Utils.Shuffle(colors);
        _colorsMap.Clear();

        for (int i = 0; i < ids.Count; i++)
            _colorsMap[ids[i]] = colors[i];
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Color set")]
[Serializable]
public class ColorSet : ScriptableObject
{
    [SerializeField] private List<Color> _colors;

    public IReadOnlyList<Color> Colors => _colors;
}
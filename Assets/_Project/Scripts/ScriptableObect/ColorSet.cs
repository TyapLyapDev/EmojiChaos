using System.Collections.Generic;
using UnityEngine;

namespace EmojiChaos.ScriptableObect
{

[CreateAssetMenu(menuName = "Scriptable object/Color set")]
public class ColorSet : ScriptableObject
{
    [SerializeField] private List<Color> _colors;

    public IReadOnlyList<Color> Colors => _colors;
}
}

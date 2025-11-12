using UnityEngine;

public interface ISwipeable : IClickable 
{ 
    Transform Transform { get; }
}
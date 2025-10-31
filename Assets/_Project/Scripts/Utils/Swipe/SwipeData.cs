using UnityEngine;

public struct SwipeData
{
    public ISwipeable TargetSwipeableObject;
    public Vector2 Direction;
    public Vector2 RawDelta;
    public float Distance;
}
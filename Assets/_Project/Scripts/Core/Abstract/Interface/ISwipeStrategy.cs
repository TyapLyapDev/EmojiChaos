public interface ISwipeStrategy
{
    bool TryGetSwipe(out SwipeData swipeData);

    bool IsPointerOverUIElement();
}
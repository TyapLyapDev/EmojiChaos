using UnityEngine;
using UnityEngine.EventSystems;

public class MobileClickHandlerStrategy : BaseClickHandlerStrategy
{
    public override Vector2 GetCurrentPosition() =>
        Input.touchCount > 0 ? Input.GetTouch(0).position : Vector2.zero;

    protected override void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Vector2 touchPosition = touch.position;

            if (IsUiElement(touchPosition, out RaycastResult result))
            {
                if (IsClickableUiElement(result, out IClickable clickable))
                {
                    InvokeClicked(clickable, touchPosition);

                    return;
                }

                return;
            }

            Ray ray = _camera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo) == false)
                return;

            if (hitInfo.collider.TryGetComponent(out IClickable clickableObject))
                InvokeClicked(clickableObject, touchPosition);
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            InvokeUnclicked();
    }
}
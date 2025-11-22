using UnityEngine;
using UnityEngine.EventSystems;

public class PCClickHandlerStrategy : BaseClickHandlerStrategy
{
    public override Vector2 GetCurrentPosition() =>
        Input.mousePosition;

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            if (IsUiElement(mousePosition, out RaycastResult result))
            {
                if (IsClickableUiElement(result, out IClickable clickable))
                {
                    InvokeClicked(clickable, mousePosition);

                    return;
                }

                return;
            }

            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo) == false)
                return;

            if (hitInfo.collider.TryGetComponent(out IClickable clickableObject))
                InvokeClicked(clickableObject, mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
            InvokeUnclicked();
    }
}
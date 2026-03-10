using UnityEngine;
using UnityEngine.EventSystems;

namespace EmojiChaos.Services.Input.ClickHandler
{
    using EmojiChaos.Core.Abstract.Interface;

    public class PCClickHandlerStrategy : BaseClickHandlerStrategy
    {
        public override Vector2 GetCurrentPosition() =>
            UnityEngine.Input.mousePosition;

        protected override void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;

                if (IsUiElement(mousePosition, out RaycastResult result))
                {
                    if (IsClickableUiElement(result, out IClickable clickable))
                    {
                        InvokeClicked(clickable, mousePosition);

                        return;
                    }

                    return;
                }

                Ray ray = CameraMain.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo) == false)
                    return;

                if (hitInfo.collider.TryGetComponent(out IClickable clickableObject))
                    InvokeClicked(clickableObject, mousePosition);
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
                InvokeUnclicked();
        }
    }
}
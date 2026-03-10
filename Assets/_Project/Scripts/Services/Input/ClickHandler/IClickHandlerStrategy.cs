using System;
using UnityEngine;

namespace EmojiChaos.Services.Input.ClickHandler
{
    using EmojiChaos.Core.Abstract.Interface;

    public interface IClickHandlerStrategy : IDisposable
    {
        event Action<IClickable, Vector2> Clicked;
        event Action Unclicked;

        Vector2 GetCurrentPosition();
    }
}
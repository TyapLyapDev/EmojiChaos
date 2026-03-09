using EmojiChaos.Core.Abstract.Interface;
using System;
using UnityEngine;

namespace EmojiChaos.Services.Input.ClickHandler
{
    public interface IClickHandlerStrategy : IDisposable
    {
        event Action<IClickable, Vector2> Clicked;
        event Action Unclicked;

        Vector2 GetCurrentPosition ( );
    }
}
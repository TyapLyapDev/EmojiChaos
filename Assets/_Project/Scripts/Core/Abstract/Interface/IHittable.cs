using System;
using UnityEngine;

namespace EmojiChaos.Core.Abstract.Interface
{
    public interface IHittable : IRepaintable
    {
        event Action<IHittable> Disappeared;

        Transform CenterBody { get; }

        bool IsActive { get; }

        void Kill();
    }
}
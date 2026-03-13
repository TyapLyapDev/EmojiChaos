using System;
using EmojiChaos.Animation;
using EmojiChaos.Core.Abstract.MonoBehaviourWrapper;
using EmojiChaos.Core.Repainters;
using UnityEngine;

namespace EmojiChaos.Entities.Cars
{
    public class CarVisual : InitializingBehaviour
    {
        [SerializeField] private MaterialIndexRepainter _repainter;
        [SerializeField] private CarAnimator _carAnimator;

        public void SetColor(Color color)
        {
            ValidateInit(nameof(SetColor));
            _repainter.SetColor(color);
        }

        public void ShowForwardAccident(Action accidentCompleted)
        {
            ValidateInit(nameof(ShowForwardAccident));
            _carAnimator.PlayForwardAccident(accidentCompleted);
        }

        public void ShowBackwardAccident(Action accidentCompleted)
        {
            ValidateInit(nameof(ShowBackwardAccident));
            _carAnimator.PlayBackwardAccident(accidentCompleted);
        }

        public void ShowUnavailable() =>
            _carAnimator.PlayUnavailable();

        protected override void OnInitialize() =>
            _repainter.Initialize();
    }
}
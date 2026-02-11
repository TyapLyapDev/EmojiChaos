using UnityEngine;

namespace UI.LevelCards
{
    public interface ICardInfo
    {
        public int LevelNumber { get; }

        public int StarCount { get; }

        public int Difficulty { get; }

        public CrowdSequenceType CrowdSequenceType { get; }

        public Sprite Preview { get; }

        bool IsLock { get; }
    }
}
using System;

namespace EmojiChaos.UtilsSpace.Splines.Graph
{
    public class VirtualNodes
    {
        private readonly SplineNode _start;
        private readonly SplineNode _goal;

        public VirtualNodes(SplineNode start, SplineNode goal)
        {
            _start = start ?? throw new ArgumentNullException(nameof(start));
            _goal = goal ?? throw new ArgumentNullException(nameof(goal));
        }

        public SplineNode Start => _start;

        public SplineNode Goal => _goal;
    }
}
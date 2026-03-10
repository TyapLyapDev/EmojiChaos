namespace EmojiChaos.Audio
{
    public readonly struct Transition
    {
        private readonly MusicState _newState;
        private readonly System.Action _action;

        public Transition(MusicState newState, System.Action action = null)
        {
            _newState = newState;
            _action = action;
        }

        public MusicState NewState => _newState;

        public System.Action Action => _action;
    }
}
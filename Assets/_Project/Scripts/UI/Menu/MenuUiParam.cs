using System;
using EmojiChaos.Core.Abstract.Interface;
using EmojiChaos.Services.Core;
using EmojiChaos.Services.Save;

namespace EmojiChaos.UI.Menu
{
    public readonly struct MenuUiParam : IParam
    {
        private readonly Saver _saver;
        private readonly SceneLoader _sceneLoader;

        public MenuUiParam(Saver saver, SceneLoader sceneLoader)
        {
            _saver = saver ?? throw new ArgumentNullException(nameof(saver));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        }

        public Saver Saver => _saver;

        public SceneLoader SceneLoader => _sceneLoader;
    }
}
using UnityEngine;

namespace EmojiChaos.UI.ShopContainer.Card
{
    using Core.Abstract.UI;
    using Enum;
    using ScriptableObect.Shop;

    public class ShopCard : ButtonClickHandler<ShopCard>
    {
        [SerializeField] private CardButtonTypeSwitcher _buttonTypeSwitcher;
        [SerializeField] private CardPreview _preview;
        [SerializeField] private CardTittle _tittle;
        [SerializeField] private CardDescription _description;

        public string RevardedAdvId { get; private set; }

        public ShopCardItemButtonType Type => _buttonTypeSwitcher.Type;

        public void SetInfo(ShopCardInfo info)
        {
            _buttonTypeSwitcher.UpdateSprite(info.Type);
            _preview.SetSprite(info.Preview);
            _tittle.SetText(info.Tittle);
            _description.SetText(info.Description);
            RevardedAdvId = info.RevardedAdvId;
        }

        public void SetType(ShopCardItemButtonType type) =>
            _buttonTypeSwitcher.UpdateSprite(type);
    }
}
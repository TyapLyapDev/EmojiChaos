namespace EmojiChaos.Core.Abstract.Interface
{
    public interface IFactory<out TValue>
    {
        TValue Create();
    }
}
public interface IFactory<out TValue>
{
    TValue Create();
}
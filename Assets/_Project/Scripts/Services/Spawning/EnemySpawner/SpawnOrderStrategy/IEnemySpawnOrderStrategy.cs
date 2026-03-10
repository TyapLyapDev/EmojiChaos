namespace EmojiChaos.Services.Spawning.EnemySpawner.SpawnOrderStrategy
{
    public interface IEnemySpawnOrderStrategy
    {
        int[] Calculate(int countLines);
    }
}
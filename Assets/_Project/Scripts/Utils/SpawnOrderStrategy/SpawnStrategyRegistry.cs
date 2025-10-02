using System;
using System.Collections.Generic;

public class SpawnStrategyRegistry
{
    private readonly Dictionary<SpawnOrderType, IEnemySpawnOrderStrategy> _strategies;

    public SpawnStrategyRegistry()
    {
        _strategies = new Dictionary<SpawnOrderType, IEnemySpawnOrderStrategy>
        {
            [SpawnOrderType.CenterFirst] = new CenterFirstSpawnOrderStrategy(),
            [SpawnOrderType.LeftToRight] = new LeftToRightSpawnStrategy(),
            [SpawnOrderType.RightToLeft] = new RightToLeftSpawnStrategy()
        };
    }

    public int[] CalculateSpawnOrder(SpawnOrderType orderType, int countLines)
    {
        if(_strategies.TryGetValue(orderType, out IEnemySpawnOrderStrategy strategy) == false) 
            throw new ArgumentNullException(nameof(strategy));

        return strategy.Calculate(countLines);
    }
}
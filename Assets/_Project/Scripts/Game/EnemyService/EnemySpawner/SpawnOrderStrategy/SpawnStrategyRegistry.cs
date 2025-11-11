using System;
using System.Collections.Generic;

public class SpawnStrategyRegistry
{
    private readonly Dictionary<SpawnOrderStrategyType, IEnemySpawnOrderStrategy> _strategies;

    public SpawnStrategyRegistry()
    {
        _strategies = new Dictionary<SpawnOrderStrategyType, IEnemySpawnOrderStrategy>
        {
            [SpawnOrderStrategyType.CenterFirst] = new CenterFirstSpawnOrderStrategy(),
            [SpawnOrderStrategyType.LeftToRight] = new LeftToRightSpawnStrategy(),
            [SpawnOrderStrategyType.RightToLeft] = new RightToLeftSpawnStrategy()
        };
    }

    public int[] CalculateSpawnOrder(SpawnOrderStrategyType orderType, int countLines)
    {
        if(_strategies.TryGetValue(orderType, out IEnemySpawnOrderStrategy strategy) == false) 
            throw new ArgumentNullException(nameof(strategy));

        return strategy.Calculate(countLines);
    }
}
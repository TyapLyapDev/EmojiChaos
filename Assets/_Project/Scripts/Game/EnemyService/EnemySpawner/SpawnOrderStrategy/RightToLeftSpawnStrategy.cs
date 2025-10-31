using System;

public class RightToLeftSpawnStrategy : IEnemySpawnOrderStrategy
{
    public int[] Calculate(int countLines)
    {
        if (countLines <= 0)
            throw new ArgumentOutOfRangeException(nameof(countLines), "Значение должно быть больше нуля");

        int[] order = new int[countLines];

        for (int i = 0; i < countLines; i++)
            order[i] = countLines - 1 - i;

        return order;
    }
}
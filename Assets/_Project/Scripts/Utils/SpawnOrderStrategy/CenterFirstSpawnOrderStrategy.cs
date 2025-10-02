using System;
using System.Reflection;

public class CenterFirstSpawnOrderStrategy : IEnemySpawnOrderStrategy
{
    private const int CenterDivisor = 2;

    public int[] Calculate(int countLines)
    {
        if (countLines <= 0)
            throw new ArgumentOutOfRangeException(nameof(countLines), "ОБАНА! Количество линий должно быть больше нуля");

        if (countLines == 1)
            return new int[] { 0 };

        int centerIndex = CalculateCenterIndex(countLines);

        return GenerateSpawnOrder(countLines, centerIndex);
    }

    private int[] GenerateSpawnOrder(int countLines, int centerIndex)
    {
        int[] spawnOrder = new int[countLines];
        spawnOrder[0] = centerIndex;

        int leftDistance = 1;
        int rightDistance = 1;
        int orderIndex = 1;

        while (orderIndex < countLines)
        {
            int leftIndex = centerIndex - leftDistance;

            if(TryAddIndexInRange(leftIndex, countLines, spawnOrder, ref orderIndex, ref leftDistance))
                if (orderIndex >= countLines)
                    break;

            int rightIndex = centerIndex + rightDistance;

            TryAddIndexInRange(rightIndex, countLines, spawnOrder, ref orderIndex, ref rightDistance);
        }

        return spawnOrder;
    }

    private int CalculateCenterIndex(int countLines)
    {
        int result = countLines / CenterDivisor;

        if (IsEven(countLines))
            --result;

        return result;
    }

    private bool IsEven(int value) =>
        value % CenterDivisor == 0;

    private bool TryAddIndexInRange(int sideIndex, int countLines, int[] spawnOrder, ref int orderIndex, ref int distance)
    {
        if (sideIndex >= 0 && sideIndex < countLines)
        {
            spawnOrder[orderIndex] = sideIndex;
            orderIndex++;
            distance++;

            return true;
        }

        return false;
    }
}
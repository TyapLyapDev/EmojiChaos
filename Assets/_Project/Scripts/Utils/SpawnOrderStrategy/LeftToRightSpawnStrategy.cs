using System;

public class LeftToRightSpawnStrategy : IEnemySpawnOrderStrategy
{
    public int[] Calculate(int countLines)
    {
        if (countLines <= 0)
            throw new ArgumentOutOfRangeException(nameof(countLines), "�������� ������ ���� ������ ����");

        int[] order = new int[countLines];

        for (int i = 0; i < countLines; i++)
            order[i] = i;

        return order;
    }
}
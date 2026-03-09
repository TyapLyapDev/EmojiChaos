using System;

namespace EmojiChaos.Services.Spawning.EnemySpawner
{

public class EnemyFormationCalculator
{
    private const int MiddlePointDivisor = 2;

    public float[] Calculate(int countLines, float stepOffset)
    {
        if (countLines <= 0)
            throw new ArgumentOutOfRangeException(nameof(countLines), "������, �����! ���������� ����� � ����� ������ ���� ������ ����!");

        if (stepOffset < 0)
            throw new ArgumentOutOfRangeException(nameof(stepOffset), "������, �����! ������� �� ����� ���� ��������������!");

        float[] offsets = new float[countLines];
        float startOffset = -(countLines - 1) * stepOffset / MiddlePointDivisor;

        for (int i = 0; i < countLines; i++)
            offsets[i] = (i * stepOffset) + startOffset;

        return offsets;
    }
}
}

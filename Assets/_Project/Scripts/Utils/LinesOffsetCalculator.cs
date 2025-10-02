using System;

public class LinesOffsetCalculator
{
    private const int CenterDivisor = 2;

    public float[] Calculate(int countLines, float stepOffset)
    {
        if (countLines <= 0)
            throw new ArgumentOutOfRangeException(nameof(countLines), "ОПАЧКИ, КОСЯК! Количество линий в толпе должно быть больше нуля!");

        if (stepOffset < 0)
            throw new ArgumentOutOfRangeException(nameof(stepOffset), "ОПАЧКИ, КОСЯК! Отступы не могут быть отрицательными!");

        float[] offsets = new float[countLines];
        float startOffset = -(countLines - 1) * stepOffset / CenterDivisor;

        for (int i = 0; i < countLines; i++)
            offsets[i] = startOffset + i * stepOffset;

        return offsets;
    }
}
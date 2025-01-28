using System;

namespace TheRegulator.Next.RegexParsing;

internal class RegexRef(RegexItem regexItem, int start, int end) : IComparable
{
    public string StringValue { get; set; } = regexItem.ToString(0);

    public int Start { get; } = start;

    public int End { get; private set; } = end;

    public int Length
    {
        get => End - Start + 1;
        set => End = Start + value - 1;
    }

    public int CompareTo(object? other)
    {
        if (other is not RegexRef regexRef) return 0;

        if (Length < regexRef.Length) return -1;
        return Length > regexRef.Length ? 1 : 0;
    }

    public bool InRange(int location) => location >= Start && location <= End;
}
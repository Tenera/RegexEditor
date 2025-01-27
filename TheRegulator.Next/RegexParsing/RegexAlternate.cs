namespace TheRegulator.Next.RegexParsing;

internal class RegexAlternate : RegexItem
{
    public RegexAlternate(RegexBuffer buffer)
    {
        buffer.AddLookup(this, buffer.Offset, buffer.Offset);
        buffer.MoveNext();
    }

    public string ToString(int indent) => new string(' ', indent) + "or";
}
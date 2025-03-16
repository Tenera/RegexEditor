/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
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
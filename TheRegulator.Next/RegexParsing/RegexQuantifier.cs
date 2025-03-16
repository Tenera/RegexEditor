/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal partial class RegexQuantifier : RegexItem
{
    private readonly string _description;

    [GeneratedRegex(@"(?<n>\d+)(?<Comma>,?)(?<m>\d*)\}")]
    private static partial Regex QuantifierRegex();

    public RegexQuantifier(RegexBuffer buffer)
    {
        var offset = buffer.Offset;
        buffer.MoveNext();

        var match = QuantifierRegex().Match(buffer.String);
        if (match.Success)
        {
            _description = match.Groups["m"].Length == 0 
                ? (match.Groups["Comma"].Length == 0 ? $"Exactly {match.Groups["n"]} times" : $"At least {match.Groups["n"]} times") 
                : $"At least {match.Groups["n"]}, but not more than {match.Groups["m"]} times";
            buffer.Offset += match.Groups[0].Length;
            if (!buffer.AtEnd && buffer.Current == '?')
            {
                _description += " (non-greedy)";
                buffer.MoveNext();
            }
        }
        else
        {
            _description = "missing '}' in quantifier";
        }
        buffer.AddLookup(this, offset, buffer.Offset - 1);
    }

    public string ToString(int indent) => _description;
}
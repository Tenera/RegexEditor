/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal partial class RegexExpression : RegexItem
{
    public List<RegexItem> Items { get; } = [];

    public static string Interpret(string regex)
    {
        return new RegexExpression(new RegexBuffer(regex)).ToString(0);
    }

    public RegexExpression(RegexBuffer buffer) => Parse(buffer);

    private static void EatComment(RegexBuffer buffer)
    {
        while (buffer.Current != '\r')
        {
            buffer.MoveNext();
        }
    }

    private void Parse(RegexBuffer buffer)
    {
        while (!buffer.AtEnd)
        {
            if (buffer.IgnorePatternWhitespace && (buffer.Current == ' ' || buffer.Current == '\r' || buffer.Current == '\n' || buffer.Current == '\t'))
            {
                buffer.MoveNext();
            }
            else
            {
                switch (buffer.Current)
                {
                    case '#':
                        if (buffer.IgnorePatternWhitespace)
                        {
                            EatComment(buffer);
                            continue;
                        }
                        Items.Add(new RegexCharacter(buffer));
                        continue;
                    case '(':
                        Items.Add(new RegexCapture(buffer));
                        continue;
                    case ')':
                        // end of closure; just return.
                        return;
                    case '[':
                        Items.Add(new RegexCharClass(buffer));
                        continue;
                    case '\\':
                        Items.Add(new RegexCharacter(buffer));
                        continue;
                    case '{':
                        Items.Add(new RegexQuantifier(buffer));
                        continue;
                    case '|':
                        Items.Add(new RegexAlternate(buffer));
                        continue;
                    default:
                        Items.Add(new RegexCharacter(buffer));
                        continue;
                }
            }
        }
    }

    [GeneratedRegex(@"\r\n$")]
    private static partial Regex NewLineRegex();

    public string ToString(int indent)
    {
        var stringBuilder1 = new StringBuilder();
        var stringBuilder2 = new StringBuilder();
        foreach (var regexItem in Items)
        {
            if (regexItem is RegexCharacter { Special: false } regexCharacter)
            {
                stringBuilder2.Append(regexCharacter.ToString(indent));
            }
            else
            {
                if (stringBuilder2.Length != 0)
                {
                    stringBuilder1.Append(' ', indent);
                    stringBuilder1.AppendLine(stringBuilder2.ToString());
                    stringBuilder2 = new StringBuilder();
                }
                stringBuilder1.Append(' ', indent);
                var input = regexItem.ToString(indent);
                if (input.Length == 0) continue;

                stringBuilder1.Append(input);
                if (!NewLineRegex().IsMatch(input))
                {
                    stringBuilder1.AppendLine();
                }
            }
        }
        if (stringBuilder2.Length != 0)
        {
            stringBuilder1.Append(' ', indent);
            stringBuilder1.AppendLine(stringBuilder2.ToString());
        }
        return stringBuilder1.ToString();
    }
}
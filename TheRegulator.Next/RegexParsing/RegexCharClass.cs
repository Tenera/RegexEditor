﻿/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
using System;
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal partial class RegexCharClass : RegexItem
{
    private readonly string _description;

    [GeneratedRegex(@"(?<Negated>\^?)(?<Class>.+?)\]")]
    private static partial Regex NegatedRegex();

    public RegexCharClass(RegexBuffer buffer)
    {
        var offset = buffer.Offset;
        buffer.MoveNext();
        var match = NegatedRegex().Match(buffer.String);
        if (match.Success)
        {
            _description = string.Equals(match.Groups["Negated"].ToString(), "^", StringComparison.Ordinal)
                ? $"Any character not in \"{match.Groups["Class"]}\""
                : $"Any character in \"{match.Groups["Class"]}\"";

            buffer.Offset += match.Groups[0].Length;
        }
        else
        {
            _description = "missing ']' in character class";
        }
        buffer.AddLookup(this, offset, buffer.Offset - 1);
    }

    public string ToString(int indent) => _description;
}
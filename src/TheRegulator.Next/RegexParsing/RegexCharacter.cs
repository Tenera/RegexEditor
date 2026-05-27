/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal partial class RegexCharacter : RegexItem
{
    private string _character;

    private static readonly ImmutableDictionary<char, string> Escaped = new Dictionary<char, string>
    {
        { 'a', "A bell (alarm) \\u0007 " },
        { 'b', "Word boundary between //w and //W" },
        { 'B', "Not at a word boundary between //w and //W" },
        { 't', "A tab \\u0009 " },
        { 'r', "A carriage return \\u000D " },
        { 'v', "A vertical tab \\u000B " },
        { 'f', "A form feed \\u000C " },
        { 'n', "A new line \\u000A " },
        { 'e', "An escape \\u001B " },
        { 'w', "Any word character " },
        { 'W', "Any non-word character " },
        { 's', "Any whitespace character " },
        { 'S', "Any non-whitespace character " },
        { 'd', "Any digit " },
        { 'D', "Any non-digit " },
        { 'A', "Anchor to start of string (ignore multiline)" },
        { 'Z', "Anchor to end of string or before \\n (ignore multiline)" },
        { 'z', "Anchor to end of string (ignore multiline)" }
    }.ToImmutableDictionary();

    public bool Special { get; private set; }

    public RegexCharacter(string characters)
    {
        _character = characters;
    }

    public RegexCharacter(RegexBuffer buffer)
    {
        var offset = buffer.Offset;
        var flag = false;
        switch (buffer.Current)
        {
            case ' ':
                _character = "' ' (space)";
                buffer.MoveNext();
                break;
            case '$':
                _character = "$ (anchor to end of string)";
                buffer.MoveNext();
                break;
            case '*':
                _character = "* (zero or more times)";
                buffer.MoveNext();
                Special = true;
                flag = true;
                break;
            case '+':
                _character = "+ (one or more times)";
                buffer.MoveNext();
                Special = true;
                flag = true;
                break;
            case '.':
                _character = ". (any character)";
                buffer.MoveNext();
                Special = true;
                break;
            case '?':
                _character = "? (zero or one time)";
                buffer.MoveNext();
                Special = true;
                flag = true;
                break;
            case '\\':
                DecodeEscape(buffer);
                break;
            case '^':
                _character = "^ (anchor to start of string)";
                buffer.MoveNext();
                break;
            default:
                _character = buffer.Current.ToString();
                buffer.MoveNext();
                Special = false;
                break;
        }
        if (flag && !buffer.AtEnd && buffer.Current == '?')
        {
            _character += " (non-greedy)";
            buffer.MoveNext();
        }
        buffer.AddLookup((RegexItem)this, offset, buffer.Offset - 1, _character.Length == 1);
    }

    private void DecodeEscape(RegexBuffer buffer)
    {
        buffer.MoveNext();
        if (Escaped.TryGetValue(buffer.Current, out var c))
        {
            _character = c;
            Special = true;
            buffer.MoveNext();
        }
        else
        {
            if (CheckBackReference(buffer)) return;
            switch (buffer.Current)
            {
                case ' ':
                    _character = "' ' (space)";
                    Special = false;
                    buffer.MoveNext();
                    break;
                case 'c':
                    buffer.MoveNext();
                    _character = "CTRL-" + buffer.Current;
                    buffer.MoveNext();
                    break;
                case 'u':
                    buffer.MoveNext();
                    _character = "Unicode " + buffer.String[..4];
                    buffer.Offset += 4;
                    break;
                case 'x':
                    buffer.MoveNext();
                    _character = "Hex " + buffer.String[..2];
                    buffer.Offset += 2;
                    break;
                default:
                    _character = new string(buffer.Current, 1);
                    Special = false;
                    buffer.MoveNext();
                    break;
            }
        }
    }

    [GeneratedRegex("\r\n\t\t\t\t\t\tk\\<(?<Name>.+?)\\>\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex BackReferenceRegex();

    private bool CheckBackReference(RegexBuffer buffer)
    {
        var match = BackReferenceRegex().Match(buffer.String);
        if (!match.Success) return false;

        Special = true;
        _character = $"Backreference to match: {match.Groups["Name"]}";
        buffer.Offset += match.Groups[0].Length;
        return true;
    }

    public string ToString(int indent) => _character;
}
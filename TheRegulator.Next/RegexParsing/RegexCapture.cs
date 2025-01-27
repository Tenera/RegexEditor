using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal partial class RegexCapture : RegexItem
{
    private RegexItem? expression;
    private string _description = "Capture";
    private int _startLocation;

    private static readonly ReadOnlyDictionary<string, string> OptionNames = new Dictionary<string, string>()
    {
        { "i", "Ignore Case" },
        { "-i", "Ignore Case Off" },
        { "m", "Multiline" },
        { "-m", "Multiline Off" },
        { "n", "Explicit Capture" },
        { "-n", "Explicit Capture Off" },
        { "s", "Singleline" },
        { "-s", "Singleline Off" },
        { "x", "Ignore Whitespace" },
        { "-x", "Ignore Whitespace Off" },
    }.AsReadOnly();

    public RegexCapture(RegexBuffer buffer)
    {
        _startLocation = buffer.Offset;
        buffer.MoveNext();
        buffer.ClearInSeries();
        if (buffer.Current == '?')
        {
            var flag = CheckNamed(buffer);
            if (!flag)
            {
                flag = CheckBalancedGroup(buffer);
            }

            if (!flag)
            {
                flag = CheckNonCapturing(buffer);
            }

            if (!flag)
            {
                flag = CheckOptions(buffer);
            }

            if (!flag)
            {
                flag = CheckLookAhead(buffer);
            }

            if (!flag)
            {
                flag = CheckNonBacktracking(buffer);
            }

            if (!flag)
            {
                CheckConditional(buffer);
            }
        }
        else if (!HandlePlainOldCapture(buffer))
        {
            throw new Exception($"Unrecognized capture: {buffer.String}");
        }
        buffer.AddLookup(this, _startLocation, buffer.Offset - 1);
    }

    private void CheckClosingParen(RegexBuffer buffer)
    {
        char current;
        try
        {
            current = buffer.Current;
        }
        catch (Exception ex)
        {
            buffer.ErrorLocation = _startLocation;
            buffer.ErrorLength = 1;
            throw new Exception("Missing closing ')' in capture", ex);
        }

        if (current != ')')
        {
            throw new Exception($"Unterminated closure at offset {buffer.Offset}");
        }
        ++buffer.Offset;
    }

    private bool HandlePlainOldCapture(RegexBuffer buffer)
    {
        if (buffer.ExplicitCapture)
        {
            _description = "Non-capturing Group";
        }
        expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?(\\<|')                  # ?< or ?'\r\n\t\t\t\t\t\t(?<Name>[a-zA-Z0-9]+?)    # Capture name\r\n\t\t\t\t\t\t(\\>|')                    # ?> or ?'\r\n\t\t\t\t\t\t(?<Rest>.+)               # The rest of the string\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex NamedRegex();

    private bool CheckNamed(RegexBuffer buffer)
    {
        var match = NamedRegex().Match(buffer.String);
        if (!match.Success) return false;

        _description = $"Capture to <{match.Groups["Name"]}>";
        buffer.Offset += match.Groups["Rest"].Index;
        expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?:\r\n\t\t\t\t\t\t(?<Rest>.+)             # The rest of the expression\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex NonCapturingRegex();

    private bool CheckNonCapturing(RegexBuffer buffer)
    {
        var match = NonCapturingRegex().Match(buffer.String);
        if (!match.Success) return false;

        _description = "Non-capturing Group";
        buffer.Offset += match.Groups["Rest"].Index;
        expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?[\\<|']                  # ?< or ?'\r\n\t\t\t\t\t\t(?<Name1>[a-zA-Z]+?)       # Capture name1\r\n\t\t\t\t\t\t-\r\n\t\t\t\t\t\t(?<Name2>[a-zA-Z]+?)       # Capture name2\r\n\t\t\t\t\t\t[\\>|']                    # ?> or ?'\r\n\t\t\t\t\t\t(?<Rest>.+)               # The rest of the expression\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex BalancedGroupRegex();

    private bool CheckBalancedGroup(RegexBuffer buffer)
    {
        var match = BalancedGroupRegex().Match(buffer.String);
        if (!match.Success) return false;

        _description = $"Balancing Group <{match.Groups["Name1"]}>-<{match.Groups["Name2"]}>";
        buffer.Offset += match.Groups["Rest"].Index;
        expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?(?<Options>[imnsx-]+):\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex OptionsRegex();

    private bool CheckOptions(RegexBuffer buffer)
    {
        var match = OptionsRegex().Match(buffer.String);
        if (!match.Success) return false;

        var key = match.Groups["Options"].Value;
        _description = $"Set options to {RegexCapture.OptionNames[key]}";
        expression = null;
        buffer.Offset += match.Groups[0].Length;
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?\r\n\t\t\t\t\t\t(?<Assertion><=|<!|=|!)   # assertion char\r\n\t\t\t\t\t\t(?<Rest>.+)               # The rest of the expression\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex LookAhaedRegex();

    private bool CheckLookAhead(RegexBuffer buffer)
    {
        var match = LookAhaedRegex().Match(buffer.String);
        if (!match.Success) return false;

        _description = match.Groups["Assertion"].Value switch
        {
            "=" => "zero-width positive lookahead",
            "!" => "zero-width negative lookahead",
            "<=" => "zero-width positive lookbehind",
            "<!" => "zero-width negative lookbehind",
            _ => _description
        };
        buffer.Offset += match.Groups["Rest"].Index;
        expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?\\>\r\n\t\t\t\t\t\t(?<Rest>.+)             # The rest of the expression\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex NonBacktrackingRegex();

    private bool CheckNonBacktracking(RegexBuffer buffer)
    {
        var match = NonBacktrackingRegex().Match(buffer.String);
        if (!match.Success) return false;

        _description = "Non-backtracking subexpression";
        buffer.Offset += match.Groups["Rest"].Index;
        expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        return true;
    }

    [GeneratedRegex("\r\n\t\t\t\t        ^                         # anchor to start of string\r\n\t\t\t\t\t\t\\?\\(\r\n\t\t\t\t\t\t(?<Rest>.+)             # The rest of the expression\r\n\t\t\t\t\t\t", RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex ConditionalRegex();

    private bool CheckConditional(RegexBuffer buffer)
    {
        var match = ConditionalRegex().Match(buffer.String);
        if (!match.Success) return false;

        _description = "Conditional Subexpression";
        buffer.Offset += match.Groups["Rest"].Index;
        expression = new RegexConditional(buffer);
        return true;
    }

    public string ToString(int indent)
    {
        var str = _description;
        if (expression != null)
        {
            str = str + "\r\n" + expression.ToString(indent + 2) + new string(' ', indent) + "End Capture";
        }
        return str;
    }
}
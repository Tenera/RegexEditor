/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal partial class RegexCapture : RegexItem
{
    private RegexItem? _expression;
    private string _description = "Capture";
    private readonly int _startLocation;

    private static readonly ImmutableDictionary<string, string> OptionNames = new Dictionary<string, string>()
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
    }.ToImmutableDictionary();

    public RegexCapture(RegexBuffer buffer)
    {
        _startLocation = buffer.Offset;
        buffer.MoveNext();

        // we're not in a series of normal characters, so clear
        buffer.ClearInSeries();

        // if the first character of the capture is a '?',
        // we need to decode what comes after it.
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
        // plain old capture...
        else if (!HandlePlainOldCapture(buffer))
        {
            throw new Exception($"Unrecognized capture: {buffer.String}");
        }
        buffer.AddLookup(this, _startLocation, buffer.Offset - 1);
    }

    private void CheckClosingParen(RegexBuffer buffer)
    {
        // check for closing ")"
        char current;
        try
        {
            current = buffer.Current;
        }
        // no closing brace. Set highlight for this capture...
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
        // we're already at the expression. Just create a new expression,
        // and make sure that we're at a ")" when we're done
        if (buffer.ExplicitCapture)
        {
            _description = "Non-capturing Group";
        }
        _expression = new RegexExpression(buffer);
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
        _expression = new RegexExpression(buffer);
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
        _expression = new RegexExpression(buffer);
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
        _expression = new RegexExpression(buffer);
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
        _expression = null;
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
        _expression = new RegexExpression(buffer);
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
        _expression = new RegexExpression(buffer);
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
        _expression = new RegexConditional(buffer);
        return true;
    }

    public string ToString(int indent)
    {
        var str = _description;
        if (_expression != null)
        {
            str = str + "\r\n" + _expression.ToString(indent + 2) + new string(' ', indent) + "End Capture";
        }
        return str;
    }
}
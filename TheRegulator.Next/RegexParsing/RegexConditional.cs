using System;

namespace TheRegulator.Next.RegexParsing;

internal class RegexConditional : RegexItem
{
    private readonly RegexExpression _expression;
    private readonly RegexExpression _yesNo;
    private readonly int _startLocation;

    public RegexConditional(RegexBuffer buffer)
    {
        _startLocation = buffer.Offset;
        _expression = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        _yesNo = new RegexExpression(buffer);
        CheckClosingParen(buffer);
        buffer.AddLookup((RegexItem) this, _startLocation, buffer.Offset - 1);
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

    public string ToString(int indent)
    {
        var str1 = new string(' ', indent);
        var str2 = str1 + "if: " + _expression.ToString(0) + str1 + "match: ";
        foreach (var regexItem in _yesNo.Items)
        {
            str2 = !(regexItem is RegexAlternate) ? str2 + regexItem.ToString(indent) : str2 + "\r\n" + str1 + "else match: ";
        }
        return str2 + "\r\n";
    }
}
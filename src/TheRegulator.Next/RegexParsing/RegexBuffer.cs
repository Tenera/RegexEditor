/*
 * Original code written by Eric Gunnerson
 * for his Regex Workbench tool:
 * http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=43D952B8-AFC6-491B-8A5F-01EBD32F2A6C
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TheRegulator.Next.RegexParsing;

internal class RegexBuffer(string expression)
{
    private bool _inSeries;
    private readonly List<RegexRef> _expressionLookup = [];

    public char Current
    {
        get
        {
            if (Offset >= expression.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Current), "Beyond end of buffer");
            }
            return expression[Offset];
        }
    }

    public void MoveNext() => ++Offset;

    public bool AtEnd => Offset >= expression.Length;

    public int Offset { get; set; } = 0;

    public string String => expression[Offset..];

    public RegexBuffer Substring(int start, int end)
    {
        return new RegexBuffer(expression.Substring(start, end - start + 1));
    }

    public int ErrorLocation { get; set; } = -1;

    public int ErrorLength { get; set; } = -1;

    public RegexOptions RegexOptions { get; set; }

    public bool IgnorePatternWhitespace => (RegexOptions & RegexOptions.IgnorePatternWhitespace) != RegexOptions.None;

    public bool ExplicitCapture => (RegexOptions & RegexOptions.ExplicitCapture) != RegexOptions.None;

    public void ClearInSeries() => _inSeries = false;

    public void AddLookup(RegexItem item, int startLocation, int endLocation, bool canCoalesce = false)
    {
        if (_inSeries)
        {
            if (canCoalesce)
            {
                var regexRef = _expressionLookup[^1];
                regexRef.StringValue += item.ToString(0);
                regexRef.Length += endLocation - startLocation + 1;
            }
            else
            {
                _expressionLookup.Add(new RegexRef(item, startLocation, endLocation));
                _inSeries = false;
            }
        }
        else
        {
            if (canCoalesce)
            {
                _inSeries = true;
            }
            _expressionLookup.Add(new RegexRef(item, startLocation, endLocation));
        }
    }

    public RegexRef? MatchLocations(int spot)
    {
        return _expressionLookup.Where(x => x.InRange(spot)).OrderBy(x => x).FirstOrDefault();
    }
}
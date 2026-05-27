using FluentAssertions;
using TheRegulator.Next.RegexParsing;
using Xunit;

namespace TheRegulator.Next.Tests;

public class TestInterpretAnchor
{
    private string Interpret(string regex)
        => new RegexExpression(new RegexBuffer(regex)).ToString(0).ReplaceLineEndings("\n");

    [Fact]
    public void TestBegOfString()
    {
        Interpret("^").Should().Be("^ (anchor to start of string)\n");
    }

    [Fact]
    public void TestBegOfStringMultiline()
    {
        Interpret("\\A").Should().Be("Anchor to start of string (ignore multiline)\n");
    }

    [Fact]
    public void TestEndOfString()
    {
        Interpret("$").Should().Be("$ (anchor to end of string)\n");
    }

    [Fact]
    public void TestEndOfStringMultiline()
    {
        Interpret("\\Z").Should().Be("Anchor to end of string or before \\n (ignore multiline)\n");
    }

    [Fact]
    public void TestEndOfStringMultiline2()
    {
        Interpret("\\z").Should().Be("Anchor to end of string (ignore multiline)\n");
    }

    [Fact]
    public void TestWordBoundary()
    {
        Interpret("\\b").Should().Be("Word boundary between //w and //W\n");
    }

    [Fact]
    public void TestNonWordBoundary()
    {
        Interpret("\\B").Should().Be("Not at a word boundary between //w and //W\n");
    }
}

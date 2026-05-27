using FluentAssertions;
using TheRegulator.Next.RegexParsing;
using Xunit;

namespace TheRegulator.Next.Tests;

public class TestInterpretOptions
{
    private string Interpret(string regex)
        => new RegexExpression(new RegexBuffer(regex)).ToString(0).ReplaceLineEndings("\n");

    [Fact]
    public void TestIgnoreCase()
    {
        Interpret("(?i:)").Should().Be("Set options to Ignore Case\n");
    }

    [Fact]
    public void TestIgnoreCaseOff()
    {
        Interpret("(?-i:)").Should().Be("Set options to Ignore Case Off\n");
    }

    [Fact]
    public void TestMultiline()
    {
        Interpret("(?m:)").Should().Be("Set options to Multiline\n");
    }

    [Fact]
    public void TestMultilineOff()
    {
        Interpret("(?-m:)").Should().Be("Set options to Multiline Off\n");
    }

    [Fact]
    public void TestExplicitCapture()
    {
        Interpret("(?n:)").Should().Be("Set options to Explicit Capture\n");
    }

    [Fact]
    public void TestExplicitCaptureOff()
    {
        Interpret("(?-n:)").Should().Be("Set options to Explicit Capture Off\n");
    }

    [Fact]
    public void TestSingleline()
    {
        Interpret("(?s:)").Should().Be("Set options to Singleline\n");
    }

    [Fact]
    public void TestSinglelineOff()
    {
        Interpret("(?-s:)").Should().Be("Set options to Singleline Off\n");
    }

    [Fact]
    public void TestIgnoreWhitespace()
    {
        Interpret("(?x:)").Should().Be("Set options to Ignore Whitespace\n");
    }

    [Fact]
    public void TestIgnoreWhitespaceOff()
    {
        Interpret("(?-x:)").Should().Be("Set options to Ignore Whitespace Off\n");
    }
}

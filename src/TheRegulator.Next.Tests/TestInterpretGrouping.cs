using FluentAssertions;
using TheRegulator.Next.RegexParsing;
using Xunit;

namespace TheRegulator.Next.Tests;

public class TestInterpretGrouping
{
    private string Interpret(string regex)
        => new RegexExpression(new RegexBuffer(regex)).ToString(0).ReplaceLineEndings("\n");

    [Fact]
    public void TestCapture()
    {
        Interpret("(abc)").Should().Be("Capture\n  abc\nEnd Capture\n");
    }

    [Fact]
    public void TestNamedCapture()
    {
        Interpret("(?<L>abc)").Should().Be("Capture to <L>\n  abc\nEnd Capture\n");
    }

    [Fact]
    public void TestNonCapture()
    {
        Interpret("(?:abc)").Should().Be("Non-capturing Group\n  abc\nEnd Capture\n");
    }

    [Fact]
    public void TestAlternation()
    {
        Interpret("(a|b)").Should().Be("Capture\n  a\n    or\n  b\nEnd Capture\n");
    }

    [Fact]
    public void TestPositiveLookahead()
    {
        Interpret("(?=a)").Should().Be("zero-width positive lookahead\n  a\nEnd Capture\n");
    }

    [Fact]
    public void TestNegativeLookahead()
    {
        Interpret("(?!b)").Should().Be("zero-width negative lookahead\n  b\nEnd Capture\n");
    }

    [Fact]
    public void TestPositiveLookbehind()
    {
        Interpret("(?<=c)").Should().Be("zero-width positive lookbehind\n  c\nEnd Capture\n");
    }

    [Fact]
    public void TestNegativeLookbehind()
    {
        Interpret("(?<!d)").Should().Be("zero-width negative lookbehind\n  d\nEnd Capture\n");
    }

    [Fact]
    public void TestConditionalExpression()
    {
        Interpret("(?(abc)yes|no)").Should().Be("Conditional Subexpression\n  if: abc\n  match: yes\n  else match: no\nEnd Capture\n");
    }

    [Fact]
    public void TestConditionalNamed()
    {
        Interpret("(?(<V>)yes|no)").Should().Be("Conditional Subexpression\n  if: <V>\n  match: yes\n  else match: no\nEnd Capture\n");
    }
}

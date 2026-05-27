using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.Tests;

[TestFixture]
public class TestInterpretGrouping
{
    private string Interpret(string regex)
    {
        return new RegexExpression(new RegexBuffer(regex)).ToString(0).ReplaceLineEndings("\n");
    }

    [Test]
    public void TestCapture()
    {
        Assert.AreEqual("Capture\n  abc\nEnd Capture\n", Interpret("(abc)"));
    }

    [Test]
    public void TestNamedCapture()
    {
        Assert.AreEqual("Capture to <L>\n  abc\nEnd Capture\n", Interpret("(?<L>abc)"));
    }

    [Test]
    public void TestNonCapture()
    {
        Assert.AreEqual("Non-capturing Group\n  abc\nEnd Capture\n", Interpret("(?:abc)"));
    }

    [Test]
    public void TestAlternation()
    {
        Assert.AreEqual("Capture\n  a\n    or\n  b\nEnd Capture\n", Interpret("(a|b)"));
    }

    [Test]
    public void TestPositiveLookahead()
    {
        Assert.AreEqual("zero-width positive lookahead\n  a\nEnd Capture\n", Interpret("(?=a)"));
    }

    [Test]
    public void TestNegativeLookahead()
    {
        Assert.AreEqual("zero-width negative lookahead\n  b\nEnd Capture\n", Interpret("(?!b)"));
    }

    [Test]
    public void TestPositiveLookbehind()
    {
        Assert.AreEqual("zero-width positive lookbehind\n  c\nEnd Capture\n", Interpret("(?<=c)"));
    }

    [Test]
    public void TestNegativeLookbehind()
    {
        Assert.AreEqual("zero-width negative lookbehind\n  d\nEnd Capture\n", Interpret("(?<!d)"));
    }

    [Test]
    public void TestConditionalExpression()
    {
        Assert.AreEqual("Conditional Subexpression\n  if: abc\n  match: yes\n  else match: no\nEnd Capture\n", Interpret("(?(abc)yes|no)"));
    }

    [Test]
    public void TestConditionalNamed()
    {
        Assert.AreEqual("Conditional Subexpression\n  if: <V>\n  match: yes\n  else match: no\nEnd Capture\n", Interpret("(?(<V>)yes|no)"));
    }
}
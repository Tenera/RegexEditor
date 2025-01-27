using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.Tests;

[TestFixture]
public class TestInterpretGrouping
{
    private string Interpret(string regex)
    {
        return new RegexExpression(new RegexBuffer(regex)).ToString(0);
    }

    [Test]
    public void TestCapture()
    {
        Assert.AreEqual("Capture\r\n  abc\r\nEnd Capture\r\n", Interpret("(abc)"));
    }

    [Test]
    public void TestNamedCapture()
    {
        Assert.AreEqual("Capture to <L>\r\n  abc\r\nEnd Capture\r\n", Interpret("(?<L>abc)"));
    }

    [Test]
    public void TestNonCapture()
    {
        Assert.AreEqual("Non-capturing Group\r\n  abc\r\nEnd Capture\r\n", Interpret("(?:abc)"));
    }

    [Test]
    public void TestAlternation()
    {
        Assert.AreEqual("Capture\r\n  a\r\n    or\r\n  b\r\nEnd Capture\r\n", Interpret("(a|b)"));
    }

    [Test]
    public void TestPositiveLookahead()
    {
        Assert.AreEqual("zero-width positive lookahead\r\n  a\r\nEnd Capture\r\n", Interpret("(?=a)"));
    }

    [Test]
    public void TestNegativeLookahead()
    {
        Assert.AreEqual("zero-width negative lookahead\r\n  b\r\nEnd Capture\r\n", Interpret("(?!b)"));
    }

    [Test]
    public void TestPositiveLookbehind()
    {
        Assert.AreEqual("zero-width positive lookbehind\r\n  c\r\nEnd Capture\r\n", Interpret("(?<=c)"));
    }

    [Test]
    public void TestNegativeLookbehind()
    {
        Assert.AreEqual("zero-width negative lookbehind\r\n  d\r\nEnd Capture\r\n", Interpret("(?<!d)"));
    }

    [Test]
    public void TestConditionalExpression()
    {
        Assert.AreEqual("Conditional Subexpression\r\n  if: abc\r\n  match: yes\r\n  else match: no\r\nEnd Capture\r\n", Interpret("(?(abc)yes|no)"));
    }

    [Test]
    public void TestConditionalNamed()
    {
        Assert.AreEqual("Conditional Subexpression\r\n  if: <V>\r\n  match: yes\r\n  else match: no\r\nEnd Capture\r\n", Interpret("(?(<V>)yes|no)"));
    }
}
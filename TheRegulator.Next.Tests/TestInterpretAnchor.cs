using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.Tests;

[TestFixture]
public class TestInterpretAnchor
{
    private string Interpret(string regex)
    {
        return new RegexExpression(new RegexBuffer(regex)).ToString(0);
    }

    [Test]
    public void TestBegOfString()
    {
        Assert.AreEqual("^ (anchor to start of string)\r\n", Interpret("^"));
    }

    [Test]
    public void TestBegOfStringMultiline()
    {
        Assert.AreEqual("Anchor to start of string (ignore multiline)\r\n", Interpret("\\A"));
    }

    [Test]
    public void TestEndOfString()
    {
        Assert.AreEqual("$ (anchor to end of string)\r\n", Interpret("$"));
    }

    [Test]
    public void TestEndOfStringMultiline()
    {
        Assert.AreEqual("Anchor to end of string or before \\n (ignore multiline)\r\n", Interpret("\\Z"));
    }

    [Test]
    public void TestEndOfStringMultiline2()
    {
        Assert.AreEqual("Anchor to end of string (ignore multiline)\r\n", Interpret("\\z"));
    }

    public void TestWordBoundary()
    {
        Assert.AreEqual("Word boundary between //w and //W\r\n", Interpret("\\b"));
    }

    public void TestNonWordBoundary()
    {
        Assert.AreEqual("Not at a word boundary between //w and //W\r\n", Interpret("\\B"));
    }
}
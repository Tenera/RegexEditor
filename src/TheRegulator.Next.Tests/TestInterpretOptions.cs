using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.Tests;

[TestFixture]
public class TestInterpretOptions
{
    private string Interpret(string regex)
    {
        return new RegexExpression(new RegexBuffer(regex)).ToString(0);
    }

    [Test]
    public void TestIgnoreCase()
    {
        Assert.AreEqual("Set options to Ignore Case\r\n", Interpret("(?i:)"));
    }

    [Test]
    public void TestIgnoreCaseOff()
    {
        Assert.AreEqual("Set options to Ignore Case Off\r\n", Interpret("(?-i:)"));
    }

    [Test]
    public void TestMultiline()
    {
        Assert.AreEqual("Set options to Multiline\r\n", Interpret("(?m:)"));
    }

    [Test]
    public void TestMultilineOff()
    {
        Assert.AreEqual("Set options to Multiline Off\r\n", Interpret("(?-m:)"));
    }

    [Test]
    public void TestExplicitCapture()
    {
        Assert.AreEqual("Set options to Explicit Capture\r\n", Interpret("(?n:)"));
    }

    [Test]
    public void TestExplicitCaptureOff()
    {
        Assert.AreEqual("Set options to Explicit Capture Off\r\n", Interpret("(?-n:)"));
    }

    [Test]
    public void TestSingleline()
    {
        Assert.AreEqual("Set options to Singleline\r\n", Interpret("(?s:)"));
    }

    [Test]
    public void TestSinglelineOff()
    {
        Assert.AreEqual("Set options to Singleline Off\r\n", Interpret("(?-s:)"));
    }

    [Test]
    public void TestIgnoreWhitespace()
    {
        Assert.AreEqual("Set options to Ignore Whitespace\r\n", Interpret("(?x:)"));
    }

    [Test]
    public void TestIgnoreWhitespaceOff()
    {
        Assert.AreEqual("Set options to Ignore Whitespace Off\r\n", Interpret("(?-x:)"));
    }
}
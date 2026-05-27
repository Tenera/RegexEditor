using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.Tests;

[TestFixture]
public class TestInterpretOptions
{
    private string Interpret(string regex)
    {
        return new RegexExpression(new RegexBuffer(regex)).ToString(0).ReplaceLineEndings("\n");
    }

    [Test]
    public void TestIgnoreCase()
    {
        Assert.AreEqual("Set options to Ignore Case\n", Interpret("(?i:)"));
    }

    [Test]
    public void TestIgnoreCaseOff()
    {
        Assert.AreEqual("Set options to Ignore Case Off\n", Interpret("(?-i:)"));
    }

    [Test]
    public void TestMultiline()
    {
        Assert.AreEqual("Set options to Multiline\n", Interpret("(?m:)"));
    }

    [Test]
    public void TestMultilineOff()
    {
        Assert.AreEqual("Set options to Multiline Off\n", Interpret("(?-m:)"));
    }

    [Test]
    public void TestExplicitCapture()
    {
        Assert.AreEqual("Set options to Explicit Capture\n", Interpret("(?n:)"));
    }

    [Test]
    public void TestExplicitCaptureOff()
    {
        Assert.AreEqual("Set options to Explicit Capture Off\n", Interpret("(?-n:)"));
    }

    [Test]
    public void TestSingleline()
    {
        Assert.AreEqual("Set options to Singleline\n", Interpret("(?s:)"));
    }

    [Test]
    public void TestSinglelineOff()
    {
        Assert.AreEqual("Set options to Singleline Off\n", Interpret("(?-s:)"));
    }

    [Test]
    public void TestIgnoreWhitespace()
    {
        Assert.AreEqual("Set options to Ignore Whitespace\n", Interpret("(?x:)"));
    }

    [Test]
    public void TestIgnoreWhitespaceOff()
    {
        Assert.AreEqual("Set options to Ignore Whitespace Off\n", Interpret("(?-x:)"));
    }
}
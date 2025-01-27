using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.Tests;

[TestFixture]
public class TestInterpret
{
    private string Interpret(string regex) => RegexExpression.Interpret(regex);

    [Test]
    public void TestNormalChars()
    {
        Assert.AreEqual("Test\r\n", Interpret("Test"));
    }

    [Test]
    [TestCase("A bell (alarm) \\u0007 \r\n", "\\a")]
    [TestCase("A tab \\u0009 \r\n", "\\t")]
    [TestCase("A carriage return \\u000D \r\n", "\\r")]
    [TestCase("A vertical tab \\u000B \r\n", "\\v")]
    [TestCase("A form feed \\u000C \r\n", "\\f")]
    [TestCase("A new line \\u000A \r\n", "\\n")]
    [TestCase("An escape \\u001B \r\n", "\\e")]
    [TestCase("Hex FF\r\n", "\\xFF")]
    [TestCase("CTRL-C\r\n", "\\cC")]
    [TestCase("Unicode 1234\r\n", "\\u1234")]
    public void TestCharacterShortcuts(string expected, string regex)
    {
        Assert.AreEqual(expected, Interpret(regex));
    }

    [Test]
    public void TestCharacterGroup()
    {
        Assert.AreEqual("Any character in \"abcdef\"\r\n", Interpret("[abcdef]"));
    }

    [Test]
    public void TestCharacterGroupNegated()
    {
        Assert.AreEqual("Any character not in \"abcdef\"\r\n", Interpret("[^abcdef]"));
    }

    [Test]
    public void TestCharacterPeriod()
    {
        Assert.AreEqual(". (any character)\r\n", Interpret("."));
    }

    [Test]
    public void TestCharacterWord()
    {
        Assert.AreEqual("Any word character \r\n", Interpret("\\w"));
    }

    [Test]
    public void TestCharacterNonWord()
    {
        Assert.AreEqual("Any non-word character \r\n", Interpret("\\W"));
    }

    [Test]
    public void TestCharacterWhitespace()
    {
        Assert.AreEqual("Any whitespace character \r\n", Interpret("\\s"));
    }

    [Test]
    public void TestCharacterNonWhitespace()
    {
        Assert.AreEqual("Any non-whitespace character \r\n", Interpret("\\S"));
    }

    [Test]
    public void TestCharacterDigit()
    {
        Assert.AreEqual("Any digit \r\n", Interpret("\\d"));
    }

    [Test]
    public void TestCharacterNonDigit()
    {
        Assert.AreEqual("Any non-digit \r\n", Interpret("\\D"));
    }

    [Test]
    public void TestQuantifierPlus()
    {
        Assert.AreEqual("+ (one or more times)\r\n", Interpret("+"));
    }

    [Test]
    public void TestQuantifierStar()
    {
        Assert.AreEqual("* (zero or more times)\r\n", Interpret("*"));
    }

    [Test]
    public void TestQuantifierQuestion()
    {
        Assert.AreEqual("? (zero or one time)\r\n", Interpret("?"));
    }

    [Test]
    public void TestQuantifierFromNToM()
    {
        Assert.AreEqual("At least 1, but not more than 2 times\r\n", Interpret("{1,2}"));
    }

    [Test]
    public void TestQuantifierAtLeastN()
    {
        Assert.AreEqual("At least 5 times\r\n", Interpret("{5,}"));
    }

    [Test]
    public void TestQuantifierExactlyN()
    {
        Assert.AreEqual("Exactly 12 times\r\n", Interpret("{12}"));
    }

    [Test]
    public void TestQuantifierPlusNonGreedy()
    {
        Assert.AreEqual("+ (one or more times) (non-greedy)\r\n", Interpret("+?"));
    }

    [Test]
    public void TestQuantifierStarNonGreedy()
    {
        Assert.AreEqual("* (zero or more times) (non-greedy)\r\n", Interpret("*?"));
    }

    [Test]
    public void TestQuantifierQuestionNonGreedy()
    {
        Assert.AreEqual("? (zero or one time) (non-greedy)\r\n", Interpret("??"));
    }

    [Test]
    public void TestQuantifierFromNToMNonGreedy()
    {
        Assert.AreEqual("At least 1, but not more than 2 times (non-greedy)\r\n", Interpret("{1,2}?"));
    }

    [Test]
    public void TestQuantifierAtLeastNNonGreedy()
    {
        Assert.AreEqual("At least 5 times (non-greedy)\r\n", Interpret("{5,}?"));
    }

    [Test]
    public void TestQuantifierExactlyNNonGreedy()
    {
        Assert.AreEqual("Exactly 12 times (non-greedy)\r\n", Interpret("{12}?"));
    }
}
using FluentAssertions;
using TheRegulator.Next.RegexParsing;
using Xunit;

namespace TheRegulator.Next.Tests;

public class TestInterpret
{
    private string Interpret(string regex) => RegexExpression.Interpret(regex).ReplaceLineEndings("\n");

    [Fact]
    public void TestNormalChars()
    {
        Interpret("Test").Should().Be("Test\n");
    }

    [Theory]
    [InlineData("A bell (alarm) \\u0007 \n", "\\a")]
    [InlineData("A tab \\u0009 \n", "\\t")]
    [InlineData("A carriage return \\u000D \n", "\\r")]
    [InlineData("A vertical tab \\u000B \n", "\\v")]
    [InlineData("A form feed \\u000C \n", "\\f")]
    [InlineData("A new line \\u000A \n", "\\n")]
    [InlineData("An escape \\u001B \n", "\\e")]
    [InlineData("Hex FF\n", "\\xFF")]
    [InlineData("CTRL-C\n", "\\cC")]
    [InlineData("Unicode 1234\n", "\\u1234")]
    public void TestCharacterShortcuts(string expected, string regex)
    {
        Interpret(regex).Should().Be(expected);
    }

    [Fact]
    public void TestCharacterGroup()
    {
        Interpret("[abcdef]").Should().Be("Any character in \"abcdef\"\n");
    }

    [Fact]
    public void TestCharacterGroupNegated()
    {
        Interpret("[^abcdef]").Should().Be("Any character not in \"abcdef\"\n");
    }

    [Fact]
    public void TestCharacterPeriod()
    {
        Interpret(".").Should().Be(". (any character)\n");
    }

    [Fact]
    public void TestCharacterWord()
    {
        Interpret("\\w").Should().Be("Any word character \n");
    }

    [Fact]
    public void TestCharacterNonWord()
    {
        Interpret("\\W").Should().Be("Any non-word character \n");
    }

    [Fact]
    public void TestCharacterWhitespace()
    {
        Interpret("\\s").Should().Be("Any whitespace character \n");
    }

    [Fact]
    public void TestCharacterNonWhitespace()
    {
        Interpret("\\S").Should().Be("Any non-whitespace character \n");
    }

    [Fact]
    public void TestCharacterDigit()
    {
        Interpret("\\d").Should().Be("Any digit \n");
    }

    [Fact]
    public void TestCharacterNonDigit()
    {
        Interpret("\\D").Should().Be("Any non-digit \n");
    }

    [Fact]
    public void TestQuantifierPlus()
    {
        Interpret("+").Should().Be("+ (one or more times)\n");
    }

    [Fact]
    public void TestQuantifierStar()
    {
        Interpret("*").Should().Be("* (zero or more times)\n");
    }

    [Fact]
    public void TestQuantifierQuestion()
    {
        Interpret("?").Should().Be("? (zero or one time)\n");
    }

    [Fact]
    public void TestQuantifierFromNToM()
    {
        Interpret("{1,2}").Should().Be("At least 1, but not more than 2 times\n");
    }

    [Fact]
    public void TestQuantifierAtLeastN()
    {
        Interpret("{5,}").Should().Be("At least 5 times\n");
    }

    [Fact]
    public void TestQuantifierExactlyN()
    {
        Interpret("{12}").Should().Be("Exactly 12 times\n");
    }

    [Fact]
    public void TestQuantifierPlusNonGreedy()
    {
        Interpret("+?").Should().Be("+ (one or more times) (non-greedy)\n");
    }

    [Fact]
    public void TestQuantifierStarNonGreedy()
    {
        Interpret("*?").Should().Be("* (zero or more times) (non-greedy)\n");
    }

    [Fact]
    public void TestQuantifierQuestionNonGreedy()
    {
        Interpret("??").Should().Be("? (zero or one time) (non-greedy)\n");
    }

    [Fact]
    public void TestQuantifierFromNToMNonGreedy()
    {
        Interpret("{1,2}?").Should().Be("At least 1, but not more than 2 times (non-greedy)\n");
    }

    [Fact]
    public void TestQuantifierAtLeastNNonGreedy()
    {
        Interpret("{5,}?").Should().Be("At least 5 times (non-greedy)\n");
    }

    [Fact]
    public void TestQuantifierExactlyNNonGreedy()
    {
        Interpret("{12}?").Should().Be("Exactly 12 times (non-greedy)\n");
    }
}

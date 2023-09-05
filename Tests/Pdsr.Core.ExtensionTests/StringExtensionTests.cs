using FluentAssertions;
using Pdsr.Core.Extensions;

namespace Pdsr.Core.ExtensionTests;

public class StringCaseExtensions
{

    [Theory]
    [InlineData("Test", "test")]
    public void SnakeCase_WithSample_ShouldProvide(string text, string converted)
    {
        var snakeCased = text.ToSnakeCase();

        snakeCased.Should().Be(converted);
    }

    [Fact]
    public void SnakeCase_OnePartString_ShouldProvide()
    {
        var text = "Test";
        var snakeCased = text.ToSnakeCase();

        snakeCased.Should().Be("test");
    }



    [Fact]
    public void SnakeCase_MultiPartString_ShouldProvide()
    {
        var text = "TestSomeValue";
        var snakeCased = text.ToSnakeCase();

        snakeCased.Should().Be("test_some_value");
    }



    [Fact]
    public void SnakeCase_WhenNumberInTheEnd_ShouldProvide()
    {
        var text = "Test14";
        var snakeCased = text.ToSnakeCase();

        snakeCased.Should().Be("test14");
    }



    [Fact]
    public void SnakeCase_WhenNumberInTheMiddle_ShouldProvide()
    {
        var text = "Test14Something";
        var snakeCased = text.ToSnakeCase();

        snakeCased.Should().Be("test14something");
    }


    [Theory]
    [InlineData("AbcdEf", "abcd-ef")]
    [InlineData("Abcd2Ef", "abcd2ef")]
    [InlineData("AbcdE2f", "abcd-e2f")]
    [InlineData("ABCD", "abcd")]
    [InlineData("ABCDeFG", "abc-de-fg")]
    [InlineData("ABCDefGH", "abc-def-gh")]
    public void KebabCase_MultipleValues_ShouldProvide(string text, string expected)
    {
        var kebabCased = text.ToKebabCase();
        kebabCased.Should().Be(expected);
    }
}

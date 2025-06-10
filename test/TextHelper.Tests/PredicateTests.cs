using System.Text;
using Xunit;

namespace TextHelper.Tests;

public class PredicateTests : IDisposable
{
    private readonly TextRemover _sut;

    public PredicateTests()
    {
        _sut = new TextRemover();
    }

    [Theory]
    [InlineData("clean", "")]              // middle 'e' - should be filtered
    [InlineData("what", "what")]           // middle 'ha' - should be filtered
    [InlineData("the", "the")]             // no vowel in middle - should pass
    [InlineData("rather", "rather")]       // no vowel in middle - should pass
    [InlineData("currently", "")]          // middle 'e' - should be filtered
    [InlineData("saab", "")]
    [InlineData("CLEAN", "")]              // uppercase middle 'E' - should be filtered
    [InlineData("WHAT", "WHAT")]           // uppercase middle 'HA' - should be filtered
    [InlineData("THE", "THE")]             // uppercase no vowel in middle - should pass
    [InlineData("CURRENTLY", "")]          // uppercase middle 'E' - should be filtered
    public async Task Filter1_FiltersWordsWithVowelsInMiddle(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter1);
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.Equal(expected, output.Trim());
    }

    [Theory]
    [InlineData("a", "")]                  // too short - should be filtered
    [InlineData("an", "")]                 // too short - should be filtered
    [InlineData("the", "the")]             // long enough - should pass
    [InlineData("word", "word")]           // long enough - should pass
    [InlineData("A", "")]                  // uppercase too short - should be filtered
    [InlineData("AN", "")]                 // uppercase too short - should be filtered
    [InlineData("THE", "THE")]             // uppercase long enough - should pass
    public async Task Filter2_FiltersWordsShorterThanThree(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter2);
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.Equal(expected, output.Trim());
    }

    [Theory]
    [InlineData("test", "")]               // contains 't' - should be filtered
    [InlineData("TEXT", "")]               // contains 't' - should be filtered
    [InlineData("word", "word")]           // no 't' - should pass
    [InlineData("hello", "hello")]         // no 't' - should pass
    [InlineData("TEST", "")]               // uppercase contains 'T' - should be filtered
    [InlineData("WORD", "WORD")]           // uppercase no 'T' - should pass
    [InlineData("HELLO", "HELLO")]         // uppercase no 'T' - should pass
    public async Task Filter3_FiltersWordsContainingT(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter3);
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.Equal(expected, output.Trim());
    }

    [Theory]
    [InlineData("clean", "")]              // fails Filter1
    [InlineData("a", "")]                  // fails Filter2
    [InlineData("test", "")]               // fails Filter3
    [InlineData("word", "word")]           // passes all filters
    [InlineData("CLEAN", "")]              // uppercase fails Filter1
    [InlineData("A", "")]                  // uppercase fails Filter2
    [InlineData("TEST", "")]               // uppercase fails Filter3
    [InlineData("WORD", "WORD")]           // uppercase passes all filters
    public async Task AllFilters_Combined(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter1);
        _sut.AddPredicate(Predicates.Filter2);
        _sut.AddPredicate(Predicates.Filter3);
        using var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.Equal(expected, output.Trim());
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
} 
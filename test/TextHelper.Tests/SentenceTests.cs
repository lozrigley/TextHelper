using System.Text;
using Xunit;

namespace TextHelper.Tests;

public class SentenceTests : IDisposable
{
    private readonly TextRemover _sut;

    public SentenceTests()
    {
        _sut = new TextRemover();
    }
    

    [Theory]
    [InlineData("xxxx    xx      xxxx        xx xxxx", "xxxx    xxxx xxxx")] //spaces before a missing word are also omitted
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
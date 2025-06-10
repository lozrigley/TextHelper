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
    [InlineData("xxxx    xx      xxxx      xx xxxx", "xxxx    xxxx      xxxx")] //spaces before a missing word are also omitted
    [InlineData("hedge.In another moment down", "hedge.down")]
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

    [Fact]
    public async Task ProcessTestFile_WithAllFilters()
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter1);
        _sut.AddPredicate(Predicates.Filter2);
        _sut.AddPredicate(Predicates.Filter3);
        
        var filePath = Path.Combine("..", "..", "..", "File", "Text.txt");
        using var fileStream = File.OpenRead(filePath);

        // Act
        var result = await _sut.ApplyFilters(fileStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.Equal(
            "beginning very bank, and having :once \r\nshe peeped reading, ,'and \r\nuse ,''?'she considering own mind (well\r\nshe ,made very and ),pleasure making \r\nand picking daisies, when suddenly pink eyes\r\n.very remarkable ;very much \r\n,'!!!'(when she over ,\r\noccurred she have wondered ,all seemed );when\r\n,and looked ,and hurried ,\r\n,flashed across mind she never before ,\r\n,and burning ,she across ,and \r\ndown large hole under hedge.down ,never\r\nonce considering world self she .hole like \r\nsome ,and dipped suddenly down, suddenly \r\nherself before she herself falling down very well.well very ,she fell very slowly,\r\nshe she down and wonder happen .\r\n,she down and make she coming ,dark ;she\r\nlooked sides well, and were filled and shelves; here and \r\nshe maps and hung upon pegs. She down from one shelves she passed; \r\nlabelled `ORANGE ',:she like drop \r\nkilling somebody, one she fell .",
            output);
    }

    public void Dispose()
    {
        _sut.Dispose();
    }
} 
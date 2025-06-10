using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextHelper.Tests;

[TestClass]
public class PredicateTests
{
    private TextRemover _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _sut = new TextRemover();
    }

    [TestMethod]
    [DataRow("clean", "")]              // middle 'e' - should be filtered
    [DataRow("what", "")]               // middle 'ha' - should be filtered
    [DataRow("the", "the")]             // no vowel in middle - should pass
    [DataRow("rather", "rather")]       // no vowel in middle - should pass
    [DataRow("currently", "")]          // middle 'e' - should be filtered
    public async Task Filter1_FiltersWordsWithVowelsInMiddle(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter1);
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.AreEqual(expected, output.Trim());
    }

    [TestMethod]
    [DataRow("a", "")]                  // too short - should be filtered
    [DataRow("an", "")]                 // too short - should be filtered
    [DataRow("the", "the")]             // long enough - should pass
    [DataRow("word", "word")]           // long enough - should pass
    public async Task Filter2_FiltersWordsShorterThanThree(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter2);
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.AreEqual(expected, output.Trim());
    }

    [TestMethod]
    [DataRow("test", "")]               // contains 't' - should be filtered
    [DataRow("text", "")]               // contains 't' - should be filtered
    [DataRow("word", "word")]           // no 't' - should pass
    [DataRow("hello", "hello")]         // no 't' - should pass
    public async Task Filter3_FiltersWordsContainingT(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter3);
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.AreEqual(expected, output.Trim());
    }

    [TestMethod]
    [DataRow("clean", "")]              // fails Filter1
    [DataRow("a", "")]                  // fails Filter2
    [DataRow("test", "")]               // fails Filter3
    [DataRow("word", "word")]           // passes all filters
    public async Task AllFilters_Combined(string input, string expected)
    {
        // Arrange
        _sut.AddPredicate(Predicates.Filter1);
        _sut.AddPredicate(Predicates.Filter2);
        _sut.AddPredicate(Predicates.Filter3);
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = await _sut.ApplyFilters(inputStream);
        var output = await new StreamReader(result).ReadToEndAsync();

        // Assert
        Assert.AreEqual(expected, output.Trim());
    }
} 
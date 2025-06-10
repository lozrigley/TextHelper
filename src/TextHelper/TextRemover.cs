using System.Text;
using System.Text.RegularExpressions;

namespace TextHelper;

public class TextRemover : IDisposable
{
    private readonly StringBuilder _wordBuffer;
    private readonly MemoryStream _outputStream;
    private readonly StreamWriter _writer;
    private static readonly Regex _wordCharacterRegex = new(@"^[a-zA-Z\-]$", RegexOptions.Compiled);
    private readonly List<Predicate<string>> _predicates = new();
    private readonly HashSet<char> _charactersToRemoveAfterRemovedWord = new() { ' ' };
    //private char? _lastWrittenChar;

    public TextRemover()
    {
        _wordBuffer = new StringBuilder();
        _outputStream = new MemoryStream();
        _writer = new StreamWriter(_outputStream, Encoding.UTF8);
    }

    public void AddPredicate(Predicate<string> predicate)
    {
        _predicates.Add(predicate);
    }

    public async Task<Stream> ApplyFilters(Stream input)
    {
        using var reader = new StreamReader(input, Encoding.UTF8);
        int character;
        bool processingWord = false;
        bool lastCharWasFromAWord = false;
        bool lastWordRemoved = false;
        while ((character = reader.Read()) != -1)
        {
            char c = (char)character;

            processingWord = IsWordCharacter(c);
            if (processingWord)
            {
                _wordBuffer.Append(c);
                lastCharWasFromAWord = true;
            }
            else
            {
                // Process any accumulated word
                if (_wordBuffer.Length > 0)
                {
                    var result = await ProcessWord();
                    lastWordRemoved = !result.WordWrittenToStream;

                }
                if (lastWordRemoved && _charactersToRemoveAfterRemovedWord.Contains(c))
                {
                    continue;
                }

                await _writer.WriteAsync(c);

            }
        }
        
        // Process any remaining word
        if (_wordBuffer.Length > 0)
        {
            await ProcessWord();
        }

        await _writer.FlushAsync();
        _outputStream.Position = 0;
        return _outputStream;
    }

    private async Task<ProcessWordResult> ProcessWord()
    {
        string word = _wordBuffer.ToString();
        _wordBuffer.Clear();

        // If any predicate returns true, discard the word
        bool shouldDiscard = _predicates.Any(predicate => predicate(word));
        
        if (!shouldDiscard)
        {
            await _writer.WriteAsync(word);
        }
        
        return new ProcessWordResult(!shouldDiscard);
    }

    private bool IsWordCharacter(char c)
    {
        return _wordCharacterRegex.IsMatch(c.ToString());
    }

    public void Dispose()
    {
        _writer.Dispose();
        _outputStream.Dispose();
    }
}

public record ProcessWordResult(bool WordWrittenToStream);

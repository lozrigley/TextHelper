using System.Text;
using System.Text.RegularExpressions;

namespace TextHelper;

public class TextRemover : IDisposable
{
    private readonly StringBuilder _wordBuffer;
    private readonly MemoryStream _outputStream;
    private readonly StreamWriter _writer;
    private static readonly Regex _wordCharacterRegex = new(@"^[a-zA-Z\-]$", RegexOptions.Compiled);
    private readonly List<Predicate<string>> _wordPredicates;

    public TextRemover()
    {
        _wordBuffer = new StringBuilder();
        _outputStream = new MemoryStream();
        _writer = new StreamWriter(_outputStream, Encoding.UTF8);
        _wordPredicates = new List<Predicate<string>>();
    }

    public void AddPredicate(Predicate<string> predicate)
    {
        _wordPredicates.Add(predicate);
    }

    public async Task<Stream> ApplyFilters(Stream input)
    {
        using var reader = new StreamReader(input, Encoding.UTF8);
        int character;

        while ((character = reader.Read()) != -1)
        {
            char c = (char)character;
            
            if (IsWordCharacter(c))
            {
                _wordBuffer.Append(c);
            }
            else
            {
                // Process any accumulated word
                if (_wordBuffer.Length > 0)
                {
                    await ProcessWord();
                }
                // Write the non-word character as is
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

    private async Task ProcessWord()
    {
        string word = _wordBuffer.ToString();
        _wordBuffer.Clear();

        // If any predicate returns true, discard the word
        bool shouldDiscard = _wordPredicates.Any(predicate => predicate(word));
        
        if (!shouldDiscard)
        {
            await _writer.WriteAsync(word);
        }
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

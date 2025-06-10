using System.Text;

namespace TextHelper;

public class TextRemover : IDisposable
{
    private readonly StringBuilder _wordBuffer;
    private readonly MemoryStream _outputStream;
    private readonly StreamWriter _writer;

    public TextRemover()
    {
        _wordBuffer = new StringBuilder();
        _outputStream = new MemoryStream();
        _writer = new StreamWriter(_outputStream, Encoding.UTF8);
    }

    public async Task<Stream> ApplyFilters(Stream input)
    {
        using var reader = new StreamReader(input, Encoding.UTF8);
        int character;

        while ((character = reader.Read()) != -1)
        {
            char c = (char)character;
            
            if (char.IsWhiteSpace(c))
            {
                await ProcessWord();
            }
            else
            {
                _wordBuffer.Append(c);
            }
        }
        
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

        if (word.Length % 2 == 0 && word.Length > 0)
        {
            await _writer.WriteAsync(word);
            await _writer.WriteAsync(' ');
        }
    }

    public void Dispose()
    {
        _writer.Dispose();
        _outputStream.Dispose();
    }
}

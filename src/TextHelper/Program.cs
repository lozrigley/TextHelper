using System;
using System.IO;
using System.Text;
using System.Linq;

namespace TextHelper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter file path:");
            string filePath = Path.Combine("..", "..", "..", "File", "Text.txt");

            try
            {
                await using var fileStream = File.OpenRead(filePath);
                using var textRemover = new TextRemover();
                
                textRemover.AddPredicate(Predicates.Filter1);
                textRemover.AddPredicate(Predicates.Filter2);
                textRemover.AddPredicate(Predicates.Filter3);
                
                var outputStream = await textRemover.ApplyFilters(fileStream);
                
                using var reader = new StreamReader(outputStream, Encoding.UTF8);
                string result = await reader.ReadToEndAsync();
                Console.WriteLine("\nProcessed text (words that pass all predicates):");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
} 
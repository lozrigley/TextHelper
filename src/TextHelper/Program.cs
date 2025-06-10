using System;
using System.IO;
using System.Text;

namespace TextHelper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter file path:");
            // string? filePath = Console.ReadLine();
            //
            // if (string.IsNullOrEmpty(filePath))
            // {
            //     Console.WriteLine("No file path provided.");
            //     return;
            // }

            string filePath = Path.Combine("..", "..", "..", "File", "Text.txt");

            try
            {
                await using var fileStream = File.OpenRead(filePath);
                using var textRemover = new TextRemover();
                var outputStream = await textRemover.ApplyFilters(fileStream);
                
                using var reader = new StreamReader(outputStream, Encoding.UTF8);
                string result = await reader.ReadToEndAsync();
                Console.WriteLine("\nProcessed text (only even-length words):");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
} 
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
                
                // Filter1: Filter out words with vowels in the middle
                textRemover.AddPredicate(word => 
                {
                    if (word.Length < 3) return false;
                    
                    int middleStart = (word.Length - 1) / 2;
                    int middleLength = word.Length % 2 == 0 ? 2 : 1;
                    string middle = word.Substring(middleStart, middleLength);
                    
                    return middle.Any(c => "aeiouAEIOU".Contains(c));
                });

                // Filter2: Filter out words shorter than 3 characters
                //textRemover.AddPredicate(word => word.Length < 3);

                // Filter3: Filter out words containing 't'
                //textRemover.AddPredicate(word => word.Contains('t', StringComparison.OrdinalIgnoreCase));
                
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
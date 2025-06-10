using System;
using System.IO;
using System.Text;

namespace TextHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter file path:");
            string? filePath = Console.ReadLine();
            
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("No file path provided.");
                return;
            }

            try
            {
                StreamFileCharacterByCharacter(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void StreamFileCharacterByCharacter(string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.UTF8);
            int character;
            
            while ((character = reader.Read()) != -1)
            {
                char c = (char)character;
                Console.Write(c);
                // You can add a small delay here if you want to see the streaming effect
                // Thread.Sleep(50);
            }
        }
    }
} 
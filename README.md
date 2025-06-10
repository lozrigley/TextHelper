# TextHelper

A .NET console application that provides text processing utilities, starting with a character-by-character file streaming feature.

## Features

- Stream files character by character using UTF8 encoding
- Real-time character display
- Error handling for file operations

## Requirements

- .NET 9.0 SDK
- Windows, macOS, or Linux operating system

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run --project src/TextHelper/TextHelper.csproj
   ```

## Usage

When you run the application:
1. You'll be prompted to enter a file path
2. Enter the full path to the text file you want to stream
3. The application will display the file contents character by character

## Project Structure

```
TextHelper/
├── src/
│   └── TextHelper/
│       ├── Program.cs
│       └── TextHelper.csproj
└── test/
    └── TextHelper.Tests/
        └── TextHelper.Tests.csproj
```

## Development

The project uses:
- .NET 9.0
- MSTest for unit testing
- UTF8 encoding for text processing

## License

MIT License 
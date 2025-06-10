namespace TextHelper;

public static class Predicates
{
    public static Predicate<string> Filter1 = word => 
    {
        if (word.Length < 3) return false;
                    
        int middleStart = (word.Length - 1) / 2;
        int middleLength = word.Length % 2 == 0 ? 2 : 1;
        string middle = word.Substring(middleStart, middleLength);
                    
        return middle.Any(c => "aeiouAEIOU".Contains(c));
    };

    public static Predicate<string> Filter2 = word => word.Length < 3;
    
    public static Predicate<string> Filter3 = word => word.Contains('t', StringComparison.OrdinalIgnoreCase);
}
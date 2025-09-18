using Figgle;

namespace PromptusMaximus.Console.Utilities;

internal static class ConsoleUtility
{
    public static void WriteLine(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        var currentForegroundColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = foregroundColor;
        System.Console.WriteLine(message);
        System.Console.ForegroundColor = currentForegroundColor;
    }

    public static void Write(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        var currentForegroundColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = foregroundColor;
        System.Console.Write(message);
        System.Console.ForegroundColor = currentForegroundColor;
    }

    public static void WriteLineWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
    }

    public static void WriteWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        Write($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
    }

    public static void WriteApplicationBanner()
    {
        WriteLine();
        WriteLine(Figgle.Fonts.FiggleFonts.Standard.Render("Promptus Maximus"), ConsoleColor.Green);
        WriteLine();
    }

}


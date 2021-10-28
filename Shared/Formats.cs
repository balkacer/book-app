namespace Shared
{
    public enum WriteType
    {
        WriteLine,
        Write
    }
   
    public class BackgroundColor
    {
        public static readonly string Black = "\x1B[40m", 
            White = "\x1B[47m", Red = "\x1B[41m", 
            Green = "\x1B[42m", Yellow = "\x1B[43m", 
            Blue = "\x1B[44m", Magenta = "\x1B[45m", 
            Cyan = "\x1B[46m";
    }

    public class Color
    {
        public static readonly string Black = "\x1B[30m",
            White = "\x1B[37m", Red = "\x1B[31m",
            Green = "\x1B[32m", Yellow = "\x1B[33m",
            Blue = "\x1B[34m", Magenta = "\x1B[35m",
            Cyan = "\x1B[36m";
    }

    public class Style
    {
        public static readonly string Bold = "\x1B[1m", 
            Faint = "\x1B[2m", Italic = "\x1B[3m", 
            Underlined = "\x1B[4m", Inverse = "\x1B[7m", 
            Strikethrough = "\x1B[9m";
    }
}

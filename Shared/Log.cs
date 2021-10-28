using System;

namespace Shared
{
    public class Log
    {
        public static readonly string ResetFormat = "\x1B[0m";
        public static void BreakLine()
        {
            Console.WriteLine();
        }

        public static void Print(string text, WriteType lines = WriteType.Write, params string[] formatters)
        {
            switch (lines)
            {
                case WriteType.WriteLine:
                    Console.WriteLine($" {string.Join("", formatters)}{text}\x1B[0m");
                    break;
                default:
                    Console.Write($" {string.Join("", formatters)}{text}\x1B[0m");
                    break;
            }
        }

        public static void WaitForKey()
        {
            Console.ReadKey();
        }

        public static string Input()
        {
            return Console.ReadLine();
        }

        public static void Clear()
        {
            Console.Clear();
        }
    }
}

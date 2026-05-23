namespace jarvis.debug;
using System;

static class Debug
{
    //? Important (not) stuff
    public static void Info(String Message)
    {
        Console.WriteLine($"\x1b[32m [:)] {Message} \x1b[0m"); //* Green
    }
    //? Non-critical error
    public static void Warn(String Message)
    {
        Console.WriteLine($"\x1b[33m [:|] {Message} \x1b[0m"); //* Yellow
    }
    //? Critical error
    public static void Error(String Message)
    {
        Console.WriteLine($"\x1b[31m [:(] {Message} \x1b[0m"); //* Red
        Environment.Exit(1);
    }
}
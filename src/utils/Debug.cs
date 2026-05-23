namespace jarvis.debug;
using System;

static class Debug
{
    //* A bear was walking through the forest and saw a car on fire. He got in and was burned to death.
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
//// 9999 Humanty corporation. All rights not reserved (until)
namespace jarvis;
using jarvis.debug;
using jarvis.config;
using System;
using jarvis.brain;
using SysProcess = System.Diagnostics.Process;

class Program
{
    static async Task Main(string[] args)
    {
        //? If program running in first, save user token
        if (!File.Exists(ConfManager.configPath))
        {
            Debug.Info("Token not found. Please, register in https://huggingface.co/, go to Settings -> Access Tokens, create a token and enter it here:");
            Console.Write(">> ");
            string? token = Console.ReadLine();
            
            if (!string.IsNullOrEmpty(token))
            {
                //? Saving the token
                Config config = new Config
                {
                    apiKey = token
                };
                ConfManager.Save(config);
            }
            else //? If the user is an asshole who doesn't enter anything
            {
                Debug.Error("Please, Enter the token");
            }
        }

        //// Wake up, Daddy`s home
        Debug.Info("Jarvis init");

        Brain brain = new Brain(); //? Init AI
        while (true)
        {
            Console.Write("[user] ");
            
            Data data = new Data //? Collect telemetry
            {
                question = Console.ReadLine(),
                time = DateTime.Now,
                cpuLoad = (byte)Math.Clamp(SysProcess.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds / 10000, 0, 100),
                memLoad = (byte)(SysProcess.GetCurrentProcess().WorkingSet64 / 1024 / 1024 / 100),
                uptime = (UInt32)(Environment.TickCount64 / 1000/ 60),
                nickname = Environment.UserName,
                activeWin = SysProcess.GetCurrentProcess().ProcessName, 
                diskFree = new DriveInfo(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory())).TotalFreeSpace,
            };
            if (data.question?.ToLower() == "!exit")
            {
                Debug.Error("Goodbye");
            }
            string? response = await brain.Think(data); //? Think, Думать, 思考, 考える
            Console.WriteLine($"[jarvis] {response}"); //? Tell recept for pancakes
        }
    }
}
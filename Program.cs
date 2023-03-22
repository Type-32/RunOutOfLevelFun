using System;
using System.Security.Cryptography;
namespace RunOutOfLevelFun.Informatics
{
    public static class GameConfiguration
    {
        public static string GameConfigPath { get { return Path.Combine(Directory.GetCurrentDirectory(), "config"); } }
        public static string SavesPath { get { return Path.Combine(GameConfigPath, "saves"); } }
        public static string ProgressionPath { get { return Path.Combine(SavesPath, "player.progress"); } }
    }
}
namespace RunOutOfLevelFun
{
    using System.Threading.Tasks;
    static class MenuUtils
    {
        public static async Task Loading(int sec, string loadingMessage = "Buffering...", bool clearConsole = true)
        {
            int seconds = sec * 1000;
            if (clearConsole) Console.Clear();
            if (seconds <= 0) return;
            int res = seconds % 500, times = seconds / 500;
            if (res == 0)
            {
                for (int i = 0; i < times; i++)
                {
                    Console.Write("\r{0}", ($"{loadingMessage}  {LoaderIcon(i)}"));
                    await Task.Delay(500);
                }
            }
            else
            {
                for (int i = 0; i < times; i++)
                {
                    Console.Write("\r{0}", ($"{loadingMessage}  {LoaderIcon(i)}"));
                    if (i != times - 1) await Task.Delay(500);
                    else await Task.Delay(res);
                }
            }
        }
        static string LoaderIcon(int inp)
        {
            string ret = "-";
            switch (inp % 4)
            {
                case 0:
                    ret = "-";
                    break;
                case 1:
                    ret = "\\";
                    break;
                case 2:
                    ret = "|";
                    break;
                case 3:
                    ret = "/";
                    break;
            }
            return ret;
        }
        public static void PrintRawContent(string[] con)
        {
            foreach (string i in con)
                Console.WriteLine(i);
        }
        public static async Task PrintRawContentAsync(string[] con, int[]? secs = null)
        {
            int temp = 0;
            foreach (string i in con)
            {
                Console.WriteLine(i);
                if (secs != null)
                    await Task.Delay(secs[temp] * 1000);
                temp++;
            }
        }
        public static async Task<ConsoleKey?> PrintManipulationContent(string[] con, int[]? secs = null, ConsoleKey? detectKey = null)
        {
            while (true)
            {
                Console.Clear();
                await PrintRawContentAsync(con, secs);
                ConsoleKeyInfo temp = Console.ReadKey();
                if (detectKey == null) return null;
                return temp.Key;
            }
        }
    }
    class Program
    {
        public static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                MenuUtils.PrintRawContent(new string[] {
                    "Run Out Of Level Fun! - Text-Adventure Game by Willy",
                    "",
                    ">Press 'Q' to Quit",
                    ">Press 'S' to Start"
                });
                ConsoleKeyInfo cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Q)
                {
                    await MenuUtils.Loading(3, "Clearing Data...");
                    Console.Clear();
                    Environment.Exit(0);
                }
                else if (cki.Key == ConsoleKey.S)
                {
                    await MenuUtils.Loading(3, "Initializing Game Data...");
                    break;
                }
                else
                    continue;
            }
            while (true)
            {
                Console.Clear();
                await MenuUtils.PrintManipulationContent(new string[]{
                    "You've noclipped into The Backrooms.",
                    "",
                    ">Continue... (Any Key)"
                }, new int[] { 1, 0, 0 });
                await MenuUtils.PrintManipulationContent(new string[]{
                    "You decide to look around.",
                    "",
                    ">Look Around... (Any Key)"
                }, new int[] { 1, 0, 0 });
                await MenuUtils.PrintManipulationContent(new string[]{
                    "You see a face of wall with an ugly-ass crayon painting on it.",
                    "",
                    ">Follow... (Any Key)"
                }, new int[] { 1, 0, 0 });
                break;
            }
        }
    }
}
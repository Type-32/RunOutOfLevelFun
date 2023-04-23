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
    public static class ProbabilityControl
    {
        public static int GenerateProbability(int[] weight)
        {
            int tot = 0, setLev = 0;
            Random prob = new Random();
            if (weight.Length > 1)
            {
                for (int i = 0; i < weight.Length; i++) tot += weight[i];
                setLev = prob.Next(0, tot);
                for (int i = 0; i < weight.Length - 1; i++)
                {
                    if (i == 0)
                        if (setLev >= 0 && setLev < weight[i]) return 1;
                        else
                        if (setLev >= weight[i] && setLev < weight[i]) return i + 1;
                }
            }
            else
            {
                setLev = prob.Next(0, 100);
                if (setLev <= weight[0]) return 1;
            }
            return 0;
        }
    }
}
namespace RunOutOfLevelFun
{
    using System.Threading.Tasks;
    using RunOutOfLevelFun.Informatics;
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
                //In-Game Intro Section
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

                //First Decision
                ConsoleKey? k = new();
                while (k != ConsoleKey.Y)
                {
                    k = await MenuUtils.PrintManipulationContent(new string[]{
                        "You Found a Door.",
                        "The Door says \"Fun\" on it.",
                        "Do you want to enter?",
                        "",
                        ">Yes (Y)",
                        ">Come On In :) (N)",
                        ">Why Would You Not Come In? (:D)"
                    }, new int[] { 1, 1, 1, 0, 1, 1, 1 }, ConsoleKey.Y);
                }
                int dec = ProbabilityControl.GenerateProbability(new int[] { 50, 80, 10 });
                if (dec == 1)
                {
                    await MenuUtils.PrintManipulationContent(new string[]{
                        "You Entered the Door.",
                        "There is another door behind the one you've entered.",
                        "",
                        ">Enter... (Any Key)"
                    }, new int[] { 1, 0, 0 ,1 });
                }
                else if (dec == 2)
                {
                    await MenuUtils.PrintManipulationContent(new string[]{
                        "You Entered the Door.",
                        "Behind the door are two doors presented in front of you.",
                        "",
                        ">Enter Door 1 (1)",
                        ">Enter Door 2 (2)"
                    }, new int[] { 1, 1, 0, 0, 0 }, ConsoleKey.D1);
                    int dec2 = ProbabilityControl.GenerateProbability(new int[]{});
                }
                else
                {

                }
                break;
            }
        }
    }
}
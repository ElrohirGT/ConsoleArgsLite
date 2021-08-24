using ConsoleArgsLite;
using System;

namespace TestClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = new ConsoleArgsManagerLite();
            manager.AddArgument("comicSource");
            manager.AddArgument("comicOutput", false);
            manager.AddArgument(
                name: "maxComicsPerSecond",
                required: true,
                parser: (s) => int.Parse(s),
                consoleName: "maxComicsPerSecond"
            );

            try
            {
                manager.ParseArguments(args);
                int maxComicsPerSecond = manager.GetValueFromArgument<int>("maxComicsPerSecond");
                Console.WriteLine(maxComicsPerSecond);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
    }
}
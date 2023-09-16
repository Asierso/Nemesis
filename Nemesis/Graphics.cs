using System;
namespace Nemesis{
    public class Graphics{
        public static string PrintQuestion(string text){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(text);
            Console.ResetColor();
            return Console.ReadLine();
        }
        public static void PrintSuccess(string text){
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void PrintSplash()
        {
            Console.WriteLine(" _______                               .__        ");
            Console.WriteLine(" \\      \\   ____   _____   ____   _____|__| ______");
            Console.WriteLine(" /   |   \\_/ __ \\ /     \\_/ __ \\ /  ___/  |/  ___/");
            Console.WriteLine("/    |    \\  ___/|  Y Y  \\  ___/ \\___ \\|  |\\___ \\ ");
            Console.WriteLine("\\____|__  /\\___  >__|_|  /\\___  >____  >__/____  >");
            Console.WriteLine("        \\/     \\/      \\/     \\/     \\/        \\/ ");
            Console.WriteLine("\nSemi-automatic bot generation");
        }
    }
}
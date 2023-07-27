using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemesis
{
    public class Graphics
    {
        public static void PrintSplash()
        {
            Console.WriteLine(" _______                               .__        ");
            Console.WriteLine(" \\      \\   ____   _____   ____   _____|__| ______");
            Console.WriteLine(" /   |   \\_/ __ \\ /     \\_/ __ \\ /  ___/  |/  ___/");
            Console.WriteLine("/    |    \\  ___/|  Y Y  \\  ___/ \\___ \\|  |\\___ \\ ");
            Console.WriteLine("\\____|__  /\\___  >__|_|  /\\___  >____  >__/____  >");
            Console.WriteLine("        \\/     \\/      \\/     \\/     \\/        \\/ ");
            Console.WriteLine("\nMassive bot generator - Powered by Asierso");
        }
        public static void PrintProgressBar(int value,int max,ConsoleColor color)
        {
            float onechunk = 20.0f / max;
            int position = 1;
            Console.Write("[");
            Console.CursorVisible = false;
            for (int i = 0; i < onechunk * value; i++)
            {
                Console.BackgroundColor = color;
                Console.ForegroundColor = color;
                Console.Write(" ");
                position++;
            }
            Console.ResetColor();
            for (int i = position; i <= 20; i++) 
            { 
               Console.ResetColor();
               Console.Write(" ");
            }
            Console.Write("]");
            Console.CursorVisible = true;
        }
        public static void PrintError(string error)
        {
            Console.ForegroundColor= ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ResetColor();
            Console.WriteLine(error);
        }
    }
}

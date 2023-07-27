using Nemesis.Models;
using Nemesis.Network;
using Nemesis.Scrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemesis
{
    public class ActionPerformer
    {
        private Database db { get; set; }
        private ScrapperEngine scrap { get; set; }
        private (int min, int max) userLength = ( 6, 10 );
        private List<User> userlist { get; set; }
        public ActionPerformer() 
        {

        }
        public void Run(string command)
        {
            if (command is null || command.Length == 0)
                return;

            (int x, int y) cursorPos = (Console.CursorLeft, Console.CursorTop);
            try
            {
                var commandSegments = command.Split(' ');
                switch(commandSegments[0])
                {
                    case "clear":Console.Clear(); break;
                    case "splash":Console.Clear();Graphics.PrintSplash(); break;
                    case "dbuse":
                        db=new Database(commandSegments[1]);
                        if (commandSegments.Length > 2)
                            db.DatabaseName = commandSegments[2];
                        else
                            db.DatabaseName = "nemesis";
                        break;
                    case "dbflush":
                        db = null;
                        break;
                    case "usergenconf":
                        userLength.min = int.Parse(commandSegments[1]); 
                        userLength.max = int.Parse(commandSegments[2]);
                        break;
                    case "usergen":
                        if (db is not null)
                        {
                            Console.Write($"Generating {commandSegments[2]} users for {commandSegments[1]}  ");
                            cursorPos = (Console.CursorLeft, Console.CursorTop);
                            for (int i = 0; i < int.Parse(commandSegments[2]); i++)
                            {
                                db.InsertUser(commandSegments[1], Identity.GeneratePlainIdentity(userLength.min,userLength.max));
                                Graphics.PrintProgressBar(i+1, int.Parse(commandSegments[2]), ConsoleColor.Green);
                                Console.SetCursorPosition(cursorPos.x, cursorPos.y);
                            }
                            Console.Write("\n");
                        }
                        else
                            Graphics.PrintError("Database conection not set");
                        break;
                    case "dbuserlist":
                        if (db is not null)
                        {
                            Console.Write($"Getting '{commandSegments[1]}' user list   ");
                            cursorPos = (Console.CursorLeft, Console.CursorTop);
                            var usrlist = db.GetUserList(commandSegments[1]);
                            while (!usrlist.IsCompleted)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    Graphics.PrintProgressBar(i + 1, 20, ConsoleColor.Green);
                                    Console.SetCursorPosition(cursorPos.x, cursorPos.y);
                                    Thread.Sleep(10);
                                }
                            }
                            Console.Write("\n");
                            Console.WriteLine($"User list '{commandSegments[1]}' with {usrlist.Result.Count} users loaded in local buffer");
                            userlist = usrlist.Result;
                            
                        }
                        else
                            Graphics.PrintError("Database conection not set");
                        break;
                    case "userlist":
                        if (userlist is not null)
                        {
                            Console.Write("User list (loaded locally)");
                            for (int i = 0; i < userlist.Count; i++)
                            {
                                Console.WriteLine($"Id: {userlist[i].Id}|Alias: {userlist[i].Alias}|Email: {userlist[i].Email}|Password: {userlist[i].Password}|Name: {userlist[i].Name}|Surname: {userlist[i].Surname}|Birth: {userlist[i].Birth}");
                            }
                        }
                        else
                            Graphics.PrintError("Userlist is not loaded");
                        break;
                    case "scrap":
                        if (scrap is null)
                            scrap = new ScrapperEngine();
                        var task = new Task(() =>
                        {
                            scrap.LoadScrappingInstructions("wsscript\\" + commandSegments[1] + ".wss", userlist[int.Parse(commandSegments[2])]);
                        });
                        task.Start();
                        Console.Write("Playing scrapping instructions  ");
                        cursorPos = (Console.CursorLeft, Console.CursorTop);
                        while (!task.IsCompleted)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                //Graphics.PrintProgressBar(i + 1, 20, ConsoleColor.Green);
                                //Console.SetCursorPosition(cursorPos.x, cursorPos.y);
                                Thread.Sleep(10);
                            }
                        }
                        Console.Write(scrap.Logs + "\n");
                        scrap.Logs = string.Empty;
                        break;
                }
            }
            catch(Exception ex)
            {
                Graphics.PrintError(ex.ToString());
            }
        }
    }
}

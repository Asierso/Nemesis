using Nemesis.Scrapper;
using System;

namespace Nemesis
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Nemesis";
            Graphics.PrintSplash();
            var aperf = new ActionPerformer();
            while (true)
            {
                Console.Write("> ");
                aperf.Run(Console.ReadLine());
            }
            /*
            var dbm = new Database("mongodb://localhost:27017");
            dbm.DatabaseName = "nemesis";
            while (true)
            {
                var usr = Identity.GeneratePlainIdentity(6,10);
                dbm.InsertUser("Test", Identity.GeneratePlainIdentity(6,10));
                Console.WriteLine($"Name: {usr.Name} Surname: {usr.Surname} Birth: {usr.Birth} Alias: {usr.Alias} Password: {usr.Password}");
                //Thread.Sleep(1000);
            }

            var i = Identity.GeneratePlainIdentity(6, 10);
            Console.Write(i.Id.ToString());
            dbm.InsertUser("Test", Identity.GeneratePlainIdentity(6, 10));
            Thread.Sleep(1000);
            */
        }

    }
}
using System;
using MongoDB.Driver;
namespace Nemesis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Graphics.PrintSplash();
            try
            {
                if (args.Length == 0)
                    AliasPlainGenerator();
                else
                    AliasRegister(args[0]);
            }
            catch (Exception ex)
            {
                Graphics.PrintError($"Error at execution: {ex.Message}");
            }
            Graphics.PrintSuccess("All done!");
            Console.ReadLine();
        }
        public static void AliasPlainGenerator()
        {
            Console.WriteLine("Alias generation mode");
            var fname = Graphics.PrintQuestion("Output filename: ");
            var amount = int.Parse(Graphics.PrintQuestion("Amount of aliases: "));
            if (File.Exists(fname))
                File.Delete(fname);
            var wrt = new StreamWriter(fname);
            var trim = new char[] {'\r','\n'};
            for (int i = 0; i < amount; i++)
            {
                var user = Identity.GeneratePlainIdentity(7, 15);
                user.Email = $"{user.Alias}@outlook.com";
                wrt.WriteLine($"{user.Alias?.Trim(trim)};{user.Name?.Trim(trim)};{user.Surname?.Trim(trim)};{user.Birth?.Trim(trim)};{user.Email.Trim(trim)};{user.Password?.Trim(trim)};{user.Biography?.Trim(trim)}");
            }
            wrt.Close();
        }
        public static void AliasRegister(string filename)
        {
            Console.WriteLine($"Alias register mode");
            var content = File.ReadAllText(filename);
            List<Models.User> users = new List<Models.User>();

            foreach (var line in content.Split('\n'))
            {
                var lineObj = line.Split(';');
                if (lineObj.Length >= 5)
                    users.Add(new Models.User()
                    {
                        Alias = lineObj[0],
                        Name = lineObj[1],
                        Surname = lineObj[2],
                        Birth = lineObj[3],
                        Email = lineObj[4],
                        Password = lineObj[5],
                        Biography = lineObj[6]
                    });
            }
            Console.WriteLine($"Total readed: {users.Count} lines");

            var mongoString = Graphics.PrintQuestion("MongoDB string: ");
            var dbname = Graphics.PrintQuestion("Database name: ");
            var collection = Graphics.PrintQuestion("Collection name: ");
            var start = int.Parse(Graphics.PrintQuestion("Start at line: "));
            var mongo = new MongoClient(mongoString);
            var db = mongo.GetDatabase(dbname);

            for (int i = start; i < users.Count; i++)
            {
                Console.WriteLine($"Register new user (NO: {i}) with these credentials:");
                Console.WriteLine($"-Alias: {users[i].Alias}\n-Name: {users[i].Name}\n-Surname: {users[i].Surname}\n-Birth: {users[i].Birth}\n-Email: {users[i].Email}\n-Password: {users[i].Password}\n-Biography: {users[i].Biography}");
                if (Graphics.PrintQuestion("User is valid? (Y/N): ").ToLower() != "y")
                    continue;

                if (Graphics.PrintQuestion("Continue with same alias? (Y/N): ").ToLower() != "y")
                    users[i].Alias = Graphics.PrintQuestion("Please enter the new alias: ");

                if (Graphics.PrintQuestion("Continue with same email? (Y/N): ").ToLower() != "y")
                    users[i].Email = Graphics.PrintQuestion("Please enter the new email: ");

                db.GetCollection<Models.User>(collection).InsertOne(users[i]);
                Graphics.PrintSuccess("Registered successfully");
            }
        }
    }
}
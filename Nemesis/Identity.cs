using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemesis.Models;

namespace Nemesis
{
    public class Identity
    {
        private static string nameListFile = "names.txt";
        private static string surnameListFile = "surnames.txt";
        private static string partKeysExpresionsListFile = "expkeys.txt";
        public static User GeneratePlainIdentity(int aliasMinLength = 5, int aliasMaxLength = 11)
        {
            var namelist = File.ReadAllText($"shuffle/{nameListFile}").Split('\n');
            var surnamelist = File.ReadAllText($"shuffle/{surnameListFile}").Split('\n');
            var random = new Random();
            var user = new User()
            {
                Name = namelist[random.Next(0, namelist.Length)],
                Surname = surnamelist[random.Next(0, surnamelist.Length)],
                Birth = $"{random.Next(1, 29)}/{random.Next(1, 13)}/{random.Next(1950, 2004)}"
            };
            if (user.Name == "" || user.Surname == "")
                GeneratePlainIdentity(aliasMinLength, aliasMaxLength);

            user.Alias = GenerateAlias(user,aliasMinLength,aliasMaxLength,random);
            user.Password = GenerateToken(12, random);
            return user;
        }
        private static string GenerateAlias(User user,int aliasMinLength,int aliasMaxLength,Random? random = null, string[] pkeBuffer = null)
        {
            if(random is null)
                random = new Random();

            var flags = new int[]
            {
                random.Next(0, 2), //Name start
                0, //Name stop (generate after)
                random.Next(0,3), //Interpart length
                random.Next(0, 2), //Surname start
                0, //Surname stop (generate after)
                random.Next(0,3), //Morepart length
            };

            string partKeys = "0123456789_***";
            string[] partKeysExpresions = Array.Empty<string>();
            if (pkeBuffer is null)
                partKeysExpresions = File.ReadAllText($"shuffle/{partKeysExpresionsListFile}").Split('\n');
            else
                partKeysExpresions = pkeBuffer;

            var part = new Func<int,string>((int length) =>
            {
                string result = string.Empty;
                string bufferedResult = string.Empty;
                for (int i = 0; i < length; i++)
                    bufferedResult += partKeys[random.Next(0, partKeys.Length)];
                bufferedResult.ToCharArray()
                    .ToList()
                    .ForEach((obj) =>
                    {
                        if (obj == '*' && random.Next(0, 2) == 0)
                            result += partKeysExpresions[random.Next(0, partKeysExpresions.Length)].ToUpper().TrimEnd('\n').TrimEnd('\r');
                        else if (obj == '*')
                            result += partKeysExpresions[random.Next(0, partKeysExpresions.Length)].TrimEnd('\n').TrimEnd('\r');
                        else
                            result += obj;
                    });
                return result;
            });

            string alias = string.Empty;
            string bufferAlias = string.Empty;
            bool valid = true;
            try
            {
                //Alias generator
                    flags[1] = random.Next(0, user.Name.Length - flags[0]);
                    flags[4] = random.Next(0, user.Name.Length - flags[3]);
                alias = user.Name.Substring(flags[0], flags[1]);
                if (flags[2] > 0)
                    alias += part(flags[2]);
                alias += user.Surname.Substring(flags[3], flags[4]);
                if (flags[5] > 0)
                    alias += part(flags[2]);

                alias = alias.Trim(new char[] { ' ','\r','\n' });

                //Alias checker
                if (alias.Length < aliasMinLength || alias.Length > aliasMaxLength)
                    valid = false;

                int nums = 0;
                foreach (char c in alias)
                {
                    if (partKeys.Contains(c) && c != '_')
                        nums++;
                    if (nums > alias.Length / 3)
                        valid = false;
                }

                //Acent parser
                bufferAlias = alias;
                alias = string.Empty;
                for(int i = 0; i < bufferAlias.Length; i++)
                {
                    if (bufferAlias[i].ToString() == "á")
                        alias += "a";
                    else if (bufferAlias[i].ToString() == "é")
                        alias += "e";
                    else if (bufferAlias[i].ToString() == "í")
                        alias += "i";
                    else if (bufferAlias[i].ToString() == "ó")
                        alias += "o";
                    else if (bufferAlias[i].ToString() == "ú")
                        alias += "u";
                    else if (bufferAlias[i].ToString() == "Á")
                        alias += "a";
                    else if (bufferAlias[i].ToString() == "É")
                        alias += "e";
                    else if (bufferAlias[i].ToString() == "Í")
                        alias += "i";
                    else if (bufferAlias[i].ToString() == "Ó")
                        alias += "o";
                    else if (bufferAlias[i].ToString() == "Ú")
                        alias += "u";
                    else
                        alias += bufferAlias[i];
                }
            }
            catch
            {
                valid = false;
            }
            if(valid is false)
                alias = GenerateAlias(user, aliasMinLength, aliasMaxLength,random, partKeysExpresions);

            return alias;
        }
        private static string GenerateToken(int length,Random? random = null)
        {
            if (random is null)
                random = new Random();

            string token = string.Empty;
            string key = "abcdefghijklmnopqrstuvwxyz01234567889";
            for(int i = 0; i < length; i++)
            {
                token += key[random.Next(0, key.Length)];
            }
            return token;
        }
    }
}

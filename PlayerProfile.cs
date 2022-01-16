using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public static class PlayerProfile
    {
        public static Datum Data = new();
        public static bool Invalid = false;
        public class Datum
        {

            public float StatLastRunTime;
            public float StatLastRunPercent;
            public int StatLastRunCoins;

            public float StatTotalTime;
            public float StatTotalTimeRunning;
            public int StatTotalDeaths;
            public int StatTotalMoneyEarned;
            
            public int Stronkth = 0, WeightLoss = 0, Undeadality = 0;
            public int Dashes;
            public int BadBananas;
            public int ExtraLives;
            public int Coins;
            public int CurrencyPrinter;
            public int MoneySpent;

            public bool Fullscreen = true;
        }

        public static bool New => !File.Exists("PLAYERPROFILE");

        static JsonSerializerOptions opt = new JsonSerializerOptions()
        {
            IncludeFields = true
        };

        static string funnyPadding = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        public static void Save()
        {
            string data = JsonSerializer.Serialize(Data, opt);
            using (var file = File.Create("PLAYERPROFILE"))
                using(var sw = new StreamWriter(file))
            {
                sw.WriteLine("SUPERMONKEYPOSTCELEBRATIONDIET");
                sw.WriteLine(data);
                sw.WriteLine(funnyPadding);
                sw.WriteLine(GetEncodedHash(data, salt));
            }
        }
        const string salt = "i know you will crack this so I don't really care";
        static string GetEncodedHash(string password, string salt) 
        { 
            MD5 md5 = new MD5CryptoServiceProvider(); 
            byte[] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt)); 
            string base64digest = Convert.ToBase64String(digest, 0, digest.Length); 
            return base64digest.Substring(0, base64digest.Length - 2);
        }
        const string file_header = "SUPERMONKEYPOSTCELEBRATIONDIET";
        public static void Load()
        {
            if (File.Exists("PLAYERPROFILE"))
            {

                using (StreamReader sr = File.OpenText("PLAYERPROFILE"))
                {
                    string HEADER = sr.ReadLine();
                    if(HEADER != file_header)
                    {
                        throw new InvalidDataException("File corrupted :(");
                    }
                    string pldata = sr.ReadLine();
                    Data = JsonSerializer.Deserialize<Datum>(pldata,opt);
                    string line;
                    int lines = 0;
                    while((line = sr.ReadLine()) == "") {
                        lines++;
                    }
                    lines--;
                    string hash = line;
                    Invalid = 
                        funnyPadding.Length != lines ||
                        GetEncodedHash(pldata, salt) != hash ||
                        (Data.StatTotalMoneyEarned - Data.MoneySpent != Data.Coins);
                }
            }
        }

    }
}

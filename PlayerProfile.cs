using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public static class PlayerProfile
    {
        public static Datum Data = new();
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
        }

        public static bool New => !File.Exists("PLAYERPROFILE");

        static JsonSerializerOptions opt = new JsonSerializerOptions()
        {
            IncludeFields = true
        };

        public static void Save()
        {
            File.WriteAllText("PLAYERPROFILE", JsonSerializer.Serialize(Data,opt));
        }

        public static void Load()
        {
            if (File.Exists("PLAYERPROFILE"))
            {
                Data = JsonSerializer.Deserialize<Datum>(File.ReadAllText("PLAYERPROFILE"), opt);
            }
        }

    }
}

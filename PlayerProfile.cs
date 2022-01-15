using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public static class PlayerProfile
    {
        public static Datum Data = new();
        public class Datum
        {
            public float LastRunTime;
            public float LastRunPercent;
            public int LastRunCoins;

            public int TotalDeaths;
            public int Stronkth = 0, WeightLoss = 0, Undeadality = 0;
            internal int Dashes;
            internal int BadBananas;
            internal int ExtraLives;
            internal int Coins = int.MaxValue;
            internal int CurrencyPrinter;
            internal int MoneySpent;

            public int MoneyEarned;
        }

    }
}

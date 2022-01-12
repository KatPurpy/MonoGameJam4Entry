using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public static class PlayerProfile
    {
        public static Datum Data;
        public class Datum
        {
            public float LastRunTime;
            public int LastRunCoins;

            public int TotalDeaths;
            public int Stronkth = 0, JumpForce = 0, Undeadality = 0;
        }

    }
}

using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    class Entity_Gym : Entity
    {
        public Entity_Gym(Main m) : base(m)
        {
            Size = new(800, 600);
            Position = new(400, 300);
            Sprite = m.Assets.BGG;
        }

        readonly int[] MoveSpeedUpgradeCosts = new[]
        {
            10,
            30,
            60
        };

        readonly int[] JumpUpgradeCosts = new[]
        {
            15,
            30
        };

        readonly int[] UndeadalityFactor = new[]
        {
            10,
        };

        readonly int[] HUH = new[]
        {
            150
        };

        readonly int[] BadBananaPrices = new[] { 5, 10, 15, 20, 25 };
        readonly int[] DashPrices = new[] { 2, 2, 2 };
        readonly int[] ExtraLivePrices = new[] { 10,20,30 };
        IntPtr placeholder;

        public enum Thing
        {
            Dash,
            BadBanana,
            ExtraLife,
            MoneyBrrr,
            Stronkth,
            Weight,
            Undeadality,
            Length
        }


        public override void Start()
        {
            PlayerProfile.Save();
            game.RenderOffset = new(0, 0);
            placeholder = game.ImGuiRenderer.BindTexture(game.PixelTexture);
            if(!showCredits) game.Assets.gym.Play();
            

        
        }

        public override void Update(GameTime time)
        {

            //Origin = new(400, 300);
          // Rotation = (float)time.TotalGameTime.TotalSeconds;
            //throw new NotImplementedException();
        }
        
        public override void IMGUI(GameTime time)
        {
            if(PlayerProfile.Invalid)
            {
                ImGui.Begin("CHEATER", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse);
                ImGui.TextColored(new(1,0,0,1),"I know you've changed save file data");
                ImGui.TextColored(new(1, 0, 0, 1), "You must be ashamed of yourself.");
                ImGui.TextColored(new(1, 0, 0, 1), "Or not? Good player, breaking the game as usual!");
                ImGui.TextColored(new(1, 0, 0, 1), "I don't care what you say now, you know the run is invalid.");
                ImGui.TextColored(new(1, 0, 0, 1), "Restore the changes and be an epic gamer next time. I hope you made a backup.");
                ImGui.Text("With love, Kat Purpy");
                ImGui.End();
            }
            if (ImGui.Begin("WORKOUT, PHAT BOI", ImGuiWindowFlags.AlwaysAutoResize))
            {
                BonusButton(ref PlayerProfile.Data.Stronkth, MoveSpeedUpgradeCosts, Thing.Stronkth, $"STRONKTH", "Control your fat body better and pwn that wind mechanic.");
                ImGui.SameLine();
                BonusButton(ref PlayerProfile.Data.WeightLoss, JumpUpgradeCosts, Thing.Weight, $"PHAT REDUCE", "Str0nkth p0wered legs 4 h0pp1n l1k3 a bunny.");
                ImGui.SameLine();
                BonusButton(ref PlayerProfile.Data.Undeadality, UndeadalityFactor, Thing.Undeadality, $"UNDEADALITY FACTOR", "20%% chance of magically resurrecting.", "tip: gud git");
                ImGui.End();
            }
            if (ImGui.Begin("LEGAL DOPPINGS", ImGuiWindowFlags.AlwaysAutoResize))
            {

                ImGui.Text("Temporary");
                BonusButton(ref PlayerProfile.Data.Dashes, DashPrices, Thing.Dash, "SNICKERS", "This thing is useful if you need to need to avoid something really fast.", "Only when you press [SPACE] and it exhausts immediately. In other words, this thing is HELLA USELESS.");
                ImGui.SameLine();
                BonusButton(ref PlayerProfile.Data.BadBananas, BadBananaPrices, Thing.BadBanana, "BAD BANANA", "Save your sorry ass (donkey) by releasing your gases out of fear, maybe it will work.", "Press [DOWN] to consume.");
                ImGui.SameLine();
                BonusButton(ref PlayerProfile.Data.ExtraLives, ExtraLivePrices, Thing.ExtraLife, "HEART", 
                    @"- Rhino, where do you get these hearts?
- Shut up and let me do the surgery
- Without anesthesia?
- >:|");
                ImGui.Text("Permament");
                BonusButton(ref PlayerProfile.Data.CurrencyPrinter, HUH, Thing.MoneyBrrr, "A MYSTERIOUS CURRENCY GENERATION DEVICE", "Pinocchio lost his money trying to grow a money tree for you noobs so you can get TWICE AS MUCH currency.");
                ImGui.End();
            }

            if (ImGui.Begin("STATS", ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.TextColored(new(1, 1, 0, 1), "CURRENCY: " + PlayerProfile.Data.Coins);

                ImGui.NewLine();
                ImGui.Text("TOTAL STATS");
                ImGui.Text("===========");
                ImGui.Text("Total time spent: " + Main.TimeToString(PlayerProfile.Data.StatTotalTime));
                ImGui.Text("Total time spent trying: " + Main.TimeToString(PlayerProfile.Data.StatTotalTimeRunning));
                ImGui.Text("Total currency wasted: " + PlayerProfile.Data.MoneySpent);
                ImGui.Text("Total currency risen: " + PlayerProfile.Data.StatTotalMoneyEarned);
                ImGui.Text("Total epic fails: " + PlayerProfile.Data.StatTotalDeaths);
                ImGui.Text("Meaning of life: 42");

                ImGui.NewLine();
                ImGui.Text("LAST RUN STATS");
                ImGui.Text("===========");
                ImGui.Text("Percentage: " + PlayerProfile.Data.StatLastRunPercent.ToString("F2") + "%%");
                ImGui.Text("Time: " + Main.TimeToString(PlayerProfile.Data.StatLastRunTime));
                ImGui.Text("Currency: " + PlayerProfile.Data.StatLastRunCoins);
                ImGui.End();
            }

            if (ImGui.Begin("FUNNY WINDOW", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse))
            {
                if (ImGui.Button("BEGIN"))
                {
                    Dead = true;
                    game.EntityManager.AddEntity(new Entity_RunStarter(game));
                }

                if (ImGui.Button("CREDITS"))
                {
                   showCredits = true;
                }



                if (ImGui.Button("QUIT"))
                {
                    game.Exit();
                }

                bool screen = game.gdm.IsFullScreen;
                if (ImGui.Checkbox("Full screen", ref screen))
                {
                    PlayerProfile.Data.Fullscreen = screen;
                    game.gdm.IsFullScreen = screen;
                    game.gdm.ApplyChanges();
                }
                ImGui.End();
            }
            if (showCredits)
            {
                if (ImGui.Begin("SUPER MONKEY POST CELEBRATION DIET (c) Kat Purpy, 2022", ref showCredits, ImGuiWindowFlags.HorizontalScrollbar))
                {
                    int i = 0;
                    foreach (var line in creditLines)
                    {
                        HsvToRgb((PlayerProfile.Data.StatTotalTime * 100 + (i--*20) )% 360, 1, 1, out var r, out var g, out var b);
                        ImGui.TextColored(new System.Numerics.Vector4(r, g, b, 255) / 255, line);
                    }
                    ImGui.End();
                }
            }
        }

        void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        public bool showCredits = false;
        static string[] creditLines = File.ReadAllLines("CREDITS.TXT");
        bool BonusButton(ref int targetval, int[] prices, Thing thing, string name, string description, string howtouse = null)
        {
            ImGui.PushID(name);
            var pointer = game.thingsToBuyImages[(int)thing];
            bool click = ImGui.ImageButton(pointer == IntPtr.Zero ? placeholder : pointer,new(64,64));
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextColored(new(1,1,0,1), name + $" ({targetval}/{prices.Length})");
                ImGui.BulletText(description);
                if (howtouse != null) ImGui.BulletText(howtouse);
                if (prices.Length != targetval)
                {
                    ImGui.BulletText("Price: " + prices[targetval] + " currencies");
                }
                else
                {
                    ImGui.BulletText("Price: MAXED OUT");
                }
                    ImGui.EndTooltip();
            }

            if (click) { 
                if(targetval < prices.Length && PlayerProfile.Data.Coins >= prices[targetval])
            {
                    game.Assets.stonks.Play();
                    PlayerProfile.Data.Coins -= prices[targetval];
                    PlayerProfile.Data.MoneySpent += prices[targetval];
                    targetval++;
                }
                else
                {
                    game.Assets.stonknt.Play();
                } 
            }
            ImGui.PopID();
            return click;
        }
    }
}

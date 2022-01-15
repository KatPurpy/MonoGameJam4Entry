using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
            20,
            30
        };

        readonly int[] JumpUpgradeCosts = new[]
        {
            5,
            20
        };

        readonly int[] UndeadalityFactor = new[]
        {
            10,
        };

        readonly int[] HUH = new[]
        {
            50
        };

        readonly int[] BadBananaPrices = new[] { 5, 5, 5, 5, 5 };
        readonly int[] DashPrices = new[] { 2, 2, 2 };
        readonly int[] ExtraLivePrices = new[] { 10,10,10 };
        IntPtr placeholder;

        enum Thing
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

        IntPtr[] thingsToBuyImages = new IntPtr[(int)Thing.Length];
        public override void Start()
        {
            PlayerProfile.Save();
            game.RenderOffset = new(0, 0);
            placeholder = game.ImGuiRenderer.BindTexture(game.PixelTexture);
            game.Assets.gym.Play();
            
            thingsToBuyImages[(int)Thing.Dash] = game.ImGuiRenderer.BindTexture(game.Assets.DASH);
            thingsToBuyImages[(int)Thing.BadBanana] = game.ImGuiRenderer.BindTexture(game.Assets.BADBANANA);
            thingsToBuyImages[(int)Thing.ExtraLife] = game.ImGuiRenderer.BindTexture(game.Assets.EXTRALIFE);
            thingsToBuyImages[(int)Thing.MoneyBrrr] = game.ImGuiRenderer.BindTexture(game.Assets.MONEYPRINTER);

            thingsToBuyImages[(int)Thing.Stronkth] = game.ImGuiRenderer.BindTexture(game.Assets.STRONKTH);
            thingsToBuyImages[(int)Thing.Weight] = game.ImGuiRenderer.BindTexture(game.Assets.WEIGHT);
            thingsToBuyImages[(int)Thing.Undeadality] = game.ImGuiRenderer.BindTexture(game.Assets.UNDEADALITY);
        
        }

        public override void Update(GameTime time)
        {

            //Origin = new(400, 300);
          // Rotation = (float)time.TotalGameTime.TotalSeconds;
            //throw new NotImplementedException();
        }
        
        public override void IMGUI(GameTime time)
        {
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

            ImGui.Begin("FUNNY WINDOW",ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse);
            if (ImGui.Button("BEGIN"))
            {
                Dead = true;
                game.EntityManager.AddEntity(new Entity_RunStarter(game));
            }
            ImGui.End();
        }

        bool BonusButton(ref int targetval, int[] prices, Thing thing, string name, string description, string howtouse = null)
        {
            ImGui.PushID(name);
            var pointer = thingsToBuyImages[(int)thing];
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

            if (click && targetval < prices.Length && PlayerProfile.Data.Coins >= prices[targetval])
            {
                PlayerProfile.Data.Coins -= prices[targetval];
                PlayerProfile.Data.MoneySpent += prices[targetval];
                targetval++;
            }
            ImGui.PopID();
            return click;
        }
    }
}

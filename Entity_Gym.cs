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
            Sprite = m.Assets.placeholder;
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
            15
        };

        readonly int[] UndeadalityFactor = new[]
        {
            10,
            20
        };

        readonly int[] HUH = new[]
        {
            50
        };

        readonly int[] BadBananaPrices = new[] { 5, 5, 5 };
        readonly int[] DashPrices = new[] { 2, 2, 2 };
        readonly int[] ExtraLivePrices = new[] { 20,20,20 };
        IntPtr placeholder;
        public override void Start()
        {
            game.RenderOffset = new(0, 0);
            placeholder = game.ImGuiRenderer.BindTexture(game.PixelTexture);
            game.Assets.gym.Play();
        }

        public override void Update(GameTime time)
        {
            //throw new NotImplementedException();
        }
        
        public override void IMGUI(GameTime time)
        {
            ImGui.Begin("WORKOUT, PHAT BOI",ImGuiWindowFlags.AlwaysAutoResize);
            BonusButton(ref PlayerProfile.Data.Stronkth, MoveSpeedUpgradeCosts, game.PixelTexture, $"STRONKTH", "Control your fat body better and pwn that wind mechanic.");
            ImGui.SameLine();
            BonusButton(ref PlayerProfile.Data.Weight, JumpUpgradeCosts, game.PixelTexture, $"PHAT REDUCE", "Str0nkth p0wered legs 4 h0pp1n l1k3 a bunny.");
            ImGui.SameLine();
            BonusButton(ref PlayerProfile.Data.Undeadality, UndeadalityFactor, game.PixelTexture, $"UNDEADALITY FACTOR", "20%% chance of magically resurrecting. tip: gud git.");
            ImGui.End();

            ImGui.Begin("LEGAL DOPPINGS");
            
            ImGui.Text("Temporary");
            BonusButton(ref PlayerProfile.Data.Dashes, DashPrices, game.PixelTexture, "Dash", "Save your sorry ass (donkey) if your fortune cheats with you");
            ImGui.SameLine();
            BonusButton(ref PlayerProfile.Data.BadBananas, BadBananaPrices, game.PixelTexture, "BAD BANANA", "Save your sorry ass (donkey) by releasing your gases out of fear, maybe it will work");
            ImGui.SameLine();
            BonusButton(ref PlayerProfile.Data.ExtraLives, ExtraLivePrices, game.PixelTexture, "Extra life (hell expensive, git gud)", "y o u l i v e o n c e");
            ImGui.Text("Permament");
            BonusButton(ref PlayerProfile.Data.CurrencyPrinter, HUH,game.PixelTexture, "Money printer", "Pinocchio lost his money trying to grow a money tree for you noobs so you can get TWICE AS MUCH currency.");
            ImGui.End();


            ImGui.Begin("funny window",ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse);
            ImGui.Text("Last run");
            ImGui.Text("Time: 00:04");
            ImGui.Text("Cause of death: workout, damn it, you are too fat for that level");
            if (ImGui.Button("BEGIN"))
            {
                Dead = true;
                game.EntityManager.AddEntity(new Entity_RunStarter(game));
            }
            ImGui.End();
        }

        bool BonusButton(ref int targetval, int[] prices, Texture2D texture, string name, string description)
        {
            ImGui.PushID(name);
            bool click = ImGui.ImageButton(placeholder,new(64,64));
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextColored(new(1,1,0,1), name + $" ({targetval}/{prices.Length})");
                ImGui.BulletText(description);
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

            if (click && targetval < prices.Length)
            {
                
                targetval++;
            }
            ImGui.PopID();
            return click;
        }
    }
}

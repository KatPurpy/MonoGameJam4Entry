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
            13,
            30
        };

        readonly int[] JumpUpgradeCosts = new[]
        {
            10,
            30
        };

        readonly int[] UndeadalityFactor = new[]
        {
            10,
            20
        };

        public override void Start()
        {
            game.Assets.gym.Play();
        }

        public override void Update(GameTime time)
        {
            //throw new NotImplementedException();
        }
        
        public override void IMGUI(GameTime time)
        {
            ImGui.Begin("WORKOUT, PHAT BOI",ImGuiWindowFlags.AlwaysAutoResize);
            BonusButton(game.PixelTexture,"STRONKTH (0/2)","Control your fat body better",50);
            ImGui.SameLine();
            BonusButton(game.PixelTexture,"PHAT REDUCE (0/2)","Higher jumps",100);
            ImGui.SameLine();
            BonusButton(game.PixelTexture,"UNDEADALITY FACTOR (0/2)","20%% chance of magically resurrecting. tip: gud git", 50);
            ImGui.End();

            ImGui.Begin("LEGAL DOPPINGS");
            
            ImGui.Text("Temporary");
            BonusButton(game.PixelTexture, "Dash", "Save your sorry ass (donkey) if your fortune cheats with you", 50);
            ImGui.SameLine();
            BonusButton(game.PixelTexture, "BAD BANANA", "Save your sorry ass (donkey) by releasing your gases out of fear, maybe it will work", 50);
            ImGui.SameLine();
            BonusButton(game.PixelTexture, "Extra life (hell expensive, git gud)", "y o u l i v e o n c e", 300);
            ImGui.Text("Permament");
            ImGui.Button("Portal to Mountain");
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

        bool BonusButton(Texture2D texture, string name, string description, int price)
        {
            bool click = ImGui.ImageButton(game.ImGuiRenderer.BindTexture(texture),new(64,64));
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextColored(new(1,1,0,1), name);
                ImGui.BulletText(description);
                ImGui.BulletText("Price: " + price + " currencies");
                ImGui.EndTooltip();
            }
            return click;
        }
    }
}

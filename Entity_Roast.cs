using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_Roast : Entity
    {
        public string[] EdgyTeenagerQuotes = new[]
        {
            "Whoops! You have to put the CDetermination in your computer.",
            "Kat Purpy saw that.",
            "Work out, dummy.",
            "It's no offence but actually I am going to say something offensive: noob.",
            "Where did you learn to jump? The answer doesn't matter as they had failed or had purposefully trained you incorrectly. Tell them thanks from me anyway.",
            "The fact you live makes you richer than dead. Rich of humiliation.",
            "YOU ARE DEAD DEAD DEAD\nYOU ARE DEAD DEAD DEAD\nTHOUGHT YOU WERE HOT, GUESS WHAT YOUR NOT",
            "Don't fake your feelings, dear. I need to make sure my sadistic game design is working.",
            "Hello, I am you from the past. I want to tell you one thing: git gud damn it!",
            "Tip: try pressing \"BEGIN\" button harder next time.",
            "Have you tried like, focusing?",
            "STOP BLAMING MY LEVEL GENERATOR IT'S YOUR SKILL BAD BAD BAD BAD BAD",
            "I'm going to say the N-word! NNNNNICE TRY!",
        };
        public string[] OneWordInsults = new[]
        {
            "Pathetic",
            "!Awesome",
            "Gone",
            "How does it feel to be dead?",
            "Death",
            "Mortis",
            "GAME OVER"
        };
        string title = null;
        string roast = null;
        public Entity_Roast(Main m) : base(m)
        {
            Sprite = m.PixelTexture;
            if (!Main.Complete)
            {
                title = OneWordInsults[Main.Random.Next(0, OneWordInsults.Length)];
                roast = EdgyTeenagerQuotes[Main.Random.Next(0, EdgyTeenagerQuotes.Length)];
            }
            else
            {
                title = "Great job!";
                roast = "No, seriously, you did great! Monke asked me to send you his regards.";
                game.Assets.ending.Play();
            }
        }

        public override void Update(GameTime time)
        {
        }
        public override void IMGUI(GameTime time)
        {
            Main.BeginFixedWindow(title, 300, 200);
            ImGui.TextWrapped(roast);
            ImGui.TextColored(new(1, 1, 0, 1), "Curr3nc135 r151ng: " + Entity_Player._.CoinsCollected);
            ImGui.TextColored(new(1, 1, 0, 1), "Time: " + Main.TimeToString(Entity_Player._.Time));
            ImGui.TextColored(new(1, 1, 0, 1), "G0al c0mplet1t10n: " + (Main.Progress).ToString("F2")  + "%%");
            if (ImGui.Button(Main.Complete ? "ACCEPT VICTORY" : "ACCEPT FAILURE"))
            {
                game.EntityManager.Clear();
                if (!Main.Complete)
                {
                    game.EntityManager.AddEntity(new Entity_Gym(game));
                }
            }
            ImGui.End();
        }
    }
}

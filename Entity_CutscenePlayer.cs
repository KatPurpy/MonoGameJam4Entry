using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_CutscenePlayer : Entity
    {
        Texture2D[] Frames;
        Dictionary<int, Action> Actions;
        Action Finished;
        float readingTime = 2;
        int Current = 0;
        int blinkFrame = 0;
        bool skipLastFrame;
        public Entity_CutscenePlayer(Main m, Texture2D[] frames, Dictionary<int,Action> actions, Action finished) : base(m)
        {
            Frames = frames;
            Actions = actions;
            Finished = finished;
            Position = new(400, 300);
            Size = new(800, 600);

        }

        public override void Update(GameTime time)
        {

            bool skip = Keyboard.GetState().IsKeyDown(Keys.Space);


            readingTime -= (float)time.ElapsedGameTime.TotalSeconds;
            if (skip && !skipLastFrame)
            {
                this.readingTime = 2;
                Current++;
            }
            skipLastFrame = skip;

            if (Current == Frames.Length)
            {
                Dead = true;
                Finished();
            }
            Sprite = Frames[Current];
        }

        public override void IMGUI(GameTime time)
        {
            if (this.readingTime < 0)
            {
                ImGui.GetForegroundDrawList().AddText(
                    Main._.FontPTR, 30,
                    new(0, 550), blinkFrame++ % 60 > 30 ? 0xFF00FFFF : 0xFF0000FF,
                    "Press SPACE but ANY KEY to continue..."
                    );
            }
        }
    }
}

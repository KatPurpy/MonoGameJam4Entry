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
            game.RenderOffset.Y = 0;
        }

        public override void Update(GameTime time)
        {

            bool skip = Keyboard.GetState().IsKeyDown(Keys.Space);


            readingTime -= (float)time.ElapsedGameTime.TotalSeconds;
            if (readingTime < 0 && skip && !skipLastFrame)
            {
                this.readingTime = 2;
                Current++;
                if(Actions.TryGetValue(Current, out var val))
                {
                    val();
                }
            }
            skipLastFrame = skip;

            if (Current == Frames.Length)
            {
                Dead = true;
                Finished();
            }
            else
            {
                Sprite = Frames[Current];
            }
        }

        public static void PlayIntro(Main game)
        {
            game.Assets.intro.Play();
            game.EntityManager.AddEntity(new Entity_CutscenePlayer(game,

    new Texture2D[]
    {
                    game.Assets.INTRO0000,
                    game.Assets.INTRO0001,
                    game.Assets.INTRO0002,
                    game.Assets.INTRO0003,
                    game.Assets.INTRO0004,
                    game.Assets.INTRO0005,
                    game.Assets.INTRO0006,
                    game.Assets.INTRO0007,
                    game.Assets.INTRO0008,
                    game.Assets.INTRO0009,
                    game.Assets.INTRO0010,
    },
    new Dictionary<int, Action>()
    {
                        {4, ()=> game.Assets.cutscene_intro_yum1.Play() },
                        {5, ()=> game.Assets.cutscene_intro_yum2.Play() }
    },
    () =>
    {
        PlayerProfile.Save();
        game.EntityManager.AddEntity(new Entity_Gym(game));
    }
    ));
        }

        public static void PlayOutro(Main game)
        {
            game.EntityManager.AddEntity(new Entity_CutscenePlayer(game,

new Texture2D[]
{
                    game.Assets.OUTRO0000,
                    game.Assets.OUTRO0001,
                    game.Assets.OUTRO0002,
                    game.Assets.OUTRO0003,
                    game.Assets.OUTRO0004,
                    game.Assets.OUTRO0005,
                    game.Assets.OUTRO0006,
                    game.Assets.OUTRO0007,
},
new Dictionary<int, Action>()
{
                        {7, ()=>
                        {
                            game.Assets.ending.Pause();
                            game.Assets.hahahahaha.Play();
                        }  },

},
() =>
{
    game.Assets.ending.Play();
    game.EntityManager.AddEntity(new Entity_Gym(game)
    {
        showCredits = true
    });
}
));
        }


        public override void IMGUI(GameTime time)
        {
            if (this.readingTime < 0)
            {
                ImGui.GetForegroundDrawList().AddText(
                    Main._.FontPTR, 30,
                    new(0, 550), blinkFrame++ % 60 > 30 ? 0xFF00FFFF : 0xFF0000FF,
                    "Press space but ANY KEY to continue..."
                    );
            }
        }
    }
}

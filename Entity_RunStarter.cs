
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_RunStarter : Entity
    {
        float StartTimer = 4;
        Entity_RunController runController;
        public Entity_RunStarter(Main m) : base(m)
        {
        }

        public override void Start()
        {
            PrepareGame();
        }

        Random r = new();
        public override void Draw(GameTime time)
        {
            StartTimer -= (float)time.ElapsedGameTime.TotalSeconds;
            var timer = Math.Truncate(StartTimer);
            string txt = timer.ToString();
            float shakeCoff = (int)((3-timer+1f)*5);
            if (timer == 0)
            {
                txt = "GO!";
               // shakeCoff = 20;
            }

                ImGui.GetForegroundDrawList().AddText(game.FontPTR,150,new System.Numerics.Vector2((float)r.NextDouble(), (float)r.NextDouble()) * shakeCoff + 
                    new System.Numerics.Vector2(400-20, 300-80), 0xFFFFFFFF,txt);
            
            if(StartTimer < 0.1f)
            {
                ActivateGame();

                Dead = true;
            }
        }

        public override void Update(GameTime time)
        {

        }

        Entity monke, platformSpawner;

        public void PrepareGame()
        {
            game.EntityManager.AddEntity(new Entity_Background(game, game.Assets.BG1)
            {
                Type = Entity_Background.BGType.Mountain
            });
            game.EntityManager.AddEntity(new Entity_Background(game, game.Assets.BG2)
            {
                Type = Entity_Background.BGType.Sky,
                Position = new(400, (-6000 / 2 + 600) - 6000)
            });
            game.EntityManager.AddEntity(new Entity_Background(game, game.Assets.BG3)
            {
                Type = Entity_Background.BGType.Space,
                Position = new(400, (-6000 / 2 + 600) - 6000 * 2)
            });

            game.EntityManager.AddEntity(new Entity_Background(game, game.Assets.BG4)
            {
                Type = Entity_Background.BGType.Space_FrikYou,
                Position = new(400, (-6000 / 2 + 600) - 6000 * 3)
            });

            game.EntityManager.AddEntity(new Entity_Platform(game)
            {
                Type = Entity_Platform.PlatformType.Still,
                Position = new Vector2(400, 350),
                Size = new(100, 20)
            }
);

            game.EntityManager.AddEntity(monke = new Entity_Player(game) { Activated = false, Position = new(400, 300) });
            game.EntityManager.AddEntity(platformSpawner = new Entity_PlatformSpawner(game)
            { Activated = false }
                );
        }

        public void ActivateGame()
        {
            monke.Activated = platformSpawner.Activated = true;
            Dead = true;
            game.EntityManager.AddEntity(new Entity_RunController(game,monke as Entity_Player));
        }

    }
}

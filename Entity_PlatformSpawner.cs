using DSastR.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonoGameJam4Entry.Entity_Platform;

namespace MonoGameJam4Entry
{
    class Entity_PlatformSpawner : Entity
    {
        public enum SpawnMode
        {
            MountainClimb,
            MountainClimbHarder,
            Clouds,
            CloudsHarder,
            Space,
            SpaceHarder
        }

        public Entity_PlatformSpawner(Main m) : base(m)
        {
            Position = new(400, 0);
            Size = new Vector2(0, 0);
            Sprite = m.PixelTexture;
            r = Main.Random;
        }
        Random r;
        public override void Start()
        {
            GenerateSafeScreen();
        }
        public override void Update(GameTime time)
        {
            Generator_Level1(time);
        }
        float screensize = 600/6;
        int lvl1_ghostplatformcount = 0;
        bool lvl1_easymode = true;
        const int lvl1_easymodethreshold = 4 * 3;
        int lvl1_easymodecount = 0;
        void Generator_Level1(GameTime time)
        {
            
            float y = 0;
            if (Entity_RunController.LengthAccumulator > screensize)
            {
                
                lvl1_easymodecount++;
                if (lvl1_easymodecount > lvl1_easymodethreshold)
                {
                    lvl1_easymode = false;
                }
                lvl1_ghostplatformcount++;
                if (lvl1_easymode)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GeneratePlatform(PlatformType.Still,
        new((float)((r.NextDouble()) * 800), 0 - ((float)r.NextDouble() * 150)));
                    }
                }
                for (int i = 0; i < 4; i++)
                {


                    GeneratePlatform(lvl1_ghostplatformcount%7==0 ? PlatformType.Ghost : PlatformType.Still,
    new((float)((r.NextDouble()) * 800), 0 + (y-=(float)r.NextDouble() * 50)));
                }
                Entity_RunController.LengthAccumulator -= screensize;
            }
        }

        void Generator_Level2(GameTime time)
        {
            if (time.TotalGameTime.Milliseconds % 500 == 0)
            {
                GeneratePlatform((PlatformType)r.Next(0, (int)PlatformType.FatSensitive),
                    new((float)((r.NextDouble()) * 800), 0));
            }
        }

        void GenerateSafeScreen()
        {
            float pos = 400;
            float x = 400;
            for (int i = 0; i < 20; i++)
            {
                if (i % 4 == 0) x = 400 + (float)(r.NextDouble() - 0.5f) * 300;

                    float apped = (float)(r.NextDouble()-0.5f) * 300;
                    x += apped;
                x %= 800;
                    GeneratePlatform(PlatformType.Still,
                    new Vector2(x,
                    (pos -= (float)r.NextDouble() * 55)));
            }
        }
        void GeneratePlatform(PlatformType type, Vector2 screenPos, Vector2? size = null)
        {
            game.EntityManager.AddEntity(new Entity_Platform(game)
            {
                Type = type,
                Position = new Vector2(screenPos.X, -game.RenderOffset.Y + screenPos.Y),
                Size = size ?? new(100, 20)
            }
); ;
        }
    }
}

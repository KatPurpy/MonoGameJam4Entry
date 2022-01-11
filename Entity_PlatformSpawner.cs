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
            Position = new(1000, 0);
            Size = new Vector2(0, 0);
            Sprite = m.PixelTexture;
        }
        Random r = new Random();
        public override void Update(GameTime time)
        {
            if (time.TotalGameTime.Milliseconds % 600 == 0)
            {
                game.EntityManager.AddEntity(new Entity_Platform(game)
                {
                    Type = (PlatformType)r.Next(0, 2),
                    Position = new Vector2((float)((r.NextDouble()) * 800), -game.RenderOffset.Y),
                    Size = new(100, 20)
                }
    ); ;
            }
        }
    }
}

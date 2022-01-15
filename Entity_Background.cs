using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    class Entity_Background : Entity
    {
        public enum BGType
        {
            None,
            Mountain,
            Sky,
            Space,
            Space_FuckYou
        }

        public BGType Type;
        public static BGType CurrentBGType;



        public Entity_Background(Main m, Texture2D bg) : base(m)
        {
            LayerDepth = 0;
            Sprite = bg;
            Position = new(400, -6000/2 + 600);
            Size = new(800,6000);

        }

        public override void Update(GameTime time)
        {
           // SourceRect = new Rectangle((int)HorizontalBGScroll, 0, 800, 6000);
            if (Type > CurrentBGType && CollidesWith<Entity_Player>())
            {
                CurrentBGType = Type;
                Console.WriteLine("CURRENT LVL IS " + Type);
                switch (Type) {
                    case BGType.Mountain:
                        game.Assets.mountain.Play();
                        Entity_PlatformSpawner.SetSpawnMode(Entity_PlatformSpawner.SpawnMode.Mountain);
                        break;
                    case BGType.Sky:
                        game.Assets.sky.Play();
                        Entity_PlatformSpawner.SetSpawnMode(Entity_PlatformSpawner.SpawnMode.Sky);
                        break;
                    case BGType.Space:
                        game.Assets.space.Play();
                        Entity_PlatformSpawner.SetSpawnMode(Entity_PlatformSpawner.SpawnMode.Space);
                        break;
                    case BGType.Space_FuckYou:
                        game.Assets.space.Play();
                        Entity_PlatformSpawner.SetSpawnMode(Entity_PlatformSpawner.SpawnMode.Space_FuckYou);
                        break;
                } }
        }
    }
}

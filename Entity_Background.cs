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
            Space
        }

        public BGType Type;
        public static BGType CurrentBGType;

        public static float HorizontalBGScroll;

        public Entity_Background(Main m, Texture2D bg) : base(m)
        {
            LayerDepth = 0;
            Sprite = bg;
            Position = new(400, -6000/2 + 600);
            Size = new(800,6000);
            HorizontalBGScroll = 0;
        }

        public override void Update(GameTime time)
        {
            SourceRect = new Rectangle((int)HorizontalBGScroll, 0, 800, 6000);
            if (Type > CurrentBGType && CollidesWith<Entity_Player>())
            {
                CurrentBGType = Type;
                Console.WriteLine("CURRENT LVL IS " + Type);
                switch (Type) {
                    case BGType.Mountain:
                        game.Assets.mountain.Play();
                        break;
                    case BGType.Sky:
                        game.Assets.space.Play();
                        break;
                } }
        }
    }
}

using DSastR.Core;
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
        static BGType CurrentBG;
        public Entity_Background(Main m, Texture2D bg) : base(m)
        {
            LayerDepth = 0;
            Sprite = bg;
            Position = new(400, -6000/2 + 600);
            Size = new(800,6000);
        }

        public override void Update(GameTime time)
        {
            if (Type > CurrentBG && CollidesWith<Entity_Player>())
            {
                CurrentBG = Type;
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

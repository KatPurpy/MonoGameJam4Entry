
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    class Entity_PlayerDeathExplosion : Entity
    {
        public Entity_PlayerDeathExplosion(Main m, Vector2 position) : base(m)
        {
            Position = position - new Vector2(0,128+32);
            Sprite = m.Assets.DEATH;
            SourceRect = new(0, 0, 64, 64);
            Size = new(512,512);
            LayerDepth = 1;
        }
        public override void Start()
        {
            game.Assets.mountain.Pause();
            game.Assets.RandomDeath.Play();
          //  game.Assets.sky.Dispose();
            game.Assets.space.Pause();
        }
        int frame = 0;
        public override void Update(GameTime time)
        {
            frame++;
            if (frame % 4 == 0)
            {
                Rectangle rect = SourceRect.Value;
                rect.Y += 5* 64;
                SourceRect = rect;
            }
            if (frame > 186-4) Size = new(0, 0);
            if (frame == 186-4)
            {
                Dead = true;
                game.EntityManager.AddEntity(new Entity_Roast(game));
            }
        }
    }
}

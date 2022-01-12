
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    class Entity_Coin : Entity
    {
        public Entity_Coin(Main m) : base(m)
        {
            Sprite = m.PixelTexture;
            Size = new(64, 64);
            LayerDepth = 0.6f;
        }

        public override void Update(GameTime time)
        {
            
        }
    }
}

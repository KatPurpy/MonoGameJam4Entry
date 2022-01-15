
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
            Sprite = m.Assets.COIN;
            Size = new(64, 64);
            LayerDepth = 0.6f;
            SourceRect = new(0, 0, 64, 64);
        }
        int frame = 0;
        public override void Update(GameTime time)
        {
            SourceRect = new Rectangle(0, frame++ / 4 % 21 * 64, 64, 64);
        }
    }
}

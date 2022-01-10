using DSastR.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_Platform : Entity
    {
        public Entity_Platform(Main m) : base(m) { Sprite = m.PixelTexture; }

  
        public override void Update(GameTime time)
        {
        }
    }
}

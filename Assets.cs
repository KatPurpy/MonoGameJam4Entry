using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Assets
    {
        public Texture2D PLACEHOLDER;
        public Assets (Main m)
        {
            PLACEHOLDER = Main.LoadTexture("placeholder.png");
        }
    }
}

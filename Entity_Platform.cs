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
        public PlatformType Type = PlatformType.FatSensitive;

        public enum PlatformType
        {
            Still,
            FatSensitive,
            Fake,
            Breakable,
            Dragging
        }
        public Entity_Platform(Main m) : base(m) { Sprite = m.PixelTexture; }

        Dictionary<PlatformType, Color> colorMap = new()
        {
            { PlatformType.Still, Color.Gray},
            { PlatformType.FatSensitive,Color.Yellow}
        };

        public void HandlePlayer(float deltatime, Entity_Player player)
        {
            switch (Type)
            {
                case PlatformType.Still:
                    if(CollisionBox.Bottom > player.CollisionBox.Bottom)
                    player.FatPlatformBlock = true;
                    break;
                case PlatformType.FatSensitive:
                    if (player.FatPlatformBlock == false)
                    {
                        player.Position.Y += 200 * deltatime;
                        player.FatPlatformBlock = true;
                    }
                    Position.Y += 200 * deltatime;
                    break;
            }
        }
  
        public override void Update(GameTime time)
        {
            Color = colorMap[Type];
            if (CollisionBox.Bottom > 800) Dead = true; 
        }
    }
}

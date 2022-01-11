using DSastR.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_Platform : Entity
    {
        public PlatformType Type = PlatformType.FatSensitive;

        public enum PlatformType
        {
            Still,
            Ghost,
            Explosive,
            FatSensitive,
            Dragging
        }

        public float? ExplosionTimer;
        const float explosiontimeout = 3;

        public Entity_Platform(Main m) : base(m) { Sprite = m.PixelTexture; }

        Dictionary<PlatformType, Color> colorMap = new()
        {
            { PlatformType.Still, Color.Gray},
            { PlatformType.Ghost, new Color(50,50,50,255/4*3)},
            { PlatformType.Explosive, new Color(128,0,0,255)},
            { PlatformType.FatSensitive,Color.Yellow},
        };

        public void HandlePlayer(float deltatime, Entity_Player player)
        {
            switch (Type)
            {
                case PlatformType.Still:
                    if(CollisionBox.Bottom > player.CollisionBox.Bottom)
                    player.FatPlatformBlock = true;
                    break;
                case PlatformType.Ghost:
                    player.Velocity.Y = -700;
                    Dead = true;
                    break;
                case PlatformType.Explosive:
                    if(ExplosionTimer == null) ExplosionTimer = explosiontimeout;
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
        int counter = 0;
        public override void Update(GameTime time)
        {

            Color = colorMap[Type];

            if (ExplosionTimer != null)
            {
                ExplosionTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                Console.WriteLine(ExplosionTimer);
                Color = Color.Lerp(Color.Red, Color.Yellow, 
                    (float)(Math.Sin(time.TotalGameTime.TotalSeconds * 2 * (1+1-ExplosionTimer.Value/explosiontimeout) ) / 2 + 0.5 ));
                if(ExplosionTimer <= 0)
                {
                    Dead = true;
                }
            }

            if (CollisionBox.Top > 600) Dead = true; 
        }
    }
}

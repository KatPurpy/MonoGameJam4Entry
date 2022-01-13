
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
        public PlatformType Type = PlatformType.Falling;

        public enum PlatformType
        {
            Still,
            Ghost,
            Explosive,
            Falling,
            EndOfList
        }

        public float? ExplosionTimer;
        const float explosiontimeout = 2;

        public Entity_Platform(Main m) : base(m) { Sprite = m.PixelTexture; }

        Dictionary<PlatformType, Color> colorMap = new()
        {
            { PlatformType.Still, Color.Gray},
            { PlatformType.Ghost, new Color(50,50,50,255/4*3)},
            { PlatformType.Explosive, new Color(128,0,0,255)},
            { PlatformType.Falling,Color.Yellow},
        };

        public override void Start()
        {
            if(Main.Random.Next(0,99) < 5)
            game.EntityManager.AddEntity(new Entity_Coin(game)
            {
                Position = new(Position.X, Position.Y - 32 - 16 - 8)
            });

            if (AllowFakes && Main.Random.Next(0, 99) < 20) FAKE = true;
        }
        public static bool AllowFakes = false;
        public bool FAKE = false;
        bool FALL = false;
        public void HandlePlayer(float deltatime, Entity_Player player)
        {
            if (FAKE)
            {
                return;
            }
            switch (Type)
            {
                case PlatformType.Still:
                    if(CollisionBox.Bottom > player.CollisionBox.Bottom)
                    player.FatPlatformBlock = true;
                    break;
                case PlatformType.Ghost:
                    player.Velocity.Y = -700;
                    game.Assets.platjump.Play();
                    Dead = true;
                    break;
                case PlatformType.Explosive:
                    if(ExplosionTimer == null) ExplosionTimer = explosiontimeout;
                    break;
                case PlatformType.Falling:
                    if (player.FatPlatformBlock == false)
                    {
                        player.Position.Y += 200 * deltatime;
                        player.FatPlatformBlock = true;
                    }
                    FALL = true;
                    break;
            }
        }
        int counter = 0;
        Vector2? FakePos;
        public override void Update(GameTime time)
        {
            if (FAKE && !FakePos.HasValue)
            {
                FakePos = Position;
            }

            if (FAKE)
            {
                Position = FakePos.Value + new Vector2(Main.Random.Next(-5, 5), Main.Random.Next(-5, 5));
            }

            if(FALL) Position.Y += 200 * (float)time.ElapsedGameTime.TotalSeconds;
            Color = colorMap[Type];

            if (ExplosionTimer != null)
            {
                ExplosionTimer -= (float)time.ElapsedGameTime.TotalSeconds;
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

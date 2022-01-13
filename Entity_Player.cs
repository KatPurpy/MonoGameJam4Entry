
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_Player : Entity
    {
        public float[] StronkthParams = new float[] {3,6,9 };
        public float[] JumpForceParams = new float[] {-375,-450,-550 };
        public float[] UndeadalityParams = new float[] {0,0.10f,0.25f };


        public bool FatPlatformBlock;
        public Vector2 Velocity;
        public static float Time = 0;
        public static int CoinsCollected = 0;

        public static float WindFactor = 0;
        SoundEffectInstance blizzardSound;
        public Entity_Player(Main m) : base(m)
        {
            Entity_Background.CurrentBGType = Entity_Background.BGType.None;
            Entity_PlatformSpawner.SetSpawnMode( Entity_PlatformSpawner.SpawnMode.Mountain);
            Time = 0;
            CoinsCollected = 0;
            WindFactor = 0;
            Position = new(400, 300);
            Size = new(100, 100);
            Sprite = m.Assets.placeholder;
            blizzardSound = game.Assets.blizzard.CreateInstance();
            blizzardSound.IsLooped = true;
            blizzardSound.Play();
        }
        bool Grounded
        {
            get
            {
                if (!(Velocity.Y >= 0)) return false;
                if (CollidesWith<Entity_Platform>(out var plats))
                {
                    foreach(var p in plats)
                    {
                        if (p.FAKE) continue;
                        if (p.CollisionBox.Bottom > CollisionBox.Bottom)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        static bool groundedLastFrame = false;
        float MaxSpeed = 800;
        public override void Update(GameTime time)
        {
            float deltatime = (float)time.ElapsedGameTime.TotalSeconds;
            Time += deltatime;
            Velocity += Vector2.UnitY * 9.8f * 100 * deltatime;
            if (Grounded)
            {
                Velocity.Y = 0;
                Position.Y -= Velocity.Y * deltatime;
            }

            KeyboardState kbstate = Keyboard.GetState();


            bool left = kbstate.IsKeyDown(Keys.Left);
            bool right = kbstate.IsKeyDown(Keys.Right);
            bool up = kbstate.IsKeyDown(Keys.Up);


            bool fullControl = PlayerProfile.Data.Stronkth == 3;
            if (fullControl)
            {
                MaxSpeed = 666;
                if (left)
                {
                    Velocity.X = -MaxSpeed;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                }

                if (right)
                {
                    Velocity.X = MaxSpeed;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                }

                if (!left && !right)
                {
                    Velocity.X = 0;
                }
            }
            else
            {
                if (left)
                {
                    Velocity.X += -StronkthParams[PlayerProfile.Data.Stronkth] * 100 * deltatime;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                }

                if (right)
                {
                    Velocity.X += StronkthParams[PlayerProfile.Data.Stronkth] * 100 * deltatime;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                }
            }
            if (up && Grounded && groundedLastFrame)
            {
                Velocity.Y = JumpForceParams[PlayerProfile.Data.Weight];
            }
            groundedLastFrame = Grounded;


            if (CollisionBox.Top > 600)
            {
                if (PlayerProfile.Data.ExtraLives > 0)
                {
                    PlayerProfile.Data.ExtraLives--;
                    Position.Y = -game.RenderOffset.Y + 600;
                    Velocity.Y = JumpForceParams[2]*2.5f;
                }
                else
                {
                    Die();
                }
            }

            if (Grounded && CollidesWith<Entity_Platform>(out var plat) && Velocity.Y >= 0)
            {
                plat.Sort((x, y) => x.Type - y.Type);
                FatPlatformBlock = false;
                    foreach(var p in plat) p.HandlePlayer(deltatime,this);
            }

            if (CollidesWith<Entity_Coin>(out var coins))
            {
                foreach (var c in coins)
                {
                    CoinsCollected+=PlayerProfile.Data.CurrencyPrinter == 1 ? 2 : 1;
                    c.Dead = true;
                    game.Assets.coin.Play(0.25f,(float)(Main.Random.NextDouble()/2),0);
                }
            }

            Velocity.X = Math.Max(-MaxSpeed, Math.Min(MaxSpeed, Velocity.X));
            if (fullControl)
            {
                Position.X -= deltatime * 50 * WindFactor;
            }
            else
            {
                Velocity.X -= StronkthParams[PlayerProfile.Data.Stronkth] * WindFactor * deltatime * 50;
            }
            Position += Velocity * deltatime;
            if (Position.X < 0) Position.X = 800;
            else if (Position.X > 800) Position.X = 0;

            Entity_Background.HorizontalBGScroll += Velocity.X / 8 * deltatime ;
        }

        public override void IMGUI(GameTime time)
        {
            ImGui.Text(Time.ToString());
            ImGui.Text("CoinsCollected: " + CoinsCollected);
            ImGui.Text("AAA " + WindFactor);
        }
        float blizzardX = 0;
        public override void Draw(GameTime time)
        {
            base.Draw(time);
            float deltatime = (float)time.ElapsedGameTime.TotalSeconds;
            blizzardX += 4 * WindFactor * deltatime * 40;
            blizzardSound.Volume = Math.Abs(WindFactor) * 1;
            game.SpriteBatch.Draw(game.Assets.BLIZZARD, new Rectangle(0, 0, 800, 600), 
                new Rectangle((int)blizzardX, 0, 800, 600), Color.White * 0.3f * (Math.Abs(WindFactor)), 0,Vector2.Zero,Microsoft.Xna.Framework.Graphics.SpriteEffects.None,1);
        }

        public void Die()
        {
            game.EntityManager.AddEntity(new Entity_PlayerDeathExplosion(game, Position));
            Dead = true;
            blizzardSound.Dispose();
            //game.Exit();
        }
    }
}


using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
        public static Entity_Player _;
        public float[] StronkthParams = new float[] {3,6,9 };
        public float[] JumpForceParams = new float[] {-375,-450,-550 };
        public float[] UndeadalityParams = new float[] {0,0.10f,0.25f };


        public bool FatPlatformBlock;
        public Vector2 Velocity;
        public float Time = 0;
        public int CoinsCollected = 0;

        public float WindFactor = 0;
        SoundEffectInstance blizzardSound;
        public Entity_Player(Main m) : base(m)
        {
            _ = this;
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
        bool downLastFrame = false;
        bool shiftLastFrame = false;
        public override void Update(GameTime time)
        {
            Color = Color.White;
            invulnurabilityFrames--;
            if (invulnurabilityFrames > 0) Color = invulnurabilityFrames % 4 > 2 ? Color.White : Color.Red * 0.25f;
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
            bool down = kbstate.IsKeyDown(Keys.Down);
            bool shift = kbstate.IsKeyDown(Keys.Space);

            if(shift && !shiftLastFrame && PlayerProfile.Data.Dashes > 0)
            {
                PlayerProfile.Data.Dashes--;
                Velocity.X = Effects.HasFlag(SpriteEffects.FlipHorizontally) ? -MaxSpeed : MaxSpeed;
            }
            shiftLastFrame = shift;

            if(down && !downLastFrame && PlayerProfile.Data.BadBananas > 0)
            {
                PlayerProfile.Data.BadBananas--;
                Velocity.Y = -866;
                game.Assets.badbanana.Play();
            }

            downLastFrame = down;
            bool fullControl = PlayerProfile.Data.Stronkth == 3;
            if (fullControl)
            {
                MaxSpeed = 666;
                if (left)
                {
                    Velocity.X = -MaxSpeed;
                    Effects = SpriteEffects.FlipHorizontally;
                }

                if (right)
                {
                    Velocity.X = MaxSpeed;
                    Effects = SpriteEffects.None;
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
                    Effects = SpriteEffects.FlipHorizontally;
                }

                if (right)
                {
                    Velocity.X += StronkthParams[PlayerProfile.Data.Stronkth] * 100 * deltatime;
                    Effects = SpriteEffects.None;
                }
            }
            if (up && Grounded && groundedLastFrame)
            {
                Velocity.Y = JumpForceParams[PlayerProfile.Data.WeightLoss];
            }
            groundedLastFrame = Grounded;


            if (CollisionBox.Top > 600)
            {

                {
                    Position.Y = -game.RenderOffset.Y + 570;
                    Die(true);
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
                    CoinsCollected +=PlayerProfile.Data.CurrencyPrinter == 1 ? 2 : 1;
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

           // Entity_Background.HorizontalBGScroll += Velocity.X / 8 * deltatime ;
        }

        private void Resurrect()
        {

            if (CollisionBox.Top > 600)
            {
                Position.Y = -game.RenderOffset.Y + 600;
            }
            Velocity.Y = JumpForceParams[2] * 1.8f;
            invulnurabilityFrames = 180;
            game.Assets.resurrect.Play();
        }
        int flashtext = 0;
        public override void IMGUI(GameTime time)
        {
            Console.WriteLine(Main.Progress);
            if(Main.Progress >= 95)
            {
                ImGui.GetForegroundDrawList().AddText(Main._.FontPTR, 50, new(0, 
                    0),
                    flashtext++/4 % 4 < 2 ? 0xFFFF0000 : 0xFF00FF00, "YOU DID GOOD, HAVE REST, BUDDY!");
            }
       //     ImGui.Text(Time.ToString());
         //   ImGui.Text("CoinsCollected: " + CoinsCollected);
        //    ImGui.Text("AAA " + WindFactor);
        }
        float blizzardX = 0;
        public override void Draw(GameTime time)
        {
            base.Draw(time);
            float deltatime = (float)time.ElapsedGameTime.TotalSeconds;
            blizzardX += 4 * WindFactor * deltatime * 40;
            blizzardSound.Volume = Math.Abs(WindFactor) * 1;
            game.SpriteBatch.Draw(game.Assets.BLIZZARD, new Rectangle(0, 0, 800, 600), 
                new Rectangle((int)blizzardX, 0, 800, 600), Color.White * 0.3f * (Math.Abs(WindFactor)), 0,Vector2.Zero,SpriteEffects.None,1);
            for (int i = 0; i < PlayerProfile.Data.ExtraLives; i++)
                game.SpriteBatch.Draw(game.Assets.EXTRALIFE, new Rectangle(i*48,0,48,48),null,Color.White,0,Vector2.Zero,SpriteEffects.None,1);
            for (int i = 0; i < PlayerProfile.Data.BadBananas; i++)
                game.SpriteBatch.Draw(game.Assets.BADBANANA, new Rectangle(i * 48, 48, 48, 48), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            for (int i = 0; i < PlayerProfile.Data.Dashes; i++)
                game.SpriteBatch.Draw(game.Assets.DASH, new Rectangle(i * 48, 96, 48, 48), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
        public bool Dieded = false;
        int invulnurabilityFrames;
        public void Die(bool forcedamage)
        {
            if (invulnurabilityFrames > 0 && !forcedamage) return;
            if (Dieded) return;

            if (PlayerProfile.Data.Undeadality == 1 && Main.Random.Next(0, 4) == 0)
            {
                Resurrect();
                return;
            }

            if (PlayerProfile.Data.ExtraLives > 0)
            {
                PlayerProfile.Data.ExtraLives--;
                Resurrect();
                return;
            }

            Dieded = true;
            Size = new(0, 0);

            PlayerProfile.Data.Coins += CoinsCollected;
            PlayerProfile.Data.MoneyEarned += CoinsCollected;
            PlayerProfile.Data.LastRunCoins = CoinsCollected;
            PlayerProfile.Data.LastRunPercent = Main.Progress;
            PlayerProfile.Data.LastRunTime = Time;
            PlayerProfile.Data.TotalDeaths++;
            game.EntityManager.AddEntity(new Entity_PlayerDeathExplosion(game, Position));
            Position = new(0, 9999999);
            Dead = true;
            blizzardSound.Dispose();
            //game.Exit();
        }
    }
}

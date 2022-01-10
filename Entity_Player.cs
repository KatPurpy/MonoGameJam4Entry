using DSastR.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    class Entity_Player : Entity
    {
        float JumpTimer = 0;
        public Vector2 Velocity;
        public Entity_Player(Main m) : base(m)
        {
            Position = new(400, 300);
            Size = new(100, 100);
            Sprite = m.Assets.PLACEHOLDER;
        }
        bool Grounded => Velocity.Y >= 0 && CollidesWith<Entity_Platform>();
        public override void Update(GameTime time)
        {
            float deltatime = (float)time.ElapsedGameTime.TotalSeconds;
             Velocity += Vector2.UnitY * 9.8f * 100 * deltatime;
            if (Grounded)
            {
                Velocity.Y = 0;
                Position.Y -= Velocity.Y * deltatime;
            }

            KeyboardState kbstate = Keyboard.GetState();


            bool left = kbstate.IsKeyDown(Keys.Left);
            bool right = kbstate.IsKeyDown(Keys.Right);
            if (left)
            {
                Velocity.X += -6 * 100 * deltatime;
                Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            if (right)
            {
                Velocity.X += 6 * 100 * deltatime;
                Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            }
            if (Grounded && kbstate.IsKeyDown(Keys.Up))
            {
                Velocity.Y = -550;
                JumpTimer = 0.25f/6;

            }
            game.RenderOffset.Y += 0.5f;

            if(!new Rectangle(-CollisionBox.Width * 2, 0, 800 + CollisionBox.Width * 4, 600+CollisionBox.Height).Contains(CollisionBox))
            {
                game.Exit();
            }

            if (CollidesWith<Entity_Platform>(out var plat))
            {
                Position.Y += 200 * deltatime;
                foreach(var p in plat) p.Position.Y += 200 * deltatime;
            }
            Position += Velocity * deltatime;
        }
    }
}

﻿using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
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
        public float[] JumpForceParams = new float[] {-375,-550,-750 };
        public float[] UndeadalityParams = new float[] {0,0.10f,0.25f };

        public int Stronkth = 0, JumpForce = 0, Undeadality = 0;

        public bool FatPlatformBlock;
        public Vector2 Velocity;
        public Entity_Player(Main m) : base(m)
        {
            Position = new(400, 300);
            Size = new(100, 100);
            Sprite = m.Assets.placeholder;
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
            bool up = kbstate.IsKeyDown(Keys.Up);

   
                if (left)
                {
                    Velocity.X += -StronkthParams[Stronkth] * 100 * deltatime;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                }

                if (right)
                {
                    Velocity.X += StronkthParams[Stronkth] * 100 * deltatime;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                }

            if (up && Grounded && groundedLastFrame)
            {
                Velocity.Y = JumpForceParams[JumpForce];
            }
            groundedLastFrame = Grounded;
            
            
            if (CollisionBox.Top > 600)
            {
                Die();
            }

            if (Grounded && CollidesWith<Entity_Platform>(out var plat) && Velocity.Y >= 0)
            {
                plat.Sort((x, y) => x.Type - y.Type);
                FatPlatformBlock = false;
                    foreach(var p in plat) p.HandlePlayer(deltatime,this);
            }

            Position += Velocity * deltatime;

        }

        public override void IMGUI(GameTime time)
        {
            ImGui.Text(time.TotalGameTime.TotalSeconds.ToString());
        }

        public void Die()
        {
            game.EntityManager.AddEntity(new Entity_PlayerDeathExplosion(game, Position));
            Dead = true;
            //game.Exit();
        }
    }
}

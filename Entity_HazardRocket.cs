using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Entity_HazardRocket : Entity
    {
        public enum StartLocation
        {
            Left, Right, Top, Bottom
        }
       public StartLocation startLoc = StartLocation.Left;//(StartLocation)Main.Random.Next(0,4);
        float timer = 0;
        Vector2 velocity;
        const float speed = 400;

        const int MaxInstances = 5;
        static int Instances = 0;
        bool launched = false;
        public Entity_HazardRocket(Main m) : base(m)
        {
            Sprite = m.Assets.ROCKET;
            SourceRect = new(0, 0, 1419 / 11, 65);
            Size = new(80, 40);
            Instances++;
            Dead = Instances > MaxInstances;
            if (Dead)
            {
                Destroy();
            }
            else
            {
                
            }
            Position = new Vector2(9999, 9999);
        }
        int animFrames = 0;
        public override void Destroy()
        {
            Instances--;
            base.Destroy();
        }
        float panPos => startLoc == StartLocation.Bottom ? 0 : (Position.X / 800 * 2 - 1);
        public override void Start()
        {
            
            Target();
            game.Assets.rocketincoming.Play(1f, 0, panPos);
        }

        public override void Update(GameTime time)
        {
            SourceRect = new(((animFrames++)/2%11) * (1419/11), 0, 1419 / 11, 65);

            float dt = (float)time.ElapsedGameTime.TotalSeconds;

            timer += dt;

            if (startLoc == StartLocation.Right) Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            Rectangle screenRect = new(0, 0, 800, 600);

            if(timer > 3)
            {
                if (!launched)
                {
                    game.Assets.RocketLaunch.Play(1, 0, panPos) ;
                    launched = true;
                }
                switch (startLoc)
                {
                    case StartLocation.Left:
                        velocity.X = speed;
                        break;
                    case StartLocation.Right:
                        velocity.X = -speed;
                        break;
                    case StartLocation.Top:
                        velocity.Y = speed;
                        break;
                    case StartLocation.Bottom:
                        velocity.Y = -speed;

                        break;
                }

                Rectangle rect = Entity_Player._.CollisionBox;
                if (CollisionBox.Contains(game.RenderOffset + Entity_Player._.Position))
                {
                    Destroy();
                    Entity_Player._.Die(false);
                }
            }
            else
            {
                Target();

            }
            Position += velocity * dt;

            if (!screenRect.Intersects(CollisionBox))
            {
                Destroy();
            }
        }
        
        private void Target()
        {
            switch (startLoc)
            {
                case StartLocation.Left:
                    Position.X = 0;
                    Position.Y = Entity_Player._.Position.Y;
                    break;
                case StartLocation.Right:
                    Position.X = 800;
                    Position.Y = Entity_Player._.Position.Y;
                    Effects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                    break;
                case StartLocation.Bottom:
                    Rotation = MathHelper.ToRadians(-90);
                    Position.Y = -game.RenderOffset.Y + 600;
                    Position.X = Entity_Player._.Position.X;
                    //i don't understand why exactly this number
                    //monogame is fun
                    Origin.Y =  -64/2;
                    Origin.X = 129/2;
                    break;
                case StartLocation.Top: throw new Exception();
                    Rotation = MathHelper.ToRadians(90);
                    Origin = new Vector2(0.5f, 0.5f) * 1419 / 11;
                    Position.Y = -game.RenderOffset.Y;
                    Position.X = Entity_Player._.Position.X;
                    break;
            }
        }
        
        int blinkframes = 0;
        public override void IMGUI(GameTime time)
        {
            /*if(startLoc == StartLocation.Bottom)
            {
                System.Numerics.Vector2 vec = new System.Numerics.Vector2(Origin.X, Origin.Y);
                ImGui.DragFloat2("ORIGIN", ref vec);
                Origin = new(vec.X, vec.Y);
            }*/
            System.Numerics.Vector2 p1 = new(), p2=new();
           
            switch (startLoc)
            {
                case StartLocation.Left:
                case StartLocation.Right:
                    p1.X = 1;
                    p2.X = 799;
                    p2.Y = p1.Y = game.RenderOffset.Y + Position.Y;
                    break;
                case StartLocation.Top:
                case StartLocation.Bottom:
                    p1.Y = 1;
                    p2.Y = 599;
                    p2.X = p1.X = Position.X;
                    break;
            }

            ImGui.GetBackgroundDrawList().AddLine(
                p1,p2, blinkframes++%4>2 ? 0xFF00FFFF : 0xFF0000FF
                );
        }
    }
}


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    class Entity_RunController : Entity
    {
        public enum ScrollSpeed
        {
            Low, Medium, High
        }

        public ScrollSpeed Speed = ScrollSpeed.Low;


        Entity_Player monkey;
        public Entity_RunController(Main m, Entity_Player monke) : base(m)
        {
            Sprite = m.PixelTexture;
            monkey = monke;
        }


        public override void Start()
        {
            
        }
        float baseSpeed => Speed switch {
            ScrollSpeed.Low => 35,
            ScrollSpeed.Medium => 70,
            ScrollSpeed.High => 100
        };
        float speed;
        float penaltySpeed = 0;
        public static float LengthAccumulator;
        public override void Update(GameTime time)
        {
            if (monkey.Dead) return;
            float deltatime = (float)time.ElapsedGameTime.TotalSeconds;
            float s = (penaltySpeed + baseSpeed + speed) * deltatime * ((float)PlayerProfile.Data.WeightLoss/2+1);
            game.RenderOffset.Y += s;
            LengthAccumulator += s;
            speed += deltatime * 0.5f;

            float edge = 1- ((game.RenderOffset.Y + monkey.Position.Y) / (600));
            penaltySpeed = (edge < (1-0.25f) ? 0 : (((float)PlayerProfile.Data.WeightLoss / 2 + 1)) * 200) * edge;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameJam4Entry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace DSastR.Core
{
    public abstract class Entity
    {
        public Entity (Main m)
        {
            game = m;
        }

        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Size = new Vector2(0, 0);
        public Texture2D Sprite;
        public Rectangle? TextureREct;
        public Color Color = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;

        protected Main game;
        public Rectangle CollisionBox
        {
            get =>
new Rectangle((int)(game.RenderOffset.X + Position.X - Size.X / 2), (int)(game.RenderOffset.Y + Position.Y - Size.Y / 2), (int)Size.X, (int)Size.Y)
; set => throw new NotImplementedException();
        }

        public bool CollidesWith<T>() where T : Entity => CollidesWith<T>(out _);
        public bool CollidesWith<T>(out List<T> what) where T : Entity => game.EntityManager.CollidesWith(this, out what);

        public bool NeedsToStart = true;
        public bool Dead = false;

        public virtual void Start() { }
        public abstract void Update(GameTime time);
        public virtual void IMGUI(GameTime time) { }
        public virtual void Draw(GameTime time)
        {
            game.SpriteBatch.Begin(blendState: BlendState.Additive);
            game.SpriteBatch.Draw(Sprite, CollisionBox, TextureREct, Color, 0, Vector2.Zero, Effects, 0);
            game.SpriteBatch.End();
        }

        public virtual void Destroy() {
            Dead = true;
        }
    }
}

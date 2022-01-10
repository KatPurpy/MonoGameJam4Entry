using DSastR.Core;
using Microsoft.Xna.Framework;
using MonoGameJam4Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class EntityManager
    {
        public List<Entity> Entities = new List<Entity>();

        public int AddEntity(Entity obj)
        {
            obj.NeedsToStart = true;
            Entities.Add(obj);
            return Entities.Count;
        }

        public void Update(GameTime time)
        {
            for (int i = Entities.Count-1; i >= 0; i--)
            {
                var c = Entities[i];
                if (c.Dead)
                {
                    continue;
                }
                if (c.NeedsToStart)
                {
                    c.Start();
                    c.NeedsToStart = false;
                }
                c.Update(time);
            }
        }
        
        public bool CollidesWith<T>(Entity ent, out List<T> result) where T : Entity
    {
        result = null;
        bool any = false;
        foreach (var e in Entities) if (e != ent && e is T && ent.CollisionBox.Intersects(e.CollisionBox))
            {
                any = true;
                if (result == null) result = new();
                result.Add(e as T);
            }
        return any;
    }

        public void Draw(GameTime time)
        {
            for(int i = Entities.Count - 1; i >= 0; i--){
                if (Entities[i].Dead)
                {
                    Entities.RemoveAt(i);
                    continue;
                }
                Entities[i].Draw(time);
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i]?.Destroy();
            }
            Entities.Clear();
        }
    }

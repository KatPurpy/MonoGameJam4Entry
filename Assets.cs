using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Assets : IDisposable
    {
        static FieldInfo[] fieldInfos = typeof(Assets).GetFields(BindingFlags.Public | BindingFlags.Instance);
        static Dictionary<Type, Func<string, object>> assetLoading = new()
        {
            { typeof(Texture2D), (p) => Main.LoadTexture(Path.Combine("IMAGES", p + ".png")) },
            { typeof(StreamedSound), (p) => Main.LoadMusic(Path.Combine("MUSIC", p + ".ogg"))},
            { typeof(SoundEffect), (p)=>Main.LoadSound(Path.Combine("SOUNDS",p + ".ogg"))}
        };

        public Texture2D placeholder;
        public Texture2D BGG;
        public Texture2D BG1;
        public Texture2D BG2;
        public Texture2D BG3;
        public Texture2D DEATH;
        public Texture2D BLIZZARD;
        public Texture2D BADBANANA;
        public Texture2D EXTRALIFE;
        public Texture2D STRONKTH;
        public Texture2D WEIGHT;
        public Texture2D UNDEADALITY;
        public Texture2D MONEYPRINTER;
        public Texture2D DASH;
        public Texture2D ROCKET;
        public Texture2D COIN;

        public StreamedSound gym;
        public StreamedSound mountain;
        public StreamedSound sky;
        public StreamedSound space;

        public SoundEffect badbanana;
        public SoundEffect blizzard;
        public SoundEffect coin;
        public SoundEffect platjump;
        public SoundEffect death1;
        public SoundEffect death2;
        public SoundEffect death3;
        public SoundEffect death4;
        public SoundEffect resurrect;
        public SoundEffect rocketincoming;
        public SoundEffect rocket_launch_1;
        public SoundEffect rocket_launch_2;
        public SoundEffect rocket_launch_3;
        public SoundEffect rocket_launch_4;

        public SoundEffect RandomDeath => new SoundEffect[] { death1, death2, death3, death4 }[Main.Random.Next(0, 4)];
        public SoundEffect RocketLaunch => new SoundEffect[] { rocket_launch_1, rocket_launch_2, rocket_launch_3, rocket_launch_4 }[Main.Random.Next(0, 4)];


        public Assets (Main m)
        {
            foreach(var a in fieldInfos)
            {
                a.SetValue(this, assetLoading[a.FieldType](a.Name));
            }
        }
        public void Dispose()
        {
            foreach(var f in typeof(Assets).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                (f.GetValue(this) as IDisposable)?.Dispose();
            }
        }
    }
}

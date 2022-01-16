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
            { typeof(Texture2D), (p) => {
                Console.Write($"Loading texture {p}...");
                var result = Main.LoadTexture(Path.Combine("IMAGES", p + ".png"));
                Console.WriteLine("DONE");
                return result;    
            } },
            { typeof(StreamedMusic), (p) =>
            {
                Console.Write($"Loading music {p}...");
                var result = Main.LoadMusic(Path.Combine("MUSIC", p + ".ogg"));
                Console.WriteLine("DONE");
                return result;
            }
            },
            { typeof(SoundEffect), (p)=>
            {
                Console.Write($"Loading sound effect {p}...");
                var result = Main.LoadSound(Path.Combine("SOUNDS", p + ".ogg"));
                Console.WriteLine("DONE");
                return result;
            }
            }
        };

        public Texture2D placeholder;
        public Texture2D BGG;
        public Texture2D BG1;
        public Texture2D BG2;
        public Texture2D BG3;
        public Texture2D BG4;
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

        public Texture2D FALLING;
        public Texture2D TRAMPOLINE;
        public Texture2D EXPLOSIVE;

        public Texture2D OUTRO0000;
        public Texture2D OUTRO0001;
        public Texture2D OUTRO0002;
        public Texture2D OUTRO0003;
        public Texture2D OUTRO0004;
        public Texture2D OUTRO0005;
        public Texture2D OUTRO0006;
        public Texture2D OUTRO0007;

        public Texture2D INTRO0000;
        public Texture2D INTRO0001;
        public Texture2D INTRO0002;
        public Texture2D INTRO0003;
        public Texture2D INTRO0004;
        public Texture2D INTRO0005;
        public Texture2D INTRO0006;
        public Texture2D INTRO0007;
        public Texture2D INTRO0008;
        public Texture2D INTRO0009;
        public Texture2D INTRO0010;

        public StreamedMusic intro;
        public StreamedMusic gym;
        public StreamedMusic mountain;
        public StreamedMusic sky;
        public StreamedMusic space;
        public StreamedMusic ending;

        public SoundEffect cutscene_intro_yum1;
        public SoundEffect cutscene_intro_yum2;
        public SoundEffect stonknt;
        public SoundEffect hahahahaha;
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
        public SoundEffect stonks;

        public SoundEffect RandomDeath => new SoundEffect[] { death1, death2, death3, death4 }[Main.Random.Next(0, 4)];
        public SoundEffect RocketLaunch => new SoundEffect[] { rocket_launch_1, rocket_launch_2, rocket_launch_3, rocket_launch_4 }[Main.Random.Next(0, 4)];


        public Assets (Main m)
        {
            Console.WriteLine("SUPER MONKEY POST CELEBRATION DIET BETA BUILD");
            Console.WriteLine("Please wait while them assets are loading...");
            foreach (var a in fieldInfos)
            {
                a.SetValue(this, assetLoading[a.FieldType](a.Name));
            }
            Console.WriteLine("Thanks for patience! Enjoy!");
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

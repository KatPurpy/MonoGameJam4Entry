using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam4Entry
{
    public class Main : Game
    {
        public static Main _;

        GraphicsDeviceManager gdm;
        public SpriteBatch SpriteBatch;
        public Texture2D PixelTexture;
        public Assets Assets;

        public static Random Random = new();

        public ImGuiRenderer ImGuiRenderer;
        public ImFontPtr FontPTR;
        RenderTarget2D mainRenderTarget;
        public Vector2 RenderOffset;

        public EntityManager EntityManager;

        public Main()
        {
            gdm = new GraphicsDeviceManager(this);
            _ = this;
        }
        protected override void Initialize()
        {
            mainRenderTarget = new RenderTarget2D(GraphicsDevice, 800, 600,false,SurfaceFormat.Color,DepthFormat.Depth24Stencil8);
            Window.Title = "Super Monkey Post Celebration Diet";
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData(new[] { Color.White });
            ImGuiRenderer = new ImGuiRenderer(this);
            ImGuiRenderer.RebuildFontAtlas();
 

            gdm.PreferredBackBufferWidth = 800;
            gdm.PreferredBackBufferHeight = 600;
            gdm.ApplyChanges();
            Assets = new(this);

            EntityManager = new EntityManager(this);

            //EntityManager.AddEntity(new Entity_RunStarter(this));
            EntityManager.AddEntity(new Entity_Gym(this));

        }

        protected override void Dispose(bool disposing)
        {
            Assets.Dispose();
            base.Dispose(disposing);
        }

        Random r = new();
        protected override void Update(GameTime gameTime)
        {
            EntityManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(mainRenderTarget);
            GraphicsDevice.Clear(Color.Black);
            ImGuiRenderer.BeforeLayout(gameTime);
            unsafe
            {
                if ((ImFont*)0 == FontPTR.NativePtr)
                {

                    FontPTR = ImGui.GetIO().Fonts.AddFontFromFileTTF("FONT/ComicNeue-Bold.ttf", 18, null, ImGui.GetIO().Fonts.GetGlyphRangesDefault());
                    ImGuiRenderer.RebuildFontAtlas();
                    ImGuiSetStyle();
                }
            }
           
            ImGui.PushFont(FontPTR);
            EntityManager.Draw(gameTime);
            ImGuiRenderer.AfterLayout();
            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin();
            SpriteBatch.Draw(mainRenderTarget, new Rectangle(0, 0, 800, 600), Color.White);
            SpriteBatch.End();
        }

        public static Texture2D LoadTexture(string s)
        {
            using var stream = File.OpenRead(s);
            Texture2D t2d = Texture2D.FromStream(_.gdm.GraphicsDevice, stream);
            byte[] data = new byte[t2d.Width * t2d.Height * 4];
            t2d.GetData(data);
            Span<Color> c = MemoryMarshal.Cast<byte, Color>(data.AsSpan());
            for (int i = 0; i < c.Length; i++) if (c[i] == Color.Magenta) c[i] = Color.Transparent;
            t2d.SetData(data);
            return t2d;
        }

        public static SoundEffect LoadSound(string s)
        {
            using (var reader = new VorbisReader(s))
            {
                reader.ClipSamples = false;
                const int chunkLength = 8192;
                float[] buffer = new float[chunkLength];
                int sampleRate = reader.SampleRate;
                byte[] pcmBytes = new byte[sampleRate * 8];
                int samplesWritten = 0;

                int samplesRead = 0;
                while ((samplesRead = reader.ReadSamples(buffer, 0, chunkLength)) > 0)
                {
                    if (pcmBytes.Length < (samplesWritten + samplesRead) * sizeof(short))
                    {
                        Array.Resize(ref pcmBytes, pcmBytes.Length + sampleRate * 8);
                    }

                    Span<short> dst = MemoryMarshal.Cast<byte, short>(pcmBytes).Slice(samplesWritten, samplesRead);
                    Span<float> src = buffer.AsSpan(0, samplesRead);
                    FunnyAudioUtils.ConvertSingleToInt16(src, dst);
                    samplesWritten += samplesRead;
                }

                // TODO: fix getting correct channels from reader
                AudioChannels channels = reader.Channels == 1 ? AudioChannels.Mono : AudioChannels.Stereo;
                var a = new SoundEffect(pcmBytes, sampleRate, channels);

                return a;
            }
        }

        public static StreamedSound LoadMusic(string s)
        {
            return new(s);
        }

        static void ImGuiSetStyle()
        {

                System.Numerics.Vector4 ImVec4(float x, float y, float z, float w) => new(x, y, z, w);

                var colors = ImGui.GetStyle().Colors;
                colors[(int)ImGuiCol.Text] = ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
                colors[(int)ImGuiCol.TextDisabled] = ImVec4(0.50f, 0.50f, 0.50f, 1.00f);
                colors[(int)ImGuiCol.WindowBg] = ImVec4(0.17f, 0.09f, 0.00f, 0.97f);
                colors[(int)ImGuiCol.ChildBg] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
                colors[(int)ImGuiCol.PopupBg] = ImVec4(0.08f, 0.08f, 0.08f, 0.94f);
                colors[(int)ImGuiCol.Border] = ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
                colors[(int)ImGuiCol.BorderShadow] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
                colors[(int)ImGuiCol.FrameBg] = ImVec4(0.41f, 0.26f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.FrameBgHovered] = ImVec4(0.26f, 0.59f, 0.98f, 0.40f);
                colors[(int)ImGuiCol.FrameBgActive] = ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
                colors[(int)ImGuiCol.TitleBg] = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
                colors[(int)ImGuiCol.TitleBgActive] = ImVec4(0.53f, 0.33f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.TitleBgCollapsed] = ImVec4(0.00f, 0.00f, 0.00f, 0.51f);
                colors[(int)ImGuiCol.MenuBarBg] = ImVec4(0.14f, 0.14f, 0.14f, 1.00f);
                colors[(int)ImGuiCol.ScrollbarBg] = ImVec4(0.02f, 0.02f, 0.02f, 0.53f);
                colors[(int)ImGuiCol.ScrollbarGrab] = ImVec4(0.31f, 0.31f, 0.31f, 1.00f);
                colors[(int)ImGuiCol.ScrollbarGrabHovered] = ImVec4(0.41f, 0.41f, 0.41f, 1.00f);
                colors[(int)ImGuiCol.ScrollbarGrabActive] = ImVec4(0.51f, 0.51f, 0.51f, 1.00f);
                colors[(int)ImGuiCol.CheckMark] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
                colors[(int)ImGuiCol.SliderGrab] = ImVec4(0.24f, 0.52f, 0.88f, 1.00f);
                colors[(int)ImGuiCol.SliderGrabActive] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
                colors[(int)ImGuiCol.Button] = ImVec4(0.40f, 0.29f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.ButtonHovered] = ImVec4(0.00f, 0.55f, 0.43f, 1.00f);
                colors[(int)ImGuiCol.ButtonActive] = ImVec4(0.55f, 0.18f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.Header] = ImVec4(0.26f, 0.59f, 0.98f, 0.31f);
                colors[(int)ImGuiCol.HeaderHovered] = ImVec4(0.26f, 0.59f, 0.98f, 0.80f);
                colors[(int)ImGuiCol.HeaderActive] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
                colors[(int)ImGuiCol.Separator] = ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
                colors[(int)ImGuiCol.SeparatorHovered] = ImVec4(0.10f, 0.40f, 0.75f, 0.78f);
                colors[(int)ImGuiCol.SeparatorActive] = ImVec4(0.10f, 0.40f, 0.75f, 1.00f);
                colors[(int)ImGuiCol.ResizeGrip] = ImVec4(0.98f, 0.40f, 0.26f, 0.25f);
                colors[(int)ImGuiCol.ResizeGripHovered] = ImVec4(0.98f, 0.52f, 0.26f, 0.67f);
                colors[(int)ImGuiCol.ResizeGripActive] = ImVec4(0.98f, 0.69f, 0.26f, 0.95f);
                colors[(int)ImGuiCol.Tab] = ImVec4(0.58f, 0.43f, 0.18f, 0.86f);
                colors[(int)ImGuiCol.TabHovered] = ImVec4(0.74f, 0.56f, 0.00f, 0.80f);
                colors[(int)ImGuiCol.TabActive] = ImVec4(0.64f, 0.36f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.TabUnfocused] = ImVec4(0.07f, 0.10f, 0.15f, 0.97f);
                colors[(int)ImGuiCol.TabUnfocusedActive] = ImVec4(0.14f, 0.26f, 0.42f, 1.00f);
                colors[(int)ImGuiCol.DockingPreview] = ImVec4(0.26f, 0.59f, 0.98f, 0.70f);
                colors[(int)ImGuiCol.DockingEmptyBg] = ImVec4(0.20f, 0.20f, 0.20f, 1.00f);
                colors[(int)ImGuiCol.PlotLines] = ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
                colors[(int)ImGuiCol.PlotLinesHovered] = ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
                colors[(int)ImGuiCol.PlotHistogram] = ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.PlotHistogramHovered] = ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
                colors[(int)ImGuiCol.TextSelectedBg] = ImVec4(0.26f, 0.59f, 0.98f, 0.35f);
                colors[(int)ImGuiCol.DragDropTarget] = ImVec4(1.00f, 1.00f, 0.00f, 0.90f);
                colors[(int)ImGuiCol.NavHighlight] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
                colors[(int)ImGuiCol.NavWindowingHighlight] = ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
                colors[(int)ImGuiCol.NavWindowingDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
                colors[(int)ImGuiCol.ModalWindowDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.35f);

                var style = ImGui.GetStyle();
                style.ChildRounding =
                style.FrameRounding =
                style.GrabRounding =
                style.PopupRounding =
                style.ScrollbarRounding =
                style.TabRounding =
                style.WindowRounding = 12;

                style.FrameRounding = 6;

                style.WindowTitleAlign = style.SelectableTextAlign = new(0.5f);
                style.FramePadding = new(4);
                style.ChildBorderSize =
                style.FrameBorderSize =
                style.PopupBorderSize =
                style.TabBorderSize =
                style.WindowBorderSize = 0;

                style.ScrollbarSize = 16;
           
                style.AntiAliasedFill = style.AntiAliasedLines = style.AntiAliasedLinesUseTex = false;
                ImGui.GetIO().MouseDrawCursor = true;
        }
        }
    }


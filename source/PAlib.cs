using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

static class PA {

    public static Game Game { get; set; }

    public class Sprite {
        public Vector2 Pos { get; set; }
        public Texture2D Texture { get; set; }
        public bool Loaded { get; private set; } = false;
        public string Path { get; }
        public int Frame { get; set; } = 0;
        public Point Size { get; set; }
        public bool Flip { get; set; } = false;
        public bool Visible { get; set; } = true;

        public Sprite(string path) {
            Path = path;
        }

        public virtual void Load() {
            using (var f = new FileStream(Path, FileMode.Open)) {
                Texture = Texture2D.FromStream(Game.GraphicsDevice, f);
            }
            Loaded = true;
        }
    }

    public class Background : Sprite {

        private Color[] _colors;

        public Background(string path) : base(path) { }

        public override void Load() {
            base.Load();
            Size = new Point(Texture.Width, Texture.Height);
            _colors = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(_colors);
        }

        public Color GetColor(int x, int y) {
            if (!Loaded) Load();
            return _colors[Texture.Width * y + x];
        }
    }

    public class Screen {
        public Background[] Backgrounds = new Background[4];
        public Sprite[] Sprites = new Sprite[128];
    }

    public static Screen TopScreen = new Screen();
    public static Screen BottomScreen = new Screen();

    private static Screen GetScreen(byte screen) {
        if (screen == 0) return BottomScreen;
        else return TopScreen;
    }

    private static Background GetBackground(byte screen, byte layer) { 
        return GetScreen(screen).Backgrounds[layer] ;
    }

    private static void SetBackground(byte screen, byte layer, Background background) {
        GetScreen(screen).Backgrounds[layer] = background;
    }

    public static void Init16cBg(byte screen, byte layer) {

    }

    public static void HideBg(byte screen, byte layer) {
        GetBackground(screen, layer).Visible = false;
    }

    public static void ShowBg(byte screen, byte layer) {
        GetBackground(screen, layer).Visible = true;
    }

    public static void EasyBgScrollXY(byte screen, byte layer, 
            int x, int y) {
        var background = GetBackground(screen, layer);
        if (background != null) background.Pos = new Vector2(x, y);
    }

    public static void EasyLoadBackground(byte screen, byte layer, string path) {
        Console.WriteLine($"Load background {path}");
        SetBackground(screen, layer, new Background(path));
    }

    public static bool EasyBgGetPixel(byte screen, byte layer, int x, int y) {
        var background = GetBackground(screen, layer);
        if (background == null) return false;
        var color = background.GetColor(x, y);
        var result = (color.A != 0);
        return result;
    }

	public static void SetBrightness(byte screen, sbyte brightness) {

    }

    public static void WaitForVBL() {

    }

    public static int _16cText(byte screen, int basex, int basey, 
        int maxx, int maxy, string text, byte color, byte size, int limit) {
            return 0;
    }

    public static void CreateSprite(byte screen, int sprite, int width, int height, string path) {
        var s = new Sprite(path);
        s.Size = new Point(width, height);
        GetScreen(screen).Sprites[sprite] = s;
    }

    public static void SetSpriteXY(byte screen, int sprite, int x, int y) {
        // Hacky sprite specific positioning
        var s = GetScreen(screen).Sprites[sprite];
        s.Pos = new Vector2(x + 2 + s.Size.X / 2 , y + s.Size.Y - 1); 
    }

    public static void SetSpriteAnim(byte screen, int sprite, int animframe) {
        GetScreen(screen).Sprites[sprite].Frame = animframe;
    }

    public static void SetSpriteHflip(byte screen, int sprite, byte hflip) {
        var s = GetScreen(screen).Sprites[sprite];
        if (hflip > 0) s.Flip = true;
        else s.Flip = false;
    }

    public static void DeleteSprite(byte screen, int sprite) {

    }

    public static int Distance(int x1, int y1, int x2, int y2) {
        var v1 = new Vector2(x1, y1);
        var v2 = new Vector2(x2, y2);
        return(int)(v2 - v1).LengthSquared();
    }

    public static int RandMax(int max) {
        return new Random().Next(0, max);
    }

    public static int GetAngle(int x1, int y1, int x2, int y2) {
        throw new NotImplementedException();
    }

    public static int Sin(int angle) {
        throw new NotImplementedException();
    }

    public static int Cos(int angle) {
        throw new NotImplementedException();
    }

    public static void PlayOgg(string path) {
        var p = Path.GetFullPath(path);
        var song = Song.FromUri(Path.GetFileName(path), new Uri(p));
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 1;
    }
}

static class Pad {
    public static Buttons Newpress;
    public static Buttons Held;

    private static KeyboardState _prev;

    public struct Buttons {
        public bool A, B, X, Y, Up, Down, Left, Right, Start, Select, L, R;
    }

    public static void Update(KeyboardState kstate) {
        if (kstate.IsKeyDown(Keys.Left)) Held.Left = true;
        else Held.Left = false;
        if (kstate.IsKeyDown(Keys.Right)) Held.Right = true;
        else Held.Right = false;
        if (kstate.IsKeyDown(Keys.Up)) Newpress.Up = true;
        else Newpress.Up = false;
    }
}
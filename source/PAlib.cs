using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParallelWorlds;

static class PA {

    public static Game Game { get; set; }

    public class Background {
        public Vector2 Pos { get; set; }
        public Texture2D Texture { get; set; }
        public bool Loaded { get; private set; } = false;
        public string Path { get; }

        public Background(string path) {
            Path = path;
        }

        public void Load() {
            using (var f = new FileStream(Path, FileMode.Open)) {
                Texture = Texture2D.FromStream(Game.GraphicsDevice, f);
            }
            Loaded = true;
        }
    }

    public static Background[] MainBackgrounds = new Background[4];
    public static Background[] MiscBackgrounds = new Background[4];

    private static Background[] GetScreen(byte screen) {
        if (screen == Globals.MAINSCREEN) return MainBackgrounds;
        else return MiscBackgrounds;
    }

    private static Background GetBackground(byte screen, byte layer) { 
        return GetScreen(screen)[layer] ;
    }

    private static void SetBackground(byte screen, byte layer, Background background) {
        GetScreen(screen)[layer] = background;
    }

    public static void Init16cBg(byte screen, byte layer) {

    }

    public static void HideBg(byte screen, byte layer) {

    }

    public static void ShowBg(byte screen, byte layer) {

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
        return false;
    }

	public static void SetBrightness(byte screen, sbyte brightness) {

    }

    public static void WaitForVBL() {

    }

    public static int _16cText(byte screen, int basex, int basey, 
        int maxx, int maxy, string text, byte color, byte size, int limit) {
            return 0;
    }

    public static void SetSpriteXY(byte screen, int sprite, int x, int y) {

    }

    public static void SetSpriteAnim(byte screen, int sprite, int animframe) {

    }

    public static void SetSpriteHflip(byte screen, int sprite, byte hflip) {

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
        return 0;
    }

    public static int Sin(int angle) {
        return 0;
    }

    public static int Cos(int angle) {
        return 0;
    }
}

static class Pad {
    public static Buttons Newpress;
    public static Buttons Held;

    public struct Buttons {
        public bool A, B, X, Y, Up, Down, Left, Right, Start, Select, L, R;
    }
}
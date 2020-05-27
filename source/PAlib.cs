using System;
using System.Numerics;

static class PA {

    public static void Init16cBg(byte screen, byte layer) {

    }

    public static void HideBg(byte screen, byte layer) {

    }

    public static void ShowBg(byte screen, byte layer) {

    }

    public static void EasyBgScrollXY(byte screen, byte layer, 
        int x, int y) {

    }

    public static void EasyLoadBackground(byte screen, byte layer, string path) {
        Console.WriteLine($"Load background {path}");
    }

    public static bool EasyBgGetPixel(byte screen, byte layer, int x, int y) {
        return false;
    }

	public static void SetBrightness(byte screen, sbyte brightness) {

    }

    public static void WaitForVBL() {
        System.Threading.Thread.Sleep(33);
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
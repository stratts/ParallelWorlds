using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;

static class Defines
{
    public const string VERSION = "Version 0.2";

    public const byte MAINSCREEN = 1;
    public const byte MISCSCREEN = 0;

    public static int LEVELNUM;
    public const int COLLISION_BORDER = 4;
    public static string ROOTPATH = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "/data/ParallelWorlds";

    public static int selectedLevel;
    public static int selectedCharacter;
    public static int selectedItem;

    public static float playerPoints = 0;

    public static Point ScreenSize;
}

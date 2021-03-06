using System;
using System.Collections.Generic;

using static Collisions;
using static Defines;

static class Functions
{

    public static void displayError(string error_text)
    {
        //PA_ResetBgSys();
        //PA_ResetSpriteSys();
        PA.SetBrightness(1, 0);
        PA.SetBrightness(0, 0);
        PA.Init16cBg(1, 3);
        string buffer = "Error:\n" + error_text;
        //PA16cTextAlign(ALIGN_CENTER);
        PA.SimpleText(1, 0, 0, buffer, true);
        while (true)
        {
            PA.WaitForVBL();
        }
    }

    public static void setSpriteXY(byte screen, int sprite, float x, float y)
    {
        if (x >= ScreenSize.X || y >= ScreenSize.Y || x <= -64 || y <= -64) PA.SetSpriteXY(screen, sprite, -64, -64);
        else PA.SetSpriteXY(screen, sprite, x, y);
    }

    public static ObjectInfo FindClosestObject(ObjectInfo current, Scene scene)
    {
        int minDist = -1;
        ObjectInfo minObj = null;

        foreach (ObjectInfo obj in scene.Objects)
        {
            if (obj == current) continue;
            var dist = PA.Distance(current.x, current.y, obj.x, obj.y);
            if (minDist == -1 || dist < minDist)
            {
                minDist = dist;
                minObj = obj;
            }
        }

        return minObj;
    }

    public static void displayDebug(byte screen, Scene scene)
    {
        var player = scene.Objects[0];

        CA.SimpleText(screen, 4, 6, "- Player/camera info -");
        CA.SimpleText(screen, 4, 14, "Player position X: {0}", (int)scene.Objects[0].x);
        CA.SimpleText(screen, 4, 22, "Player position Y: {0}", (int)scene.Objects[0].y);
        CA.SimpleText(screen, 4, 30, "Camera position X: {0}", (int)scene.Camera.x);
        CA.SimpleText(screen, 4, 38, "Camera position Y: {0}", (int)scene.Camera.y);

        CA.SimpleText(screen, 4, 58, "- Collisions -");
        CA.SimpleText(screen, 4, 66, "Left collision: {0}", leftCollision(player));
        CA.SimpleText(screen, 4, 74, "Right collision: {0}", rightCollision(player));
        CA.SimpleText(screen, 4, 82, "Up collision: {0}", upCollision(player));
        CA.SimpleText(screen, 4, 90, "Down collision: {0}", downCollision(player));
        CA.SimpleText(screen, 4, 98, "Touching ground: {0}", touchingGround(player));

        //struct mallinfo info = mallinfo();

        CA.SimpleText(screen, 4, 116, "- Hardware -");
        CA.SimpleText(screen, 4, 124, "CPU usage: {0} percent", "?");
        CA.SimpleText(screen, 4, 132, "Memory in use: {0} MB", GC.GetTotalMemory(false) / 1_000_000);
    }
}

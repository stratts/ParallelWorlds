using System;
using System.Collections.Generic;

using static Objects;
using static Collisions;
using static GlobalCamera;

static class Functions {
    
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
        while(true)
        {
            PA.WaitForVBL();
        }
    }

    public static void setSpriteXY(byte screen, int sprite, int x, int y)
    {
        if (x >= 256 || y >= 192 || x <= -64 || y <= -64) PA.SetSpriteXY(screen, sprite, 256, 192);
        else PA.SetSpriteXY(screen, sprite, x, y);
    }


    public static int lowestXinObj(ObjectInfo current, List<ObjectInfo> ar, int size)
    {
        int i;
        int oldDistance = 10000000;
        int place = 0;

        if(size > 1)
        {
            for(i=0;i<size+1;i++)
            {
                if (i >= ar.Count) break;
                ObjectInfo obj = ar[i];
                if(PA.Distance(current.x>>8, current.y>>8, obj.x>>8, obj.y>>8) < oldDistance && current != obj)
                {
                    place = i;
                    oldDistance = PA.Distance(current.x>>8, current.y>>8, obj.x>>8, obj.y>>8);
                }

            }
        }

        return place;   
    }

    public static void displayDebug(byte screen)
    {
        var player = objects[0];

        CA.SimpleText(screen, 4, 6, "- Player/camera info -");
        CA.SimpleText(screen, 4, 14, "Player position X: {0}", objects[0].x>>8);
        CA.SimpleText(screen, 4, 22, "Player position Y: {0}", objects[0].y>>8);
        CA.SimpleText(screen, 4, 30, "Camera position X: {0}", camera.x>>8);
        CA.SimpleText(screen, 4, 38, "Camera position Y: {0}", camera.y>>8);

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

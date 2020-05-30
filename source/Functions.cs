using System;
using System.Collections;

using static Objects;
using static Collisions;
using static Camera;

static class Functions {
    
    public static IEnumerator displayError(string error_text)
    {
        //PA_ResetBgSys();
        //PA_ResetSpriteSys();
        PA.SetBrightness(1, 0);
        PA.SetBrightness(0, 0);
        PA.Init16cBg(1, 3);
        var text = "Error:\n" + error_text;
        //PA16cTextAlign(ALIGN_CENTER);
        PA.SimpleText(1, 0, 0, error_text, true);
        while(true)
        {
            PA.WaitForVBL();
            yield return null;
        }
    }

    public static void setSpriteXY(byte screen, int sprite, int x, int y)
    {
        if (x >= 256 || y >= 192 || x <= -64 || y <= -64) PA.SetSpriteXY(screen, sprite, 256, 192);
        else PA.SetSpriteXY(screen, sprite, x, y);
    }


    public static int lowestXinObj(ObjectInfo[] ar, int size)
    {
        int i;
        int oldDistance = 10000000;
        int place = 0;
        var currentObject = Objects.currentObject;

        if(size > 1)
        {
            for(i=0;i<size+1;i++)
            {
                if(PA.Distance(ar[currentObject].x>>8, ar[currentObject].y>>8, ar[i].x>>8, ar[i].y>>8) < oldDistance && currentObject != i)
                {
                    place = i;
                    oldDistance = PA.Distance(ar[currentObject].x>>8, ar[currentObject].y>>8, ar[i].x>>8, ar[i].y>>8);
                }

            }
        }

        return place;   
    }

    public static void displayDebug(byte screen)
    {
        CA.SimpleText(screen, 4, 6, "- Player/camera info -");
        CA.SimpleText(screen, 4, 14, "Player position X: {0}", objects[0].x>>8);
        CA.SimpleText(screen, 4, 22, "Player position Y: {0}", objects[0].y>>8);
        CA.SimpleText(screen, 4, 30, "Camera position X: {0}", camera.x>>8);
        CA.SimpleText(screen, 4, 38, "Camera position Y: {0}", camera.y>>8);

        CA.SimpleText(screen, 4, 58, "- Collisions -");
        CA.SimpleText(screen, 4, 66, "Left collision: {0}", leftCollision(0));
        CA.SimpleText(screen, 4, 74, "Right collision: {0}", rightCollision(0));
        CA.SimpleText(screen, 4, 82, "Up collision: {0}", upCollision(0));
        CA.SimpleText(screen, 4, 90, "Down collision: {0}", downCollision(0));
        CA.SimpleText(screen, 4, 98, "Touching ground: {0}", touchingGround(0));

        //struct mallinfo info = mallinfo();

        CA.SimpleText(screen, 4, 116, "- Hardware -");
        CA.SimpleText(screen, 4, 124, "CPU usage: {0} percent", "?");
        CA.SimpleText(screen, 4, 132, "Memory in use: {0} MB", GC.GetTotalMemory(false) / 1_000_000);
    }   
}

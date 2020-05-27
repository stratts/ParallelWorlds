using System;
using System.Numerics;

static class Functions {
    
    public static void PrintFields(Object obj) {
        System.Console.WriteLine(obj.GetType().Name);
        foreach (var f in  obj.GetType().GetFields()) {
            System.Console.WriteLine($"{f.Name}: {f.GetValue(obj)}");
        }
    }

    public static void displayError(string error_text)
    {
        //PA_ResetBgSys();
        //PA_ResetSpriteSys();
        PA.SetBrightness(1, 0);
        PA.SetBrightness(0, 0);
        PA.Init16cBg(1, 3);
        var text = "Error:\n" + error_text;
        //PA16cTextAlign(ALIGN_CENTER);
        PA._16cText(1, 0, 80, 255, 192, text, 1, 0, 100);
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
}
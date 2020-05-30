using System;
using System.Collections;

public static class CA {

    public static string rootf(string path) {
        string filepath = Defines.ROOTPATH;
        filepath += path;
        return filepath;
    }

    public static void Information(byte screen, string text) {
        PA.SimpleText(screen, 0, 0, text, true);
    }

    public static void SimpleText(byte screen, int x, int y, string text) {
        PA.SimpleText(screen, x, y, text);
    }

    public static void SimpleText(byte screen, int x, int y, string format, params object[] args) {
        if (args == null) SimpleText(screen, x, y, format);
        else SimpleText(screen, x, y, String.Format(format, args));
    }

    public static void Update16c() {
        PA.QueueClearText = true;
    }

    public static void FadeOut(byte type) {
        sbyte i;

        if (type == 1)
        {   
            for (i = 0; i <= 31; i++) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
            }   
        }
        
        else if (type == 0)
        {
            for (i = 0; i >= -31; i--) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
            }   
        }   
    }

    public static void FadeIn(byte type) {
        sbyte i;

        if (type == 1)
            {   
            for (i = 31; i >= 0; i--) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
            }   
        }
            
            else if (type == 0)
            {
            for (i = -31; i <= 0; i++) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
            }   
        }   
    }
}
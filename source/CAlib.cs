using System;
using System.Collections;

public static class CA {

    public static string rootf(string path) {
        string filepath = Globals.ROOTPATH;
        filepath += path;
        return filepath;
    }

    public static void Information(uint screen, string text) {
        
    }

    public static void SimpleText(uint screen, int x, int y, string text) {

    }

    public static void SimpleText(uint screen, int x, int y, string format, params object[] args) {
        if (args == null) SimpleText(screen, x, y, format);
        else SimpleText(screen, x, y, String.Format(format, args));
    }

    public static void Update16c() {

    }

    public static IEnumerator FadeOut(byte type) {
        sbyte i;

        if (type == 1)
        {	
            for (i = 0; i <= 31; i++) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
                yield return null;
            }	
        }
        
        else if (type == 0)
        {
            for (i = 0; i >= -31; i--) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
                yield return null;
            }	
        }   
    }

    public static IEnumerator FadeIn(byte type) {
        sbyte i;

        if (type == 1)
            {	
            for (i = 31; i >= 0; i--) 
            {
                PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
                yield return null;
            }	
        }
            
            else if (type == 0)
            {
            for (i = -31; i <= 0; i++) 
            {
                //PA.SetBrightness(0, i); 
                PA.SetBrightness(1, i);
                PA.WaitForVBL();
                yield return null;
            }	
        }   
    }
}
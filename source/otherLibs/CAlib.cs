using System;
using System.Collections;

public static class CA
{

    public static string rootf(string path)
    {
        string filepath = Defines.ROOTPATH;
        filepath += path;
        return filepath;
    }

    public static void Information(byte screen, string text)
    {
        PA.SimpleText(screen, 0, 0, text, true);
    }

    public static void SimpleText(byte screen, int x, int y, string text)
    {
        PA.SimpleText(screen, x, y, text);
    }

    public static void SimpleText(byte screen, int x, int y, string format, params object[] args)
    {
        if (args == null) SimpleText(screen, x, y, format);
        else SimpleText(screen, x, y, String.Format(format, args));
    }

    public static void Update16c()
    {
        PA.QueueClearText = true;
    }

    private static void Fade(sbyte from, sbyte to)
    {
        sbyte i = from;
        sbyte inc = (sbyte)Math.Sign(to - from);

        while (i != to + inc)
        {
            PA.SetBrightness(0, i);
            PA.SetBrightness(1, i);
            PA.WaitForVBL();
            i += (sbyte)Math.Sign(to - from);
        }
    }

    public static void FadeOut(byte type)
    {
        if (type == 1) Fade(0, 31);
        else if (type == 0) Fade(0, -31);
    }

    public static void FadeIn(byte type)
    {
        if (type == 1) Fade(31, 0);
        else if (type == 0) Fade(-31, 0);
    }
}
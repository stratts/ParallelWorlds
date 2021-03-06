using System;
using System.Collections;
using System.IO;
using IniParser;
using static Defines;
using static Functions;

struct LevelInfo
{
    public string name;
    public string music;
    public string stagebg, midbg, backbg;
    public string collision;
    public int width, height, friction, gravity;
    public float midscroll, backscroll;

    private bool TryLoadBackground(byte screen, byte layer, string name)
    {
        string rootPath = CA.rootf("/levels");
        string bgPath = String.Format("{0}/{1}/{2}.png", rootPath, this.name, name);
        if (!File.Exists(bgPath)) return false;
        PA.EasyLoadBackground(screen, layer, bgPath);
        PA.HideBg(screen, layer);
        return true;
    }

    public void Load()
    {
        Levels.currentLevel = this;

        PA.Init16cBg(0, 0);
        PA.Init16cBg(1, 0);

        CA.Information(1, "Loading...");
        CA.FadeIn(0);

        if (!TryLoadBackground(MAINSCREEN, 1, "StageBG"))
        {
            displayError(String.Format("Could not load stage for {0}.", name));
        }

        bool midLoaded = TryLoadBackground(MAINSCREEN, 2, "MidBG");
        bool backLoaded = TryLoadBackground(MAINSCREEN, 3, "BackBG");

        if (!TryLoadBackground(MISCSCREEN, 3, "CollisionMap"))
        {
            displayError(String.Format("Could not load collision map for {0}.", name));
        }

        CA.FadeOut(0);
        CA.Update16c();

        PA.ShowBg(MAINSCREEN, 1);
        if (midLoaded) PA.ShowBg(MAINSCREEN, 2);
        if (backLoaded) PA.ShowBg(MAINSCREEN, 3);

        if (music != null)
        {
            string musicPath = CA.rootf("/music/") + music;
            PA.PlayOgg(musicPath);
        }
    }

    public bool getCollisionPix(byte screen, byte bglayer, int x, int y)
    {
        int xPos = Math.Min(width - 1, Math.Max(0, x));
        int yPos = Math.Min(height - 1, Math.Max(0, y));

        return PA.EasyBgGetPixel(screen, bglayer, xPos, yPos);
    }
}

class WorldInfo
{
    public string name;
    public LevelInfo[] level = new LevelInfo[128];
}

static class Levels
{
    public static WorldInfo currentWorld;
    public static LevelInfo currentLevel;

    public static WorldInfo Jelli = new WorldInfo();

    public static void setLevels()
    {
        LEVELNUM = -1;

        int i = 0;

        var root = CA.rootf("/levels");
        var dirInfo = new DirectoryInfo(root);

        foreach (var dir in dirInfo.GetDirectories())
        {
            Jelli.level[i].name = dir.Name;
            var data = new FileIniDataParser().ReadFile(root + $"/{Jelli.level[i].name}/config.ini");
            Jelli.level[i].music = data["Level"]["music"];
            Jelli.level[i].width = int.Parse(data["Level"]["width"]);
            Jelli.level[i].height = int.Parse(data["Level"]["height"]);
            Jelli.level[i].gravity = int.Parse(data["Level"]["gravity"]);
            Jelli.level[i].midscroll = float.Parse(data["Scrolling"]["midscroll"]) / 256;
            Jelli.level[i].backscroll = float.Parse(data["Scrolling"]["backscroll"]) / 256;
            i++;
            LEVELNUM++;
        }
    }
}

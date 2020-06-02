using System;
using System.IO;
using System.Collections;
using IniParser;

using static Defines;
using static Functions;

struct LevelInfo {
    public string name;
    public string music;
    public string stagebg, midbg, backbg;
    public string collision;
    public int width, height, friction, gravity;
    public int midscroll, backscroll;

    public void Load()
    {
        Levels.currentLevel = this;

        string levelPath;
        string rootPath;

        PA.Init16cBg(0, 0);
        PA.Init16cBg(1, 0);

        bool midLoaded, backLoaded;

        rootPath = CA.rootf("/levels"); 
        levelPath = String.Format("{0}/{1}/{2}.png", rootPath, name, "StageBG");

        CA.Information(1, "Loading...");
        CA.FadeIn(0);

        if(!File.Exists(levelPath))
        {
            displayError(String.Format("Could not load stage for {0}.", name));
        }
        else 
        {
            PA.EasyLoadBackground(MAINSCREEN, 1, levelPath);
            PA.HideBg(MAINSCREEN, 1);
        }

        levelPath = String.Format("{0}/{1}/{2}.png", rootPath, name, "MidBG");
        if(File.Exists(levelPath)) 
        {
            PA.EasyLoadBackground(MAINSCREEN, 2, levelPath);
            PA.HideBg(MAINSCREEN, 2);
            midLoaded = true;
        }
        else midLoaded = false;

        levelPath = String.Format("{0}/{1}/{2}.png", rootPath, name, "BackBG");
        if(File.Exists(levelPath)) 
        {
            PA.EasyLoadBackground(MAINSCREEN, 3, levelPath);
            PA.HideBg(MAINSCREEN, 3);
            backLoaded = true;
        }
        else backLoaded = false;

        levelPath = String.Format("{0}/{1}/{2}.png", rootPath, name, "CollisionMap");
        if(File.Exists(levelPath)) 
        {
            PA.EasyLoadBackground(MISCSCREEN, 3, levelPath);
            PA.HideBg(MAINSCREEN, 3);
        }
        else
        {
            displayError(String.Format("Could not load collision map for {0}.", name));
        }

        CA.FadeOut(0);
        CA.Update16c();

        //PA_DeleteBg(1, 0);
        //PA_DeleteBg(0, 0);

        PA.ShowBg(MAINSCREEN, 1);
        if(midLoaded) PA.ShowBg(MAINSCREEN, 2);
        if(backLoaded) PA.ShowBg(MAINSCREEN, 3);


        if(music != null)
        {
            string musicPath = CA.rootf("/music/") + music;
            PA.PlayOgg(musicPath);
        }   
    }

    public bool getCollisionPix(byte screen, byte bglayer, int x, int y) {
        int xPos = x;
        int yPos = y;

        if(xPos > width - 1) xPos = width - 1;
        if(xPos < 0) xPos = 0;
        if(yPos > height - 1) yPos = height - 1;
        if (yPos < 0) yPos = 0;

        return PA.EasyBgGetPixel(screen, bglayer, xPos, yPos);
    }
}

class WorldInfo {
    public string name;
    public LevelInfo[] level = new LevelInfo[128];
}

static class Levels {
    public static WorldInfo currentWorld;
    public static LevelInfo currentLevel;

    public static WorldInfo Jelli = new WorldInfo();

    public static void setLevels() {
        LEVELNUM = -1;

        int i = 0;

        var root = CA.rootf("/levels");
        var dirInfo = new DirectoryInfo(root);

        foreach (var dir in dirInfo.GetDirectories()) {
            Jelli.level[i].name = dir.Name;
            var data = new FileIniDataParser().ReadFile(root + $"/{Jelli.level[i].name}/config.ini");
            Jelli.level[i].music = data["Level"]["music"];
            Jelli.level[i].width = int.Parse(data["Level"]["width"]);
            Jelli.level[i].height = int.Parse(data["Level"]["height"]);
            Jelli.level[i].gravity = int.Parse(data["Level"]["gravity"]);
            Jelli.level[i].midscroll = int.Parse(data["Scrolling"]["midscroll"]);
            Jelli.level[i].backscroll = int.Parse(data["Scrolling"]["backscroll"]);
            i++;
            LEVELNUM++;
        }
    }

    public static bool getCollisionPix(byte screen, byte bglayer, int x, int y) {
        return currentLevel.getCollisionPix(screen, bglayer, x, y);
    }
}

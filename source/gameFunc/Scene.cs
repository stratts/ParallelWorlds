using System;
using System.Collections.Generic;

using static Functions;
using static Defines;
using static Classes;
using IniParser;

class Scene {
    private int midBgX = 0, backBgX = 0;

    public LevelInfo Level { get; private set; }
    public List<ObjectInfo> Objects { get; } = new List<ObjectInfo>();
    public Camera Camera { get; } = new Camera();

    public Scene(LevelInfo level) {
        Level = level;
    }

    public void Load() {
        PA.Reset();
        Level.Load();

        int i = 1;
        var path = CA.rootf("/levels") + $"/{Level.name}/config.ini";
        var data = new FileIniDataParser().ReadFile(path);

        while(true)
        {
            string key = $"Object{i}";
            if(data.Sections.ContainsSection(key))
            {
                AddObject(new ObjectInfo(classes[int.Parse(data[key]["class"])],
                                    int.Parse(data[key]["x"]),
                                     int.Parse(data[key]["y"]),
                                    0
                ));
                int flip = -1;
                if (data[key].ContainsKey("flip")) flip = int.Parse(data[key]["flip"]);
                Objects[i-1].moveDirection = flip;
            }
            else break;
            i++;
        }
    }

    public void AddObject(ObjectInfo obj) {
        Objects.Add(obj);
    }

    public void Update() {
        foreach (var obj in Objects) {
            obj.UpdateCentre();
            obj.Update(this);
        }

        Camera.Scroll();

        foreach (var obj in Objects)
        {
            obj.UpdateCentre();
            setSpriteXY(MAINSCREEN, obj.sprite, obj.x - (Camera.x>>8), obj.y - (Camera.y>>8));
        }    

        midBgX -= Level.midscroll;
        backBgX -= Level.backscroll;

        PA.EasyBgScrollXY(MAINSCREEN, 1, Camera.x>>8, Camera.y>>8);
        PA.EasyBgScrollXY(MAINSCREEN, 2, ((Camera.x+midBgX)>>8)>>1, (Camera.y>>8)>>1);
        PA.EasyBgScrollXY(MAINSCREEN, 3, ((Camera.x+backBgX)>>8)>>2, (Camera.y>>8)>>2);
    }

    public bool InCanvas(ObjectInfo obj) {
        int x = (int)obj.x;
        int y = (int)obj.y;
        if ((x-Camera.x)>>8 > 256 || (y-Camera.y)>>8 > 192 || (x-Camera.x)>>8 < -64 || (y-Camera.y)>>8 < -64) return false;
        return true;
    }
}
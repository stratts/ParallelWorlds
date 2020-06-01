using System;
using System.Collections.Generic;

using static Functions;
using static Defines;

class Scene {
    private int midBgX = 0, backBgX = 0;

    public LevelInfo Level { get; private set; }
    public List<ObjectInfo> Objects { get; } = new List<ObjectInfo>();
    public Camera Camera { get; } = new Camera();

    public Scene(LevelInfo level) {
        Level = level;
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
            setSpriteXY(MAINSCREEN, obj.sprite, (obj.x-Camera.x)>>8, (obj.y-Camera.y)>>8);
        }    

        midBgX -= Level.midscroll;
        backBgX -= Level.backscroll;

        PA.EasyBgScrollXY(MAINSCREEN, 1, Camera.x>>8, Camera.y>>8);
        PA.EasyBgScrollXY(MAINSCREEN, 2, ((Camera.x+midBgX)>>8)>>1, (Camera.y>>8)>>1);
        PA.EasyBgScrollXY(MAINSCREEN, 3, ((Camera.x+backBgX)>>8)>>2, (Camera.y>>8)>>2);
    }

    public bool InCanvas(ObjectInfo obj) {
        int x = obj.x;
        int y = obj.y;
        if ((x-Camera.x)>>8 > 256 || (y-Camera.y)>>8 > 192 || (x-Camera.x)>>8 < -64 || (y-Camera.y)>>8 < -64) return false;
        return true;
    }
}
using System;
using System.Collections.Generic;

using static Collisions;
using static Functions;
using static Defines;
using static Levels;
using static Camera;

public class ObjectInfo {
    public int x, y;
    public int cx, cy;
    public int vy, vx;
    public ObjectClass objClass;
    public int sprite;
    public int currentFrame, frameCount;
    public bool alive;
    public int oldposx, newposx, relspeedx;
    public int oldposy, newposy, relspeedy;
    public int startX, startY;
    public int moveDirection;
    public bool jumping;
    public int action;
    public int cpuStopPos;
    public int typeOverride;
    public int i;
    public bool activated;
    public int rotation;
    public int rotsetSlot;
    public bool loaded;

    //public int cx => x + (32 << 8);
    //public int cy => (y + (64<<8)) - (objClass.height << 8);

    public void Update() {
        UpdateCentre();
        if(loaded) objClass.ai(this);
    }

    public void UpdateCentre() {
        cx = x + (32 << 8);
        cy = (y + (64<<8)) - (objClass.height << 8);
    }

    public void Kill() {
        alive = false;
        PA.SetSpriteXY(MAINSCREEN, sprite, 256, 192);
    }

    public void Revive() {
        alive = true;
        PA.SetSpriteXY(MAINSCREEN, sprite, x, y);
    }

    public void Delete() {
        Objects.sprite[this.sprite] = 0;
        alive = false;
        loaded = false;
        PA.DeleteSprite(MAINSCREEN, sprite);
    }

    public void Animate(int startFrame, int endFrame, int frameSpeed) {
        frameCount++;
        if (frameCount >= frameSpeed)
        {
            frameCount = -1;
            currentFrame++;
        }

        if (currentFrame < startFrame) currentFrame = startFrame;
        if (currentFrame > endFrame) currentFrame = startFrame;

        PA.SetSpriteAnim(MAINSCREEN, sprite, currentFrame);
    }

    public void CheckCollision() {
        if(rightCollision(this)) 
        {
            if(relspeedx <= -256) x += relspeedx;
            else x -= 256;
        }

        if(leftCollision(this))
        {
            if(relspeedx >= 256) x += relspeedx;
            else x += 256;
        }

        if(upCollision(this)) 
        {
            if(relspeedy <= -256) y -= relspeedy;
            else y += 256;
            vy = 0;
        }

        if(downCollision(this))
        {
            if(relspeedy >= 256) y -= relspeedy;
            else y -= 512;
            action = 0;
        }
    }

    public void AddGravity() {
        y += vy;
        if(!touchingGround(this) && vy < currentWorld.level[currentLevel].gravity) vy += objClass.weight;
        else if(touchingGround(this)) vy = 0;
    }

    public bool InStageZone() {
        if (y>>8 > currentWorld.level[currentLevel].height + 256) return false;
        return true;
    }

    public bool InCanvas() {
        if ((x-camera.x)>>8 > 256 || (y-camera.y)>>8 > 192 || (x-camera.x)>>8 < -64 || (y-camera.y)>>8 < -64) return false;
        return true;
    }
}

static class Objects {
    public static int MAXOBJECTS = -1;      // Object count, starts at nil (-1)

    public static List<ObjectInfo> objects = new List<ObjectInfo>();

    public static int currentObject;
    public static int[] sprite = new int[96];
    
    public static void createObject(ObjectClass objClass, int x, int y, int type) {
        MAXOBJECTS++;           // Increase object count

        // Initialise and set our struct + variables
        //------------------------------------------------------------------------
        var obj = new ObjectInfo();
        objects.Add(obj);

        // Starting positions, can be used to reset the object
        obj.startX = x << 8;        
        obj.startY = y << 8;
        
        // Object class: defines the attributes and AI, unless otherwise specified
        obj.objClass = objClass;

        // Coordinates, X and Y
        obj.x = x << 8;
        obj.y = y << 8;

        // Center positions, very useful for small sprites in a large canvas
        obj.UpdateCentre();

        // Sprite number and rotation slot
        obj.sprite = getSprite();
        //obj.rotsetSlot = getRotsetSlot();

        // Misc. stuff
        obj.cpuStopPos = new Random().Next(15, 196);
        obj.moveDirection = -1;
        obj.activated = false;
        obj.loaded = true;
        if(type > 0) obj.typeOverride = type;

        // Anything else to initalise
        obj.vy = 0;

    // Load the object's sprite and palette
    //------------------------------------------------------------------------
        PA.CreateSprite(MAINSCREEN, obj.sprite, 32, 32, objClass.sprite);
        setSpriteXY(MAINSCREEN, obj.sprite, x-(Camera.camera.x>>8), y - (Camera.camera.y>>8));
        obj.alive = true;
    }

    public static int getSprite() {
        int i;
        for(i=32;i<128;i++){
            if(sprite[i] == 0){
                sprite[i] = 1;
                return i;
            }           
        }
        return -1;
    }

    //----------------------------------------------------------------------------
    public static void processObjects() {
    //----------------------------------------------------------------------------
        for(int i = 0; i < MAXOBJECTS+1; i++)
        {
            currentObject = i;
            ObjectInfo obj = objects[currentObject];
            obj.UpdateCentre();
            obj.Update();
        }
    }

    public static void MoveSprites() {
        for(int i = 0; i < MAXOBJECTS+1; i++)
        {
            currentObject = i;
            ObjectInfo obj = objects[currentObject];
            obj.UpdateCentre();
            setSpriteXY(MAINSCREEN, obj.sprite, (obj.x-Camera.camera.x)>>8, (obj.y-Camera.camera.y)>>8);
        }
        
    }
}
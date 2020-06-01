using System;
using System.Collections.Generic;

using static Collisions;
using static Functions;
using static Defines;
using static Levels;
using static GlobalCamera;

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

    public ObjectInfo(ObjectClass objClass, int x, int y, int type) {
        // Initialise and set our struct + variables
        //------------------------------------------------------------------------

        // Starting positions, can be used to reset the object
        startX = x << 8;        
        startY = y << 8;
        
        // Object class: defines the attributes and AI, unless otherwise specified
        this.objClass = objClass;

        // Coordinates, X and Y
        this.x = x << 8;
        this.y = y << 8;

        // Center positions, very useful for small sprites in a large canvas
        UpdateCentre();

        // Sprite number and rotation slot
        sprite = Objects.getSprite();
        //rotsetSlot = getRotsetSlot();

        // Misc. stuff
        cpuStopPos = new Random().Next(15, 196);
        moveDirection = -1;
        activated = false;
        loaded = true;
        if(type > 0) typeOverride = type;

        // Anything else to initalise
        vy = 0;

    // Load the object's sprite and palette
    //------------------------------------------------------------------------
        PA.CreateSprite(MAINSCREEN, sprite, 32, 32, objClass.sprite);
        setSpriteXY(MAINSCREEN, sprite, x-(GlobalCamera.camera.x>>8), y - (GlobalCamera.camera.y>>8));
        alive = true;
    }


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
    public static List<ObjectInfo> objects = new List<ObjectInfo>();

    public static int[] sprite = new int[96];
    
    public static void AddObject(ObjectInfo obj) {
        objects.Add(obj);
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
        foreach (var obj in objects) {
            obj.UpdateCentre();
            obj.Update();
        }
    }

    public static void MoveSprites() {
        foreach (var obj in objects)
        {
            obj.UpdateCentre();
            setSpriteXY(MAINSCREEN, obj.sprite, (obj.x-GlobalCamera.camera.x)>>8, (obj.y-GlobalCamera.camera.y)>>8);
        }     
    }
}
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
        obj.cx = (x + 32) << 8;
        obj.cy = (obj.y + (64<<8)) - (obj.objClass.height << 8);

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
    public static void moveObjects() {
    //----------------------------------------------------------------------------

        int i;

        for(i = 0; i < MAXOBJECTS+1; i++)
        {
            currentObject = i;
            ObjectInfo obj = objects[currentObject];
            obj.cx = obj.x + (32 << 8);
            obj.cy = (obj.y + (64<<8)) - (obj.objClass.height << 8);

            /*if(object[currentObject].loaded)*/ 
            setSpriteXY(MAINSCREEN, obj.sprite, 
                (obj.x-Camera.camera.x)>>8, (obj.y-Camera.camera.y)>>8);
            
        }
    }

    //----------------------------------------------------------------------------
    public static void processObjects() {
    //----------------------------------------------------------------------------

        int i;

        for(i = 0; i < MAXOBJECTS+1; i++)
        {
            currentObject = i;
            ObjectInfo obj = objects[currentObject];
            obj.cx = obj.x + (32 << 8);
            obj.cy = (obj.y + (64<<8)) - (obj.objClass.height << 8);

            //if(object[currentObject].rotation < 0) object[currentObject].rotation = 511;
            //if(object[currentObject].rotation > 511) object[currentObject].rotation = 0;
            //PA_SetRotsetNoZoom(MAINSCREEN, object[currentObject].rotsetSlot, 300);

            //PA_SetSpriteRotDisable(MAINSCREEN, object[currentObject].sprite);
            //PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);

            if(obj.loaded) obj.objClass.ai(obj);
        }

    }

    //----------------------------------------------------------------------------
    public static void animateObject(ObjectInfo obj, int startFrame, int endFrame, int frameSpeed) {
    //----------------------------------------------------------------------------
        obj.frameCount++;
        if (obj.frameCount >= frameSpeed)
        {
            obj.frameCount = -1;
            obj.currentFrame++;
        }

        if (obj.currentFrame < startFrame) obj.currentFrame = startFrame;
        if (obj.currentFrame > endFrame) obj.currentFrame = startFrame;

        PA.SetSpriteAnim(MAINSCREEN, obj.sprite, obj.currentFrame);
    }

    //----------------------------------------------------------------------------
    public static void killObject(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        obj.alive = false;
        PA.SetSpriteXY(MAINSCREEN, obj.sprite, 256, 192);
    }

    //----------------------------------------------------------------------------
    public static void reviveObject(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        obj.alive = true;
        PA.SetSpriteXY(MAINSCREEN, obj.sprite, obj.x, obj.y);
    }

    //----------------------------------------------------------------------------
    public static void deleteObject(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        sprite[obj.sprite] = 0;
        obj.alive = false;
        obj.loaded = false;
        PA.DeleteSprite(MAINSCREEN, obj.sprite);
    }

    //----------------------------------------------------------------------------
    public static void objectCheckCollision(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        if(rightCollision(currentObject)) 
        {
            if(obj.relspeedx <= -256) obj.x += obj.relspeedx;
            else obj.x -= 256;
        }

        if(leftCollision(currentObject))
        {
            if(obj.relspeedx >= 256) obj.x += obj.relspeedx;
            else obj.x += 256;
        }

        if(upCollision(currentObject)) 
        {
            if(obj.relspeedy <= -256) obj.y -= obj.relspeedy;
            else obj.y += 256;
            obj.vy = 0;
        }

        if(downCollision(currentObject))
        {
            if(obj.relspeedy >= 256) obj.y -= obj.relspeedy;
            else obj.y -= 512;
            obj.action = 0;
        }




        //if(touchingGround(currentObject)) obj.vy = 0;

    }

    //----------------------------------------------------------------------------
    public static void objectAddGravity(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        obj.y += obj.vy;
        if(!touchingGround(currentObject) && obj.vy < currentWorld.level[currentLevel].gravity) obj.vy += obj.objClass.weight;
        else if(touchingGround(currentObject)) obj.vy = 0;
    }

    //----------------------------------------------------------------------------
    public static bool inStageZone(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        if (obj.y>>8 > currentWorld.level[currentLevel].height + 256) return false;
        return true;
    }

    //----------------------------------------------------------------------------
    public static bool objectInCanvas(ObjectInfo obj) {
    //----------------------------------------------------------------------------
        if ((obj.x-camera.x)>>8 > 256 || (obj.y-camera.y)>>8 > 192 || (obj.x-camera.x)>>8 < -64 || (obj.y-camera.y)>>8 < -64) return false;
        return true;
    }
}
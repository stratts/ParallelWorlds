using System;
using System.Collections.Generic;
using static Collisions;
using static Defines;
using static Functions;
using static Levels;

class ObjectInfo
{
    public float x, y;
    public float cx, cy;
    public float vy, vx;
    public ObjectClass objClass;
    public int sprite;
    public int currentFrame, frameCount;
    public bool alive;
    public float oldposx, newposx, relspeedx;
    public float oldposy, newposy, relspeedy;
    public float startX, startY;
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

    public static int[] sprites = new int[96];

    public static int getSprite()
    {
        int i;
        for (i = 32; i < 128; i++)
        {
            if (sprites[i] == 0)
            {
                sprites[i] = 1;
                return i;
            }
        }
        return -1;
    }

    public ObjectInfo(ObjectClass objClass, float x, float y, int type)
    {
        // Initialise and set our struct + variables
        //------------------------------------------------------------------------

        // Starting positions, can be used to reset the object
        startX = x;
        startY = y;

        // Object class: defines the attributes and AI, unless otherwise specified
        this.objClass = objClass;

        // Coordinates, X and Y
        this.x = x;
        this.y = y;

        // Center positions, very useful for small sprites in a large canvas
        UpdateCentre();

        // Sprite number and rotation slot
        sprite = getSprite();
        //rotsetSlot = getRotsetSlot();

        // Misc. stuff
        cpuStopPos = new Random().Next(15, 196);
        moveDirection = -1;
        activated = false;
        loaded = true;
        if (type > 0) typeOverride = type;

        // Anything else to initalise
        vy = 0;

        // Load the object's sprite and palette
        //------------------------------------------------------------------------
        PA.CreateSprite(MAINSCREEN, sprite, 32, 32, objClass.spriteOffset, objClass.sprite);
        alive = true;
    }


    public void Update(Scene scene)
    {
        UpdateCentre();
        if (loaded) objClass.ai(this, scene);
    }

    public void UpdateCentre()
    {
        cx = x + objClass.width / 2;
        cy = y + objClass.height / 2;
    }

    public void Kill()
    {
        alive = false;
        PA.SetSpriteXY(MAINSCREEN, sprite, 256, 192);
    }

    public void Revive()
    {
        alive = true;
        PA.SetSpriteXY(MAINSCREEN, sprite, x, y);
    }

    public void Delete()
    {
        sprites[this.sprite] = 0;
        alive = false;
        loaded = false;
        PA.DeleteSprite(MAINSCREEN, sprite);
    }

    public void Animate(Animation animation)
    {
        var (startFrame, endFrame) = objClass.GetFrames(animation);
        int frameSpeed = objClass.animSpeed;
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

    public void CheckCollision()
    {
        if (rightCollision(this))
        {
            if (relspeedx <= -1) x += relspeedx;
            else x -= 1;
        }

        if (leftCollision(this))
        {
            if (relspeedx >= 1) x += relspeedx;
            else x += 1;
        }

        if (upCollision(this))
        {
            if (relspeedy <= -1) y -= relspeedy;
            else y += 1;
            vy = 0;
        }

        if (downCollision(this))
        {
            if (relspeedy >= 1) y -= relspeedy;
            else y -= 2;
            action = 0;
        }
    }

    public void AddGravity()
    {
        y += vy;
        if (!touchingGround(this) && vy < (float)currentLevel.gravity / 256) vy += objClass.weight;
        else if (touchingGround(this)) vy = 0;
    }

    public bool InStageZone()
    {
        if (y > currentLevel.height + 256) return false;
        return true;
    }
}

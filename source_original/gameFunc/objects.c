//----------------------------------------------------------------------------
// 'objects.c' - Object functions and tools
//----------------------------------------------------------------------------

// Includes
//----------------------------------------------------------------------------
#include <PA9.h>            // Include PAlib
#include "defines.h"        // Global variables/defines

#include "classes.h"        // Object types/classes
#include "objects.h"        // Include for object functions + struct
#include "ai.h"             // AI functions

#include "collisions.h"     // Collision functions
#include "level.h"          // Level/world struct + loading functions
#include "camera.h"         // Camera system and coordinates

#include "functions.h"      // Misc. functions
#include "cacaoLib.h"       // More functions, from cacaoOS



int MAXOBJECTS = -1;        // Object count, starts at nil (-1)

//----------------------------------------------------------------------------
void createObject(objectClass* class, int x, int y, int type) {
//----------------------------------------------------------------------------

    MAXOBJECTS++;           // Increase object count

    // Initialise and set our struct + variables
    //------------------------------------------------------------------------

        // Starting positions, can be used to reset the object
        object[MAXOBJECTS].startX = x << 8;     
        object[MAXOBJECTS].startY = y << 8;
        
        // Object class: defines the attributes and AI, unless otherwise specified
        object[MAXOBJECTS].class = class;

        // Coordinates, X and Y
        object[MAXOBJECTS].x = x << 8;
        object[MAXOBJECTS].y = y << 8;

        // Center positions, very useful for small sprites in a large canvas
        object[MAXOBJECTS].cx = (x + 32) << 8;
        object[MAXOBJECTS].cy = (object[currentObject].y + (64<<8)) - (object[MAXOBJECTS].class->height << 8);

        // Sprite number and rotation slot
        object[MAXOBJECTS].sprite = getSprite();
        //object[MAXOBJECTS].rotsetSlot = getRotsetSlot();

        // Misc. stuff
        object[MAXOBJECTS].cpuStopPos = PA_RandMinMax(15, 196);
        object[MAXOBJECTS].moveDirection = -1;
        object[MAXOBJECTS].activated = false;
        object[MAXOBJECTS].loaded = true;
        if(type) object[MAXOBJECTS].typeOverride = type;

        // Anything else to initalise
        object[MAXOBJECTS].vy = 0;


    // Load the object's sprite and palette
    //------------------------------------------------------------------------

        // Get a number for the palette if it hasn't already been loaded 
        if(object[MAXOBJECTS].class->palTaken == 0)
        {
            object[MAXOBJECTS].class->palTaken = 1;
            object[MAXOBJECTS].class->palNum = getPal();
        }   

        // Buffers and path strings
        char sprite_[1024];
        char palette_[1024];

        // Set the path of the data and load it into the buffer
        if(!object[MAXOBJECTS].class->spritebuf)
        {
            sprintf(sprite_, "%s%s", object[MAXOBJECTS].class->sprite, "_Sprite.bin");
            sprintf(palette_, "%s%s", object[MAXOBJECTS].class->sprite, "_Pal.bin");
            object[MAXOBJECTS].class->spritebuf = (u16*) FAT_LoadData(sprite_, NULL);
            object[MAXOBJECTS].class->palbuf = (u16*) FAT_LoadData(palette_, NULL);
        }

        // Flush the cache: else there would be glitches
        DC_FlushAll();


    // Create the object
    //------------------------------------------------------------------------

        PA_LoadSpritePal(MAINSCREEN, object[MAXOBJECTS].class->palNum, (void*)object[MAXOBJECTS].class->palbuf);
        PA_CreateSprite(MAINSCREEN, object[MAXOBJECTS].sprite, object[MAXOBJECTS].class->spritebuf, OBJ_SIZE_64X64, 1, object[MAXOBJECTS].class->palNum, x - (camera.x>>8), y - (camera.y>>8));
        PA_SetSpritePrio(MAINSCREEN, object[MAXOBJECTS].sprite, 1);
        setSpriteXY(MAINSCREEN, object[MAXOBJECTS].sprite, x-(camera.x>>8), y - (camera.y>>8));
        //PA_SetSpriteRotEnable(MAINSCREEN, object[MAXOBJECTS].sprite, object[MAXOBJECTS].rotsetSlot);

        object[MAXOBJECTS].alive = true;
}

//----------------------------------------------------------------------------
void processObjects() {
//----------------------------------------------------------------------------

    int i;

    for(i = 0; i < MAXOBJECTS+1; i++)
    {
        currentObject = i;
        object[currentObject].cx = object[currentObject].x + (32 << 8);
        object[currentObject].cy = (object[currentObject].y + (64<<8)) - (object[currentObject].class->height << 8);

        //if(object[currentObject].rotation < 0) object[currentObject].rotation = 511;
        //if(object[currentObject].rotation > 511) object[currentObject].rotation = 0;
        //PA_SetRotsetNoZoom(MAINSCREEN, object[currentObject].rotsetSlot, 300);

        //PA_SetSpriteRotDisable(MAINSCREEN, object[currentObject].sprite);
                //PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);

        if(object[currentObject].loaded) object[currentObject].class->ai();


    }

}

//----------------------------------------------------------------------------
void moveObjects() {
//----------------------------------------------------------------------------

    int i;

    for(i = 0; i < MAXOBJECTS+1; i++)
    {
        currentObject = i;
        object[currentObject].cx = object[currentObject].x + (32 << 8);
        object[currentObject].cy = (object[currentObject].y + (64<<8)) - (object[currentObject].class->height << 8);

        /*if(object[currentObject].loaded)*/ setSpriteXY(MAINSCREEN, object[currentObject].sprite, (object[currentObject].x-camera.x)>>8, (object[currentObject].y-camera.y)>>8);
        
    }
}
    
//----------------------------------------------------------------------------
int getSprite() {
//----------------------------------------------------------------------------

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
int getPal() {
//----------------------------------------------------------------------------

    int i;
    for(i=6;i<16;i++){
        if(palette[i] == 0){
            palette[i] = 1;
            return i;
        }           
    }
    return -1;
}

//----------------------------------------------------------------------------
void animateObject(int objectNum, int startFrame, int endFrame, int frameSpeed) {
//----------------------------------------------------------------------------

    object[objectNum].frameCount++;
    if (object[objectNum].frameCount >= frameSpeed)
    {
        object[objectNum].frameCount = -1;
        object[objectNum].currentFrame++;
    }

    if (object[objectNum].currentFrame < startFrame) object[objectNum].currentFrame = startFrame;
    if (object[objectNum].currentFrame > endFrame) object[objectNum].currentFrame = startFrame;

    PA_SetSpriteAnim(MAINSCREEN, object[objectNum].sprite, object[objectNum].currentFrame);
}
    
//----------------------------------------------------------------------------
void killObject(int objectNum) {
//----------------------------------------------------------------------------

    object[objectNum].alive = false;
    PA_SetSpriteXY(MAINSCREEN, object[objectNum].sprite, 256, 192);
}

//----------------------------------------------------------------------------
void reviveObject(int objectNum) {
//----------------------------------------------------------------------------

    object[objectNum].alive = true;
    PA_SetSpriteXY(MAINSCREEN, object[objectNum].sprite, object[objectNum].x, object[objectNum].y);
}

//----------------------------------------------------------------------------
void deleteObject(int objectNum) {
//----------------------------------------------------------------------------

    sprite[object[objectNum].sprite] = 0;
    object[objectNum].alive = false;
    object[objectNum].loaded = false;
    PA_DeleteSprite(MAINSCREEN, object[objectNum].sprite);
}

//----------------------------------------------------------------------------
void objectCheckCollision() {
//----------------------------------------------------------------------------

    if(rightCollision(currentObject)) 
    {
        if(object[currentObject].relspeedx <= -256) object[currentObject].x += object[currentObject].relspeedx;
        else object[currentObject].x -= 256;
    }

    if(leftCollision(currentObject))
    {
        if(object[currentObject].relspeedx >= 256) object[currentObject].x += object[currentObject].relspeedx;
        else object[currentObject].x += 256;
    }

    if(upCollision(currentObject)) 
    {
        if(object[currentObject].relspeedy <= -256) object[currentObject].y -= object[currentObject].relspeedy;
        else object[currentObject].y += 256;
        object[currentObject].vy = 0;
    }

    if(downCollision(currentObject))
    {
        if(object[currentObject].relspeedy >= 256) object[currentObject].y -= object[currentObject].relspeedy;
        else object[currentObject].y -= 512;
        object[currentObject].action = 0;
    }




    //if(touchingGround(currentObject)) object[currentObject].vy = 0;

}

//----------------------------------------------------------------------------
void objectAddGravity() {
//----------------------------------------------------------------------------

    object[currentObject].y += object[currentObject].vy;
    if(!touchingGround(currentObject) && object[currentObject].vy < currentWorld->level[currentLevel].gravity) object[currentObject].vy += object[currentObject].class->weight;
    else if(touchingGround(currentObject)) object[currentObject].vy = 0;
}

//----------------------------------------------------------------------------
bool inStageZone(int objectNum) {
//----------------------------------------------------------------------------

    if (object[objectNum].y>>8 > currentWorld->level[currentLevel].height + 256) return 0;
    return 1;
}

//----------------------------------------------------------------------------
bool objectInCanvas(int objectNum) {
//----------------------------------------------------------------------------

    if ((object[objectNum].x-camera.x)>>8 > 256 || (object[objectNum].y-camera.y)>>8 > 192 || (object[objectNum].x-camera.x)>>8 < -64 || (object[objectNum].y-camera.y)>>8 < -64) return 0;
    return 1;
}


    




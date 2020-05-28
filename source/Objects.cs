using System;

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

    public static ObjectInfo[] objects = new ObjectInfo[96];

    public static int currentObject;
    public static int[] sprite = new int[96];
    
    public static void createObject(ObjectClass objClass, int x, int y, int type) {
        MAXOBJECTS++;			// Increase object count

        // Initialise and set our struct + variables
        //------------------------------------------------------------------------
		objects[MAXOBJECTS] = new ObjectInfo();

		// Starting positions, can be used to reset the object
		objects[MAXOBJECTS].startX = x << 8;		
		objects[MAXOBJECTS].startY = y << 8;
		
		// Object class: defines the attributes and AI, unless otherwise specified
		objects[MAXOBJECTS].objClass = objClass;

		// Coordinates, X and Y
		objects[MAXOBJECTS].x = x << 8;
		objects[MAXOBJECTS].y = y << 8;

		// Center positions, very useful for small sprites in a large canvas
		objects[MAXOBJECTS].cx = (x + 32) << 8;
		objects[MAXOBJECTS].cy = (objects[currentObject].y + (64<<8)) - (objects[MAXOBJECTS].objClass.height << 8);

		// Sprite number and rotation slot
		objects[MAXOBJECTS].sprite = getSprite();
		//objects[MAXOBJECTS].rotsetSlot = getRotsetSlot();

		// Misc. stuff
		objects[MAXOBJECTS].cpuStopPos = new Random().Next(15, 196);
		objects[MAXOBJECTS].moveDirection = -1;
		objects[MAXOBJECTS].activated = false;
		objects[MAXOBJECTS].loaded = true;
		if(type > 0) objects[MAXOBJECTS].typeOverride = type;

		// Anything else to initalise
		objects[MAXOBJECTS].vy = 0;

        Console.WriteLine($"Create {objClass.name} at {x}, {y}");
	// Load the object's sprite and palette
	//------------------------------------------------------------------------

		/*// Get a number for the palette if it hasn't already been loaded 
		if(objects[MAXOBJECTS].objClass.palTaken == 0)
		{
			objects[MAXOBJECTS].objClass.palTaken = 1;
			objects[MAXOBJECTS].objClass.palNum = getPal();
		}	

		// Buffers and path strings
		char sprite_[1024];
		char palette_[1024];

		// Set the path of the data and load it into the buffer
		if(!objects[MAXOBJECTS].objClass.spritebuf)
		{
			sprintf(sprite_, "%s%s", objects[MAXOBJECTS].objClass.sprite, "_Sprite.bin");
			sprintf(palette_, "%s%s", objects[MAXOBJECTS].objClass.sprite, "_Pal.bin");
			objects[MAXOBJECTS].objClass.spritebuf = (u16*) FAT_LoadData(sprite_, NULL);
			objects[MAXOBJECTS].objClass.palbuf = (u16*) FAT_LoadData(palette_, NULL);
		}

		// Flush the cache: else there would be glitches
		DC_FlushAll();*/


	// Create the object
	//------------------------------------------------------------------------

		/*PA_LoadSpritePal(MAINSCREEN, objects[MAXOBJECTS].objClass.palNum, (void*)objects[MAXOBJECTS].objClass.palbuf);
		PA_CreateSprite(MAINSCREEN, objects[MAXOBJECTS].sprite, objects[MAXOBJECTS].objClass.spritebuf, OBJ_SIZE_64X64, 1, objects[MAXOBJECTS].objClass.palNum, x - (camera.x>>8), y - (camera.y>>8));
		PA_SetSpritePrio(MAINSCREEN, objects[MAXOBJECTS].sprite, 1);*/
		Functions.setSpriteXY(Globals.MAINSCREEN, objects[MAXOBJECTS].sprite, x-(Camera.camera.x>>8), y - (Camera.camera.y>>8));

		objects[MAXOBJECTS].alive = true;
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
			objects[currentObject].cx = objects[currentObject].x + (32 << 8);
			objects[currentObject].cy = (objects[currentObject].y + (64<<8)) - (objects[currentObject].objClass.height << 8);

			/*if(object[currentObject].loaded)*/ 
			Functions.setSpriteXY(Globals.MAINSCREEN, objects[currentObject].sprite, 
				(objects[currentObject].x-Camera.camera.x)>>8, (objects[currentObject].y-Camera.camera.y)>>8);
			
		}
	}

	//----------------------------------------------------------------------------
	public static void processObjects() {
	//----------------------------------------------------------------------------

		int i;

		for(i = 0; i < MAXOBJECTS+1; i++)
		{
			currentObject = i;
			objects[currentObject].cx = objects[currentObject].x + (32 << 8);
			objects[currentObject].cy = (objects[currentObject].y + (64<<8)) - (objects[currentObject].objClass.height << 8);

			//if(object[currentObject].rotation < 0) object[currentObject].rotation = 511;
			//if(object[currentObject].rotation > 511) object[currentObject].rotation = 0;
			//PA_SetRotsetNoZoom(MAINSCREEN, object[currentObject].rotsetSlot, 300);

			//PA_SetSpriteRotDisable(MAINSCREEN, object[currentObject].sprite);
					//PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);

			if(objects[currentObject].loaded) objects[currentObject].objClass.ai();
		}

	}

	//----------------------------------------------------------------------------
	public static void animateObject(int objectNum, int startFrame, int endFrame, int frameSpeed) {
	//----------------------------------------------------------------------------

		objects[objectNum].frameCount++;
		if (objects[objectNum].frameCount >= frameSpeed)
		{
			objects[objectNum].frameCount = -1;
			objects[objectNum].currentFrame++;
		}

		if (objects[objectNum].currentFrame < startFrame) objects[objectNum].currentFrame = startFrame;
		if (objects[objectNum].currentFrame > endFrame) objects[objectNum].currentFrame = startFrame;

		PA.SetSpriteAnim(Globals.MAINSCREEN, objects[objectNum].sprite, objects[objectNum].currentFrame);
	}

	//----------------------------------------------------------------------------
	public static void killObject(int objectNum) {
	//----------------------------------------------------------------------------

		objects[objectNum].alive = false;
		PA.SetSpriteXY(Globals.MAINSCREEN, objects[objectNum].sprite, 256, 192);
	}

	//----------------------------------------------------------------------------
	public static void reviveObject(int objectNum) {
	//----------------------------------------------------------------------------

		objects[objectNum].alive = true;
		PA.SetSpriteXY(Globals.MAINSCREEN, objects[objectNum].sprite, objects[objectNum].x, objects[objectNum].y);
	}

	//----------------------------------------------------------------------------
	public static void deleteObject(int objectNum) {
	//----------------------------------------------------------------------------

		sprite[objects[objectNum].sprite] = 0;
		objects[objectNum].alive = false;
		objects[objectNum].loaded = false;
		PA.DeleteSprite(Globals.MAINSCREEN, objects[objectNum].sprite);
	}

	//----------------------------------------------------------------------------
	public static void objectCheckCollision() {
	//----------------------------------------------------------------------------

		if(Collisions.rightCollision(currentObject)) 
		{
			if(objects[currentObject].relspeedx <= -256) objects[currentObject].x += objects[currentObject].relspeedx;
			else objects[currentObject].x -= 256;
		}

		if(Collisions.leftCollision(currentObject))
		{
			if(objects[currentObject].relspeedx >= 256) objects[currentObject].x += objects[currentObject].relspeedx;
			else objects[currentObject].x += 256;
		}

		if(Collisions.upCollision(currentObject)) 
		{
			if(objects[currentObject].relspeedy <= -256) objects[currentObject].y -= objects[currentObject].relspeedy;
			else objects[currentObject].y += 256;
			objects[currentObject].vy = 0;
		}

		if(Collisions.downCollision(currentObject))
		{
			if(objects[currentObject].relspeedy >= 256) objects[currentObject].y -= objects[currentObject].relspeedy;
			else objects[currentObject].y -= 512;
			objects[currentObject].action = 0;
		}




		//if(touchingGround(currentObject)) objects[currentObject].vy = 0;

	}

	//----------------------------------------------------------------------------
	public static void objectAddGravity() {
	//----------------------------------------------------------------------------
		var currentWorld = Levels.currentWorld;
		var currentLevel = Levels.currentLevel;
		objects[currentObject].y += objects[currentObject].vy;
		if(!Collisions.touchingGround(currentObject) && objects[currentObject].vy < currentWorld.level[currentLevel].gravity) objects[currentObject].vy += objects[currentObject].objClass.weight;
		else if(Collisions.touchingGround(currentObject)) objects[currentObject].vy = 0;
	}

	//----------------------------------------------------------------------------
	public static bool inStageZone(int objectNum) {
	//----------------------------------------------------------------------------
		var currentWorld = Levels.currentWorld;
		var currentLevel = Levels.currentLevel;
		if (objects[objectNum].y>>8 > currentWorld.level[currentLevel].height + 256) return false;
		return true;
	}

	//----------------------------------------------------------------------------
	public static bool objectInCanvas(int objectNum) {
	//----------------------------------------------------------------------------
		var camera = Camera.camera;
		if ((objects[objectNum].x-camera.x)>>8 > 256 || (objects[objectNum].y-camera.y)>>8 > 192 || (objects[objectNum].x-camera.x)>>8 < -64 || (objects[objectNum].y-camera.y)>>8 < -64) return false;
		return true;
	}
}
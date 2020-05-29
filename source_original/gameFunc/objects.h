#ifndef OBJECTSENG__
#define OBJECTSENG__

#include <PA9.h>
#include "classes.h"

int sprite[96];
int palette[10];
int MAXOBJECTS;
int currentObject;

typedef struct
{
	int x, y;
	int cx, cy;
	int vy, vx;
	objectClass* class;
    int sprite;
	int currentFrame, frameCount;
	bool alive;
	int oldposx, newposx, relspeedx;
	int oldposy, newposy, relspeedy;
	int startX, startY;
	int moveDirection;
	bool jumping;
	int action;
	int cpuStopPos;
	int typeOverride;
	int i;
	bool activated;
	int rotation;
	int rotsetSlot;
	bool loaded;
} objectInfo;



objectInfo object[96];
u16 gfx[96];
u16 nsprites;

u16 *spritebuf;
		u16 *palbuf;

void createObject(objectClass* class, int x, int y, int type);

void deleteObject(int objectNum);
void killObject(int objectNum);
void reviveObject(int objectNum);

void setObjects();
void processObjects();
void moveObjects();
void animateObject(int objectNum, int startFrame, int endFrame, int frameSpeed);
void objectCheckCollision();
void objectAddGravity();

bool inStageZone(int objectNum);
bool objectInCanvas(int objectNum);

int getSprite();
int getPal();


#endif

#ifndef LEVELENG__
#define LEVELENG__

#include <maxmod9.h>

//#include "soundbank_bin.h"
#include "soundbank.h"

#define WIDTH 0;
#define HEIGHT 1;

#define NO_MUSIC 123456

typedef struct
{
    char name[512];
    char music[512];
    char stagebg[512], midbg[512], backbg[512];
    char collision[512];
    int width, height, friction, gravity;
    int midscroll, backscroll;
} levelinfo;

typedef struct
{
    char name[512];
    levelinfo level[128];
} worldinfo;

worldinfo world[128];

worldinfo* currentWorld;
int currentLevel;

void setLevels();
void loadLevel(worldinfo* world, int levelNum);
bool getCollisionPix(int screen, int bglayer, int x, int y);




#endif

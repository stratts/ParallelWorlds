#include "collisions.h"
#include "defines.h"
#include "objects.h"
#include "level.h"

bool objectCollision(int object1,int object2){
//grab sprite demensions (center positions and length and width)
  int w1 = object[object1].class->width;
  int h1 = object[object1].class->height;
  int x1 = object[object1].cx>>8;
  int y1 = object[object1].cy>>8;

  int w2 = object[object2].class->width;
  int h2 = object[object2].class->height;
  int x2 = object[object2].cx>>8;
  int y2 = object[object2].cy>>8;
  
  if(object1 != object2) 
      return (((x2 >= x1 - ((w1 + w2)>>1)) && (x2 <= x1 + ((w1 + w2)>>1)) && (y2 >= y1 - ((h1 + h2)>>1)) && (y2 <= y1 + ((h1 + h2)>>1))));
  return 0;
}

bool objectCollisionTop(int object1,int object2){
//grab sprite demensions (center positions and length and width)
  int w1 = object[object1].class->width;
  int h1 = object[object1].class->height;
  int x1 = object[object1].cx>>8;
  int y1 = object[object1].cy>>8;

  int w2 = object[object2].class->width;
  int h2 = object[object2].class->height;
  int x2 = object[object2].cx>>8;
  int y2 = object[object2].cy>>8;

  if(object1 != object2 && (y1+(h1>>1))-h1 < y2 - (h2))
      return (((x2 >= x1 - ((w1 + w2)>>1)) && (x2 <= x1 + ((w1 + w2)>>1)) && (y2 >= y1 - ((h1 + h2)>>1)) && (y2 <= y1 + ((h1 + h2)>>1))));

  return 0;
}


bool upCollision(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 62;

    if(getCollisionPix(MISCSCREEN,3,playerx,playery-playerheight))return 1;
    if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery-playerheight))return 1;
    if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery-playerheight))return 1;
    return 0;


}

bool downCollision(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 62;

    
    if(getCollisionPix(MISCSCREEN,3,playerx,playery))return 1;
    if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery))return 1;
    if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery))return 1;
    return 0;
}

bool touchingGround(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 63;

    if(getCollisionPix(MISCSCREEN,3,playerx,playery))return 1;
    if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery))return 1;
    if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery))return 1;
    return 0;
}

bool leftCollision(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 62;

    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-COLLISION_BORDER))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>1)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>2)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+(playerheight>>2)+(playerheight>>3)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+(playerheight>>3)))return 1;
    return 0;
}

bool rightCollision(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 62;


    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-COLLISION_BORDER))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>1)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>2)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+(playerheight>>2)+(playerheight>>3)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+(playerheight>>3)))return 1;
    return 0;
}

bool leftCollisionLarge(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 24;

    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-COLLISION_BORDER))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>1)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return 1;
    return 0;
}

bool rightCollisionLarge(int objectNum)
{
    if(!object[objectNum].alive) return 0;

    s8 playerheight=object[objectNum].class->height;
    s8 playerwidth=object[objectNum].class->width;
    int playerx = object[objectNum].cx>>8;
    int playery = (object[objectNum].y>>8) + 32;


    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-COLLISION_BORDER))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>1)))return 1;
    if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return 1;
    return 0;
}

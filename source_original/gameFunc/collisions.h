#ifndef COLLISIONS__
#define COLLISIONS__
#include <PA9.h>

bool objectCollision(int object1,int object2);
bool objectCollisionTop(int object1,int object2);
bool leftCollision(int objectNum);
bool rightCollision(int objectNum);
bool leftCollisionLarge(int objectNum);
bool rightCollisionLarge(int objectNum);
bool upCollision(int objectNum);
bool downCollision(int objectNum);
bool touchingGround(int objectNum);

#endif

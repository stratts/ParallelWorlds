#ifndef __CLASSENG_
#define __CLASSENG_

#define STATIC 0
#define DYNAMIC 1


#include <PA9.h>

typedef struct
{
	char name[48];
	char sprite[48];
	int speed, weight, jumpPower, width, height;
	int idle[2];
	int walk[2], run[2], hurt[2], jump[2], fall[2];
	int animSpeed;
	int type;
	void (*ai)();
	int palTaken, palNum;
	bool created;
	int createdSprite;
	u16 *spritebuf, *palbuf;
} objectClass;

//--------------------------------------------------------------
// Start defining characters
//--------------------------------------------------------------
	objectClass MARIO;
	objectClass DUMMY;
	objectClass LINK;
//--------------------------------------------------------------

static objectClass* classes[] =
{
	&LINK, &MARIO, &DUMMY
};






#endif

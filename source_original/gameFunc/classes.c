#include "classes.h"
#include "objects.h"
#include "ai.h"
#include "cacaoLib.h"

void setObjects()
{

	// Mario
	//--------------------------------------------------------------------------------
	strcpy(MARIO.name, "Mario");
	strcpy(MARIO.sprite, rootf("/characters/Mario/mario"));
	MARIO.speed = 512;
    MARIO.weight = 80;
	MARIO.ai = &generalCharacter;
	MARIO.width = 27;
	MARIO.height = 36;
	MARIO.animSpeed = 7;
	MARIO.idle[0] = 0;
	MARIO.idle[1] = 3;
	MARIO.walk[0] = 4;
	MARIO.walk[1] = 11;
	MARIO.run[0] = 0;
	MARIO.run[1] = 0;
	MARIO.jump[0] = 12;
	MARIO.jump[1] = 12;
	MARIO.fall[0] = 19;
	MARIO.fall[1] = 20;

	strcpy(DUMMY.name, "Mario");
	strcpy(DUMMY.sprite, rootf("/characters/Mario/mario"));
	DUMMY.speed = 256;
    DUMMY.weight = 80;
	DUMMY.ai = &aiGenericGround;
	DUMMY.width = 27;
	DUMMY.height = 40;
	DUMMY.animSpeed = 7;
	DUMMY.idle[0] = 0;
	DUMMY.idle[1] = 3;
	DUMMY.walk[0] = 4;
	DUMMY.walk[1] = 11;

}



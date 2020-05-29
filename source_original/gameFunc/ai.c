#include <PA9.h>
#include "ai.h"
#include "defines.h"
#include "objects.h"
#include "camera.h"
#include "functions.h"
#include "level.h"
#include "collisions.h"

void generalCharacter()
{
	object[currentObject].oldposx = object[currentObject].x;

	if(Pad.Held.Left || Pad.Held.Right)
	{
		//--------------------------------------------------------------------------------
		// Basic Movement Code
		//--------------------------------------------------------------------------------

		// Moving left
		if(Pad.Held.Left)
		{
			PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);

				object[currentObject].x -= object[currentObject].class->speed;
				if(object[currentObject].action == 0)
					animateObject(currentObject, 
								object[currentObject].class->walk[0], 
								object[currentObject].class->walk[1], 
								object[currentObject].class->animSpeed);
			

		}

		// Moving right
		if(Pad.Held.Right)
		{
			PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 0);

				object[currentObject].x += object[currentObject].class->speed;
				if(object[currentObject].action == 0)
					animateObject(currentObject, 
									object[currentObject].class->walk[0], 
									object[currentObject].class->walk[1], 
									object[currentObject].class->animSpeed);

		}



		// End movement code
	}

	else if(object[currentObject].action == 0)
	{
			animateObject(currentObject, 
								object[currentObject].class->idle[0], 
								object[currentObject].class->idle[1], 
								object[currentObject].class->animSpeed);
	}


	objectAddGravity();

	if(touchingGround(currentObject)) 
	{
		object[currentObject].vy = 0;
		object[currentObject].action = 0;
		object[currentObject].jumping = false;	
	}

	if(Pad.Newpress.Up && touchingGround(currentObject)) 
	{
		object[currentObject].action = 1;
		object[currentObject].vy = -1800;
		object[currentObject].jumping = true;
	}

	if(object[currentObject].vy > 512) object[currentObject].action = 2;
	else if(object[currentObject].jumping && object[currentObject].vy > 0) object[currentObject].action = 2;

	if(object[currentObject].action == 1) animateObject(currentObject, 
												object[currentObject].class->jump[0], 
												object[currentObject].class->jump[1], 
												object[currentObject].class->animSpeed);
				

	else if(object[currentObject].action == 2)animateObject(currentObject, 
													object[currentObject].class->fall[0], 
													object[currentObject].class->fall[1], 
													object[currentObject].class->animSpeed);
				


	object[currentObject].newposx = object[currentObject].x;
	object[currentObject].relspeedx = object[currentObject].oldposx - object[currentObject].newposx;
	object[currentObject].relspeedy = object[currentObject].vy;

	objectCheckCollision();

	if (!inStageZone(currentObject))
	{
		camera.x = 0;
		camera.y = 0;
		object[currentObject].y = object[currentObject].startY;
		object[currentObject].x = object[currentObject].startX;
	}

	setSpriteXY(MAINSCREEN, object[currentObject].sprite, (object[currentObject].x - camera.x)>>8, (object[currentObject].y - camera.y)>>8);

}

void generalCPU()
{
	object[currentObject].oldposx = object[currentObject].x;

	if((object[lowestXinObj(object, 3)].x - object[currentObject].x)>>8 < -(object[lowestXinObj(object, 3)].class->width+2) || (object[lowestXinObj(object, 3)].x - object[currentObject].x)>>8 > object[lowestXinObj(object, 3)].class->width+2)
	{
		if(object[lowestXinObj(object, 3)].x > object[currentObject].x)
		{
			PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 0);
			if(!object[currentObject].jumping) object[currentObject].action = 1;
			object[currentObject].x += object[currentObject].class->speed-PA_RandMax(128);
		}

		else if(object[lowestXinObj(object, 3)].x < object[currentObject].x)
		{	
			PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);
			if(!object[currentObject].jumping) object[currentObject].action = 1;
			object[currentObject].x -= object[currentObject].class->speed-PA_RandMax(128);
		}

		/*if(((object[lowestXinObj(object, 3)].x - object[currentObject].x)>>8 < -80 || (object[lowestXinObj(object, 3)].x - object[currentObject].x)>>8 > 80) && !object[currentObject].jumping)
		{
			if((object[currentObject].y - object[lowestXinObj(object, 3)].y) >> 8 > 48) 
			{
				object[currentObject].vy = -1200;
				object[currentObject].jumping = true;
			}
		}*/

		if((leftCollision(currentObject) || rightCollision(currentObject)) && (!rightCollisionLarge(currentObject) && !leftCollisionLarge(currentObject)) && !object[currentObject].jumping) 
		{
			object[currentObject].vy = -1400;
			object[currentObject].action = 2;
			object[currentObject].jumping = true;
		}
	}

	else object[currentObject].action = 0;

	object[currentObject].y += object[currentObject].vy;
	if(!touchingGround(currentObject) && object[currentObject].vy < object[currentObject].class->weight) object[currentObject].vy += 80;
	if(!touchingGround(currentObject) && object[currentObject].vy > 512) object[currentObject].action = 3;

	

	if(touchingGround(currentObject)) 
	{
		object[currentObject].vy = 0;
		object[currentObject].jumping = false;	
	}

	switch(object[currentObject].action)
	{
		case 0:
			animateObject(currentObject, 
					object[currentObject].class->idle[0], 
					object[currentObject].class->idle[1], 
					object[currentObject].class->animSpeed); break;
		case 1:
			animateObject(currentObject, 
					object[currentObject].class->walk[0], 
					object[currentObject].class->walk[1], 
					object[currentObject].class->animSpeed); break;

		case 2:
			animateObject(currentObject, 
					object[currentObject].class->jump[0], 
					object[currentObject].class->jump[1], 
					object[currentObject].class->animSpeed); break;
		case 3:
			animateObject(currentObject, 
					object[currentObject].class->fall[0], 
					object[currentObject].class->fall[1], 
					object[currentObject].class->animSpeed); break;
	}

	object[currentObject].newposx = object[currentObject].x;
	object[currentObject].relspeedx = object[currentObject].oldposx - object[currentObject].newposx;
	object[currentObject].relspeedy = object[currentObject].vy;

	objectCheckCollision();

	if (!inStageZone(currentObject))
	{
		camera.x = 0;
		camera.y = 0;

		object[currentObject].y = object[currentObject].startY;
		object[currentObject].x = object[currentObject].startX;
	}


	setSpriteXY(MAINSCREEN, object[currentObject].sprite, (object[currentObject].x - camera.x)>>8, (object[currentObject].y - camera.y)>>8);

}

void aiGenericGround()
{
	// Only activate the AI when the object is visible
	if(objectInCanvas(currentObject)) object[currentObject].activated = true;

	// AI code
	if(object[currentObject].activated && object[currentObject].alive)
	{
		object[currentObject].x += object[currentObject].class->speed*object[currentObject].moveDirection;
		

		if(leftCollision(currentObject) || rightCollision(currentObject)) object[currentObject].i++;
		else object[currentObject].i = 0; 

		if(object[currentObject].i > 80)
		{ 
			object[currentObject].i = 0; 

			if(leftCollision(currentObject)) object[currentObject].moveDirection = 1;
			else if(rightCollision(currentObject)) object[currentObject].moveDirection = -1; 
		}

		else if(object[currentObject].i > 5)
		{ 
			object[currentObject].action = 0;

			if(object[currentObject].i > 40)
			{
				if(leftCollision(currentObject)) PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 0);
				else if(rightCollision(currentObject)) PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);
			}
		}

		else 
		{
			object[currentObject].action = 1;
			if(object[currentObject].moveDirection == -1) PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 1);
			else PA_SetSpriteHflip(MAINSCREEN, object[currentObject].sprite, 0);
		}

		

		switch(object[currentObject].action)
		{
			case 1:
				animateObject(currentObject,
					object[currentObject].class->walk[0],
					object[currentObject].class->walk[1],
					object[currentObject].class->animSpeed); break;

			default:
				animateObject(currentObject,
					object[currentObject].class->idle[0],
					object[currentObject].class->idle[1],
					object[currentObject].class->animSpeed); break;
		}

		if(objectCollisionTop(0, currentObject) && object[0].vy>0 && object[currentObject].alive) 
		{
			object[currentObject].alive = false;
			if(object[0].relspeedx >= 0) playerPoints += (object[0].vy<<2)*(object[0].relspeedx+1);
			else playerPoints += object[0].vy*-(object[0].relspeedx+1);
			if(!object[currentObject].jumping)
			{
				object[currentObject].vy = -500;
				object[0].vy = -1200;
				object[currentObject].jumping = true;
			}
		}

		if(!inStageZone(currentObject)) object[currentObject].alive = false;

	}

	else if (!object[currentObject].alive) 
	{

		if(!inStageZone(currentObject)) deleteObject(currentObject);
			
		//object[currentObject].rotation -= object[currentObject].moveDirection*5;
	}

	// Collisions and gravity
	if(object[currentObject].alive) objectCheckCollision();

	objectAddGravity();
}

void aiGenericNone()
{

}

void aiDummy()
{

	object[currentObject].oldposx = object[currentObject].x;
	object[currentObject].oldposy = object[currentObject].y;

	animateObject(currentObject, 
								object[currentObject].class->idle[0], 
								object[currentObject].class->idle[1], 
								object[currentObject].class->animSpeed);

	object[currentObject].y += object[currentObject].vy;
	if(!touchingGround(currentObject)) object[currentObject].vy += 80;

	if(touchingGround(currentObject)) object[currentObject].vy = 0;

	object[currentObject].newposx = object[currentObject].x;
	object[currentObject].newposy = object[currentObject].y;
	object[currentObject].relspeedx = object[currentObject].oldposx - object[currentObject].newposx;
	object[currentObject].relspeedy = object[currentObject].vy;
	objectCheckCollision();

	if (object[currentObject].y>>8 > currentWorld->level[currentLevel].height + 256)
	{
		camera.x = 1<<8;
		camera.y = 1<<8;
		object[currentObject].y = object[currentObject].startY;
		object[currentObject].x = object[currentObject].startX;
		object[currentObject].vy = 0;
	}

}


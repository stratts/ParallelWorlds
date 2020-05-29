#include <PA9.h>
#include "camera.h"

cameraInfo camera;

// Intitialises the type (and sets the position) of the camera
void cameraInit(int type, int x, int y)
{
	camera.type = type;
	camera.x = x<<8;
	camera.y = y<<8;
}

// Move the camera to a different place on the screen, using angles.
bool cameraSet(int cx, int cy, int speed)
{
    // Constants, to eliminate magic numbers
	const int scroll_acc = 5; // Scrolling acceleration speed
    const int scroll_catch = 20; // Catchment area, to protect against overscroll

    int angle;
	bool set1 = false;
	bool set2 = false;
	int x = cx-128;
	int y = cy-96;

	// Moving the camera into position
	if(!(camera.x>>8 < x + (48*speed) && camera.x>>8 > x - (48*speed) && camera.y>>8 < y + (48*speed) && camera.y>>8 > y - (48*speed)))
	{
		angle = PA_GetAngle(camera.x>>8, camera.y>>8, x, y);
		if(camera.xscroll < PA_Cos(angle)*speed) camera.xscroll+=scroll_acc; 
			else if (camera.xscroll > PA_Cos(angle) *speed) camera.xscroll-=scroll_acc; 

		if (camera.yscroll < -PA_Sin(angle)*speed) camera.yscroll+=scroll_acc;
			else if (camera.yscroll > -PA_Sin(angle) *speed) camera.yscroll-=scroll_acc;

		camera.set = false;
	}
    
	// Stopping the camera
	else
	{
		if (camera.xscroll > 0) camera.xscroll -= scroll_acc;
			else if (camera.xscroll < 0) camera.xscroll += scroll_acc;

		if (camera.yscroll > 0) camera.yscroll -= scroll_acc;
			else if (camera.yscroll < 0) camera.yscroll += scroll_acc;

		if (camera.xscroll < scroll_catch && camera.xscroll > -scroll_catch) {camera.xscroll = 0; set1 = true;}
		if (camera.yscroll < scroll_catch && camera.yscroll > -scroll_catch) {camera.yscroll = 0; set2 = true;}
	}

	camera.x += camera.xscroll; camera.y += camera.yscroll;

	if (set1 && set2) return 1;
	else return 0;
}

// Set the camera's target (to follow)
void cameraTarget(int* x, int* y, int* speed, int* vy, int limitl, int limith)
{
	camera.targetx = x;
	camera.targety = y;
	camera.targetspeed = speed;
	camera.targetvy = vy;
	camera.limitl = limitl;
	camera.limith = limith;
}

// Main camera scrolling function
void cameraScroll() 
{ 
	// Constants, to eliminate magic numbers
    const int v_border = 60;
	const int h_border = 80;

	// Just to make the code easier to read
	int x = (*camera.targetx) >> 8;
	int y = (*camera.targety) >> 8;
	int speed = (*camera.targetspeed);
	int fall = (*camera.targetvy);	
	int camerax = (camera.x) >> 8;
	int cameray = (camera.y) >> 8;
	int vy_speed = 5; // Fixed point division... the smaller the number, the faster the speed
	int vx_speed = 5; // See above ^^

	/*                       *\
	   'v*_speed' is the
	   acceleration speed of 
	   the camera.
	\*                       */
    
	// Camera types
	switch(camera.type)
	{
		case 0: vy_speed = 3; break;
		case 1: break;
	}
    
	// Special conditions
	if(!(*camera.targetspeed < 0 || *camera.targetspeed > 0)) speed = 1024;
	if (speed < 0) speed = -speed;	
	if(!(*camera.targetvy < 0 || *camera.targetvy > 0)) fall = 1024;	
	if(fall < 0) fall = -fall;
    
	// Start main camera code
	if((x - camerax > 256 - (h_border-10) && camera.xscroll < speed + (speed >> 2)) ||
        (x - camerax < (h_border-10) && camera.xscroll > -(speed + (speed >> 2))))
	{
		if(x - camerax > 256 - (h_border-10) && camera.xscroll < speed + (speed >> 2)) camera.xscroll += speed >> vx_speed;
		if(x - camerax < (h_border-10) && camera.xscroll > -(speed + (speed >> 2))) camera.xscroll -= speed >> vx_speed;
	}

	else if ((x - camerax > 256 - h_border && camera.xscroll < speed)||
		     (x - camerax < h_border && camera.xscroll > -(speed)))
	{
		if(x - camerax > 256 - h_border && camera.xscroll < speed) camera.xscroll += speed >> vx_speed;
		if(x - camerax < h_border && camera.xscroll > -(speed)) camera.xscroll -= speed >> vx_speed;
	}

	else if (camera.xscroll > 0 || camera.xscroll < 0)
	{
		if (camera.xscroll > 0) camera.xscroll -= speed >> vx_speed;
		if (camera.xscroll < 0) camera.xscroll += speed >> vx_speed;
	}

	if (camera.xscroll > -(speed >> vx_speed) && camera.xscroll < speed >> vx_speed) camera.xscroll = 0;

    if((y - cameray > 192 - (v_border-10) && camera.yscroll < fall + (fall >> 2)) ||
        (y - cameray < (v_border-10) && camera.yscroll > -(fall + (fall >> 2))))
	{
		if(y - cameray > 192 - (v_border-10) && camera.yscroll < fall + (fall >> 2)) camera.yscroll += fall >> vy_speed;
		if(y - cameray < (v_border-10) && camera.yscroll > -(fall + (fall >> 2))) camera.yscroll -= fall >> vy_speed;
	}

	else if ((y - cameray > 192 - v_border && camera.yscroll < fall)||
		     (y - cameray < v_border && camera.yscroll > -(fall)))
	{
		if(y - cameray > 192 - v_border && camera.yscroll < fall) camera.yscroll += fall >> vy_speed;
		if(y - cameray < v_border && camera.yscroll > -(fall)) camera.yscroll -= fall >> vy_speed;
	}

	else if (camera.yscroll > 0 || camera.yscroll < 0)
	{
		if (camera.yscroll > 0) camera.yscroll -= fall >> vy_speed;
		if (camera.yscroll < 0) camera.yscroll += fall >> vy_speed;
	}

	if (camera.yscroll > -(fall >> vy_speed) && camera.yscroll < fall >> vy_speed) camera.yscroll = 0;

	camera.x += camera.xscroll; 
	camera.y += camera.yscroll;
    
	// Camera limits
    if(camera.x > (camera.limitl-256)<<8)
	{
		camera.x = (camera.limitl-256)<<8;
		//camera.xscroll = -(camera.xscroll>>1);
	}

	if (camera.x>>8 < 0)
	{
		camera.x = 0;
		//camera.xscroll = -(camera.xscroll>>1);
	}

	if(camera.y > (camera.limith-192)<<8)
	{
		camera.y = (camera.limith-192)<<8;
		//camera.yscroll = -(camera.yscroll>>1);
	}

	if (camera.y>>8 < 0)
	{
		camera.y = 0;
		//camera.yscroll = -(camera.yscroll>>1);
	}

}


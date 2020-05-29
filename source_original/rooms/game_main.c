#include <PA9.h>
#include <ulib/ulib.h>

#include "level.h"
#include "cacaoLib.h"
#include "defines.h"
#include "objects.h"
#include "characters.h"
#include "camera.h"
#include "functions.h"

#include "rooms.h"

void rm_mainGame()
{

	//PA_Init16cBg(0, 1);
	//PA_Init16cBg(1, 1);

	loadLevel(&Jelli, selectedLevel);
		
	//AS_MP3StreamPlay(rootf("music/jumper.mp3"));
	int i = 1;
	char buffer[1024];
	char configBuffer[1024];

	sprintf(configBuffer, "/levels/%s/config.ini", Jelli.level[currentLevel].name);

	while(1)
	{
		sprintf(buffer, "Object%d", i);
		if(ini_getl(buffer, "class", 0, rootf(configBuffer)))
		{
			createObject(classes[(int)ini_getl(buffer, "class", 0, rootf(configBuffer))],
								(int)ini_getl(buffer, "x", 0, rootf(configBuffer)),
								(int)ini_getl(buffer, "y", 0, rootf(configBuffer)),
								0
			);
			object[i-1].moveDirection = (int)ini_getl(buffer, "flip", -1, configBuffer);
		}
		else break;
		i++;
	}
							


	
	/*createObject(&DUMMY, 300, 0, 0);
	createObject(&DUMMY, 330, 0, 0);
	createObject(&DUMMY, 360, 0, 0);
	createObject(&DUMMY, 390, 0, 0);
	createObject(&DUMMY, 420, 0, 0);
	createObject(&DUMMY, 450, 0, 0);
	createObject(&DUMMY, 480, 0, 0);
	createObject(&DUMMY, 510, 0, 0);
	createObject(&DUMMY, 540, 0, 0);
	createObject(&DUMMY, 570, 0, 0);
	createObject(&DUMMY, 600, 0, 0);
	createObject(&DUMMY, 630, 0, 0);
	createObject(&DUMMY, 660, 0, 0);
	createObject(&DUMMY, 690, 0, 0);
	createObject(&DUMMY, 720, 0, 0);
	createObject(&DUMMY, 750, 0, 0);
	createObject(&DUMMY, 780, 0, 0);
	createObject(&DUMMY, 810, 0, 0);*/

	cameraInit(0, (object[0].x>>8) - 128, (object[0].y>>8) - 96);
	cameraTarget(&object[0].cx, &object[0].cy, &object[0].class->speed, &object[0].vy, currentWorld->level[currentLevel].width, currentWorld->level[currentLevel].height);	
	moveObjects();
	cameraScroll();
	processObjects();
	//PA_EasyBgScrollXY(MAINSCREEN, 0, camera.x>>8, camera.y>>8);
	PA_EasyBgScrollXY(MAINSCREEN, 1, camera.x>>8, camera.y>>8);
	PA_EasyBgScrollXY(MAINSCREEN, 2, (camera.x>>8)>>1, (camera.y>>8)>>1);
	PA_EasyBgScrollXY(MAINSCREEN, 3, (camera.x>>8)>>2, (camera.y>>8)>>2);

	CA_FadeIn(0);



	int midBgX, backBgX;
	int timer;
	int paused = false;


	// Infinite loop to keep the program running
	while (1)
	{
		start_test();
		midBgX -= Jelli.level[currentLevel].midscroll;
		backBgX -= Jelli.level[currentLevel].backscroll;
		i++;

		if(i >= 60) { i = 0; timer++; }

		
		while(paused)
		{
			PA_SetBrightness(1, -10); 
			AS_SetMasterVolume(48);
			if(Pad.Newpress.Start) { PA_SetBrightness(1, 0); AS_SetMasterVolume(127); paused = false;}
			while(Pad.Held.Select)
			{
				CA_SimpleText(0, 4, 4, "Music: 'Jumper' by Waterflame");
				CA_SimpleText(0, 4, 12, "Mario sprite by RangeTE");
				PA_WaitForVBL();
				CA_Update16c();
			}
			CA_Information(0, "\n- Paused -\nPress START to unpause the game\nHold SELECT for credits");
			CA_SimpleText(0, 4, 180, "Hint: your movement affects the points you gain...");
			PA_WaitForVBL();
			CA_Update16c();
		}
		if(Pad.Newpress.Start) { paused = true; }
			
			
			
		if(Pad.Newpress.B && Pad.Newpress.L && Pad.Newpress.R) { createObject(&DUMMY, object[0].x>>8, object[0].y>>8, 0); }

		processObjects();
		cameraScroll();
		moveObjects();
		PA_EasyBgScrollXY(MAINSCREEN, 1, camera.x>>8, camera.y>>8);
		PA_EasyBgScrollXY(MAINSCREEN, 2, ((camera.x+midBgX)>>8)>>1, (camera.y>>8)>>1);
		PA_EasyBgScrollXY(MAINSCREEN, 3, ((camera.x+backBgX)>>8)>>2, (camera.y>>8)>>2);

		displayDebug(0);

		//CA_BoxText(0, 100, 48, 32, "Welcome to Alpha 2 of Parallel Worlds! The aim of this demo is to gather as many points as you can by jumping on the Mario streakers. Be quick though, as your points decrease over time!");
		
		CA_SimpleText(0, 4, 180, "Points: %d", playerPoints>>12);
		CA_SimpleText(0, 208, 180, VERSION);

		
		PA_WaitForVBL();
		CA_Update16c();
		
	}
}
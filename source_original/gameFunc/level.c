#include <PA9.h>
#include <fat.h>
#include "level.h"
#include "defines.h"
#include "fsload.h"
#include "functions.h"
#include "camera.h"
#include "objects.h"
#include "cacaoLib.h"

//	name, music, stagebg, midbg, backbg, collision

void setLevels()
{
	char configBuffer[1024];
	int i = 0;
	LEVELNUM = -1;

	/*//-----------------------------------------------------
	// Jelli
	//-----------------------------------------------------

	strcpy(Jelli.level[0].name, "Nostalgia");
	sprintf(configBuffer, "levels/%s/config.ini", Jelli.level[0].name);
	ini_gets("Level","music","jumper2.mp3",Jelli.level[0].music,512,configBuffer);
	Jelli.level[0].width = (int)ini_getl("Level", "width", 0, configBuffer);
	Jelli.level[0].height = (int)ini_getl("Level", "height", 0, configBuffer);
	Jelli.level[0].gravity = (int)ini_getl("Level", "gravity", 1024, configBuffer);*/

	struct stat st;
	char filename[1024];

	DIR_ITER* dir = diropen(rootf("/levels"));
	while(1)
	{
		if(dirnext(dir, filename, &st)) break;
		if(st.st_mode && S_IFDIR && strcmp(filename, ".") && strcmp(filename, "..")) 
		{
			strcpy(Jelli.level[i].name, filename);
			i++;
			LEVELNUM++;
		}
	}

	i = 0;

	while(strcmp(Jelli.level[i].name,""))
	{	
		sprintf(configBuffer, "/levels/%s/config.ini", Jelli.level[i].name);
		ini_gets("Level","music","jumper.mp3",Jelli.level[i].music,512,rootf(configBuffer));
		Jelli.level[i].width = (int)ini_getl("Level", "width", 0, rootf(configBuffer));
		Jelli.level[i].height = (int)ini_getl("Level", "height", 0, rootf(configBuffer));
		Jelli.level[i].gravity = (int)ini_getl("Level", "gravity", 1024, rootf(configBuffer));
		Jelli.level[i].midscroll = (int)ini_getl("Scrolling", "midscroll", 0, rootf(configBuffer));
		Jelli.level[i].backscroll = (int)ini_getl("Scrolling", "backscroll", 0, rootf(configBuffer));
		i++;
	}


}

void loadLevel(worldinfo* world, int levelNum)
{
	currentWorld = world;
	currentLevel = levelNum;

	char levelPath[512];
	char bgPath[512];
	char rootPath[512];
	FILE* f;

	int i;

	PA_Init16cBg(0, 0);
	PA_Init16cBg(1, 0);

	bool stageLoaded, midLoaded, backLoaded;


	strcpy(rootPath,rootf("/levels"));	

	sprintf(levelPath, "%s/%s/%s", rootPath, world->level[levelNum].name, "StageBG");
	sprintf(bgPath, "%s/%s", levelPath, "info.bin");
	f = fopen(bgPath, "rb");
	CA_Information(1, "Loading...");
	CA_FadeIn(0);

	if(!f)
	{
		char bufferE[512];
		sprintf(bufferE, "Could not load stage for %s.", world->level[levelNum].name);
		displayError(bufferE);
	}
	else 
	{
		FS_EasyLoadBackground(MAINSCREEN, 1, levelPath);
		PA_HideBg(MAINSCREEN, 1);
	}

	sprintf(levelPath, "%s/%s/%s", rootPath, world->level[levelNum].name, "MidBG");
	sprintf(bgPath, "%s/%s", levelPath, "info.bin");
	f = fopen(bgPath, "rb");
	if(f) 
	{
		FS_EasyLoadBackground(MAINSCREEN, 2, levelPath);
		PA_HideBg(MAINSCREEN, 2);
		midLoaded = true;
	}
	else midLoaded = false;

	sprintf(levelPath, "%s/%s/%s", rootPath, world->level[levelNum].name, "BackBG");
	sprintf(bgPath, "%s/%s", levelPath, "info.bin");
	f = fopen(bgPath, "rb");
	if(f) 
	{
		FS_EasyLoadBackground(MAINSCREEN, 3, levelPath);
		PA_HideBg(MAINSCREEN, 3);
			backLoaded = true;
	}
	else backLoaded = false;

	sprintf(levelPath, "%s/%s/%s", rootPath, world->level[levelNum].name, "CollisionMap");
	sprintf(bgPath, "%s/%s", levelPath, "info.bin");
	if(FS_LoadFile(bgPath, NULL)) 
	{
		FS_EasyLoadBackground(MISCSCREEN, 3, levelPath);
		PA_HideBg(MAINSCREEN, 3);
	}
	else
	{
		char bufferE[512];
		sprintf(bufferE, "Could not load collision map for %s.", world->level[levelNum].name);
		displayError(bufferE);
	}

	fclose(f);

	CA_FadeOut(0);
	CA_Update16c();

	//PA_DeleteBg(1, 0);
	//PA_DeleteBg(0, 0);

	PA_ShowBg(MAINSCREEN, 1);
	if(midLoaded) PA_ShowBg(MAINSCREEN, 2);
	if(backLoaded) PA_ShowBg(MAINSCREEN, 3);


	if(world->level[levelNum].music)
	{
		char __str[1024];
		sprintf(__str, "/music/%s", world->level[levelNum].music);
		int *levelMusic = FAT_LoadData(rootf(__str), 0);
		AS_MP3DirectPlay((u8*)levelMusic, lastLoadedSize);
		AS_SetMP3Loop(true);
	}

	
}

bool getCollisionPix(int screen, int bglayer, int x, int y)
{
	int xPos = x;
	int yPos = y;

	if(xPos > currentWorld->level[currentLevel].width) xPos = currentWorld->level[currentLevel].width;
	if(xPos < 0) xPos = 0;
	if(yPos > currentWorld->level[currentLevel].height) yPos = currentWorld->level[currentLevel].height;
	if (yPos < 0) yPos = 0;

	return PA_EasyBgGetPixel(screen, bglayer, xPos, yPos);
}



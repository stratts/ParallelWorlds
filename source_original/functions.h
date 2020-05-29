#ifndef FUNCTIONS__
#define FUNCTIONS__

#include <PA9.h>
#include <time.h>
#include "objects.h"
#include "defines.h"
#include <stdio.h>
#include <sys/dir.h>
#include <fat.h>

bool rotsetSlot[32];
int lastLoadedSize;

void displayError(char *error_text);
int getRotsetSlot();

void* FAT_LoadData(char *pFilename, u32 *pSize);
void setSpriteXY(int screen, int sprite, int x, int y);
int lowestXinObj(objectInfo ar[], int size);
void initRand();
void customVBL();
void start_test();
u32 end_test();
void displayDebug(bool screen);
void textBox(int screen, int x, int y, int x2, int y2, const char* text);



#endif

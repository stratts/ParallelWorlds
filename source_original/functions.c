#include "functions.h"
#include "camera.h"
#include "objects.h"
#include "collisions.h"
#include "cacaoLib.h"


void customVBL()
{
	 
}

void displayError(char *error_text)
{
	char buffer[1024];

	PA_ResetBgSys();
	PA_ResetSpriteSys();
	PA_SetBrightness(1, 0);
	PA_SetBrightness(0, 0);
	PA_Init16cBg(1, 3);
	sprintf(buffer, "Error:\n%s", error_text);
	PA_16cTextAlign(ALIGN_CENTER);
	PA_16cText(1, 0, 80, 255, 192, buffer, 1, 0, 100);
	while(1)
	{
		PA_WaitForVBL();
	}
}

void* FAT_LoadData(char *pFilename, u32 *pSize)
{
   FILE	*pFile;
	u8		*pData = NULL;
	struct stat FileInfo;
	
	pFile = fopen(pFilename, "rb");
	if (!(pFile)) 
	{
		char buffer[1024];
		sprintf(buffer, "'%s' not found", pFilename);
		displayError(buffer);
	}
		
		
	stat(pFilename, &FileInfo);
	
	pData = (u8*) calloc(FileInfo.st_size, sizeof(u8));
	
	fread(pData, 1, FileInfo.st_size, pFile);

	lastLoadedSize = FileInfo.st_size;
	
	fclose(pFile);
	
	PA_WaitForVBL();
	
	if(pSize != NULL) { *pSize = FileInfo.st_size; }
	   
   return (void*)pData;
}


void setSpriteXY(int screen, int sprite, int x, int y)
{
	if (x >= 256 || y >= 192 || x <= -64 || y <= -64) PA_SetSpriteXY(screen, sprite, 256, 192);
	else PA_SetSpriteXY(screen, sprite, x, y);
}


//----------------------------------------------------------------------------
int getRotsetSlot() {
//----------------------------------------------------------------------------

    int i;
    for(i=0;i<32;i++){
        if(rotsetSlot[i] == 0){
            rotsetSlot[i] = 1;
            return i;
        }			
    }
    return -1;
}

int lowestXinObj(objectInfo ar[], int size)
{
	int i;
	u64 oldDistance = 10000000;
	int place;

	if(size > 1)
	{
		for(i=0;i<size+1;i++)
		{
			if(PA_Distance(ar[currentObject].x>>8, ar[currentObject].y>>8, ar[i].x>>8, ar[i].y>>8) < oldDistance && currentObject != i)
			{
				place = i;
				oldDistance = PA_Distance(ar[currentObject].x>>8, ar[currentObject].y>>8, ar[i].x>>8, ar[i].y>>8);
			}

		}
	}

	return place;	
}

void initRand()
{
	u32 randomSeed = time(0);
	srand(randomSeed);
}


void start_test() {
    TIMER0_CR = 0;
    TIMER1_CR = 0;
    
    TIMER0_DATA = 0;
    TIMER1_DATA = 0;
    
    TIMER1_CR = TIMER_CASCADE | TIMER_ENABLE;
    TIMER0_CR = TIMER_ENABLE;
}

u32 end_test() {
    TIMER0_CR = 0;
    TIMER1_CR = 0;
    
    int CPUusage = TIMER0_DATA | (TIMER1_DATA<<16);
	return ((CPUusage * 100 + (560190/2)) / 560190 );
    
}

void displayDebug(bool screen)
{
	CA_SimpleText(screen, 4, 6, "- Player/camera info -");
	CA_SimpleText(screen, 4, 14, "Player position X: %d", object[0].x>>8);
	CA_SimpleText(screen, 4, 22, "Player position Y: %d", object[0].y>>8);
	CA_SimpleText(screen, 4, 30, "Camera position X: %d", camera.x>>8);
	CA_SimpleText(screen, 4, 38, "Camera position Y: %d", camera.y>>8);

	CA_SimpleText(screen, 4, 58, "- Collisions -");
	CA_SimpleText(screen, 4, 66, "Left collision: %d", leftCollision(0));
	CA_SimpleText(screen, 4, 74, "Right collision: %d", rightCollision(0));
	CA_SimpleText(screen, 4, 82, "Up collision: %d", upCollision(0));
	CA_SimpleText(screen, 4, 90, "Down collision: %d", downCollision(0));
	CA_SimpleText(screen, 4, 98, "Touching ground: %d", touchingGround(0));

	struct mallinfo info = mallinfo();

	CA_SimpleText(screen, 4, 116, "- Hardware -");
	CA_SimpleText(screen, 4, 124, "CPU usage: %d percent", end_test());
	CA_SimpleText(screen, 4, 132, "Memory in use: %d.%d MB", ((info.usmblks + info.uordblks)>>10)>>10, ((info.usmblks + info.uordblks)>>10));
}	

void textBox(int screen, int x, int y, int x2, int y2, const char* text)
{
	int xpos, ypos;
	for(xpos = x; xpos < x2; xpos++)
		{
			for(ypos = y; ypos < y2; ypos++)
				PA_16cPutPixel(screen, xpos, ypos, PA_RGB(31, 31, 31));
		}
}
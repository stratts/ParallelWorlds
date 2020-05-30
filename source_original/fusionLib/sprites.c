#include "fusionLib/sprites.h"

arrayPos = 0;

bool createGIFSprite(bool screen, u8 spriteNum, const char *filePath, u8 shape, u8 size, u8 colourMode, u8 palNum, s16 x, s16 y)
{
    FILE    *pFile;
    u8      *pData = NULL;
    struct stat FileInfo;
    
    pFile = fopen(pFilename, "rb");
    if (!pFile) return 0;
        
    stat(pFilename, &FileInfo);
    
    pData = (u8*) calloc(FileInfo.st_size, sizeof(u8));
    
    fread(pData, 1, FileInfo.st_size, pFile);

    //lastLoadedSize = FileInfo.st_size;
    
    fclose(pFile);
    
    PA_WaitForVBL();
    
    if(pSize != NULL) { *pSize = FileInfo.st_size; }
#pragma once

u8 room;
char location[1024];

#ifndef PAGfx_struct
    typedef struct{
    void *Map;
    int MapSize;
    void *Tiles;
    int TileSize;
    void *Palette;
    int *Info;
} PAGfx_struct;
#endif

//--------------------------------------------------------------------------------------------------
void CA_InitASLib(void);
void CA_PlayMyMP3(char *name);
void CA_DisplayInfo(bool screen, u8 info[512], s32 x, s32 y, char* prefix, char* suffix);
void CA_OutputRTC(bool screen, s32 x, s32 y, char* prefix);
void CA_Update16c();
void CA_FadeIn(bool type);
void CA_FadeOut(bool type);
void CA_ChangeRoom(s8 new_room, bool fade_type);
//bool CA_SetFatRoot(char *Zrootname);
void CA_OutputText(bool screen, s32 boxx, s32 boxy, char *text, s32 limit, s32 boxwidth, s32 boxheight, char *orientation);
void CA_Information(bool screen, char *text);
char* rootf(char *path);
#define CA_SimpleText(screen, x, y, format...)	({ char __str[1000]; sprintf(__str , ##format); CA_OutputText(screen, x, y, __str, 1024, 256-x, 192-y, "Justified"); })
#define CA_BoxTextEx(screen, x, y, width, format...)	({ char __str[1000]; sprintf(__str , ##format); CA_OutputText(screen, x, y, __str, 1024, x+width, 192-y, "Justified"); })
#define CA_BoxText(screen, x, y, width, text)	({ CA_OutputText(screen, x, y, text, 2048, x+width, 192-y, "Justified"); })
//--------------------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------------------
// Smallfont data
//--------------------------------------------------------------------------------------------------

// Background files : 
extern const char smallfont_Height;
extern const char smallfont_Sizes[256];
extern const int smallfont_Info[3]; // BgMode, Width, Height
extern const unsigned short smallfont_Map[256] __attribute__ ((aligned (4))) ;  // Pal : smallfont_Pal
extern const unsigned char smallfont_Tiles[3200] __attribute__ ((aligned (4))) ;  // Pal : smallfont_Pal
extern PAGfx_struct smallfont; // background pointer


// Palette files : 
extern const unsigned short smallfont_Pal[2] __attribute__ ((aligned (4))) ;
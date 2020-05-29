#include <PA9.h>
#include "cacaoLib.h"
#include "defines.h"



//--------------------------------------------------------------------------------------------------
void CA_Init16c() {
//--------------------------------------------------------------------------------------------------

   PA_Init16cBg(0, 0); 	
	PA_Init16cBg(1, 0);
	PA_16cCustomFont(5, smallfont);
}	

//--------------------------------------------------------------------------------------------------
void CA_OutputRTC(bool screen, s32 x, s32 y, char* prefix) {
//--------------------------------------------------------------------------------------------------

   char buffer[512];	

	sprintf(buffer, "%s%02d:%02d", prefix, PA_RTC.Hour, PA_RTC.Minutes);
	PA_16cText(screen, x, y, 256, y+10, buffer, 1, 0, 25); 
	PA_UpdateRTC();
}	


//--------------------------------------------------------------------------------------------------  
void CA_DisplayInfo(bool screen, u8 info[512], s32 x, s32 y, char* prefix, char* suffix) {
//--------------------------------------------------------------------------------------------------

   char buffer[512];	

   sprintf(buffer, "%s%s%s", prefix, info, suffix);
   PA_16cText(screen, x, y, 256, y+10, buffer, 1, 0, 50); 
   PA_UpdateUserInfo();
}  

//--------------------------------------------------------------------------------------------------
void CA_Update16c() {
//--------------------------------------------------------------------------------------------------

   PA_16cErase(0);
   PA_16cErase(1);
}   

//--------------------------------------------------------------------------------------------------
void CA_FadeOut(bool type) {
//--------------------------------------------------------------------------------------------------

   s8 i;

   if (type == 1)
	{	
      for (i = 0; i <= 31; i++) 
      {
	      PA_SetBrightness(0, i); 
	      PA_SetBrightness(1, i);
	      PA_WaitForVBL();
      }	
   }
   
   else if (type == 0)
	{
	   for (i = 0; i >= -31; i--) 
      {
	      PA_SetBrightness(0, i); 
	      PA_SetBrightness(1, i);
	     PA_WaitForVBL();
      }	
   }   
}

//--------------------------------------------------------------------------------------------------
void CA_FadeIn(bool type) { 
//--------------------------------------------------------------------------------------------------	

   s8 i;

   if (type == 1)
	{	
      for (i = 31; i >= 0; i--) 
      {
	      PA_SetBrightness(0, i); 
	      PA_SetBrightness(1, i);
	      PA_WaitForVBL();
      }	
   }
	
	else if (type == 0)
	{
	   for (i = -31; i <= 0; i++) 
      {
	      PA_SetBrightness(0, i); 
	      PA_SetBrightness(1, i);
	      PA_WaitForVBL();
      }	
   }   
}   


//--------------------------------------------------------------------------------------------------
void CA_ChangeRoom (s8 new_room, bool fade_type) {
//--------------------------------------------------------------------------------------------------

   CA_FadeOut(fade_type);
   PA_ResetBgSys();
   PA_ResetSpriteSys();
   room = new_room;
}  

// Deprecated function...
//--------------------------------------------------------------------------------------------------
/*bool CA_SetFatRoot(char *rootname) {
//--------------------------------------------------------------------------------------------------

   if (PA_Locate("/", rootname, true, 5, location)) return 1;
   else return 0;
   

}  
*/

//--------------------------------------------------------------------------------------------------
void CA_InitASLib(void) {
//--------------------------------------------------------------------------------------------------

     PA_VBLFunctionInit(AS_SoundVBL); // easy way to make sure that AS_SoundVBL() is called every frame
     AS_Init(AS_MODE_MP3 | AS_MODE_SURROUND | AS_MODE_16CH);  // initializes AS_Lib
     AS_SetDefaultSettings(AS_PCM_8BIT, 11025, AS_SURROUND);  // or your preferred default sound settings
}   

//--------------------------------------------------------------------------------------------------
void CA_PlayMyMP3(char *name) {
//--------------------------------------------------------------------------------------------------

   char path[512];
   
   strcpy(path, location);
   strcat(path, name);
   
   AS_MP3StreamPlay(path);
}  

//--------------------------------------------------------------------------------------------------
void CA_OutputText(bool screen, s32 boxx, s32 boxy, char *text, s32 limit, s32 boxwidth, s32 boxheight, char *orientation) {
//--------------------------------------------------------------------------------------------------
    
    if (strcmp(orientation, "left") == 0) PA_16cTextAlign(ALIGN_LEFT);
    else if (strcmp(orientation, "right") == 0) PA_16cTextAlign(ALIGN_RIGHT);
    else if (strcmp(orientation, "center") == 0) PA_16cTextAlign(ALIGN_CENTER);
    else if (strcmp(orientation, "justify") == 0) PA_16cTextAlign(ALIGN_JUSTIFY);
    else PA_16cTextAlign(ALIGN_JUSTIFY);
    
    PA_16cText(screen, boxx, boxy, boxx+boxwidth, boxy+boxheight, text, 1, 5, limit); 
}  

//--------------------------------------------------------------------------------------------------
void CA_Information(bool screen, char *text) {
//--------------------------------------------------------------------------------------------------

	PA_16cTextAlign(ALIGN_CENTER);
	PA_16cText(screen, 0, 96-16, 255, 192, text, 1, 5, 255);
}

char* rootf(char *path)
{
   char filepath[512];
   strcpy(filepath, ROOTPATH);
   strcat(filepath, path);
   
   char *msg = filepath;
   strncpy(msg, filepath, sizeof(filepath));

#ifdef USE_FAT
	return msg;
#endif

#ifdef USE_EFS
	return path;
#endif
}	

//--------------------------------------------------------------------------------------------------
// Misc Stuff
//--------------------------------------------------------------------------------------------------

char Version[24];
char Date[24];


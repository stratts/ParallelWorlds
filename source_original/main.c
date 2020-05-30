

#include <PA9.h>       // Include for PA_Lib
#include <ulib/ulib.h>
#include <unistd.h>
#include <fat.h>

#include "defines.h"

//-- MaxMod includes ----------
#include <maxmod9.h>
//-----------------------------

#include "rooms.h"
#include "functions.h"
#include "level.h"
#include "objects.h"
#include "cacaoLib.h"

#ifdef USE_FAT
    #include <fat.h>
#endif

#ifdef USE_EFS
    #include "efs_lib.h"
#endif

#ifdef USE_NITRO
    #include "filesystem.h"
#endif


int main()
{
        //defaultExceptionHandler();

    PA_Init();    // Initializes PA_Lib
    PA_InitVBL(); // Initializes a standard VBL


    //Initialization of µlibrary
    //ulInit(UL_INIT_ALL);
    //ulInitGfx();
    //ulInitText();

    //ulSetMainLcd(0);

    PA_VBLFunctionInit(AS_SoundVBL);
    AS_Init(AS_MODE_MP3 | AS_MODE_SURROUND | AS_MODE_16CH);
    AS_SetDefaultSettings(AS_PCM_8BIT, 11025, AS_SURROUND);

    PA_16cCustomFont(5, smallfont);

    PA_SetBrightness(0, -31);
    PA_SetBrightness(1, -31);

    PA_Init16cBg(0, 2);
    PA_Init16cBg(1, 2);

    //- Filesystem initialisation ------------------------------------------
    CA_Information(1, "Initialising filesystem");
    CA_FadeIn(0);


    #ifdef USE_EFS

        if(!EFS_Init(EFS_AND_FAT | EFS_DEFAULT_DEVICE, NULL))
        {
            displayError("EFS initialisation failed.");
        }

    #endif

    #ifdef USE_NITRO

        if(!nitroFSInit())
        {
            displayError("NitroFS initialisation failed.");
        }

        chdir("efs:/");

    #endif

    #ifdef USE_FAT
        if(!fatInitDefault())
        {
            displayError("FAT initialisation failed.");
        }
    #endif  

    //- Set the variables for the levels and objects -----------------------

    setObjects();
    setLevels();
    

    //mmInitDefault("soundbank.bin"); // Specify our music file
    //mmSelectMode(MM_MODE_C);

        CA_FadeOut(0);
        CA_Update16c();
        PA_ResetBgSys();
    

    //- Start the game -----------------------------------------------------
    rm_mainMenu();

    return 0;
}


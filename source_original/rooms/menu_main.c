#include <PA9.h>

#include "defines.h"
#include "characters.h"
#include "cacaoLib.h"
#include "level.h"
#include "functions.h"

#include "rooms.h"

void rm_mainMenu()
{
    selectedLevel = 0;
    selectedCharacter = 0;
    selectedItem = 0;

    PA_Init16cBg(0, 0);

    PA_SetBrightness(0, 0);

    int i = 0;


    while(1)
    {
        i = 0;

        while(strcmp(Jelli.level[i].name, ""))
        {
            CA_SimpleText(0, 15, 15+(8*i), "%s", Jelli.level[i].name);
            i++;
            //PA_WaitForVBL();
        }

        CA_SimpleText(0, 5, 5, "Choose a level:");
        CA_SimpleText(0, 5, 15+(8*selectedLevel), "->");

        if(LEVELNUM == -1) CA_SimpleText(0, 5, 180, "Found no levels.");
        else if(LEVELNUM == 0) CA_SimpleText(0, 5, 180, "Found %d level.", LEVELNUM+1);
        else CA_SimpleText(0, 5, 180, "Found %d levels.", LEVELNUM+1);

        if(Pad.Newpress.Up && selectedLevel > 0) selectedLevel--;
        if(Pad.Newpress.Down && selectedLevel < LEVELNUM) selectedLevel++;

        if(Pad.Newpress.A) break;

        PA_WaitForVBL();
        CA_Update16c();
    }

    CA_FadeOut(0); PA_ResetBgSys(); rm_mainGame();
}
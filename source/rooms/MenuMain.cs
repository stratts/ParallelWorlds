using System.Collections;

using static Defines;
using static Levels;

public static partial class Rooms {
    public static IEnumerator mainMenu() {
        selectedLevel = 0;
        selectedCharacter = 0;
        selectedItem = 0;

        PA.Init16cBg(0, 0);

        PA.SetBrightness(0, 0);

        int i = 0;

        while(true)
        {
            i = 0;

            while(Jelli.level[i].name != null)
            {
                CA.SimpleText(0, 15, 15+(8*i), Jelli.level[i].name);
                i++;
                //PA_WaitForVBL();
            }

            CA.SimpleText(0, 5, 5, "Choose a level:");
            CA.SimpleText(0, 5, 15+(8*selectedLevel), "->");

            if(LEVELNUM == -1) CA.SimpleText(0, 5, 180, "Found no levels.");
            else if(LEVELNUM == 0) CA.SimpleText(0, 5, 180, "Found {0} level.", LEVELNUM+1);
            else CA.SimpleText(0, 5, 180, "Found {0} levels.", LEVELNUM+1);

            if(Pad.Newpress.Up && selectedLevel > 0) selectedLevel--;
            if(Pad.Newpress.Down && selectedLevel < LEVELNUM) selectedLevel++;

            if(Pad.Newpress.A) break;

            PA.WaitForVBL();
            CA.Update16c();
            yield return null;
        }

        yield return CA.FadeOut(0); 

        yield return mainGame();
    }
}
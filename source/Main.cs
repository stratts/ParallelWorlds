using System.Collections;
using static Classes;
using static Levels;

public static partial class Rooms
{

    public static void main()
    {
        PA.SetBrightness(0, -31);
        PA.SetBrightness(1, -31);

        PA.Init16cBg(0, 2);
        PA.Init16cBg(1, 2);

        //- Filesystem initialisation ------------------------------------------
        CA.Information(1, "Initialising filesystem");
        CA.FadeIn(0);

        //- Set the variables for the levels and objects -----------------------
        setClasses();
        setLevels();

        CA.FadeOut(0);
        CA.Update16c();

        //- Start the game -----------------------------------------------------
        mainMenu();
    }
}
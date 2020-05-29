using System.Collections;

using static Levels;
using static Classes;

public static partial class Rooms {

    public static IEnumerator main() {
        PA.SetBrightness(0, -31);
        PA.SetBrightness(1, -31);

        PA.Init16cBg(0, 2);
        PA.Init16cBg(1, 2);

        //- Filesystem initialisation ------------------------------------------
        CA.Information(1, "Initialising filesystem");
        yield return CA.FadeIn(0);

        //- Set the variables for the levels and objects -----------------------
        setClasses();
        setLevels();

		yield return CA.FadeOut(0);
		CA.Update16c();
	
	    //- Start the game -----------------------------------------------------
	    yield return mainMenu();
    }
}
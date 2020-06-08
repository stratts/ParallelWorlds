using System.Collections;
using static Classes;
using static Defines;
using static Functions;
using static Levels;

using Microsoft.Xna.Framework;

public static partial class Rooms
{
    public static void mainGame()
    {

        var scene = new Scene(Jelli.level[selectedLevel]);
        scene.Load();

        var player = scene.Objects[0];
        var camera = scene.Camera;

        camera.SetPos(0, (int)player.x >> 8 - 128, (int)player.y >> 8 - 96);
        camera.Target(player, scene.Level.width, scene.Level.height);
        camera.Size = ScreenSize;

        scene.Update();

        CA.FadeIn(0);

        int timer = 0;
        bool paused = false;

        // Infinite loop to keep the program running
        while (true)
        {
            //start_test();
            int i = 1;
            i++;

            if (i >= 60) { i = 0; timer++; }

            while (paused)
            {
                PA.SetBrightness(1, -10);
                //AS_SetMasterVolume(48);
                if (Pad.Newpress.Start)
                {
                    PA.SetBrightness(1, 0);
                    //AS_SetMasterVolume(127); 
                    paused = false;
                }
                while (Pad.Held.Select)
                {
                    CA.SimpleText(0, 4, 4, "Music: 'Jumper' by Waterflame");
                    CA.SimpleText(0, 4, 12, "Ninja Frog sprite by Pixel Frog");
                    PA.WaitForVBL();
                    CA.Update16c();
                }
                CA.Information(0, "\n- Paused -\nPress START to unpause the game\nHold SELECT for credits");
                CA.SimpleText(0, 4, 180, "Hint: your movement affects the points you gain...");
                PA.WaitForVBL();
                CA.Update16c();
            }
            if (Pad.Newpress.Start) { paused = true; }
            if (Pad.Newpress.Select) break;

            if (Pad.Newpress.B && Pad.Newpress.L && Pad.Newpress.R)
            {
                scene.AddObject(new ObjectInfo(DUMMY, player.x, player.y, 0));
            }

            scene.Update();

            displayDebug(0, scene);

            //CA_BoxText(0, 100, 48, 32, "Welcome to Alpha 2 of Parallel Worlds! The aim of this demo is to gather as many points as you can by jumping on the Mario streakers. Be quick though, as your points decrease over time!");

            CA.SimpleText(0, 4, 180, "Points: {0}", playerPoints);
            CA.SimpleText(0, 208, 180, VERSION);

            PA.WaitForVBL();
            CA.Update16c();
        }

        CA.FadeOut(0);
        PA.Reset();
    }
}
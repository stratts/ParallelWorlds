using System.Collections;
using IniParser;

using static Defines;
using static Levels;
using static Classes;
using static Functions;

public static partial class Rooms {
    public static void mainGame() {
        loadLevel(Jelli, selectedLevel);
        
        //AS_MP3StreamPlay(rootf("music/jumper.mp3"));
        int i = 1;

        var path = CA.rootf("/levels") + $"/{Jelli.level[currentLevel].name}/config.ini";
        var data = new FileIniDataParser().ReadFile(path);

        var scene = new Scene(Jelli.level[currentLevel]);
        
        while(true)
        {
            string key = $"Object{i}";
            if(data.Sections.ContainsSection(key))
            {
                scene.AddObject(new ObjectInfo(classes[int.Parse(data[key]["class"])],
                                    int.Parse(data[key]["x"]),
                                     int.Parse(data[key]["y"]),
                                    0
                ));
                int flip = -1;
                if (data[key].ContainsKey("flip")) flip = int.Parse(data[key]["flip"]);
                scene.Objects[i-1].moveDirection = flip;
            }
            else break;
            i++;
        }

        var player = scene.Objects[0];
        var camera = scene.Camera;

        camera.SetPos(0, (player.x>>8) - 128, (player.y>>8) - 96);
        camera.Target(player, currentWorld.level[currentLevel].width, currentWorld.level[currentLevel].height);
        
        scene.Update();

        CA.FadeIn(0);

        int timer = 0;
        bool paused = false;

        // Infinite loop to keep the program running
        while (true)
        {
            //start_test();
            i++;

            if(i >= 60) { i = 0; timer++; }
    
            while(paused)
            {
                PA.SetBrightness(1, -10); 
                //AS_SetMasterVolume(48);
                if(Pad.Newpress.Start) { 
                    PA.SetBrightness(1, 0); 
                    //AS_SetMasterVolume(127); 
                    paused = false;
                }
                while(Pad.Held.Select)
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
            if(Pad.Newpress.Start) { paused = true; }
  
            if(Pad.Newpress.B && Pad.Newpress.L && Pad.Newpress.R) { 
                scene.AddObject(new ObjectInfo(DUMMY, player.x>>8, player.y>>8, 0)); 
            }

            scene.Update();

            displayDebug(0, scene);

            //CA_BoxText(0, 100, 48, 32, "Welcome to Alpha 2 of Parallel Worlds! The aim of this demo is to gather as many points as you can by jumping on the Mario streakers. Be quick though, as your points decrease over time!");
            
            CA.SimpleText(0, 4, 180, "Points: {0}", playerPoints>>12);
            CA.SimpleText(0, 208, 180, VERSION);
            
            PA.WaitForVBL();
            CA.Update16c();    
        }     
    }
}
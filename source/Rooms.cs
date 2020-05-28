using System;
using System.Collections;
using IniParser;

public static partial class Rooms {
    public static IEnumerator CurrentRoom { get; set; } = mainMenu();

    public static void Update() {
        CurrentRoom.MoveNext();
    }

    public static IEnumerator mainMenu() {
        Globals.selectedLevel = 0;
        Globals.selectedCharacter = 0;
        Globals.selectedItem = 0;
        var Jelli = Levels.Jelli;

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
            CA.SimpleText(0, 5, 15+(8*Globals.selectedLevel), "->");

            if(Globals.LEVELNUM == -1) CA.SimpleText(0, 5, 180, "Found no levels.");
            else if(Globals.LEVELNUM == 0) CA.SimpleText(0, 5, 180, "Found {0} level.", Globals.LEVELNUM+1);
            else CA.SimpleText(0, 5, 180, "Found {0} levels.", Globals.LEVELNUM+1);

            if(Pad.Newpress.Up && Globals.selectedLevel > 0) Globals.selectedLevel--;
            if(Pad.Newpress.Down && Globals.selectedLevel < Globals.LEVELNUM) Globals.selectedLevel++;

            if(Pad.Newpress.A) break;

            PA.WaitForVBL();
            CA.Update16c();
            yield return null;
            break;
        }

        CA.FadeOut(0); 

        CurrentRoom = mainGame();
    }

    public static IEnumerator mainGame() {
        var Jelli = Levels.Jelli;
        Levels.loadLevel(Jelli, Globals.selectedLevel);
		
        //AS_MP3StreamPlay(rootf("music/jumper.mp3"));
        int i = 1;

        var path = CA.rootf("/levels") + $"/{Jelli.level[Levels.currentLevel].name}/config.ini";
        var data = new FileIniDataParser().ReadFile(path);
        
        while(true)
        {
            string key = $"Object{i}";
            if(data.Sections.ContainsSection(key))
            {
                Objects.createObject(Classes.classes[int.Parse(data[key]["class"])],
                                    int.Parse(data[key]["x"]),
                                     int.Parse(data[key]["y"]),
                                    0
                );
                int flip = -1;
                if (data[key].ContainsKey("flip")) flip = int.Parse(data[key]["flip"]);
                Objects.objects[i-1].moveDirection = flip;
            }
            else break;
            i++;
        }

        var objects = Objects.objects;
        var currentWorld = Levels.currentWorld;
        var currentLevel = Levels.currentLevel;

        Camera.cameraInit(0, (objects[0].x>>8) - 128, (objects[0].y>>8) - 96);
        Camera.cameraTarget(objects[0], currentWorld.level[currentLevel].width, currentWorld.level[currentLevel].height);
        Objects.moveObjects();
        Camera.cameraScroll();
        Objects.processObjects();
        var camera = Camera.camera;
        PA.EasyBgScrollXY(Globals.MAINSCREEN, 1, camera.x>>8, camera.y>>8);
	    PA.EasyBgScrollXY(Globals.MAINSCREEN, 2, (camera.x>>8)>>1, (camera.y>>8)>>1);
	    PA.EasyBgScrollXY(Globals.MAINSCREEN, 3, (camera.x>>8)>>2, (camera.y>>8)>>2);

        CA.FadeIn(0);

        int midBgX = 0, backBgX = 0;
        int timer = 0;
        bool paused = false;

        // Infinite loop to keep the program running
        while (true)
        {
            //start_test();
            midBgX -= Jelli.level[currentLevel].midscroll;
            backBgX -= Jelli.level[currentLevel].backscroll;
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
                    CA.SimpleText(0, 4, 12, "Mario sprite by RangeTE");
                    PA.WaitForVBL();
                    CA.Update16c();
                    yield return null;
                }
                CA.Information(0, "\n- Paused -\nPress START to unpause the game\nHold SELECT for credits");
                CA.SimpleText(0, 4, 180, "Hint: your movement affects the points you gain...");
                PA.WaitForVBL();
                CA.Update16c();
                yield return null;
            }
            if(Pad.Newpress.Start) { paused = true; }
  
            if(Pad.Newpress.B && Pad.Newpress.L && Pad.Newpress.R) { 
                Objects.createObject(Classes.DUMMY, Objects.objects[0].x>>8, Objects.objects[0].y>>8, 0); 
            }

            Objects.processObjects();
            Camera.cameraScroll();
            Objects.moveObjects();
            camera = Camera.camera;
            PA.EasyBgScrollXY(Globals.MAINSCREEN, 1, camera.x>>8, camera.y>>8);
            PA.EasyBgScrollXY(Globals.MAINSCREEN, 2, ((camera.x+midBgX)>>8)>>1, (camera.y>>8)>>1);
            PA.EasyBgScrollXY(Globals.MAINSCREEN, 3, ((camera.x+backBgX)>>8)>>2, (camera.y>>8)>>2);

            //displayDebug(0);

            //CA_BoxText(0, 100, 48, 32, "Welcome to Alpha 2 of Parallel Worlds! The aim of this demo is to gather as many points as you can by jumping on the Mario streakers. Be quick though, as your points decrease over time!");
            
            CA.SimpleText(0, 4, 180, "Points: {0}", Globals.playerPoints>>12);
            CA.SimpleText(0, 208, 180, Globals.VERSION);
            
            PA.WaitForVBL();
            CA.Update16c();    
            yield return null;
        }
        
    }
}
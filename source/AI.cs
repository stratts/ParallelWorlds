static class AI {

    private static ObjectInfo[] objects => Objects.objects;
    private static int currentObject => Objects.currentObject;

    public static void generalCharacter()
    {
        
        objects[currentObject].oldposx = objects[currentObject].x;

        if(Pad.Held.Left || Pad.Held.Right)
        {
            //--------------------------------------------------------------------------------
            // Basic Movement Code
            //--------------------------------------------------------------------------------

            // Moving left
            if(Pad.Held.Left)
            {
                PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 1);

                    objects[currentObject].x -= objects[currentObject].objClass.speed;
                    if(objects[currentObject].action == 0)
                        Objects.animateObject(currentObject, 
                                    objects[currentObject].objClass.walk.f0, 
                                    objects[currentObject].objClass.walk.f1, 
                                    objects[currentObject].objClass.animSpeed);
                

            }

            // Moving right
            if(Pad.Held.Right)
            {
                PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 0);

                    objects[currentObject].x += objects[currentObject].objClass.speed;
                    if(objects[currentObject].action == 0)
                        Objects.animateObject(currentObject, 
                                        objects[currentObject].objClass.walk.f0, 
                                        objects[currentObject].objClass.walk.f1, 
                                        objects[currentObject].objClass.animSpeed);

            }



            // End movement code
        }

        else if(objects[currentObject].action == 0)
        {
                Objects.animateObject(currentObject, 
                                    objects[currentObject].objClass.idle.f0, 
                                    objects[currentObject].objClass.idle.f1, 
                                    objects[currentObject].objClass.animSpeed);
        }


        Objects.objectAddGravity();

        if(Collisions.touchingGround(currentObject)) 
        {
            objects[currentObject].vy = 0;
            objects[currentObject].action = 0;
            objects[currentObject].jumping = false;	
        }

        if(Pad.Newpress.Up && Collisions.touchingGround(currentObject)) 
        {
            objects[currentObject].action = 1;
            objects[currentObject].vy = -1800;
            objects[currentObject].jumping = true;
        }

        if(objects[currentObject].vy > 512) objects[currentObject].action = 2;
        else if(objects[currentObject].jumping && objects[currentObject].vy > 0) objects[currentObject].action = 2;

        if(objects[currentObject].action == 1) Objects.animateObject(currentObject, 
                                                    objects[currentObject].objClass.jump.f0, 
                                                    objects[currentObject].objClass.jump.f1, 
                                                    objects[currentObject].objClass.animSpeed);
                    

        else if(objects[currentObject].action == 2)Objects.animateObject(currentObject, 
                                                        objects[currentObject].objClass.fall.f0, 
                                                        objects[currentObject].objClass.fall.f1, 
                                                        objects[currentObject].objClass.animSpeed);
                    


        objects[currentObject].newposx = objects[currentObject].x;
        objects[currentObject].relspeedx = objects[currentObject].oldposx - objects[currentObject].newposx;
        objects[currentObject].relspeedy = objects[currentObject].vy;

        Objects.objectCheckCollision();

        if (!Objects.inStageZone(currentObject))
        {
            Camera.camera.x = 0;
            Camera.camera.y = 0;
            objects[currentObject].y = objects[currentObject].startY;
            objects[currentObject].x = objects[currentObject].startX;
        }

        Functions.setSpriteXY(Globals.MAINSCREEN, objects[currentObject].sprite, (objects[currentObject].x - Camera.camera.x)>>8, (objects[currentObject].y - Camera.camera.y)>>8);

    }

    public static void generalCPU()
    {
        objects[currentObject].oldposx = objects[currentObject].x;

        if((objects[Functions.lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 < -(objects[Functions.lowestXinObj(objects, 3)].objClass.width+2) || (objects[Functions.lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 > objects[Functions.lowestXinObj(objects, 3)].objClass.width+2)
        {
            if(objects[Functions.lowestXinObj(objects, 3)].x > objects[currentObject].x)
            {
                PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 0);
                if(!objects[currentObject].jumping) objects[currentObject].action = 1;
                objects[currentObject].x += objects[currentObject].objClass.speed-PA.RandMax(128);
            }

            else if(objects[Functions.lowestXinObj(objects, 3)].x < objects[currentObject].x)
            {	
                PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 1);
                if(!objects[currentObject].jumping) objects[currentObject].action = 1;
                objects[currentObject].x -= objects[currentObject].objClass.speed-PA.RandMax(128);
            }

            /*if(((objects[Functions.lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 < -80 || (objects[Functions.lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 > 80) && !objects[currentObject].jumping)
            {
                if((objects[currentObject].y - objects[Functions.lowestXinObj(objects, 3)].y) >> 8 > 48) 
                {
                    objects[currentObject].vy = -1200;
                    objects[currentObject].jumping = true;
                }
            }*/

            if((Collisions.leftCollision(currentObject) || Collisions.rightCollision(currentObject)) && (!Collisions.rightCollisionLarge(currentObject) && !Collisions.leftCollisionLarge(currentObject)) && !objects[currentObject].jumping) 
            {
                objects[currentObject].vy = -1400;
                objects[currentObject].action = 2;
                objects[currentObject].jumping = true;
            }
        }

        else objects[currentObject].action = 0;

        objects[currentObject].y += objects[currentObject].vy;
        if(!Collisions.touchingGround(currentObject) && objects[currentObject].vy < objects[currentObject].objClass.weight) objects[currentObject].vy += 80;
        if(!Collisions.touchingGround(currentObject) && objects[currentObject].vy > 512) objects[currentObject].action = 3;

        

        if(Collisions.touchingGround(currentObject)) 
        {
            objects[currentObject].vy = 0;
            objects[currentObject].jumping = false;	
        }

        switch(objects[currentObject].action)
        {
            case 0:
                Objects.animateObject(currentObject, 
                        objects[currentObject].objClass.idle.f0, 
                        objects[currentObject].objClass.idle.f1, 
                        objects[currentObject].objClass.animSpeed); break;
            case 1:
                Objects.animateObject(currentObject, 
                        objects[currentObject].objClass.walk.f0, 
                        objects[currentObject].objClass.walk.f1, 
                        objects[currentObject].objClass.animSpeed); break;

            case 2:
                Objects.animateObject(currentObject, 
                        objects[currentObject].objClass.jump.f0, 
                        objects[currentObject].objClass.jump.f1, 
                        objects[currentObject].objClass.animSpeed); break;
            case 3:
                Objects.animateObject(currentObject, 
                        objects[currentObject].objClass.fall.f0, 
                        objects[currentObject].objClass.fall.f1, 
                        objects[currentObject].objClass.animSpeed); break;
        }

        objects[currentObject].newposx = objects[currentObject].x;
        objects[currentObject].relspeedx = objects[currentObject].oldposx - objects[currentObject].newposx;
        objects[currentObject].relspeedy = objects[currentObject].vy;

        Objects.objectCheckCollision();

        if (!Objects.inStageZone(currentObject))
        {
            Camera.camera.x = 0;
            Camera.camera.y = 0;

            objects[currentObject].y = objects[currentObject].startY;
            objects[currentObject].x = objects[currentObject].startX;
        }


        Functions.setSpriteXY(Globals.MAINSCREEN, objects[currentObject].sprite, (objects[currentObject].x - Camera.camera.x)>>8, (objects[currentObject].y - Camera.camera.y)>>8);

    }

    public static void aiGenericGround()
    {
        // Only activate the AI when the object is visible
        if(Objects.objectInCanvas(currentObject)) objects[currentObject].activated = true;

        // AI code
        if(objects[currentObject].activated && objects[currentObject].alive)
        {
            objects[currentObject].x += objects[currentObject].objClass.speed*objects[currentObject].moveDirection;
            

            if(Collisions.leftCollision(currentObject) || Collisions.rightCollision(currentObject)) objects[currentObject].i++;
            else objects[currentObject].i = 0; 

            if(objects[currentObject].i > 80)
            { 
                objects[currentObject].i = 0; 

                if(Collisions.leftCollision(currentObject)) objects[currentObject].moveDirection = 1;
                else if(Collisions.rightCollision(currentObject)) objects[currentObject].moveDirection = -1; 
            }

            else if(objects[currentObject].i > 5)
            { 
                objects[currentObject].action = 0;

                if(objects[currentObject].i > 40)
                {
                    if(Collisions.leftCollision(currentObject)) PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 0);
                    else if(Collisions.rightCollision(currentObject)) PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 1);
                }
            }

            else 
            {
                objects[currentObject].action = 1;
                if(objects[currentObject].moveDirection == -1) PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 1);
                else PA.SetSpriteHflip(Globals.MAINSCREEN, objects[currentObject].sprite, 0);
            }

            

            switch(objects[currentObject].action)
            {
                case 1:
                    Objects.animateObject(currentObject,
                        objects[currentObject].objClass.walk.f0,
                        objects[currentObject].objClass.walk.f1,
                        objects[currentObject].objClass.animSpeed); break;

                default:
                    Objects.animateObject(currentObject,
                        objects[currentObject].objClass.idle.f0,
                        objects[currentObject].objClass.idle.f1,
                        objects[currentObject].objClass.animSpeed); break;
            }

            if(Collisions.objectCollisionTop(0, currentObject) && objects[0].vy>0 && objects[currentObject].alive) 
            {
                objects[currentObject].alive = false;
                if(objects[0].relspeedx >= 0) Globals.playerPoints += (objects[0].vy<<2)*(objects[0].relspeedx+1);
                else Globals.playerPoints += objects[0].vy*-(objects[0].relspeedx+1);
                if(!objects[currentObject].jumping)
                {
                    objects[currentObject].vy = -500;
                    objects[0].vy = -1200;
                    objects[currentObject].jumping = true;
                }
            }

            if(!Objects.inStageZone(currentObject)) objects[currentObject].alive = false;

        }

        else if (!objects[currentObject].alive) 
        {

            if(!Objects.inStageZone(currentObject)) Objects.deleteObject(currentObject);
                
            //objects[currentObject].rotation -= objects[currentObject].moveDirection*5;
        }

        // Collisions and gravity
        if(objects[currentObject].alive) Objects.objectCheckCollision();

        Objects.objectAddGravity();
    }

    public static void aiGenericNone()
    {

    }

    public static void aiDummy()
    {
        var currentWorld = Levels.currentWorld;
        var currentLevel = Levels.currentLevel;

        objects[currentObject].oldposx = objects[currentObject].x;
        objects[currentObject].oldposy = objects[currentObject].y;

        Objects.animateObject(currentObject, 
                                    objects[currentObject].objClass.idle.f0, 
                                    objects[currentObject].objClass.idle.f1, 
                                    objects[currentObject].objClass.animSpeed);

        objects[currentObject].y += objects[currentObject].vy;
        if(!Collisions.touchingGround(currentObject)) objects[currentObject].vy += 80;

        if(Collisions.touchingGround(currentObject)) objects[currentObject].vy = 0;

        objects[currentObject].newposx = objects[currentObject].x;
        objects[currentObject].newposy = objects[currentObject].y;
        objects[currentObject].relspeedx = objects[currentObject].oldposx - objects[currentObject].newposx;
        objects[currentObject].relspeedy = objects[currentObject].vy;
        Objects.objectCheckCollision();

        if (objects[currentObject].y>>8 > currentWorld.level[currentLevel].height + 256)
        {
            Camera.camera.x = 1<<8;
            Camera.camera.y = 1<<8;
            objects[currentObject].y = objects[currentObject].startY;
            objects[currentObject].x = objects[currentObject].startX;
            objects[currentObject].vy = 0;
        }

    }
}


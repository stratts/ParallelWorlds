using static Objects;
using static Collisions;
using static Globals;
using static Functions;

static class AI {

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
                PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 1);

                    objects[currentObject].x -= objects[currentObject].objClass.speed;
                    if(objects[currentObject].action == 0)
                        animateObject(currentObject, 
                                    objects[currentObject].objClass.walk.start, 
                                    objects[currentObject].objClass.walk.end, 
                                    objects[currentObject].objClass.animSpeed);
                

            }

            // Moving right
            if(Pad.Held.Right)
            {
                PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 0);

                    objects[currentObject].x += objects[currentObject].objClass.speed;
                    if(objects[currentObject].action == 0)
                        animateObject(currentObject, 
                                        objects[currentObject].objClass.walk.start, 
                                        objects[currentObject].objClass.walk.end, 
                                        objects[currentObject].objClass.animSpeed);

            }



            // End movement code
        }

        else if(objects[currentObject].action == 0)
        {
                animateObject(currentObject, 
                                    objects[currentObject].objClass.idle.start, 
                                    objects[currentObject].objClass.idle.end, 
                                    objects[currentObject].objClass.animSpeed);
        }


        objectAddGravity();

        if(touchingGround(currentObject)) 
        {
            objects[currentObject].vy = 0;
            objects[currentObject].action = 0;
            objects[currentObject].jumping = false;	
        }

        if(Pad.Newpress.Up && touchingGround(currentObject)) 
        {
            objects[currentObject].action = 1;
            objects[currentObject].vy = -1800;
            objects[currentObject].jumping = true;
        }

        if(objects[currentObject].vy > 512) objects[currentObject].action = 2;
        else if(objects[currentObject].jumping && objects[currentObject].vy > 0) objects[currentObject].action = 2;

        if(objects[currentObject].action == 1) animateObject(currentObject, 
                                                    objects[currentObject].objClass.jump.start, 
                                                    objects[currentObject].objClass.jump.end, 
                                                    objects[currentObject].objClass.animSpeed);
                    

        else if(objects[currentObject].action == 2)animateObject(currentObject, 
                                                        objects[currentObject].objClass.fall.start, 
                                                        objects[currentObject].objClass.fall.end, 
                                                        objects[currentObject].objClass.animSpeed);
                    


        objects[currentObject].newposx = objects[currentObject].x;
        objects[currentObject].relspeedx = objects[currentObject].oldposx - objects[currentObject].newposx;
        objects[currentObject].relspeedy = objects[currentObject].vy;

        objectCheckCollision();

        if (!inStageZone(currentObject))
        {
            Camera.camera.x = 0;
            Camera.camera.y = 0;
            objects[currentObject].y = objects[currentObject].startY;
            objects[currentObject].x = objects[currentObject].startX;
        }

        setSpriteXY(MAINSCREEN, objects[currentObject].sprite, (objects[currentObject].x - Camera.camera.x)>>8, (objects[currentObject].y - Camera.camera.y)>>8);

    }

    public static void generalCPU()
    {
        objects[currentObject].oldposx = objects[currentObject].x;

        if((objects[lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 < -(objects[lowestXinObj(objects, 3)].objClass.width+2) || (objects[lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 > objects[lowestXinObj(objects, 3)].objClass.width+2)
        {
            if(objects[lowestXinObj(objects, 3)].x > objects[currentObject].x)
            {
                PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 0);
                if(!objects[currentObject].jumping) objects[currentObject].action = 1;
                objects[currentObject].x += objects[currentObject].objClass.speed-PA.RandMax(128);
            }

            else if(objects[lowestXinObj(objects, 3)].x < objects[currentObject].x)
            {	
                PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 1);
                if(!objects[currentObject].jumping) objects[currentObject].action = 1;
                objects[currentObject].x -= objects[currentObject].objClass.speed-PA.RandMax(128);
            }

            /*if(((objects[lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 < -80 || (objects[lowestXinObj(objects, 3)].x - objects[currentObject].x)>>8 > 80) && !objects[currentObject].jumping)
            {
                if((objects[currentObject].y - objects[lowestXinObj(objects, 3)].y) >> 8 > 48) 
                {
                    objects[currentObject].vy = -1200;
                    objects[currentObject].jumping = true;
                }
            }*/

            if((leftCollision(currentObject) || rightCollision(currentObject)) && (!rightCollisionLarge(currentObject) && !leftCollisionLarge(currentObject)) && !objects[currentObject].jumping) 
            {
                objects[currentObject].vy = -1400;
                objects[currentObject].action = 2;
                objects[currentObject].jumping = true;
            }
        }

        else objects[currentObject].action = 0;

        objects[currentObject].y += objects[currentObject].vy;
        if(!touchingGround(currentObject) && objects[currentObject].vy < objects[currentObject].objClass.weight) objects[currentObject].vy += 80;
        if(!touchingGround(currentObject) && objects[currentObject].vy > 512) objects[currentObject].action = 3;

        

        if(touchingGround(currentObject)) 
        {
            objects[currentObject].vy = 0;
            objects[currentObject].jumping = false;	
        }

        switch(objects[currentObject].action)
        {
            case 0:
                animateObject(currentObject, 
                        objects[currentObject].objClass.idle.start, 
                        objects[currentObject].objClass.idle.end, 
                        objects[currentObject].objClass.animSpeed); break;
            case 1:
                animateObject(currentObject, 
                        objects[currentObject].objClass.walk.start, 
                        objects[currentObject].objClass.walk.end, 
                        objects[currentObject].objClass.animSpeed); break;

            case 2:
                animateObject(currentObject, 
                        objects[currentObject].objClass.jump.start, 
                        objects[currentObject].objClass.jump.end, 
                        objects[currentObject].objClass.animSpeed); break;
            case 3:
                animateObject(currentObject, 
                        objects[currentObject].objClass.fall.start, 
                        objects[currentObject].objClass.fall.end, 
                        objects[currentObject].objClass.animSpeed); break;
        }

        objects[currentObject].newposx = objects[currentObject].x;
        objects[currentObject].relspeedx = objects[currentObject].oldposx - objects[currentObject].newposx;
        objects[currentObject].relspeedy = objects[currentObject].vy;

        objectCheckCollision();

        if (!inStageZone(currentObject))
        {
            Camera.camera.x = 0;
            Camera.camera.y = 0;

            objects[currentObject].y = objects[currentObject].startY;
            objects[currentObject].x = objects[currentObject].startX;
        }


        setSpriteXY(MAINSCREEN, objects[currentObject].sprite, (objects[currentObject].x - Camera.camera.x)>>8, (objects[currentObject].y - Camera.camera.y)>>8);

    }

    public static void aiGenericGround()
    {
        // Only activate the AI when the object is visible
        if(objectInCanvas(currentObject)) objects[currentObject].activated = true;

        // AI code
        if(objects[currentObject].activated && objects[currentObject].alive)
        {
            objects[currentObject].x += objects[currentObject].objClass.speed*objects[currentObject].moveDirection;
            

            if(leftCollision(currentObject) || rightCollision(currentObject)) objects[currentObject].i++;
            else objects[currentObject].i = 0; 

            if(objects[currentObject].i > 80)
            { 
                objects[currentObject].i = 0; 

                if(leftCollision(currentObject)) objects[currentObject].moveDirection = 1;
                else if(rightCollision(currentObject)) objects[currentObject].moveDirection = -1; 
            }

            else if(objects[currentObject].i > 5)
            { 
                objects[currentObject].action = 0;

                if(objects[currentObject].i > 40)
                {
                    if(leftCollision(currentObject)) PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 0);
                    else if(rightCollision(currentObject)) PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 1);
                }
            }

            else 
            {
                objects[currentObject].action = 1;
                if(objects[currentObject].moveDirection == -1) PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 1);
                else PA.SetSpriteHflip(MAINSCREEN, objects[currentObject].sprite, 0);
            }

            

            switch(objects[currentObject].action)
            {
                case 1:
                    animateObject(currentObject,
                        objects[currentObject].objClass.walk.start,
                        objects[currentObject].objClass.walk.end,
                        objects[currentObject].objClass.animSpeed); break;

                default:
                    animateObject(currentObject,
                        objects[currentObject].objClass.idle.start,
                        objects[currentObject].objClass.idle.end,
                        objects[currentObject].objClass.animSpeed); break;
            }

            if(objectCollisionTop(0, currentObject) && objects[0].vy>0 && objects[currentObject].alive) 
            {
                objects[currentObject].alive = false;
                if(objects[0].relspeedx >= 0) playerPoints += (objects[0].vy<<2)*(objects[0].relspeedx+1);
                else playerPoints += objects[0].vy*-(objects[0].relspeedx+1);
                if(!objects[currentObject].jumping)
                {
                    objects[currentObject].vy = -500;
                    objects[0].vy = -1200;
                    objects[currentObject].jumping = true;
                }
            }

            if(!inStageZone(currentObject)) objects[currentObject].alive = false;

        }

        else if (!objects[currentObject].alive) 
        {

            if(!inStageZone(currentObject)) deleteObject(currentObject);
                
            //objects[currentObject].rotation -= objects[currentObject].moveDirection*5;
        }

        // Collisions and gravity
        if(objects[currentObject].alive) objectCheckCollision();

        objectAddGravity();
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

        animateObject(currentObject, 
                                    objects[currentObject].objClass.idle.start, 
                                    objects[currentObject].objClass.idle.end, 
                                    objects[currentObject].objClass.animSpeed);

        objects[currentObject].y += objects[currentObject].vy;
        if(!touchingGround(currentObject)) objects[currentObject].vy += 80;

        if(touchingGround(currentObject)) objects[currentObject].vy = 0;

        objects[currentObject].newposx = objects[currentObject].x;
        objects[currentObject].newposy = objects[currentObject].y;
        objects[currentObject].relspeedx = objects[currentObject].oldposx - objects[currentObject].newposx;
        objects[currentObject].relspeedy = objects[currentObject].vy;
        objectCheckCollision();

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


using static Objects;
using static Collisions;
using static Defines;
using static Functions;
using static Levels;

static class AI {

    public static void generalCharacter()
    {   
        ObjectInfo obj = objects[currentObject];
        obj.oldposx = obj.x;

        if(Pad.Held.Left || Pad.Held.Right)
        {
            //--------------------------------------------------------------------------------
            // Basic Movement Code
            //--------------------------------------------------------------------------------

            // Moving left
            if(Pad.Held.Left)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);

                    obj.x -= obj.objClass.speed;
                    if(obj.action == 0)
                        animateObject(currentObject, 
                                    obj.objClass.walk.start, 
                                    obj.objClass.walk.end, 
                                    obj.objClass.animSpeed);
                

            }

            // Moving right
            if(Pad.Held.Right)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);

                    obj.x += obj.objClass.speed;
                    if(obj.action == 0)
                        animateObject(currentObject, 
                                        obj.objClass.walk.start, 
                                        obj.objClass.walk.end, 
                                        obj.objClass.animSpeed);

            }



            // End movement code
        }

        else if(obj.action == 0)
        {
                animateObject(currentObject, 
                                    obj.objClass.idle.start, 
                                    obj.objClass.idle.end, 
                                    obj.objClass.animSpeed);
        }


        objectAddGravity();

        if(touchingGround(currentObject)) 
        {
            obj.vy = 0;
            obj.action = 0;
            obj.jumping = false; 
        }

        if(Pad.Newpress.Up && touchingGround(currentObject)) 
        {
            obj.action = 1;
            obj.vy = -1800;
            obj.jumping = true;
        }

        if(obj.vy > 512) obj.action = 2;
        else if(obj.jumping && obj.vy > 0) obj.action = 2;

        if(obj.action == 1) animateObject(currentObject, 
                                                    obj.objClass.jump.start, 
                                                    obj.objClass.jump.end, 
                                                    obj.objClass.animSpeed);
                    

        else if(obj.action == 2)animateObject(currentObject, 
                                                        obj.objClass.fall.start, 
                                                        obj.objClass.fall.end, 
                                                        obj.objClass.animSpeed);
                    


        obj.newposx = obj.x;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;

        objectCheckCollision();

        if (!inStageZone(currentObject))
        {
            Camera.camera.x = 0;
            Camera.camera.y = 0;
            obj.y = obj.startY;
            obj.x = obj.startX;
        }

        setSpriteXY(MAINSCREEN, obj.sprite, (obj.x - Camera.camera.x)>>8, (obj.y - Camera.camera.y)>>8);

    }

    public static void generalCPU()
    {
        ObjectInfo obj = objects[currentObject];
        obj.oldposx = obj.x;

        if((objects[lowestXinObj(objects, 3)].x - obj.x)>>8 < -(objects[lowestXinObj(objects, 3)].objClass.width+2) || (objects[lowestXinObj(objects, 3)].x - obj.x)>>8 > objects[lowestXinObj(objects, 3)].objClass.width+2)
        {
            if(objects[lowestXinObj(objects, 3)].x > obj.x)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                if(!obj.jumping) obj.action = 1;
                obj.x += obj.objClass.speed-PA.RandMax(128);
            }

            else if(objects[lowestXinObj(objects, 3)].x < obj.x)
            {   
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                if(!obj.jumping) obj.action = 1;
                obj.x -= obj.objClass.speed-PA.RandMax(128);
            }

            /*if(((objects[lowestXinObj(objects, 3)].x - obj.x)>>8 < -80 || (objects[lowestXinObj(objects, 3)].x - obj.x)>>8 > 80) && !obj.jumping)
            {
                if((obj.y - objects[lowestXinObj(objects, 3)].y) >> 8 > 48) 
                {
                    obj.vy = -1200;
                    obj.jumping = true;
                }
            }*/

            if((leftCollision(currentObject) || rightCollision(currentObject)) && (!rightCollisionLarge(currentObject) && !leftCollisionLarge(currentObject)) && !obj.jumping) 
            {
                obj.vy = -1400;
                obj.action = 2;
                obj.jumping = true;
            }
        }

        else obj.action = 0;

        obj.y += obj.vy;
        if(!touchingGround(currentObject) && obj.vy < obj.objClass.weight) obj.vy += 80;
        if(!touchingGround(currentObject) && obj.vy > 512) obj.action = 3;

        

        if(touchingGround(currentObject)) 
        {
            obj.vy = 0;
            obj.jumping = false; 
        }

        switch(obj.action)
        {
            case 0:
                animateObject(currentObject, 
                        obj.objClass.idle.start, 
                        obj.objClass.idle.end, 
                        obj.objClass.animSpeed); break;
            case 1:
                animateObject(currentObject, 
                        obj.objClass.walk.start, 
                        obj.objClass.walk.end, 
                        obj.objClass.animSpeed); break;

            case 2:
                animateObject(currentObject, 
                        obj.objClass.jump.start, 
                        obj.objClass.jump.end, 
                        obj.objClass.animSpeed); break;
            case 3:
                animateObject(currentObject, 
                        obj.objClass.fall.start, 
                        obj.objClass.fall.end, 
                        obj.objClass.animSpeed); break;
        }

        obj.newposx = obj.x;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;

        objectCheckCollision();

        if (!inStageZone(currentObject))
        {
            Camera.camera.x = 0;
            Camera.camera.y = 0;

            obj.y = obj.startY;
            obj.x = obj.startX;
        }


        setSpriteXY(MAINSCREEN, obj.sprite, (obj.x - Camera.camera.x)>>8, (obj.y - Camera.camera.y)>>8);

    }

    public static void aiGenericGround()
    {
        ObjectInfo obj = objects[currentObject];
        // Only activate the AI when the object is visible
        if(objectInCanvas(currentObject)) obj.activated = true;

        // AI code
        if(obj.activated && obj.alive)
        {
            obj.x += obj.objClass.speed*obj.moveDirection;
            

            if(leftCollision(currentObject) || rightCollision(currentObject)) obj.i++;
            else obj.i = 0; 

            if(obj.i > 80)
            { 
                obj.i = 0; 

                if(leftCollision(currentObject)) obj.moveDirection = 1;
                else if(rightCollision(currentObject)) obj.moveDirection = -1; 
            }

            else if(obj.i > 5)
            { 
                obj.action = 0;

                if(obj.i > 40)
                {
                    if(leftCollision(currentObject)) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                    else if(rightCollision(currentObject)) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                }
            }

            else 
            {
                obj.action = 1;
                if(obj.moveDirection == -1) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                else PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
            }

            

            switch(obj.action)
            {
                case 1:
                    animateObject(currentObject,
                        obj.objClass.walk.start,
                        obj.objClass.walk.end,
                        obj.objClass.animSpeed); break;

                default:
                    animateObject(currentObject,
                        obj.objClass.idle.start,
                        obj.objClass.idle.end,
                        obj.objClass.animSpeed); break;
            }

            if(objectCollisionTop(0, currentObject) && objects[0].vy>0 && obj.alive) 
            {
                obj.alive = false;
                if(objects[0].relspeedx >= 0) playerPoints += (objects[0].vy<<2)*(objects[0].relspeedx+1);
                else playerPoints += objects[0].vy*-(objects[0].relspeedx+1);
                if(!obj.jumping)
                {
                    obj.vy = -500;
                    objects[0].vy = -1200;
                    obj.jumping = true;
                }
            }

            if(!inStageZone(currentObject)) obj.alive = false;

        }

        else if (!obj.alive) 
        {

            if(!inStageZone(currentObject)) deleteObject(currentObject);
                
            //obj.rotation -= obj.moveDirection*5;
        }

        // Collisions and gravity
        if(obj.alive) objectCheckCollision();

        objectAddGravity();
    }

    public static void aiGenericNone()
    {

    }

    public static void aiDummy()
    {
        ObjectInfo obj = objects[currentObject];
        obj.oldposx = obj.x;
        obj.oldposy = obj.y;

        animateObject(currentObject, 
                                    obj.objClass.idle.start, 
                                    obj.objClass.idle.end, 
                                    obj.objClass.animSpeed);

        obj.y += obj.vy;
        if(!touchingGround(currentObject)) obj.vy += 80;

        if(touchingGround(currentObject)) obj.vy = 0;

        obj.newposx = obj.x;
        obj.newposy = obj.y;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;
        objectCheckCollision();

        if (obj.y>>8 > currentWorld.level[currentLevel].height + 256)
        {
            Camera.camera.x = 1<<8;
            Camera.camera.y = 1<<8;
            obj.y = obj.startY;
            obj.x = obj.startX;
            obj.vy = 0;
        }

    }
}


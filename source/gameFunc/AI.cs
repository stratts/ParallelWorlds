using static Collisions;
using static Defines;
using static Functions;
using static Levels;

static class AI
{

    public static void generalCharacter(ObjectInfo obj, Scene scene)
    {
        obj.oldposx = obj.x;

        if (Pad.Held.Left || Pad.Held.Right)
        {
            //--------------------------------------------------------------------------------
            // Basic Movement Code
            //--------------------------------------------------------------------------------

            // Moving left
            if (Pad.Held.Left)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);

                obj.x -= obj.objClass.speed;
                if (obj.action == 0) obj.Animate(Animation.Walk);
            }

            // Moving right
            if (Pad.Held.Right)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);

                obj.x += obj.objClass.speed;
                if (obj.action == 0) obj.Animate(Animation.Walk);
            }

            // End movement code
        }

        else if (obj.action == 0)
        {
            obj.Animate(Animation.Idle);
        }


        obj.AddGravity();

        if (touchingGround(obj))
        {
            obj.vy = 0;
            obj.action = 0;
            obj.jumping = false;
        }

        if (Pad.Newpress.Up && touchingGround(obj))
        {
            obj.action = 1;
            obj.vy = -1800f / 256;
            obj.jumping = true;
        }

        if (obj.vy > 2) obj.action = 2;
        else if (obj.jumping && obj.vy > 0) obj.action = 2;

        if (obj.action == 1) obj.Animate(Animation.Jump);
        else if (obj.action == 2) obj.Animate(Animation.Fall);

        obj.newposx = obj.x;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;

        obj.CheckCollision();

        if (!obj.InStageZone())
        {
            scene.Camera.x = 0;
            scene.Camera.y = 0;
            obj.y = obj.startY;
            obj.x = obj.startX;
        }
    }

    public static void generalCPU(ObjectInfo obj, Scene scene)
    {
        obj.oldposx = obj.x;
        var objects = scene.Objects;

        if ((objects[lowestXinObj(obj, objects, 3)].x - obj.x) < -(objects[lowestXinObj(obj, objects, 3)].objClass.width + 2) || (objects[lowestXinObj(obj, objects, 3)].x - obj.x) > objects[lowestXinObj(obj, objects, 3)].objClass.width + 2)
        {
            if (objects[lowestXinObj(obj, objects, 3)].x > obj.x)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                if (!obj.jumping) obj.action = 1;
                obj.x += obj.objClass.speed - (((float)PA.RandMax(128)) / 256);
            }

            else if (objects[lowestXinObj(obj, objects, 3)].x < obj.x)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                if (!obj.jumping) obj.action = 1;
                obj.x -= obj.objClass.speed - (((float)PA.RandMax(128)) / 256);
            }

            /*if(((objects[lowestXinObj(objects, 3)].x - obj.x)>>8 < -80 || (objects[lowestXinObj(objects, 3)].x - obj.x)>>8 > 80) && !obj.jumping)
            {
                if((obj.y - objects[lowestXinObj(objects, 3)].y) >> 8 > 48) 
                {
                    obj.vy = -1200;
                    obj.jumping = true;
                }
            }*/

            if ((leftCollision(obj) || rightCollision(obj)) && (!rightCollisionLarge(obj) && !leftCollisionLarge(obj)) && !obj.jumping)
            {
                obj.vy = -1400f / 256;
                obj.action = 2;
                obj.jumping = true;
            }
        }

        else obj.action = 0;

        obj.AddGravity();
        if (!touchingGround(obj) && obj.vy > 2f) obj.action = 3;

        if (touchingGround(obj))
        {
            obj.vy = 0;
            obj.jumping = false;
        }

        switch (obj.action)
        {
            case 0:
                obj.Animate(Animation.Idle); break;
            case 1:
                obj.Animate(Animation.Walk); break;
            case 2:
                obj.Animate(Animation.Jump); break;
            case 3:
                obj.Animate(Animation.Fall); break;
        }

        obj.newposx = obj.x;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;

        obj.CheckCollision();

        if (!obj.InStageZone())
        {
            obj.y = obj.startY;
            obj.x = obj.startX;
        }
    }

    public static void aiGenericGround(ObjectInfo obj, Scene scene)
    {
        var objects = scene.Objects;
        // Only activate the AI when the object is visible
        if (scene.InCanvas(obj)) obj.activated = true;

        // AI code
        if (obj.activated && obj.alive)
        {
            obj.x += obj.objClass.speed * obj.moveDirection;


            if (leftCollision(obj) || rightCollision(obj)) obj.i++;
            else obj.i = 0;

            if (obj.i > 80)
            {
                obj.i = 0;

                if (leftCollision(obj)) obj.moveDirection = 1;
                else if (rightCollision(obj)) obj.moveDirection = -1;
            }

            else if (obj.i > 5)
            {
                obj.action = 0;

                if (obj.i > 40)
                {
                    if (leftCollision(obj)) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                    else if (rightCollision(obj)) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                }
            }

            else
            {
                obj.action = 1;
                if (obj.moveDirection == -1) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                else PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
            }



            switch (obj.action)
            {
                case 1:
                    obj.Animate(Animation.Walk); break;

                default:
                    obj.Animate(Animation.Idle); break;
            }

            if (objectCollisionTop(objects[0], obj) && objects[0].vy > 0 && obj.alive)
            {
                obj.alive = false;
                if (objects[0].relspeedx >= 0) playerPoints += (objects[0].vy / 4) * (objects[0].relspeedx + 1);
                else playerPoints += objects[0].vy * -(objects[0].relspeedx + 1);
                if (!obj.jumping)
                {
                    obj.vy = -500f / 256;
                    objects[0].vy = -1200f / 256;
                    obj.jumping = true;
                }
            }

            if (!obj.InStageZone()) obj.alive = false;

        }

        else if (!obj.alive)
        {

            if (!obj.InStageZone()) obj.Delete();

            //obj.rotation -= obj.moveDirection*5;
        }

        // Collisions and gravity
        if (obj.alive) obj.CheckCollision();

        obj.AddGravity();
    }

    public static void aiGenericNone(ObjectInfo obj, Scene scene)
    {

    }

    public static void aiDummy(ObjectInfo obj, Scene scene)
    {
        obj.oldposx = obj.x;
        obj.oldposy = obj.y;

        obj.Animate(Animation.Idle);

        obj.y += obj.vy;
        if (!touchingGround(obj)) obj.vy += 80f / 256;

        if (touchingGround(obj)) obj.vy = 0;

        obj.newposx = obj.x;
        obj.newposy = obj.y;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;
        obj.CheckCollision();

        if (obj.y > currentLevel.height + 256)
        {
            obj.y = obj.startY;
            obj.x = obj.startX;
            obj.vy = 0;
        }

    }
}


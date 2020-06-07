using static Collisions;
using static Defines;
using static Functions;
using static Levels;

static class AI
{

    public static void generalCharacter(ObjectInfo obj, Scene scene)
    {
        Animation animation = Animation.Idle;
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
            }

            // Moving right
            if (Pad.Held.Right)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                obj.x += obj.objClass.speed;
            }

            // End movement code
            animation = Animation.Walk;
        }

        obj.AddGravity();

        if (touchingGround(obj)) obj.vy = 0;
        else
        {
            if (obj.vy > 2) animation = Animation.Fall;
            else if (obj.vy < 0) animation = Animation.Jump;
        }
        if (Pad.Newpress.Up && touchingGround(obj)) obj.vy = -1800f / 256;

        obj.newposx = obj.x;
        obj.relspeedx = obj.oldposx - obj.newposx;
        obj.relspeedy = obj.vy;

        obj.CheckCollision();
        obj.Animate(animation);

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
        Animation animation = Animation.Idle;
        obj.oldposx = obj.x;
        var objects = scene.Objects;
        var closest = objects[lowestXinObj(obj, objects, 3)];

        if ((closest.x - obj.x) < -(closest.objClass.width + 2) || (closest.x - obj.x) > closest.objClass.width + 2)
        {
            if (closest.x > obj.x)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                obj.x += obj.objClass.speed - (((float)PA.RandMax(128)) / 256);
            }

            else if (closest.x < obj.x)
            {
                PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
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

            animation = Animation.Walk;

            if ((leftCollision(obj) || rightCollision(obj)) && touchingGround(obj)) obj.vy = -1400f / 256;
        }

        obj.AddGravity();

        if (!touchingGround(obj))
        {
            if (obj.vy > 2) animation = Animation.Fall;
            else if (obj.vy < 0) animation = Animation.Jump;
        }

        if (touchingGround(obj)) obj.vy = 0;

        obj.Animate(animation);

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
        Animation animation = Animation.Idle;
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
                if (obj.i > 40)
                {
                    if (leftCollision(obj)) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
                    else if (rightCollision(obj)) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                }
            }

            else
            {
                animation = Animation.Walk;
                if (obj.moveDirection == -1) PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 1);
                else PA.SetSpriteHflip(MAINSCREEN, obj.sprite, 0);
            }

            obj.Animate(animation);

            if (objectCollisionTop(objects[0], obj) && objects[0].vy > 0 && obj.alive)
            {
                obj.alive = false;
                if (objects[0].relspeedx >= 0) playerPoints += (objects[0].vy / 4) * (objects[0].relspeedx + 1);
                else playerPoints += objects[0].vy * -(objects[0].relspeedx + 1);
                if (obj.vy >= 0)
                {
                    obj.vy = -500f / 256;
                    objects[0].vy = -1200f / 256;
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


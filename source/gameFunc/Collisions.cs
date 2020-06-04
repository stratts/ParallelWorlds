using static Defines;
using static Levels;

static class Collisions
{
    public static bool objectCollision(ObjectInfo obj1, ObjectInfo obj2)
    {
        // Get edges of both rectangles
        int left1 = (int)obj1.x;
        int up1 = (int)obj1.y;
        int right1 = left1 + obj1.objClass.width;
        int down1 = up1 + obj1.objClass.height;

        int left2 = (int)obj2.x;
        int up2 = (int)obj2.y;
        int right2 = left2 + obj2.objClass.width;
        int down2 = up2 + obj2.objClass.height;

        if (left1 > right2 || right1 < left2 || up1 > down2 || down1 < up2) return false;
        return true;
    }

    public static bool objectCollisionTop(ObjectInfo obj1, ObjectInfo obj2)
    {
        int down1 = (int)obj1.y + obj1.objClass.height;
        int up2 = (int)obj2.y;
        // Return true if collision point is within top quarter of object
        return objectCollision(obj1, obj2) && down1 - up2 < obj2.objClass.height / 4;
    }

    public static bool upCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight - 2;

        if (getCollisionPix(MISCSCREEN, 3, playerx, playery - playerheight)) return true;
        if (getCollisionPix(MISCSCREEN, 3, (playerx - (playerwidth >> 1)) + COLLISION_BORDER, playery - playerheight)) return true;
        if (getCollisionPix(MISCSCREEN, 3, (playerx + (playerwidth >> 1)) - COLLISION_BORDER, playery - playerheight)) return true;
        return false;


    }

    public static bool downCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight - 2;


        if (getCollisionPix(MISCSCREEN, 3, playerx, playery)) return true;
        if (getCollisionPix(MISCSCREEN, 3, (playerx - (playerwidth >> 1)) + COLLISION_BORDER, playery)) return true;
        if (getCollisionPix(MISCSCREEN, 3, (playerx + (playerwidth >> 1)) - COLLISION_BORDER, playery)) return true;
        return false;
    }

    public static bool touchingGround(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight - 1;

        if (getCollisionPix(MISCSCREEN, 3, playerx, playery)) return true;
        if (getCollisionPix(MISCSCREEN, 3, (playerx - (playerwidth >> 1)) + COLLISION_BORDER, playery)) return true;
        if (getCollisionPix(MISCSCREEN, 3, (playerx + (playerwidth >> 1)) - COLLISION_BORDER, playery)) return true;
        return false;
    }

    public static bool leftCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight - 2;

        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), playery - COLLISION_BORDER)) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), playery - (playerheight >> 1))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), playery - (playerheight >> 2))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), (playery - playerheight) + COLLISION_BORDER)) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), (playery - playerheight) + (playerheight >> 2) + (playerheight >> 3))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), (playery - playerheight) + (playerheight >> 3))) return true;
        return false;
    }

    public static bool rightCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight - 2;


        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), playery - COLLISION_BORDER)) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), playery - (playerheight >> 1))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), playery - (playerheight >> 2))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), (playery - playerheight) + COLLISION_BORDER)) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), (playery - playerheight) + (playerheight >> 2) + (playerheight >> 3))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), (playery - playerheight) + (playerheight >> 3))) return true;
        return false;
    }

    public static bool leftCollisionLarge(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight / 2;

        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), playery - COLLISION_BORDER)) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), playery - (playerheight >> 1))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx - (playerwidth >> 1), (playery - playerheight) + COLLISION_BORDER)) return true;
        return false;
    }

    public static bool rightCollisionLarge(ObjectInfo obj)
    {
        if (!obj.alive) return false;

        int playerheight = obj.objClass.height;
        int playerwidth = obj.objClass.width;
        int playerx = (int)obj.cx;
        int playery = (int)obj.y + playerheight / 2;


        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), playery - COLLISION_BORDER)) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), playery - (playerheight >> 1))) return true;
        if (getCollisionPix(MISCSCREEN, 3, playerx + (playerwidth >> 1), (playery - playerheight) + COLLISION_BORDER)) return true;
        return false;
    }
}
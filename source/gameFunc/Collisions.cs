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

    public enum LineDir { Horizontal, Vertical }

    public static bool lineCollision((int x, int y) start, LineDir direction, int length)
    {
        (int x, int y) vector;
        if (direction == LineDir.Horizontal) vector = (1, 0);
        else vector = (0, 1);

        for (int i = 0; i < length; i++)
        {
            if (currentLevel.getCollisionPix(MISCSCREEN, 3, start.x + vector.x * i, start.y + vector.y * i)) return true;
            if (currentLevel.getCollisionPix(MISCSCREEN, 3, start.x - vector.x * i, start.y - vector.y * i)) return true;
        }

        return false;
    }

    public static bool upCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;
        return lineCollision(((int)obj.cx, (int)obj.y),
            LineDir.Horizontal, (obj.objClass.width / 2));
    }

    public static bool downCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;
        return lineCollision(((int)obj.cx, (int)obj.y + obj.objClass.height),
            LineDir.Horizontal, (obj.objClass.width / 2));
    }

    public static bool touchingGround(ObjectInfo obj)
    {
        if (!obj.alive) return false;
        return lineCollision(((int)obj.cx, (int)obj.y + obj.objClass.height + 1),
            LineDir.Horizontal, (obj.objClass.width / 2));
    }

    public static bool leftCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;
        return lineCollision(((int)obj.x, (int)obj.cy),
            LineDir.Vertical, (obj.objClass.height / 2) - 6);
    }

    public static bool rightCollision(ObjectInfo obj)
    {
        if (!obj.alive) return false;
        return lineCollision(((int)obj.x + obj.objClass.width, (int)obj.cy),
            LineDir.Vertical, (obj.objClass.height / 2) - 6);
    }
}
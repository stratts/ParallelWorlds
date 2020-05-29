using static Objects;
using static Levels;
using static Defines;

public static class Collisions {
    public static bool objectCollision(int object1,int object2){
        //grab sprite demensions (center positions and length and width)
        int w1 = objects[object1].objClass.width;
        int h1 = objects[object1].objClass.height;
        int x1 = objects[object1].cx>>8;
        int y1 = objects[object1].cy>>8;

        int w2 = objects[object2].objClass.width;
        int h2 = objects[object2].objClass.height;
        int x2 = objects[object2].cx>>8;
        int y2 = objects[object2].cy>>8;
        
        if(object1 != object2) 
            return (((x2 >= x1 - ((w1 + w2)>>1)) && (x2 <= x1 + ((w1 + w2)>>1)) && (y2 >= y1 - ((h1 + h2)>>1)) && (y2 <= y1 + ((h1 + h2)>>1))));
        return false;
    }

    public static bool objectCollisionTop(int object1,int object2){
        //grab sprite demensions (center positions and length and width)
        int w1 = objects[object1].objClass.width;
        int h1 = objects[object1].objClass.height;
        int x1 = objects[object1].cx>>8;
        int y1 = objects[object1].cy>>8;

        int w2 = objects[object2].objClass.width;
        int h2 = objects[object2].objClass.height;
        int x2 = objects[object2].cx>>8;
        int y2 = objects[object2].cy>>8;

        if(object1 != object2 && (y1+(h1>>1))-h1 < y2 - (h2))
            return (((x2 >= x1 - ((w1 + w2)>>1)) && (x2 <= x1 + ((w1 + w2)>>1)) && (y2 >= y1 - ((h1 + h2)>>1)) && (y2 <= y1 + ((h1 + h2)>>1))));

        return false;
    }


    public static bool upCollision(int objectNum)
    {
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 62;

        if(getCollisionPix(MISCSCREEN,3,playerx,playery-playerheight))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery-playerheight))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery-playerheight))return true;
        return false;


    }

    public static bool downCollision(int objectNum)
    {
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 62;

        
        if(getCollisionPix(MISCSCREEN,3,playerx,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery))return true;
        return false;
    }

    public static bool touchingGround(int objectNum)
    {
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 63;

        if(getCollisionPix(MISCSCREEN,3,playerx,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery))return true;
        return false;
    }

    public static bool leftCollision(int objectNum)
    {
        var objects = Objects.objects;
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 62;

        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>2)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+(playerheight>>2)+(playerheight>>3)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+(playerheight>>3)))return true;
        return false;
    }

    public static bool rightCollision(int objectNum)
    {
        var objects = Objects.objects;
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 62;


        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>2)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+(playerheight>>2)+(playerheight>>3)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+(playerheight>>3)))return true;
        return false;
    }

    public static bool leftCollisionLarge(int objectNum)
    {
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 24;

        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        return false;
    }

    public static bool rightCollisionLarge(int objectNum)
    {
        if(!objects[objectNum].alive) return false;

        int playerheight=objects[objectNum].objClass.height;
        int playerwidth=objects[objectNum].objClass.width;
        int playerx = objects[objectNum].cx>>8;
        int playery = (objects[objectNum].y>>8) + 32;


        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        return false;
    }
}
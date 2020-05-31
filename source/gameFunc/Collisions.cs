using static Objects;
using static Levels;
using static Defines;

public static class Collisions {
    public static bool objectCollision(ObjectInfo obj1, ObjectInfo obj2){
        //grab sprite demensions (center positions and length and width)
        int w1 = obj1.objClass.width;
        int h1 = obj1.objClass.height;
        int x1 = obj1.cx>>8;
        int y1 = obj1.cy>>8;

        int w2 = obj2.objClass.width;
        int h2 = obj2.objClass.height;
        int x2 = obj2.cx>>8;
        int y2 = obj2.cy>>8;
        
        if(obj1 != obj2) 
            return (((x2 >= x1 - ((w1 + w2)>>1)) && (x2 <= x1 + ((w1 + w2)>>1)) && (y2 >= y1 - ((h1 + h2)>>1)) && (y2 <= y1 + ((h1 + h2)>>1))));
        return false;
    }

    public static bool objectCollisionTop(ObjectInfo obj1, ObjectInfo obj2) {
        //grab sprite demensions (center positions and length and width)
        int w1 = obj1.objClass.width;
        int h1 = obj1.objClass.height;
        int x1 = obj1.cx>>8;
        int y1 = obj1.cy>>8;

        int w2 = obj2.objClass.width;
        int h2 = obj2.objClass.height;
        int x2 = obj2.cx>>8;
        int y2 = obj2.cy>>8;

        if(obj1 != obj2 && (y1+(h1>>1))-h1 < y2 - (h2))
            return (((x2 >= x1 - ((w1 + w2)>>1)) && (x2 <= x1 + ((w1 + w2)>>1)) && (y2 >= y1 - ((h1 + h2)>>1)) && (y2 <= y1 + ((h1 + h2)>>1))));

        return false;
    }


    public static bool upCollision(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 62;

        if(getCollisionPix(MISCSCREEN,3,playerx,playery-playerheight))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery-playerheight))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery-playerheight))return true;
        return false;


    }

    public static bool downCollision(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 62;

        
        if(getCollisionPix(MISCSCREEN,3,playerx,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery))return true;
        return false;
    }

    public static bool touchingGround(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 63;

        if(getCollisionPix(MISCSCREEN,3,playerx,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx-(playerwidth>>1))+COLLISION_BORDER,playery))return true;
        if(getCollisionPix(MISCSCREEN,3,(playerx+(playerwidth>>1))-COLLISION_BORDER,playery))return true;
        return false;
    }

    public static bool leftCollision(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 62;

        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>2)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+(playerheight>>2)+(playerheight>>3)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+(playerheight>>3)))return true;
        return false;
    }

    public static bool rightCollision(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 62;


        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>2)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+(playerheight>>2)+(playerheight>>3)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+(playerheight>>3)))return true;
        return false;
    }

    public static bool leftCollisionLarge(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 24;

        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx-(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        return false;
    }

    public static bool rightCollisionLarge(ObjectInfo obj)
    {
        if(!obj.alive) return false;

        int playerheight=obj.objClass.height;
        int playerwidth=obj.objClass.width;
        int playerx = obj.cx>>8;
        int playery = (obj.y>>8) + 32;


        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-COLLISION_BORDER))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),playery-(playerheight>>1)))return true;
        if(getCollisionPix(MISCSCREEN,3,playerx+(playerwidth>>1),(playery-playerheight)+COLLISION_BORDER))return true;
        return false;
    }
}
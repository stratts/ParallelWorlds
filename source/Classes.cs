using System;

public struct ObjectClass {
    public string name;
	public string sprite;
	public int speed, weight, jumpPower, width, height;
	public (int f0, int f1) idle;
	public (int f0, int f1) walk, run, hurt, jump, fall;
	public int animSpeed;
	public int type;
	//void (*ai)();
    public Action ai;
	public int palTaken, palNum;
	public bool created;
	public int createdSprite;
	//u16 *spritebuf, *palbuf;
}

public static class Classes {
    //--------------------------------------------------------------
    // Start defining characters
    //--------------------------------------------------------------
    public static ObjectClass MARIO;
    public static ObjectClass DUMMY;
    public static ObjectClass LINK;
    //--------------------------------------------------------------

    public static ObjectClass[] classes;

    public static void setClasses() {
        MARIO.name = "Mario";
        MARIO.sprite = CA.rootf("/characters/Mario/mario");
        MARIO.speed = 512;
        MARIO.weight = 80;
        MARIO.ai = AI.generalCharacter;
        MARIO.width = 27;
        MARIO.height = 36;
        MARIO.animSpeed = 7;
        MARIO.idle.f0 = 0;
        MARIO.idle.f1 = 3;
        MARIO.walk.f0 = 4;
        MARIO.walk.f1 = 11;
        MARIO.run.f0 = 0;
        MARIO.run.f1 = 0;
        MARIO.jump.f0 = 12;
        MARIO.jump.f1 = 12;
        MARIO.fall.f0 = 19;
        MARIO.fall.f1 = 20;

        DUMMY.name = "Dummy";
        DUMMY.sprite = CA.rootf("/characters/Mario/mario");
        DUMMY.speed = 256;
        DUMMY.weight = 80;
        DUMMY.ai = AI.aiGenericGround;
        DUMMY.width = 27;
        DUMMY.height = 40;
        DUMMY.animSpeed = 7;
        DUMMY.idle.f0 = 0;
        DUMMY.idle.f1 = 3;
        DUMMY.walk.f0 = 4;
        DUMMY.walk.f1 = 11;

        classes = new [] { LINK, MARIO, DUMMY };
    }
}
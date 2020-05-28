using System;

public struct ObjectClass {
    public string name;
	public string sprite;
	public int speed, weight, jumpPower, width, height;
	public (int start, int end) idle;
	public (int start, int end) walk, run, hurt, jump, fall;
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
        MARIO.idle.start = 0;
        MARIO.idle.end = 3;
        MARIO.walk.start = 4;
        MARIO.walk.end = 11;
        MARIO.run.start = 0;
        MARIO.run.end = 0;
        MARIO.jump.start = 12;
        MARIO.jump.end = 12;
        MARIO.fall.start = 19;
        MARIO.fall.end = 20;

        DUMMY.name = "Dummy";
        DUMMY.sprite = CA.rootf("/characters/Mario/mario");
        DUMMY.speed = 256;
        DUMMY.weight = 80;
        DUMMY.ai = AI.aiGenericGround;
        DUMMY.width = 27;
        DUMMY.height = 40;
        DUMMY.animSpeed = 7;
        DUMMY.idle.start = 0;
        DUMMY.idle.end = 3;
        DUMMY.walk.start = 4;
        DUMMY.walk.end = 11;

        classes = new [] { LINK, MARIO, DUMMY };
    }
}
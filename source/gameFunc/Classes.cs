using System;
using Microsoft.Xna.Framework;

struct ObjectClass
{
    public string name;
    public string sprite;
    public Point spriteOffset { get; set; }
    public int width, height;
    public float speed, weight, jumpPower;
    public (int start, int end) idle;
    public (int start, int end) walk, run, hurt, jump, fall;
    public int animSpeed;
    public int type;
    //void (*ai)();
    public Action<ObjectInfo, Scene> ai;
    public int palTaken, palNum;
    public bool created;
    public int createdSprite;
    //u16 *spritebuf, *palbuf;

    public (int start, int end) GetFrames(Animation animation)
    {
        switch (animation)
        {
            case Animation.Idle: return idle;
            case Animation.Walk: return walk;
            case Animation.Run: return run;
            case Animation.Hurt: return hurt;
            case Animation.Jump: return jump;
            case Animation.Fall: return fall;
            default: return idle;
        }
    }
}

enum Animation
{
    Idle,
    Walk,
    Run,
    Jump,
    Hurt,
    Fall
}

static class Classes
{
    //--------------------------------------------------------------
    // Start defining characters
    //--------------------------------------------------------------
    public static ObjectClass FROG;
    public static ObjectClass DUMMY;
    public static ObjectClass LINK;
    //--------------------------------------------------------------

    public static ObjectClass[] classes;

    public static void setClasses()
    {
        FROG.name = "Frog";
        FROG.sprite = CA.rootf("/characters/Frog/frog.png");
        FROG.spriteOffset = new Point(-8, -5);
        FROG.speed = 2;
        FROG.weight = 80f / 256;
        FROG.ai = AI.generalCharacter;
        FROG.width = 16;
        FROG.height = 28;
        FROG.animSpeed = 3;
        FROG.idle.start = 0;
        FROG.idle.end = 10;
        FROG.walk.start = 12;
        FROG.walk.end = 23;
        FROG.run.start = 0;
        FROG.run.end = 0;
        FROG.jump.start = 24;
        FROG.jump.end = 24;
        FROG.fall.start = 25;
        FROG.fall.end = 25;

        DUMMY.name = "Dummy";
        DUMMY.sprite = CA.rootf("/characters/Frog/frog.png");
        DUMMY.spriteOffset = new Point(-8, -5);
        DUMMY.speed = 1;
        DUMMY.weight = 80f / 256;
        DUMMY.ai = AI.generalCPU;
        DUMMY.width = 16;
        DUMMY.height = 28;
        DUMMY.animSpeed = 3;
        DUMMY.idle.start = 0;
        DUMMY.idle.end = 10;
        DUMMY.walk.start = 12;
        DUMMY.walk.end = 23;

        classes = new[] { LINK, FROG, DUMMY };
    }
}
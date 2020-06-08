using System;
using Microsoft.Xna.Framework;

// Main camera struct
class Camera
{
    public float x, y; // Camera's postion
    public float xscroll, yscroll; // Current velocity
    public ObjectInfo target;
    //int* targetx, * targety, *targetspeed, *targetvy; // 'Target' X and Y, used to adjust the camera
    public bool set; // Function specific variable - not used (?)
    public int type; // Camera mode
    public int limitl, limith; // Scroll limits
    public Point Size { get; set; }

    public Camera(int type = 0)
    {
        this.type = type;
    }

    // Intitialises the type (and sets the position) of the camera
    public void SetPos(int type, int x, int y)
    {
        this.type = type;
        this.x = x;
        this.y = y;
    }

    // Set the camera's target (to follow)
    public void Target(ObjectInfo obj, int limitl, int limith)
    {
        this.target = obj;
        this.limitl = limitl;
        this.limith = limith;
    }

    /*// Move the camera to a different place on the screen, using angles.
    public bool Set(int cx, int cy, int speed)
    {
        // Constants, to eliminate magic numbers
        const int scroll_acc = 5; // Scrolling acceleration speed
        const int scroll_catch = 20; // Catchment area, to protect against overscroll

        int angle;
        bool set1 = false;
        bool set2 = false;
        int x = cx - 128;
        int y = cy - 96;

        // Moving the camera into position
        if (!(x >> 8 < x + (48 * speed) && x >> 8 > x - (48 * speed) && y >> 8 < y + (48 * speed) && y >> 8 > y - (48 * speed)))
        {
            angle = PA.GetAngle(x >> 8, y >> 8, x, y);

            if (xscroll < PA.Cos(angle) * speed) xscroll += scroll_acc;
            else if (xscroll > PA.Cos(angle) * speed) xscroll -= scroll_acc;

            if (yscroll < -PA.Sin(angle) * speed) yscroll += scroll_acc;
            else if (yscroll > -PA.Sin(angle) * speed) yscroll -= scroll_acc;

            set = false;
        }

        // Stopping the camera
        else
        {
            if (xscroll > 0) xscroll -= scroll_acc;
            else if (xscroll < 0) xscroll += scroll_acc;

            if (yscroll > 0) yscroll -= scroll_acc;
            else if (yscroll < 0) yscroll += scroll_acc;

            if (xscroll < scroll_catch && xscroll > -scroll_catch) { xscroll = 0; set1 = true; }
            if (yscroll < scroll_catch && yscroll > -scroll_catch) { yscroll = 0; set2 = true; }
        }

        x += xscroll; y += yscroll;

        if (set1 && set2) return true;
        else return false;
    }*/

    public bool InCanvas(int x, int y)
    {
        if ((x - this.x) > Size.X || (y - this.y) > Size.Y || (x - this.x) < -64 || (y - this.y) < -64) return false;
        return true;
    }

    // Main camera scrolling function
    public void Scroll()
    {
        int screenWidth = Size.X;
        int screenHeight = Size.Y;
        int padding = screenWidth / 20;
        int h_border = screenWidth / 3;
        int v_border = screenHeight / 3;

        // Constants, to eliminate magic numbers
        int top_border = v_border;
        int left_border = h_border;
        int right_border = screenWidth - h_border;
        int bottom_border = screenHeight - v_border;

        // Just to make the code easier to read
        float x = target.cx;
        float y = target.cy;
        float maxspeedx = target.objClass.speed;
        float maxspeedy = target.vy;
        float camerax = this.x;
        float cameray = this.y;
        float vy_speed = 0.03f;
        float vx_speed = 0.03f;

        /*                       *\
            'v*_speed' is the
            acceleration speed of 
            the 
        \*                       */

        // Camera types
        switch (type)
        {
            case 0: vy_speed = 0.125f; break;
            case 1: break;
        }

        // Special conditions
        if (target.objClass.speed == 0) maxspeedx = 4;
        if (maxspeedx < 0) maxspeedx = -maxspeedx;
        if (target.vy == 0) maxspeedy = 4;
        if (maxspeedy < 0) maxspeedy = -maxspeedy;

        // Start main camera code
        float overtakingspeedx = maxspeedx + (maxspeedx / 2);
        float overtakingspeedy = maxspeedy + (maxspeedy / 2);

        float accelerationx = maxspeedx * vx_speed;
        float accelerationy = maxspeedy * vy_speed;

        float canvasx = x - camerax;
        float canvasy = y - cameray;

        if ((canvasx > right_border + padding && xscroll < overtakingspeedx) ||
            (canvasx < left_border - padding && xscroll > -overtakingspeedx))
        {
            if (canvasx > right_border + padding && xscroll < overtakingspeedx) xscroll += accelerationx;
            if (canvasx < left_border - padding && xscroll > -overtakingspeedx) xscroll -= accelerationx;
        }

        else if ((canvasx > right_border && xscroll < maxspeedx) ||
                (canvasx < left_border && xscroll > -(maxspeedx)))
        {
            if (canvasx > right_border && xscroll < maxspeedx) xscroll += accelerationx;
            if (canvasx < left_border && xscroll > -(maxspeedx)) xscroll -= accelerationx;
        }

        else if (xscroll > 0 || xscroll < 0)
        {
            if (xscroll > 0) xscroll -= accelerationx;
            if (xscroll < 0) xscroll += accelerationx;
        }

        if (xscroll > -accelerationx && xscroll < accelerationx) xscroll = 0;

        if ((canvasy > bottom_border + padding && yscroll < overtakingspeedy) ||
            (canvasy < top_border - padding && yscroll > -overtakingspeedy))
        {
            if (canvasy > bottom_border + padding && yscroll < overtakingspeedy) yscroll += accelerationy;
            if (canvasy < top_border - padding && yscroll > -overtakingspeedy) yscroll -= accelerationy;
        }

        else if ((canvasy > bottom_border && yscroll < maxspeedy) ||
                (canvasy < top_border && yscroll > -maxspeedy))
        {
            if (canvasy > bottom_border && yscroll < maxspeedy) yscroll += accelerationy;
            if (canvasy < top_border && yscroll > -maxspeedy) yscroll -= accelerationy;
        }

        else if (yscroll > 0 || yscroll < 0)
        {
            if (yscroll > 0) yscroll -= accelerationy;
            if (yscroll < 0) yscroll += accelerationy;
        }

        if (yscroll > -accelerationy && yscroll < accelerationy) yscroll = 0;

        this.x += xscroll;
        this.y += yscroll;

        // Camera limits
        if (this.x > (limitl - screenWidth))
        {
            this.x = (limitl - screenWidth);
            //xscroll = -(xscroll>>1);
        }

        if (this.x < 0)
        {
            this.x = 0;
            //xscroll = -(xscroll>>1);
        }

        if (this.y > (limith - screenHeight))
        {
            this.y = (limith - screenHeight);
            //yscroll = -(yscroll>>1);
        }

        if (this.y < 0)
        {
            this.y = 0;
            //yscroll = -(yscroll>>1);
        }
    }
}
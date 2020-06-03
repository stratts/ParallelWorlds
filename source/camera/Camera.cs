using System;

// Main camera struct
class Camera
{
    public int x, y; // Camera's postion
    public int xscroll, yscroll; // Current velocity
    public ObjectInfo target;
    //int* targetx, * targety, *targetspeed, *targetvy; // 'Target' X and Y, used to adjust the camera
    public bool set; // Function specific variable - not used (?)
    public int type; // Camera mode
    public int limitl, limith; // Scroll limits

    public Camera(int type = 0)
    {
        this.type = type;
    }

    // Intitialises the type (and sets the position) of the camera
    public void SetPos(int type, int x, int y)
    {
        this.type = type;
        this.x = x << 8;
        this.y = y << 8;
    }

    // Set the camera's target (to follow)
    public void Target(ObjectInfo obj, int limitl, int limith)
    {
        this.target = obj;
        this.limitl = limitl;
        this.limith = limith;
    }

    // Move the camera to a different place on the screen, using angles.
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
    }

    // Main camera scrolling function
    public void Scroll()
    {
        const int screenWidth = 256;
        const int screenHeight = 192;
        const int padding = 10;
        const int h_border = 80;
        const int v_border = 60;

        // Constants, to eliminate magic numbers
        const int top_border = v_border;
        const int left_border = h_border;
        const int right_border = screenWidth - h_border;
        const int bottom_border = screenHeight - v_border;

        // Just to make the code easier to read
        int x = (int)target.cx;
        int y = (int)target.cy;
        int maxspeedx = (int)target.objClass.speed << 8;
        int maxspeedy = (int)(target.vy * 256);
        int camerax = (this.x) >> 8;
        int cameray = (this.y) >> 8;
        int vy_speed = 5; // Fixed point division... the smaller the number, the faster the speed
        int vx_speed = 5; // See above ^^

        /*                       *\
            'v*_speed' is the
            acceleration speed of 
            the 
        \*                       */

        // Camera types
        switch (type)
        {
            case 0: vy_speed = 3; break;
            case 1: break;
        }

        // Special conditions
        if (target.objClass.speed == 0) maxspeedx = 1024;
        if (maxspeedx < 0) maxspeedx = -maxspeedx;
        if (target.vy == 0) maxspeedy = 1024;
        if (maxspeedy < 0) maxspeedy = -maxspeedy;

        // Start main camera code
        int overtakingspeedx = maxspeedx + (maxspeedx >> 2);
        int overtakingspeedy = maxspeedy + (maxspeedy >> 2);

        int accelerationx = maxspeedx >> vx_speed;
        int accelerationy = maxspeedy >> vy_speed;

        int canvasx = x - camerax;
        int canvasy = y - cameray;

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
        if (this.x > (limitl - screenWidth) << 8)
        {
            this.x = (limitl - screenWidth) << 8;
            //xscroll = -(xscroll>>1);
        }

        if (this.x >> 8 < 0)
        {
            this.x = 0;
            //xscroll = -(xscroll>>1);
        }

        if (this.y > (limith - screenHeight) << 8)
        {
            this.y = (limith - screenHeight) << 8;
            //yscroll = -(yscroll>>1);
        }

        if (this.y >> 8 < 0)
        {
            this.y = 0;
            //yscroll = -(yscroll>>1);
        }
    }
}
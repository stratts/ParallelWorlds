
// Main camera struct
struct CameraInfo {
    public int x, y; // Camera's postion
    public int xscroll, yscroll; // Current velocity
    public ObjectInfo target;
    //int* targetx, * targety, *targetspeed, *targetvy; // 'Target' X and Y, used to adjust the camera
    public bool set; // Function specific variable - not used (?)
    public int type; // Camera mode
    public int limitl, limith; // Scroll limits
}

static class Camera {
    public static CameraInfo camera;

    // Intitialises the type (and sets the position) of the camera
    public static void cameraInit(int type, int x, int y) {
        camera.type = type;
        camera.x = x<<8;
        camera.y = y<<8;
    }
  
    // Set the camera's target (to follow)
    public static void cameraTarget(ObjectInfo obj, int limitl, int limith) {
        camera.target = obj;
        camera.limitl = limitl;
        camera.limith = limith;
    }

    // Move the camera to a different place on the screen, using angles.
    public static bool cameraSet(int cx, int cy, int speed)
    {
        // Constants, to eliminate magic numbers
        const int scroll_acc = 5; // Scrolling acceleration speed
        const int scroll_catch = 20; // Catchment area, to protect against overscroll

        int angle;
        bool set1 = false;
        bool set2 = false;
        int x = cx-128;
        int y = cy-96;

        // Moving the camera into position
        if(!(camera.x>>8 < x + (48*speed) && camera.x>>8 > x - (48*speed) && camera.y>>8 < y + (48*speed) && camera.y>>8 > y - (48*speed)))
        {
            angle = PA.GetAngle(camera.x>>8, camera.y>>8, x, y);

            if(camera.xscroll < PA.Cos(angle)*speed) camera.xscroll+=scroll_acc; 
                else if (camera.xscroll > PA.Cos(angle) *speed) camera.xscroll-=scroll_acc; 

            if (camera.yscroll < -PA.Sin(angle)*speed) camera.yscroll+=scroll_acc;
                else if (camera.yscroll > -PA.Sin(angle) *speed) camera.yscroll-=scroll_acc;

            camera.set = false;
        }
        
        // Stopping the camera
        else
        {
            if (camera.xscroll > 0) camera.xscroll -= scroll_acc;
                else if (camera.xscroll < 0) camera.xscroll += scroll_acc;

            if (camera.yscroll > 0) camera.yscroll -= scroll_acc;
                else if (camera.yscroll < 0) camera.yscroll += scroll_acc;

            if (camera.xscroll < scroll_catch && camera.xscroll > -scroll_catch) {camera.xscroll = 0; set1 = true;}
            if (camera.yscroll < scroll_catch && camera.yscroll > -scroll_catch) {camera.yscroll = 0; set2 = true;}
        }

        camera.x += camera.xscroll; camera.y += camera.yscroll;

        if (set1 && set2) return true;
        else return false;
    }

    // Main camera scrolling function
    public static void cameraScroll() 
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
        int x = (camera.target.cx) >> 8;
        int y = (camera.target.cy) >> 8;
        int maxspeedx = (camera.target.objClass.speed);
        int maxspeedy = (camera.target.vy);  
        int camerax = (camera.x) >> 8;
        int cameray = (camera.y) >> 8;
        int vy_speed = 5; // Fixed point division... the smaller the number, the faster the speed
        int vx_speed = 5; // See above ^^

        /*                       *\
            'v*_speed' is the
            acceleration speed of 
            the camera.
        \*                       */
        
        // Camera types
        switch(camera.type)
        {
            case 0: vy_speed = 3; break;
            case 1: break;
        }
        
        // Special conditions
        if(camera.target.objClass.speed == 0) maxspeedx = 1024;
        if (maxspeedx < 0) maxspeedx = -maxspeedx;  
        if(camera.target.vy == 0) maxspeedy = 1024;    
        if(maxspeedy < 0) maxspeedy = -maxspeedy;
        
        // Start main camera code
        int overtakingspeedx = maxspeedx + (maxspeedx >> 2);
        int overtakingspeedy = maxspeedy + (maxspeedy >> 2);

        int accelerationx = maxspeedx >> vx_speed;
        int accelerationy = maxspeedy >> vy_speed;

        int canvasx = x - camerax;
        int canvasy = y - cameray;

        if((canvasx > right_border + padding && camera.xscroll < overtakingspeedx) ||
            (canvasx < left_border - padding && camera.xscroll > -overtakingspeedx))
        {
            if(canvasx > right_border + padding && camera.xscroll < overtakingspeedx) camera.xscroll += accelerationx;
            if(canvasx < left_border - padding && camera.xscroll > -overtakingspeedx) camera.xscroll -= accelerationx;
        }

        else if ((canvasx > right_border && camera.xscroll < maxspeedx)||
                (canvasx < left_border && camera.xscroll > -(maxspeedx)))
        {
            if(canvasx > right_border && camera.xscroll < maxspeedx) camera.xscroll += accelerationx;
            if(canvasx < left_border && camera.xscroll > -(maxspeedx)) camera.xscroll -= accelerationx;
        }

        else if (camera.xscroll > 0 || camera.xscroll < 0)
        {
            if (camera.xscroll > 0) camera.xscroll -= accelerationx;
            if (camera.xscroll < 0) camera.xscroll += accelerationx;
        }

        if (camera.xscroll > -accelerationx && camera.xscroll < accelerationx) camera.xscroll = 0;

        if((canvasy > bottom_border + padding && camera.yscroll < overtakingspeedy) ||
            (canvasy < top_border - padding && camera.yscroll > -overtakingspeedy))
        {
            if(canvasy > bottom_border + padding && camera.yscroll < overtakingspeedy) camera.yscroll += accelerationy;
            if(canvasy < top_border - padding && camera.yscroll > -overtakingspeedy) camera.yscroll -= accelerationy;
        }

        else if ((canvasy > bottom_border && camera.yscroll < maxspeedy)||
                (canvasy < top_border && camera.yscroll > -maxspeedy))
        {
            if(canvasy > bottom_border && camera.yscroll < maxspeedy) camera.yscroll += accelerationy;
            if(canvasy < top_border && camera.yscroll > -maxspeedy) camera.yscroll -= accelerationy;
        }

        else if (camera.yscroll > 0 || camera.yscroll < 0)
        {
            if (camera.yscroll > 0) camera.yscroll -= accelerationy;
            if (camera.yscroll < 0) camera.yscroll += accelerationy;
        }

        if (camera.yscroll > -accelerationy && camera.yscroll < accelerationy) camera.yscroll = 0;

        camera.x += camera.xscroll; 
        camera.y += camera.yscroll;
        
        // Camera limits
        if(camera.x > (camera.limitl - screenWidth)<<8)
        {
            camera.x = (camera.limitl - screenWidth)<<8;
            //camera.xscroll = -(camera.xscroll>>1);
        }

        if (camera.x>>8 < 0)
        {
            camera.x = 0;
            //camera.xscroll = -(camera.xscroll>>1);
        }

        if(camera.y > (camera.limith - screenHeight)<<8)
        {
            camera.y = (camera.limith - screenHeight)<<8;
            //camera.yscroll = -(camera.yscroll>>1);
        }

        if (camera.y>>8 < 0)
        {
            camera.y = 0;
            //camera.yscroll = -(camera.yscroll>>1);
        }
    }
}
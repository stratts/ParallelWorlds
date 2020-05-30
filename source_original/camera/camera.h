#ifndef CAMERAENG_
#define CAMERAENG_

#include <PA9.h>

// Main camera struct
typedef struct
{
    int x, y; // Camera's postion
    int xscroll, yscroll; // Current velocity
    int* targetx, * targety, *targetspeed, *targetvy; // 'Target' X and Y, used to adjust the camera
    bool set; // Function specific variable - not used (?)
    bool type; // Camera mode
    int limitl, limith; // Scroll limits
} cameraInfo;

extern cameraInfo camera;

// Function declaration
void cameraInit(int type, int x, int y);
bool cameraSet(int x, int y, int speed);
void cameraTarget(int* x, int* y, int* speed, int* vy, int limitl, int limith);
void cameraScroll();

#endif

using System;

namespace ParallelWorlds
{
    class Program
    {
        static void Main(string[] args)
        {
            Classes.setClasses();
            Levels.setLevels();
            Rooms.mainMenu();
        }
    }
}

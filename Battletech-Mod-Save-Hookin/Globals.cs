using System;
namespace BattletechModSaveHookin
{
    static class Globals
    {
        // create a perminant instance of SqLightTest instead of re-creating it every time Dbrun() is called
        private static SqLightTest test = new SqLightTest();

        public static void Dbrun()
        {
            test.Start();
        }
    }
}

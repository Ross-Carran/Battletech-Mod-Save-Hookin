using Harmony;
using System;
using System.Reflection;

namespace BattletechModSaveHookin
{
    public class Battletech_Mod_Save_Hookin
    {
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("com.Ross-Carran.BattletechModSaveHookin.Battletech");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            FileLog.Log("hello world");
        }
    }
}

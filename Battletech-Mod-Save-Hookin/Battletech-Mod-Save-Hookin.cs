using Harmony;
using System;
using System.Reflection;
using BattleTech;
using BattleTech.Save;
using BattleTech.Save.SaveGameStructure;

namespace BattletechModSaveHookin
{
    public class Battletech_Mod_Save_Hookin
    {
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("com.github.Ross-Carran.BattletechModSaveHookin");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(GameInstanceSave))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(GameInstance), typeof(SaveReason) })]
        public class Sql_Hookin_Location_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(GameInstanceSave __instance)
            {
                FileLog.Log("A Save has been made");
                Globals.Dbrun();
                FileLog.Log("hello world");
            }
        }
    }
}

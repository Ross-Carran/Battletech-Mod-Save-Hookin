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
            public static void Postfix(GameInstanceSave __instance, GameInstance gameInstance)
            {
                FileLog.Log("A Save has been made");
                Globals.Dbrun();
                FileLog.Log("Save Time: " + __instance.SaveTime.Ticks.ToString());
                FileLog.Log("FileGUID: " + __instance.InstanceGUID);
                FileLog.Log("Commander Name: " + gameInstance.Simulation.Commander.Name);
                FileLog.Log("Company Name: " + gameInstance.Simulation.CompanyName);
            }
        }

        [HarmonyPatch(typeof(GameInstance))]
        [HarmonyPatch("CreateCombatFromSave")]
        public class Sql_Hookin_Combat_Load_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(GameInstanceSave save)
            {
                FileLog.Log("Combat save being loaded");
                FileLog.Log("Save Time: " + save.SaveTime.Ticks.ToString());
                FileLog.Log("FileGUID: " + save.InstanceGUID);
            }
        }
    }
}

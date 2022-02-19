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

        // These are just where I figure the entry points should be, should still have at least one more for loading campaign/career saves
        // obviously there will need to be some code that does checks and what not.
        // the heavy lifting is actually going to be in writing the code that will generate the base save structure in sql
        // if thats crap im going to to have no end of problems moving forward, pretty sure i'm overthinking this.
        [HarmonyPatch(typeof(GameInstanceSave))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(GameInstance), typeof(SaveReason) })]
        public class Sql_Hookin_Location_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(GameInstanceSave __instance, GameInstance gameInstance)
            {
                FileLog.Log("A Save has been made");
                Globals.GetSaveReason(__instance);
                FileLog.Log("begginer dave reason: " + Globals.SaveReason());
                Globals.ParamTest(__instance, gameInstance);
                Globals.TimeChain(__instance);
                Globals.Dbrun();

                // Class needs to be made, well doesnt need to be
                // going to pass __instance and gameInstance to globals in a method
                // then run these logs bit stuffed if i can't do it 
                // have been reading about sql and normalisation from domain classes to relational databases
                // the data base side of things, is kinda new well a re-learn -_- may as well be a new learn 
                // guess the sayings true, if you don't use it you lose it.

                // works, that is a releif 
               
            }
        }

        /*
         * Combat load hookin, test point.       
         * Not Currently needed hopefully, adding in potential hookin points
         * but will be focusing on getting carrer save's/load's working first.
         */       
        [HarmonyPatch(typeof(GameInstance))]
        [HarmonyPatch("CreateCombatFromSave")]
        public class Sql_Hookin_Combat_Load_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(GameInstance __instance, GameInstanceSave save)
            {
                FileLog.Log("Combat save being loaded");
                if (!save.IsSkirmish)
                {
                    FileLog.Log("Commander Name: " + __instance.Simulation.Commander.Name);
                    FileLog.Log("Company Name: " + __instance.Simulation.CompanyName);
                }
                FileLog.Log("Save Time: " + save.SaveTime.Ticks.ToString());
                FileLog.Log("FileGUID: " + save.InstanceGUID);
            }
        }

        /*
         * Generic save game load point       
         */       
        [HarmonyPatch(typeof(GameInstance))]
        [HarmonyPatch("Load")]
        public class Sql_Hookin_Load_Game_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(GameInstanceSave save)
            {
                Globals.ClearGlobalsOnSaveLoad();
                Globals.GetLastTimeChain(save.SaveTime.Ticks.ToString());
                FileLog.Log("Save Game Being Loaded");
                FileLog.Log("Save Time: " + save.SaveTime.Ticks.ToString());
                FileLog.Log("FileGUID: " + save.InstanceGUID);
            }
        }
    }
}

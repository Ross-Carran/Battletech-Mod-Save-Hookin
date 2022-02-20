using System;
using Harmony;
using BattleTech;
using BattleTech.Save;
namespace BattletechModSaveHookin
{
    /*
     * So long as this mod dosn't initialise anything in BattleTech and keep all the work being done inside the mods space
     * Serialisation will not be needed or required, at least that is my understanding anyway, will find out soon enough
     * making it so the mod related data only needs to be save/loaded on a hard save/load.
     * 
     * Unless my understanding of the unity documentation was incorrect serialization is only required for things 
     * that are attached to an object in a scene, as they are updated every tick ?.
     * 
     * So if a mod stores and controlls, any types or values not originaly made for battletech
     * in it's own environment, there should be no reason for it to be serialsed.
     * 
     * Anything that needs to be constantly, adjusted or checked can be refered to from a static global value.
     * The main cavats would be people making mods would have to take that into account while making mods.
     * 
     * So my thinking is, if a mod directly changes Battletech type field values, those can be ignored by this mods save and load
     * as battle tech itself should pick it up not needing to know a change has been made.
     *     
     * E.g. you change the value of heatsinks or something similar, battletech wont need to have some dodgy serialisation code hacked into
     * it because all yourve done is changed the value of a original property in heatsinks which battletech already knows how to properly save/serialise.
     * 
     * The objective is to use this mod to save the data that BattleTech doesn't know how to work with, which is needed to be saved, for the mods to 
     * continue working correctly.
     * 
     * Which should in turn, make this mods saving process significantly easier as it will not be trying re-save the entirity of what battletech normally
     * saves/serializes. So instead of re-creating the wheel, this mod is placing a new wheel that works in tandem with the old wheel.
     *     
     * Just theory on my end, still need to impliment to see if the thought process is correct.    
     */
    static class Globals
    {
        // create a perminant instance of SqLightTest instead of re-creating it every time Dbrun() is called
        // private static SqLightTest test = new SqLightTest();
        private static WorkSpace test = new WorkSpace();
        private static string currentTimeChain;
        private static string lastTimeChain;
        private static string saveReason;
        private static string fileGuid;
        private static bool isIronmanCampaign;
        private static bool isCampaign;
        private static bool isCareer;
        private static bool isStoryMission;

        public static void Dbrun()
        {
            test.Start();
        }

        public static void ParamTest(GameInstanceSave __instance, GameInstance gameInstance)
        {

            FileLog.Log("Save Time: " + __instance.SaveTime.Ticks.ToString());
            FileLog.Log("FileGUID: " + __instance.InstanceGUID);
            FileLog.Log("Commander Name: " + gameInstance.Simulation.Commander.Name);
            FileLog.Log("Company Name: " + gameInstance.Simulation.CompanyName);

            FileLog.Log("Save Reason: " + saveReason);
            FileLog.Log("Ironman Campaign: " + isIronmanCampaign);
            FileLog.Log("Campaign: " + isCampaign);
            FileLog.Log("Career: " + isCareer);
            FileLog.Log("Story Mission: " + isStoryMission);
        }

        public static void TimeChain(GameInstanceSave __instance)
        {
            //saveReason = __instance.SaveReason.ToString();
            isIronmanCampaign = __instance.IsIronmanCampaign;
            isCampaign = __instance.IsCampaign;
            isCareer = __instance.IsCareer;
            isStoryMission = __instance.IsStoryMission;

            currentTimeChain = __instance.SaveTime.Ticks.ToString();

            if(lastTimeChain == null)
            {
                lastTimeChain = currentTimeChain;
            }
        }

        public static void ClearGlobalsOnSaveLoad()
        {
            currentTimeChain = null;
            lastTimeChain = null;
            saveReason = null;
            isIronmanCampaign = false;
            isCampaign = false;
            isCareer = false;
            isStoryMission = false;
        }

        public static string CurrentTimeChain()
        {
            return currentTimeChain;
        }

        public static bool IsIronmanCampaign()
        {
            return isIronmanCampaign;
        }

        public static string LastTimeChain()
        {
            return lastTimeChain;
        }

        public static void GetLastTimeChain(string timechain)
        {
            lastTimeChain = timechain;
        }

        public static string SaveReason()
        {
            return saveReason;
        }

        public static void GetSaveReason(GameInstanceSave __instance)
        {
            saveReason = __instance.SaveReason.ToString();
        }

        public static void SetFileGuid(GameInstanceSave __instance)
        {
            fileGuid = __instance.InstanceGUID;
        }

        public static string FileGuid()
        {
            return fileGuid;
        }
    }
}

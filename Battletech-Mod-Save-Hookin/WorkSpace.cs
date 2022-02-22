using System;
using Harmony;

namespace BattletechModSaveHookin
{
    public class WorkSpace
    {
        public void Start()
        {
            FileGuidTable fileGuids = new FileGuidTable();
            fileGuids.AddData(new FileGuidEntity(Globals.FileGuid()));
            fileGuids.Close();

            FileGuidAndTimeChainTable fileGuidsAndTimeChains = new FileGuidAndTimeChainTable();
            //fileGuidsAndTimeChains.FakeEntry();
            fileGuidsAndTimeChains.AddData(new FileGuidAndTimeChainEntity());
            string hello = fileGuidsAndTimeChains.GetTableName();
            FileLog.Log("Table Name: " + hello);
            //string output = fileGuidsAndTimeChains.IdExists(hello, Globals.FileGuid(), Globals.CurrentTimeChain());

            //FileLog.Log("FGATCT Check: (0 or 1):" + output);

            //fileGuidsAndTimeChains.DeleteDataByString(Globals.CurrentTimeChain());

            fileGuidsAndTimeChains.Close();
        }

        public WorkSpace()
        {
        }
    }
}

using System;
namespace BattletechModSaveHookin
{
    public class WorkSpace
    {
        public void Start()
        {
            FileGuidTable fileGuids = new FileGuidTable();
            fileGuids.AddData(new FileGuidEntity(Globals.FileGuid()));
            fileGuids.Close();
        }

        public WorkSpace()
        {
        }
    }
}

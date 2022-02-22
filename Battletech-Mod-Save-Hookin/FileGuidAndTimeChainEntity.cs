namespace BattletechModSaveHookin
{
    public class FileGuidAndTimeChainEntity
    {
        public string _id;
        public string _timeChain;

        public FileGuidAndTimeChainEntity()
        {
            _id = Globals.FileGuid();
            _timeChain = Globals.CurrentTimeChain();
        }

        /*
         * This function may not be needed, for some reason im thinking it is atm though. 
         */
        public FileGuidAndTimeChainEntity(string lastTimeChain)
        {
            _id = Globals.FileGuid();
            _timeChain = lastTimeChain;
        }
    }
}

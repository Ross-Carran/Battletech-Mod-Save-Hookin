using Harmony;
using System.Data;
using Mono.Data.Sqlite;
namespace BattletechModSaveHookin
{
    public class FileGuidAndTimeChainTable : SqLightBase
    {
        private const string Tag = "Ross FileGuidAndTimeChainTable\t";

        private const string TABLE_NAME = "fileGuidsAndTimeChains";
        private const string KEY_ID_PRIMARY = "id";
        private const string KEY_ID_SECONDARY = "timeChain";

        public FileGuidAndTimeChainTable() : base()
        {
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (" + KEY_ID_PRIMARY + " TEXT NOT NULL, " + KEY_ID_SECONDARY + 
                " TEXT NOT NULL, PRIMARY KEY ("+KEY_ID_PRIMARY+","+KEY_ID_SECONDARY+")," +
                " FOREIGN KEY ("+KEY_ID_PRIMARY+ ") REFERENCES fileGuids ("+KEY_ID_PRIMARY+")" +
                " ON DELETE CASCADE ON UPDATE NO ACTION)";
            dbcmd.ExecuteNonQuery();
        }

        public void FakeEntry()
        {
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "INSERT INTO " + TABLE_NAME + " ("+KEY_ID_PRIMARY+","+KEY_ID_SECONDARY+")" +
                " VALUES (@fileGuid, @currentTimeChain)";
            SqliteParameter[] parameters = new SqliteParameter[2];
            parameters[0] = new SqliteParameter("@fileGuid", DbType.String)
            {
                Value = Globals.FileGuid()
            };
            parameters[1] = new SqliteParameter("@currentTimeChain", DbType.String)
            {
                Value = Globals.CurrentTimeChain()
            };
            foreach(SqliteParameter param in parameters)
            {
                dbcmd.Parameters.Add(param);
            }
            dbcmd.ExecuteNonQuery();
        }
    }
}

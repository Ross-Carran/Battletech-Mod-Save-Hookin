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

        public void AddData(FileGuidAndTimeChainEntity entity)
        {
            if(Globals.SaveReason() != "SIM_GAME_FIRST_SAVE" && Globals.IsIronmanCampaign())
            {
                this.DeleteDataByString(Globals.LastTimeChain());
            }
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "INSERT INTO "+TABLE_NAME+" ("+KEY_ID_PRIMARY+","+KEY_ID_SECONDARY+") VALUES (@id1,@id2)";
            SqliteParameter[] parameters = new SqliteParameter[2];
            parameters[0] = new SqliteParameter("@id1", DbType.String)
            {
                Value = entity._id
            };
            parameters[1] = new SqliteParameter("@id2", DbType.String)
            {
                Value = entity._timeChain
            };
            foreach(SqliteParameter param in parameters)
            {
                dbcmd.Parameters.Add(param);
            }
            dbcmd.ExecuteNonQuery();
        }

        public override void DeleteDataByString(string id)
        {
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID_SECONDARY + "=@id";
            SqliteParameter parameter = new SqliteParameter("@id", DbType.String)
            {
                Value = id
            };
            dbcmd.Parameters.Add(parameter);
            dbcmd.ExecuteNonQuery();
            FileLog.Log("filechain deleted!");
        }

        public override string GetTableName()
        {
            return TABLE_NAME;
        }

        public string[] GetCOLUMNS()
        {
            string[] columns = new string[] { KEY_ID_PRIMARY , KEY_ID_SECONDARY };
            return columns;
        }

        /*
         * To be Used when removing a timechain/save game from the database.
         */
        public string IdExists(string table_name, string id, string id2)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "SELECT EXISTS(SELECT 1 FROM " + table_name + " WHERE " + KEY_ID_PRIMARY + "=@id AND " + KEY_ID_SECONDARY + "=@id2)";

            SqliteParameter[] parameters = new SqliteParameter[2];
            parameters[0] = new SqliteParameter("@id", DbType.String)
            {
                Value = id
            };
            parameters[1] = new SqliteParameter("@id2", DbType.String)
            {
                Value = id2
            };
            foreach (SqliteParameter param in parameters)
            {
                dbcmd.Parameters.Add(param);
            }
            IDataReader reader = dbcmd.ExecuteReader();
            return reader[0].ToString();
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

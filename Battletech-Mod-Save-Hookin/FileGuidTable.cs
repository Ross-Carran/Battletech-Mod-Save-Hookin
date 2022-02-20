using Harmony;
using System.Data;
using Mono.Data.Sqlite;
namespace BattletechModSaveHookin
{
    public class FileGuidTable : SqLightBase
    {
        private const string Tag = "Ross FileGuidTable\t";

        private const string TABLE_NAME = "fileGuids";
        private const string KEY_ID = "id";

        public FileGuidTable() : base()
        {

            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " +TABLE_NAME+ " (" +KEY_ID+ " TEXT PRIMARY KEY)";
            dbcmd.ExecuteNonQuery();
                     
        }

        public void AddData(FileGuidEntity fileGuid)
        {

            if (Globals.SaveReason() == "SIM_GAME_FIRST_SAVE")
            {
                IDbCommand dbcmd = GetDbCommand();
                dbcmd.CommandText = "INSERT INTO " + TABLE_NAME + " (" + KEY_ID + ") VALUES (@fileGuid)";
                SqliteParameter param = new SqliteParameter("@fileGuid", DbType.String)
                {
                    Value = fileGuid._id
                };
                dbcmd.Parameters.Add(param);
                dbcmd.ExecuteNonQuery();
            }
        }

        public override IDataReader GetDataById(int id)
        {
            return base.GetDataById(id);
        }

        public override IDataReader GetDataByString(string str)
        {
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "SELECT FROM " +TABLE_NAME+ " WHERE " +KEY_ID+ "=@str";
            SqliteParameter param = new SqliteParameter("@str", DbType.String)
            {
                Value = str
            };
            dbcmd.Parameters.Add(param);
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public override void DeleteAllData()
        {
            base.DeleteAllData();
        }

        public override void DeleteDataById(int id)
        {
            base.DeleteDataById(id);
        }

        public override void DeleteDataByString(string id)
        {
            FileLog.Log("made it to the delete method");
            IDbCommand dbcmd = GetDbCommand();
            dbcmd.CommandText = "DELETE FROM " +TABLE_NAME+ " WHERE " +KEY_ID+ "=@id";
            SqliteParameter param = new SqliteParameter("@id", DbType.String)
            {
                Value = id
            };
            dbcmd.Parameters.Add(param);
            FileLog.Log("added the paramater to the Delete Command Text");
            dbcmd.ExecuteNonQuery();
            FileLog.Log("exited the delete Method");
        }

        public override IDataReader GetAllData()
        {
            return base.GetAllData();
        }

        public override IDataReader GetNumOfRows()
        {
            return base.GetNumOfRows();
        }
    }
}

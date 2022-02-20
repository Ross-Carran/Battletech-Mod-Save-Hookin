using Harmony;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace BattletechModSaveHookin
{
    public class SqLightBase
    {
        private const string Tag = "Ross: SqLightBase:\t";

        private const string database_name = "Saves";

        public string db_connection_string;
        public IDbConnection db_connection;

        public SqLightBase()
        {
            string mod_Save_Path = Directory.GetCurrentDirectory() + "/Mod_Saves";
            if (!Directory.Exists(mod_Save_Path))
            {
                Directory.CreateDirectory(mod_Save_Path);
            }
            db_connection_string = "URI=file:" + mod_Save_Path + "/" + database_name;
            FileLog.Log("db_connection_string: " + db_connection_string);
            db_connection = new SqliteConnection(db_connection_string);
            db_connection.Open();
        }

        ~SqLightBase()
        {
            db_connection.Close();
        }

        //virtual functions
        public virtual IDataReader GetDataById(int id)
        {
            FileLog.Log(Tag + " This function is not implimented");
            throw null;
        }

        public virtual IDataReader GetDataByString(string str)
        {
            FileLog.Log(Tag + " This function is not implimented");
            throw null;
        }

        public virtual void DeleteDataById(int id)
        {
            FileLog.Log(Tag + "This function is not implimented");
            throw null;
        }

        public virtual void DeleteDataByString(string id)
        {
            FileLog.Log(Tag + "This function is not implimented");
            throw null;
        }

        public virtual IDataReader GetAllData()
        {
            FileLog.Log(Tag + "This function is not implimented");
            throw null;
        }

        public virtual void DeleteAllData()
        {
            FileLog.Log(Tag + "This function is not implimented");
            throw null;
        }

        public virtual IDataReader GetNumOfRows()
        {
            FileLog.Log(Tag + "This function is not implimented");
            throw null;
        }

        //Helper Function
        public IDbCommand GetDbCommand()
        {
            return db_connection.CreateCommand();
        }

        public IDataReader GetAllData(string table_name)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "SELECT * FROM " +table_name;
            IDataReader reader = dbcmd.ExecuteReader();
            return reader; 
        }

        public void DeleteAllData(string table_name)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "DROP TABLE IF EXISTS " +table_name;
            dbcmd.ExecuteNonQuery();
        }

        public IDataReader GetNumOfRows(string table_name)
        {
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "SELECT COALESCE(MAX(1d)+1, 0) FROM "+table_name;

            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public string IdExists(string table_name, string id)
        {
            FileLog.Log("made it into Idexists");
            IDbCommand dbcmd = db_connection.CreateCommand();
            dbcmd.CommandText = "SELECT EXISTS(SELECT 1 FROM "+ table_name+ " WHERE id=@id)";

            SqliteParameter param = new SqliteParameter("@id", DbType.String)
            {
                Value = id
            };
            dbcmd.Parameters.Add(param);
            FileLog.Log("Added Paramater to IdExists");
            IDataReader reader = dbcmd.ExecuteReader();
            FileLog.Log("finished executing the Reader - ready to exit");
            return reader[0].ToString();
        }

        public void Close()
        {
            db_connection.Close();
        }
    }
}

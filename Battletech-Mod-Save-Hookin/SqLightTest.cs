/**
 * This is a direct copy and past of code made by Rizwan Asif
 * https://medium.com/@rizasif92/sqlite-and-unity-how-to-do-it-right-31991712190
 * 
 * Going to use this class just to do basic tests
 * The DataBase construction for Battletech will be complex and I don't see the point in re-creating the wheel for a test.
 * 
 * */
using Harmony;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

namespace BattletechModSaveHookin
{
    public class SqLightTest
    {

        public void Start()
        {
            //--------------------------------------------------------------------------------------------------
            /* 
             * Gets the directory path to where the Battletech executable is adds a custom subdirectory
             * checks if the path exists, if it doesn't creats the directory.
             * Directory is place for storing the database.
             */
            // Create database
            FileLog.Log("Current Directory is: " + Directory.GetCurrentDirectory());
            string mod_Save_Path = Directory.GetCurrentDirectory() + "/Mod_Saves";
            if(!Directory.Exists(mod_Save_Path))
            {
                Directory.CreateDirectory(mod_Save_Path);
            }

            string connection = "URI=file:" + mod_Save_Path + "/Saves";
            // string connection = "URI=file:" + "/home/ross" + "/" + "My_Database";
            
            // Open connection
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            // Create table
            IDbCommand dbcmd;
            dbcmd = dbcon.CreateCommand();
            string q_createTable = "CREATE TABLE IF NOT EXISTS save_table (id STRING PRIMARY KEY)";

            dbcmd.CommandText = q_createTable;
            dbcmd.ExecuteReader();
            //---------------------------------------------------------------------------------------------------

            // Insert values in table
            IDbCommand cmnd = dbcon.CreateCommand();

            string timeChain = Globals.CurrentTimeChain();

            cmnd.CommandText = "SELECT NOT EXISTS(SELECT 1 FROM save_table WHERE id=@timeChain)";
            SqliteParameter myParam = new SqliteParameter("@timeChain", DbType.String)
            {
                Value = timeChain
            };
            cmnd.Parameters.Add(myParam);

            // Read and print all values in table
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;

            reader = cmnd.ExecuteReader();
            FileLog.Log("value " + reader.GetValue(0));
             

            if (reader.GetValue(0).ToString() == "1")
            {
                reader.Dispose();
                if (Globals.IsIronmanCampaign() && (Globals.SaveReason() != "SIM_GAME_FIRST_SAVE"))
                {
                    FileLog.Log("Last Time Chain: " + Globals.LastTimeChain()); 
                    cmnd.CommandText = "DELETE FROM save_table WHERE id=@lastTimeChain";
                    SqliteParameter lastTimeChainParam = new SqliteParameter("@lastTimeChain", DbType.String)
                    {
                        Value = Globals.LastTimeChain()
                    };
                    cmnd.Parameters.Add(lastTimeChainParam);
                    cmnd.ExecuteNonQuery();
                    reader.Dispose();
                    Globals.GetLastTimeChain(timeChain);
                    FileLog.Log("Made it to the Ironman Loop: " + Globals.SaveReason());

                }

                cmnd.CommandText = "INSERT INTO save_table (id) VALUES (@timeChain)";
                SqliteParameter timeChainParam = new SqliteParameter("@timeChain", DbType.String)
                {
                    Value = timeChain
                };
                cmnd.Parameters.Add(timeChainParam);
                cmnd.ExecuteNonQuery();

            }

            reader.Dispose();

            string query = "SELECT * FROM save_table";
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();

            while (reader.Read())
            {
                 FileLog.Log("id: " + reader[0].ToString());
                 //FileLog.Log("val: " + reader[1].ToString());
            }

            // Close connection
            dbcon.Close();

        }
    }
}

﻿/**
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

            string connection = "URI=file:" + mod_Save_Path + "/My_Database";
            // string connection = "URI=file:" + "/home/ross" + "/" + "My_Database";
            
            // Open connection
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            // Create table
            IDbCommand dbcmd;
            dbcmd = dbcon.CreateCommand();
            string q_createTable = "CREATE TABLE IF NOT EXISTS my_table (id INTEGER PRIMARY KEY, val INTEGER )";

            dbcmd.CommandText = q_createTable;
            dbcmd.ExecuteReader();
            //---------------------------------------------------------------------------------------------------

            // Insert values in table
            IDbCommand cmnd = dbcon.CreateCommand();

            int myId = 0;
            int myValue = 20;

            cmnd.CommandText = "SELECT NOT EXISTS(SELECT 1 FROM my_table WHERE id=" + myId + " AND val=" + myValue + ")";
            
            //cmnd.CommandText = "INSERT OR REPLACE INTO my_table (id, val) VALUES (0, 5)";
            //cmnd.ExecuteNonQuery();

            // Read and print all values in table
            IDbCommand cmnd_read = dbcon.CreateCommand();
            IDataReader reader;

            reader = cmnd.ExecuteReader();
            FileLog.Log("value " + reader.GetValue(0));

            // Will clean all of this up, winging it instead of properly planning anything.

            // The current objective is to be able to create a dictionary populate it, save the data appropriatly 
            // to the data base, then be able to retrieve the data on load.
            // After that process is successfull and stable(ish) will clean the process up in earnest.

            // The purpose of this, is to be able to store variables and types specific to a MOD and store them
            // and related data in a seperate database, from the main game, anything the game handles normally on it's own 
            // WILL NOT BE STORED IN HERE.

            // e.g. for some reason you want to add Attack Rating (AR) and Defence Rating (DR)
            // to your planets. the base games serialization has no idea what the hell they are and will screw up storing them.
            // so you make 2 dictionaries in this case, planetAR and planetDR.

            // These two Dictionaries will only ever exist in your mod space and the values in them will never properly work with the games base seriliazation.
            // so you save them into this external database.

            // The primary key for both dictionaries will be a planet name, and the value for each will either be AR or DR.
            // you could apply this method to any new data types/variables you add to the game through mods.

            // after messing around, with the games built in saving functionality, the path im looking at taking in regards to sqlight table construction is.
            // create a empty table of save game savetime/date sinse they are the easiest way to identify and make that time date number chain the primary key,
            // for a save. any following tables made for that save will have that savegame number chain as a key or part of a combined key or table name, currently unsure.
            // savegame table --> name of dicionary as table name --> this is a horrible mess and ill just try and do it -_-

            if (reader.GetValue(0).ToString() == "1")
            {
                reader.Dispose();
                cmnd.CommandText = "SELECT EXISTS(SELECT 1 FROM my_table WHERE id=" + myId + ")";
                reader = cmnd.ExecuteReader();

                if (reader.GetValue(0).ToString() == "1")
                {
                    reader.Dispose();
                    cmnd.CommandText = "UPDATE my_table SET val=" + myValue + " WHERE id=" + myId;
                    cmnd.ExecuteNonQuery();
                }
                else
                {
                    reader.Dispose();
                    cmnd.CommandText = "INSERT OR REPLACE INTO my_table (id, val) VALUES (" + myId + ", " + myValue + ")";
                    cmnd.ExecuteNonQuery();
                }

            }
            reader.Dispose();

            string query = "SELECT * FROM my_table";
            cmnd_read.CommandText = query;
            reader = cmnd_read.ExecuteReader();

            while (reader.Read())
            {
                 FileLog.Log("id: " + reader[0].ToString());
                 FileLog.Log("val: " + reader[1].ToString());
            }

            // Close connection
            dbcon.Close();

        }
    }
}

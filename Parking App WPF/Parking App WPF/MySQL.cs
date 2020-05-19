using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Data;

namespace Parking_App_WPF
{
    class MySQL
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public MySQL()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "rylund.dev";
            database = "u748359586_cskarp";
            uid = "u748359586_cskarp";
            password = "cskarp";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + "CharSet=utf8; pooling=false;";

            connection = new MySqlConnection(connectionString);
        }


        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Debug.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Debug.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Execute(string query)
        {
            if (this.OpenConnection() == true)
            {
                Debug.WriteLine(query);
                MySqlCommand cmd = new MySqlCommand(query, connection);

                int rows = cmd.ExecuteNonQuery();
                this.CloseConnection();
                return rows != 0;
            }
            return false;
        }


        //Select statement
        public DataTable Select(string query)
        {
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dataReader);

                this.CloseConnection();
                return dt;
            }
            return null;
        }
    }
}

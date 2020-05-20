using Parking_App_WPF;
using System;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace global_funcs
{
    public static class global_functions
    {
        private static readonly MySQL mysql = new MySQL();

        // Function to encrypt a string with SHA256 encryption
        public static string Encrypt(string s)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(s);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                string encrypted = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return encrypted;
            }
        }


        // Function to check if a license plate is of valid format (2 letters followed by 5 numbers)
        // Return: Formatted Licenseplate of format (AA XX XXX)
        public static (bool, string) CheckPlate(string licensePlate)
        {
            string msg;
            string licensePattern = "[A-Za-z][A-Za-z]\\s?[0-9][0-9]\\s?[0-9][0-9][0-9]";

            // Check if the licenseplate matches the regex defined above, checks if the license plate is 2 letters with 5 numbers.
            if (!Regex.Match(licensePlate, licensePattern).Success)
            {
                msg = "The license plate does not match the correct format";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            // Format the licenseplate to (AA XX XXX) format
            licensePlate = licensePlate.Replace(" ", string.Empty);
            licensePlate = licensePlate.Insert(2, " ");
            licensePlate = licensePlate.Insert(5, " ");
            licensePlate = licensePlate.ToUpper();

            return (true, licensePlate);
        }


        // Check if the given credentials match up with a user and return the user object if it does.
        public static User AuthenticateUser(string username, string password)
        {
            // Encrypt the password using SHA256.
            string shaPassword = Encrypt(password);

            // Select data from the database and check if we have exactly 1 result
            // will either have 1 or 0 results due to LIMIT 1 in query.
            string query = string.Format("SELECT ID, Username, Name, Room, LicensePlate, Rank FROM users WHERE Username='{0}' AND password='{1}' LIMIT 1", username, shaPassword);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count != 1)
            {
                return null;
            }

            // Collect the first row from the result and set the values
            // We validate all values to make sure the program will not crash.
            DataRow dr = dt.Rows[0];
            int userID = dr["ID"] == DBNull.Value ? 0 : (int)dr["ID"];
            string name = dr["Name"] == DBNull.Value ? string.Empty : (string)dr["Name"];
            string roomNumber = dr["Room"] == DBNull.Value ? string.Empty : (string)dr["Room"];
            string licensePlate = dr["LicensePlate"] == DBNull.Value ? string.Empty : (string)dr["LicensePlate"];
            string rank = dr["Rank"] == DBNull.Value ? string.Empty : (string)dr["Rank"];

            // Create a user object with the collected information and return the object.
            User user = new User(userID, username, name, roomNumber, licensePlate, rank);
            return user;
        }


        // A function to get a user out from a room number.
        public static User GetUserByRoom(string roomNumber)
        {
            // Select data from the database and check if we have exactly 1 result
            // will either have 1 or 0 results due to LIMIT 1 in query.
            string query = string.Format("SELECT ID, Username, Name, Room, LicensePlate, Rank FROM users WHERE Room='{0}' LIMIT 1", roomNumber);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count != 1)
            {
                Debug.WriteLine("No user found with provided room number");
                return null;
            }

            // Collect the first row from the result and set the values
            // We validate all values to make sure the program will not crash.
            DataRow dr = dt.Rows[0];
            int userID = dr["ID"] == DBNull.Value ? 0 : (int)dr["ID"];
            string username = dr["Username"] == DBNull.Value ? string.Empty : (string)dr["Username"];
            string name = dr["Name"] == DBNull.Value ? string.Empty : (string)dr["Name"];
            string licensePlate = dr["LicensePlate"] == DBNull.Value ? string.Empty : (string)dr["LicensePlate"];
            string rank = dr["Rank"] == DBNull.Value ? string.Empty : (string)dr["Rank"];

            // Create a user object with the collected information and return the object.
            User user = new User(userID, username, name, roomNumber, licensePlate, rank);
            return user;
        }

        // Function to create a new resident from the entered information in the text boxes.
        public static (bool, string) CreateResident(string name, string username, string password, string roomNumber, string licensePlate)
        {
            // Check if the licensePlate field had any text, if it had any text it 
            // will validate if the license plate is of valid format (2 letters 5 numbers).
            if (licensePlate != string.Empty)
            {
                var info = CheckPlate(licensePlate);
                if (!info.Item1)
                {
                    return info;
                }
                licensePlate = info.Item2;
            }

            // Insert the user into the database
            string query = string.Format("INSERT INTO users (Name, Room, username, password, rank, LicensePlate) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", name, roomNumber, username, Encrypt(password), "Resident", licensePlate);
            bool success = mysql.Execute(query);

            // Return wether it was a success or not and a corresponding message.
            return (success, success ? "Successfully created the user!" : "Something went wrong when creating the user!");

        }


        public static (bool, string) RemoveResident(string roomNumber)
        {
            // Remove the user from the database.
            string query = string.Format("DELETE from users WHERE Room={0} AND Rank='Resident'", roomNumber);
            bool success = mysql.Execute(query);

            // Return wether it was a success or not and a corresponding message.
            return (success, success ? "Successfully removed the user!" : "Something went wrong when removing the user!");
        }
    }
}

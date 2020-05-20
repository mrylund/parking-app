using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using static global_funcs.global_functions;

namespace Parking_App_WPF
{
    public class User
    {
        // Initialize all global class variables and their getters/setters.
        private readonly MySQL mysql;
        private int UserID { get; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string RoomNumber { get; set; }
        public string LicensePlate { get; set; }
        public string Rank { get; set; }
        public List<(string, DateTime)> guests { get; set; } = new List<(string, DateTime)>();


        // Constructor
        public User(int userID, string username, string name, string roomNumber, string licensePlate, string rank)
        {
            // Set all the values to the class variables from the given constructor variables.
            mysql = new MySQL();
            UserID = userID;
            Username = username;
            Name = name;
            RoomNumber = roomNumber;
            LicensePlate = licensePlate;
            Rank = rank;

            // Fetch all active guest vehicles by the user.
            FetchGuests();
        }


        // Fetch all the guests of the user and insert them into the guests list.
        private void FetchGuests()
        {
            // Clear the list of guests before fetching them.
            guests.Clear();

            // Select data from the database and check if we have more than 0 rows
            // if we have less than 1 rows we will exit the function.
            string query = string.Format("SELECT LicensePlate, created FROM guests WHERE Resident={0} AND created > TIMESTAMPADD(DAY, -1, NOW()) ORDER BY created DESC", UserID);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count < 1)
            {
                Debug.WriteLine("No guests found for this user!");
                return;
            }

            // Loop through all the rows in the fetched data
            // for each row a guest licensePlate and creation time is inserted into the guests list.
            foreach(DataRow row in dt.Rows)
            {
                string license = row["LicensePlate"] == DBNull.Value ? string.Empty : (string)row["LicensePlate"];
                DateTime created = row["created"] == DBNull.Value ? DateTime.Parse("0") : (DateTime)row["created"];
                guests.Add((license, created));
            }
        }


        // Add a guest to the user
        // will add the guest to the database and the object itself.
        public (bool, string) AddGuest(string licensePlate)
        {
            string msg;
            bool success;

            // Check if the licenseplate is of valid type
            // if it is valid replace licensePlate variable with formatted licensePlate.
            (bool, string) tempTuple = CheckPlate(licensePlate);
            if (!tempTuple.Item1) return tempTuple;
            licensePlate = tempTuple.Item2;


            // Query to select all guest vehicles with the same licenseplate and where the start time is older than 1 day.
            string query = string.Format("SELECT ID, LicensePlate, created, Resident FROM guests WHERE LicensePlate='{0}' AND created > TIMESTAMPADD(DAY, -1, NOW())", licensePlate);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count > 0)
            {
                // If the vehicle already have an active pass, set the row to first entry from the result.
                DataRow dr = dt.Rows[0];
                int Resident = dr["Resident"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Resident"]);

                // Check if the guest vehicle pass is older than 23 hours and if the user is the one who added it.
                if ((DateTime.Now - (DateTime)dr["created"]).TotalHours > 23 && Resident == UserID)
                {
                    // True: Update the guestpass creation time to current time.
                    query = string.Format("UPDATE guests SET created=current_timestamp() WHERE ID='{0}' AND Resident='{1}'", Convert.ToInt32(dr["ID"]), UserID);
                    success = mysql.Execute(query);
                    msg = string.Format("Updated expiration date of guest pass for license plate {0}", licensePlate);
                    return (success, msg);
                }
                else
                {
                    // False: Do nothing
                    msg = "An active guest pass with given license already exist";
                    Debug.WriteLine(msg);
                    return (false, msg);
                }
            }

            // Select the total count of active guest vehicles the user currently have.
            query = string.Format("SELECT COUNT(*) AS NumCars FROM guests WHERE Resident='{0}' AND created > TIMESTAMPADD(DAY, -1, NOW())", UserID);
            dt = mysql.Select(query);
            int NumCars = dt.Rows[0]["NumCars"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["NumCars"]);

            // If the user got 5 cars or more we will not allow the user to add more guest vehicles.
            if (NumCars >= 5)
            {
                msg = "You have more than the max allowed of guest vehicles";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            // Add the guest vehicle to the database
            query = string.Format("INSERT INTO guests(LicensePlate, Resident) VALUES('{0}', '{1}')", licensePlate, UserID);
            success = mysql.Execute(query);

            // Re-fetch guest vehicles
            FetchGuests();
            return (success, "");
        }

        // Remove a guest from the user
        // Will expire the guest pass and reload the guests list
        public (bool, string) RemoveGuest(string licensePlate)
        {
            // Check if the licenseplate is of valid type
            // if it is valid replace licensePlate variable with formatted licensePlate.
            (bool, string) tempTuple = CheckPlate(licensePlate);
            if (!tempTuple.Item1) return tempTuple;
            licensePlate = tempTuple.Item2;

            // Expire the guest pass in the database
            // TODO: Make it remove data after x time, probably a database fix
            string query = string.Format("UPDATE guests SET created = TIMESTAMPADD(DAY, -2, NOW()) WHERE LicensePlate = '{0}' AND Resident = '{1}' AND created > TIMESTAMPADD(DAY, -1, NOW())", licensePlate, UserID);
            bool success = mysql.Execute(query);

            // Re-fetch guest vehicles
            FetchGuests();

            return (success, "!");
        }

    }
}

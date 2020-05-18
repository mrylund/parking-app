using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Parking_App_WPF
{
    class User
    {
        private MySQL mysql;
        private int UID;
        public string Username { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }
        public string LicensePlate { get; set; }
        public string Rank { get; set; }
        List<(string, DateTime)> guests { get; set; } = new List<(string, DateTime)>();

        public User(int UID, string Username, string Name, string Room, string LicensePlate, string Rank)
        {
            mysql = new MySQL();
            this.UID = UID;
            this.Username = Username;
            this.Name = Name;
            this.Room = Room;
            this.LicensePlate = LicensePlate;
            this.Rank = Rank;

            fetchGuests();
        }

        private void fetchGuests()
        {
            string query = String.Format("SELECT LicensePlate, created FROM guests WHERE Resident={0} AND created > TIMESTAMPADD(DAY, -1, NOW())", UID);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count < 1)
            {
                Debug.WriteLine("No guests found for this user");
                return;
            }
            foreach(DataRow row in dt.Rows)
            {
                string license = row["LicensePlate"] == DBNull.Value ? string.Empty : (String)row["LicensePlate"];
                DateTime created = row["created"] == DBNull.Value ? DateTime.Parse("0") : (DateTime)row["created"];
                guests.Add((license, created));

                Debug.WriteLine(String.Format("License Plate: {0}    Created: {1}", license, created));
            }
        }

        private (bool, string) checkPlate(string LicensePlate)
        {
            string msg;
            string LicensePattern = "[A-Za-z][A-Za-z]\\s?[0-9][0-9]\\s?[0-9][0-9][0-9]";
            if (!Regex.Match(LicensePlate, LicensePattern).Success)
            {
                msg = "The license plate does not match the correct format";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            LicensePlate = LicensePlate.Replace(" ", String.Empty);
            LicensePlate = LicensePlate.Insert(2, " ");
            LicensePlate = LicensePlate.Insert(5, " ");
            LicensePlate = LicensePlate.ToUpper();

            return (true, LicensePlate);
        }

        public (bool, string) addGuest(string LicensePlate)
        {
            string msg;

            (bool, string) temp = checkPlate(LicensePlate);
            if (!temp.Item1) return temp;
            LicensePlate = temp.Item2;

            string query = String.Format("SELECT ID, LicensePlate, created, Resident FROM guests WHERE LicensePlate='{0}' AND created > TIMESTAMPADD(DAY, -1, NOW())", LicensePlate);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int Resident = dr["Resident"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Resident"]);
                if ((DateTime.Now - (DateTime)dr["created"]).TotalHours > 23 && Resident == UID)
                {
                    query = String.Format("UPDATE guests SET created=current_timestamp() WHERE ID='{0}' AND Resident='{1}'", Convert.ToInt32(dr["ID"]), UID);
                    mysql.Execute(query);
                    msg = String.Format("Updated expiration date of guest pass for license plate {0}", LicensePlate);
                    return (true, msg);
                }
                msg = "An active guest pass with given license already exist";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            query = String.Format("SELECT COUNT(*) AS NumCars FROM guests WHERE Resident='{0}' AND created > TIMESTAMPADD(DAY, -1, NOW())", UID);
            dt = mysql.Select(query);
            int NumCars = dt.Rows[0]["NumCars"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["NumCars"]);
            if (NumCars >= 5)
            {
                msg = "You have more than the max allowed of guest vehicles";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            query = string.Format("INSERT INTO guests(LicensePlate, Resident) VALUES('{0}', '{1}')", LicensePlate, UID);
            mysql.Execute(query);
            Debug.WriteLine(LicensePlate);
            return (false, "hej");
        }

        public (bool, string) removeGuest(string LicensePlate)
        {
            (bool, string) temp = checkPlate(LicensePlate);
            if (!temp.Item1) return temp;
            LicensePlate = temp.Item2;

            // Kinda lazy way of doing it, but it works
            string query = String.Format("UPDATE guests SET created = TIMESTAMPADD(DAY, -2, NOW()) WHERE LicensePlate = {0}' AND Resident = {1} AND created > TIMESTAMPADD(DAY, -1, NOW())", LicensePlate, UID);
            mysql.Execute(query);

            return (true, "");
        }

    }
}

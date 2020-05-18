﻿using System;
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
            if (dt.Rows.Count > 1)
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

        public (bool, string) addGuest(string LicensePlate)
        {
            string LicensePattern = "[A-Za-z][A-Za-z]\\s?[0-9][0-9]\\s?[0-9][0-9][0-9]";
            if (!Regex.Match(LicensePlate, LicensePattern).Success)
            {
                string msg = "The license plate does not match the correct format";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            LicensePlate = LicensePlate.Replace(" ", String.Empty);
            LicensePlate = LicensePlate.Insert(2, " ");
            LicensePlate = LicensePlate.Insert(5, " ");
            LicensePlate = LicensePlate.ToUpper();

            string query = String.Format("SELECT LicensePlate, created FROM guests WHERE LicensePlate='{0}' AND created > TIMESTAMPADD(DAY, -1, NOW())", LicensePlate);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count > 0)
            {
                string msg = "An active guest pass with given license already exist";
                Debug.WriteLine(msg);
                return (false, msg);
            }

            Debug.WriteLine(LicensePlate);
            return (false, "hej");

        }

    }
}

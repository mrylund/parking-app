using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Parking_App_WPF
{
    /// <summary>
    /// Interaction logic for ManageUsersWindow.xaml
    /// </summary>
    public partial class ManageUsersWindow : Window
    {
        private MySQL mysql;
        public ManageUsersWindow()
        {
            InitializeComponent();
            mysql = new MySQL();
        }

        public void goBackHome(object sender, RoutedEventArgs e)
        {
            LandingWindowManager lwm = new LandingWindowManager();
            lwm.Show();
            this.Close();

        }

        private User GetUserByRoom(string roomNumber)
        {
            string query = String.Format("SELECT ID, Username, Name, Room, LicensePlate, Rank FROM users WHERE Room='{0}' LIMIT 1", roomNumber);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count != 1)
            {
                Debug.WriteLine("No user found with provided room number");
                return null;
            }
            DataRow dr = dt.Rows[0];
            int UID = dr["ID"] == DBNull.Value ? 0 : (int)dr["ID"];
            string Username = dr["Username"] == DBNull.Value ? string.Empty : (String)dr["Username"];
            string Name = dr["Name"] == DBNull.Value ? string.Empty : (String)dr["Name"];
            string Room = dr["Room"] == DBNull.Value ? string.Empty : (String)dr["Room"];
            string LicensePlate = dr["LicensePlate"] == DBNull.Value ? string.Empty : (String)dr["LicensePlate"];
            string Rank = dr["Rank"] == DBNull.Value ? string.Empty : (String)dr["Rank"];
            User user = new User(UID, Username, Name, Room, LicensePlate, Rank);
            return user;
        }

        private string Encrypt(string s)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(s);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                string encrypted = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return encrypted;
            }
        }

        private void CreateResident(object sender, RoutedEventArgs e)
        {
            TextBox firstnameTextBox = (TextBox)firstname_txtbx;
            TextBox lastnameTextBox = (TextBox)lastname_txtbox;
            TextBox usernameTextBox = (TextBox)username_txtbx;
            TextBox passwordTextBox = (TextBox)password_txtbx;
            TextBox roomNumberTextBox = (TextBox)roomnr_txtbx;
            TextBox licensePlateTextBox = (TextBox)licenseplate_txtbx;
            string firstname = firstnameTextBox.Text;
            string lastname = lastnameTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string roomnumber = roomNumberTextBox.Text;
            string licenseplate = licensePlateTextBox.Text;

            User user = new User();
            if (licenseplate != string.Empty)
            {
                var info = user.checkPlate(licenseplate);
                if (!info.Item1)
                {
                    return;
                }
                licenseplate = info.Item2;
            }


            string query = String.Format("INSERT INTO users (Name, Room, username, password, rank, LicensePlate) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", firstname + " " + lastname, roomnumber, username, Encrypt(password), "Resident", licenseplate);

            bool success = mysql.Execute(query);
            if (success)
            {
                firstnameTextBox.Text = string.Empty;
                lastnameTextBox.Text = string.Empty;
                usernameTextBox.Text = string.Empty;
                passwordTextBox.Text = string.Empty;
                roomNumberTextBox.Text = string.Empty;
                licensePlateTextBox.Text = string.Empty;

            }
            // TODO: Show a message with fail / success message
        }

        private void RemoveByRoom(object sender, RoutedEventArgs e)
        {
            TextBox roomTextBox = (TextBox)searchResident_txtbx;
            string roomNumber = roomTextBox.Text;
            string query = String.Format("DELETE from users WHERE Room={0} AND Rank='Resident'", roomNumber);
            bool success = mysql.Execute(query);
            // TODO: Show a message with fail / success message
        }

        private void SearchByRoom(object sender, RoutedEventArgs e)
        {
            TextBox roomTextBox = (TextBox)searchResident_txtbx;
            string roomNumber = roomTextBox.Text;
            User user = GetUserByRoom(roomNumber);
            if (user == null)
            {
                findResidentFirstname_label.Visibility = Visibility.Hidden;
                findResidentLastname_label.Visibility = Visibility.Hidden;
                findResidentLicense_label.Visibility = Visibility.Hidden;
                findResidentRoom_label.Visibility = Visibility.Hidden;
                findResidentUsername_label.Visibility = Visibility.Hidden;
                return;
            }
            string[] fullname = user.Name.Split(' ');
            string firstname = fullname.First();
            string lastname = fullname.Last();

            findResidentFirstname_label.Content = firstname;
            findResidentFirstname_label.Visibility = Visibility.Visible;

            findResidentLastname_label.Content = lastname;
            findResidentLastname_label.Visibility = Visibility.Visible;

            findResidentLicense_label.Content = String.IsNullOrEmpty(user.LicensePlate) ? "No vehicle registered" : user.LicensePlate;
            findResidentLicense_label.Visibility = Visibility.Visible;

            findResidentRoom_label.Content = user.Room;
            findResidentRoom_label.Visibility = Visibility.Visible;

            findResidentUsername_label.Content = user.Username;
            findResidentUsername_label.Visibility = Visibility.Visible;

            roomTextBox.Text = string.Empty;
        }
    }
}

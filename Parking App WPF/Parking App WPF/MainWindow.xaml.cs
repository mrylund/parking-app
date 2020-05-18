using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Parking_App_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySQL mysql;

        public MainWindow()
        {
            InitializeComponent();
            mysql = new MySQL();
            //string query = String.Format("INSERT INTO users (Name, Room, username, password, rank) VALUES ('Martin Rylund', '228', '{0}', '{1}', 'Admin')", "admin", Encrypt("admin"));
            //mysql.Execute(query);
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

        private User AuthenticateUser(string username, string password)
        {
            string shaPassword = Encrypt(password);
            string query = String.Format("SELECT ID, Username, Name, Room, LicensePlate, Rank FROM users WHERE Username='{0}' AND password='{1}' LIMIT 1", username, shaPassword);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count != 1) {
                Debug.WriteLine("No user found with provided credentials");
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

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) login(sender, e);
        }

        private void login(object sender, RoutedEventArgs e)
        {
            TextBox usnTextBox = (TextBox)username_txtbx;
            TextBox pwTextBox = (TextBox)password_txtbx;
            string username = usnTextBox.Text;
            string password = pwTextBox.Text;

            var user = AuthenticateUser(username, password);

            if (user == null) return;

            if (user.Rank == "Admin")
            {
                LandingWindowManager lwm = new LandingWindowManager();
                lwm.Show();
            } else
            {
                LandingWindowResident l = new LandingWindowResident(user);
                l.Show();
            }
            this.Close();
        }

        private void username_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void password_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}

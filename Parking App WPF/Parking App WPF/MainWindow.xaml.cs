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

        private Tuple<bool, string> AuthenticateUser(string username, string password)
        {
            string shaPassword = Encrypt(password);
            string query = String.Format("SELECT * FROM users WHERE Username='{0}' AND password='{1}' LIMIT 1", username, shaPassword);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count != 1) {
                Debug.WriteLine("No user found with provided credentials");
                return Tuple.Create(false, "");
            }
            DataRow dr = dt.Rows[0];
            string rank = (String)dr["Rank"];
            return Tuple.Create(true, rank);
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

            var info = AuthenticateUser(username, password);

            if (!info.Item1) return;

            if (info.Item2 == "Admin")
            {
                LandingWindowManager lwm = new LandingWindowManager();
                lwm.Show();
            } else
            {
                LandingPage l = new LandingPage();
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

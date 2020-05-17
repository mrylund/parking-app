using System;
using System.Collections.Generic;
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
            //string query = String.Format("INSERT INTO users (Name, Room, username, password) VALUES ('Martin Rylund', '228', '{0}', '{1}')", "sup3", Encrypt("sup3"));
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

        private void AuthenticateUser(string username, string password)
        {
            Debug.WriteLine("TRYING TO FIND USERNAME");
            string shaPassword = Encrypt(password);
            string query = String.Format("SELECT * FROM users WHERE Username='{0}' AND password='{1}' LIMIT 1", username, shaPassword);
            MySqlDataReader reader = mysql.Select(query);
            while (reader.Read())
            {
                Debug.WriteLine(reader["Name"]);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AuthenticateUser("sup2", "sup2");
            test obj = new test();
            //this.Close();
            //obj.Show();
        }

        private void username_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void password_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            LandingPage lp = new LandingPage();
            this.Close();
            lp.Show();
        }

    }
}

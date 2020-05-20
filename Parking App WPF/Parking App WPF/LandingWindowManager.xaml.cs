using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for LandingWindowManager.xaml
    /// </summary>
    public partial class LandingWindowManager : Window
    {
        private MySQL mysql;

        public LandingWindowManager()
        {
            mysql = new MySQL();
            InitializeComponent();
        }

        public void SignOut(object sender, RoutedEventArgs E)
        {
            MainWindow mainW = new MainWindow
            {
                Owner = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
            };
            mainW.Show();
            mainW.Owner = null;

            this.Close();
        }

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) GetLicensePlateInfo(sender, e);
        }

        private void GetLicensePlateInfo(object sender, RoutedEventArgs e)
        {
            TextBox licensePlateTextBox = (TextBox)checklicense_txtbx;
            string LicensePlate = licensePlateTextBox.Text;
            User user = new User();
            if (LicensePlate != string.Empty)
            {
                var info = user.checkPlate(LicensePlate);
                if (!info.Item1)
                {
                    findLicense_label.Content = "Invalid Licenseplate";
                    findHrs_label.Content = "";
                    findRoomNumber_label.Content = "";
                    return;
                }
                LicensePlate = info.Item2;
            }
            else
            {
                findLicense_label.Content = "Invalid Licenseplate";
                findHrs_label.Content = "";
                findRoomNumber_label.Content = "";
                return;
            }

            string query = String.Format("SELECT guests.LicensePlate, guests.created, users.Room FROM guests " +
                "INNER JOIN users ON guests.Resident = users.ID " +
                "WHERE guests.LicensePlate = '{0}' AND guests.created > TIMESTAMPADD(DAY, -1, NOW()) " +
                "LIMIT 1;", LicensePlate);
            DataTable dt = mysql.Select(query);
            if (dt.Rows.Count != 1)
            {
                query = String.Format("SELECT Room FROM users WHERE LicensePlate =  '{0}' LIMIT 1", LicensePlate);
                dt = mysql.Select(query);
                if (dt.Rows.Count != 1)
                {
                    findLicense_label.Content = LicensePlate;
                    findHrs_label.Content = 0;
                    findRoomNumber_label.Content = "Not Found";
                    checklicense_txtbx.Text = string.Empty;
                    return;
                }
                DataRow dr = dt.Rows[0];
                findHrs_label.Content = "R";
                findRoomNumber_label.Content = dr["Room"] == DBNull.Value ? string.Empty : (String)dr["Room"];
            } else
            {
                DataRow dr = dt.Rows[0];
                findHrs_label.Content = 24 - Convert.ToInt32((TimeZoneInfo.ConvertTimeToUtc(DateTime.Now) - (DateTime)dr["created"]).TotalHours);
                findRoomNumber_label.Content = dr["Room"] == DBNull.Value ? string.Empty : (String)dr["Room"];
            }
            findLicense_label.Content = LicensePlate;
            checklicense_txtbx.Text = string.Empty;
        }

        // TODO: CREATE METHOD FOR CHANGING PAGE 
        public void ManageUsers(object sender, RoutedEventArgs e)
        {
      
            ManageUsersWindow muw = new ManageUsersWindow
            {
                Owner = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
            };

            muw.Show();
            muw.Owner = null;
            
            this.Close();
        }
    }
}

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
                findHrs_label.Content = 0;
                findRoomNumber_label.Content = "Not Found";
                Debug.WriteLine("No user found with provided room number");
            } else
            {
                DataRow dr = dt.Rows[0];
                int TimeLeft = 24 - Convert.ToInt32((TimeZoneInfo.ConvertTimeToUtc(DateTime.Now) - (DateTime)dr["created"]).TotalHours);
                string Room = dr["Room"] == DBNull.Value ? string.Empty : (String)dr["Room"];
                findHrs_label.Content = TimeLeft;
                findRoomNumber_label.Content = Room;
            }

            findLicense_label.Content = LicensePlate;
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

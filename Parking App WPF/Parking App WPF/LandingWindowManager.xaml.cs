using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static global_funcs.global_functions;

namespace Parking_App_WPF
{
    public partial class LandingWindowManager : Window
    {
        private readonly MySQL mysql = new MySQL();

        public LandingWindowManager()
        {
            InitializeComponent();
        }

        // Function to sign out, takes you back to login window.
        public void SignOut(object sender, RoutedEventArgs E)
        {
            MainWindow mainW = new MainWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            mainW.Show();
            mainW.Owner = null;

            Close();
        }


        // Functon to change window to manage users.
        public void ManageUsers(object sender, RoutedEventArgs e)
        {
            ManageUsersWindow muw = new ManageUsersWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            muw.Show();
            muw.Owner = null;

            Close();
        }


        // Function that runs when a user presses ENTER in a textbox
        private void SubmitForm(object sender, KeyEventArgs e)
        {
            // Check if the key is enter, then run the login function.
            if (e.Key == Key.Return) GetLicensePlateInfo(sender, e);
        }


        // Get information about a license plate and set the corresponding labels content.
        private void GetLicensePlateInfo(object sender, RoutedEventArgs e)
        {
            TextBox licensePlateTextBox = checklicense_txtbx;
            string licensePlate = licensePlateTextBox.Text;

            // Check if the license plate is valid
            if (licensePlate != string.Empty)
            {
                var info = CheckPlate(licensePlate);
                if (!info.Item1)
                {
                    findLicense_label.Content = "Invalid License plate";
                    findHrs_label.Content = "";
                    findRoomNumber_label.Content = "";
                    return;
                }
                licensePlate = info.Item2;
            }
            else
            {
                findLicense_label.Content = "Invalid Licenseplate";
                findHrs_label.Content = "";
                findRoomNumber_label.Content = "";
                return;
            }


            // Select the license plate from the guest license plate database.
            string query = string.Format("SELECT guests.LicensePlate, guests.created, users.Room FROM guests " +
                "INNER JOIN users ON guests.Resident = users.ID " +
                "WHERE guests.LicensePlate = '{0}' AND guests.created > TIMESTAMPADD(DAY, -1, NOW()) " +
                "LIMIT 1;", licensePlate);
            DataTable dt = mysql.Select(query);

            // Check if we have at least one instance of the license plate.
            if (dt.Rows.Count != 1)
            {
                // If not, check if the license plate is one of the residents instead.
                query = string.Format("SELECT Room FROM users WHERE LicensePlate =  '{0}' LIMIT 1", licensePlate);
                dt = mysql.Select(query);

                // Check if a resident owns the License plate
                if (dt.Rows.Count != 1)
                {
                    // If no guests nor residents own the license plate, set the time left as 0 and display "not found" on room number.
                    findLicense_label.Content = licensePlate;
                    findHrs_label.Content = 0;
                    findRoomNumber_label.Content = "Not Found";
                    checklicense_txtbx.Text = string.Empty;
                    return;
                }

                // If a resident owns the car, set the timeleft as R and set their room number.
                DataRow dr = dt.Rows[0];
                findHrs_label.Content = "R";
                findRoomNumber_label.Content = dr["Room"] == DBNull.Value ? string.Empty : (string)dr["Room"];
            } 
            else
            {
                // If the license plate is a guest, fetch how long time is left and what room number the guest is from.
                DataRow dr = dt.Rows[0];
                findHrs_label.Content = 24 - Convert.ToInt32((TimeZoneInfo.ConvertTimeToUtc(DateTime.Now) - (DateTime)dr["created"]).TotalHours);
                findRoomNumber_label.Content = dr["Room"] == DBNull.Value ? string.Empty : (string)dr["Room"];
            }
            findLicense_label.Content = licensePlate;
            checklicense_txtbx.Text = string.Empty;
        }
    }
}

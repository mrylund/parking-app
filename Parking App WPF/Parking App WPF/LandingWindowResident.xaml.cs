using System;
using System.Collections.Generic;
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
    /// Interaction logic for LandingWindowResident.xaml
    /// </summary>
    public partial class LandingWindowResident : Window
    {
        private User user;
        public LandingWindowResident(Object user)
        {
            this.user = (User)user;
            InitializeComponent();
            setPageInfo();
        }

        private void setPageInfo()
        {
            residentName_label.Content = user.Name;
            residentRoom_label.Content = "Room " + user.Room;
            residentLicens_label.Content = String.IsNullOrEmpty(user.LicensePlate) ? "No vehicle registered" : user.LicensePlate;

            switch (user.guests.Count)
            {
                case 0:
                    guestCar1_label.Visibility = Visibility.Hidden;
                    goto case 1;
                case 1:
                    guestCar2_label.Visibility = Visibility.Hidden;
                    goto case 2;
                case 2:
                    guestCar3_label.Visibility = Visibility.Hidden;
                    goto case 3;
                case 3:
                    guestCar4_label.Visibility = Visibility.Hidden;
                    goto case 4;
                case 4:
                    guestCar5_label.Visibility = Visibility.Hidden;
                    break;
            }

            for (int i = 1; i <= user.guests.Count; i++)
            {
                string content = string.Format("{0} - Time left: {1}", user.guests[i-1].Item1, 24 - Convert.ToInt32((TimeZoneInfo.ConvertTimeToUtc(DateTime.Now) - user.guests[i-1].Item2).TotalHours));
                switch (i)
                {
                    case 1:
                        guestCar1_label.Content = content;
                        guestCar1_label.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        guestCar2_label.Content = content;
                        guestCar2_label.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        guestCar3_label.Content = content;
                        guestCar3_label.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        guestCar4_label.Content = content;
                        guestCar4_label.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        guestCar5_label.Content = content;
                        guestCar5_label.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void addGuest(object sender, RoutedEventArgs e)
        {
            TextBox LicenseTextBox = (TextBox)licensePlate_txtbx;
            user.addGuest(LicenseTextBox.Text);
            setPageInfo();
        }

        private void removeGuest(object sender, RoutedEventArgs e)
        {
            TextBox LicenseTextBox = (TextBox)licensePlate_txtbx;
            user.removeGuest(LicenseTextBox.Text);
            setPageInfo();
        }
    }
}

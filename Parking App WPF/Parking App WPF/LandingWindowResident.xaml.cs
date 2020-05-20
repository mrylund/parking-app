using System;
using System.Windows;
using System.Windows.Controls;

namespace Parking_App_WPF
{
    public partial class LandingWindowResident : Window
    {
        private readonly User user;

        public LandingWindowResident(object user)
        {
            this.user = (User)user;
            InitializeComponent();
            SetPageInfo();
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


        // A function to set all the information about the user on the page.
        private void SetPageInfo()
        {
            // Set the content of the users own information.
            residentName_label.Content = user.Name;
            residentRoom_label.Content = "Room " + user.RoomNumber;
            residentLicens_label.Content = string.IsNullOrEmpty(user.LicensePlate) ? "No vehicle registered" : user.LicensePlate;

            // Check how many guests the user have and hide all labels exceeding that count.
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

            // Loop through all the guest vehicles and display them on the page
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


        // Function to add a new guest.
        private void AddGuest(object sender, RoutedEventArgs e)
        {
            TextBox LicenseTextBox = licensePlate_txtbx;
            user.AddGuest(LicenseTextBox.Text);
            SetPageInfo();
        }

        // Function to remove an existing guest.
        private void RemoveGuest(object sender, RoutedEventArgs e)
        {
            TextBox LicenseTextBox = licensePlate_txtbx;
            user.RemoveGuest(LicenseTextBox.Text);
            SetPageInfo();
        }
    }
}

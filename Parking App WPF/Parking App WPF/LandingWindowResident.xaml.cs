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
        }

        private void addGuest(object sender, RoutedEventArgs e)
        {
            TextBox LicenseTextBox = (TextBox)licensePlate_txtbx;
            user.addGuest(LicenseTextBox.Text);
        }

        private void removeGuest_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

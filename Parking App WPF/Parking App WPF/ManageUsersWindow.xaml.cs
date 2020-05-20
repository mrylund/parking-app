using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static global_funcs.global_functions;

namespace Parking_App_WPF
{
    public partial class ManageUsersWindow : Window
    {
        public ManageUsersWindow()
        {
            InitializeComponent();
        }


        // Function to go back to the home screen
        public void GoBackHome(object sender, RoutedEventArgs e)
        {
            LandingWindowManager lwm = new LandingWindowManager()
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            lwm.Show();
            lwm.Owner = null;

            this.Close();
        }


        // Function to create a new resident from the entered information in the text boxes.
        private void SubmitResident(object sender, RoutedEventArgs e)
        {
            TextBox firstnameTextBox = firstname_txtbx;
            TextBox lastnameTextBox = lastname_txtbox;
            TextBox usernameTextBox = username_txtbx;
            PasswordBox passwordTextBox = password_txtbx;
            TextBox roomNumberTextBox = roomnr_txtbx;
            TextBox licensePlateTextBox = licenseplate_txtbx;
            string firstname = firstnameTextBox.Text;
            string lastname = lastnameTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Password;
            string roomNumber = roomNumberTextBox.Text;
            string licensePlate = licensePlateTextBox.Text;


            // Create the resident
            (bool, string) res = CreateResident(firstname + " " + lastname, username, password, roomNumber, licensePlate);

            // If the insertion was successfull, clear the text boxes text for better user experience.
            if (res.Item1)
            {
                firstnameTextBox.Text = string.Empty;
                lastnameTextBox.Text = string.Empty;
                usernameTextBox.Text = string.Empty;
                passwordTextBox.Password = string.Empty;
                roomNumberTextBox.Text = string.Empty;
                licensePlateTextBox.Text = string.Empty;

            }


            // TODO: Show a message with fail / success message (Using res.Item2)
        }


        // Function that runs when a user presses ENTER in a textbox
        private void SubmitForm(object sender, KeyEventArgs e)
        {
            // Check if the key is enter, then run the SearchByRoom function.
            if (e.Key == Key.Return) SearchByRoom(sender, e);
        }

        private void SearchByRoom(object sender, RoutedEventArgs e)
        {
            TextBox roomTextBox = searchResident_txtbx;
            string roomNumber = roomTextBox.Text;

            // Try to fetch a user from the room number, if none found exit function.
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

            // Split the users name into a last name and first name (Don't care about middle names)
            string[] fullname = user.Name.Split(' ');
            string firstname = fullname.First();
            string lastname = fullname.Last();


            // Set the corresponding labels with information and make the labels visible.
            findResidentFirstname_label.Content = firstname;
            findResidentFirstname_label.Visibility = Visibility.Visible;

            findResidentLastname_label.Content = lastname;
            findResidentLastname_label.Visibility = Visibility.Visible;

            findResidentLicense_label.Content = string.IsNullOrEmpty(user.LicensePlate) ? "No vehicle registered" : user.LicensePlate;
            findResidentLicense_label.Visibility = Visibility.Visible;

            findResidentRoom_label.Content = user.RoomNumber;
            findResidentRoom_label.Visibility = Visibility.Visible;

            findResidentUsername_label.Content = user.Username;
            findResidentUsername_label.Visibility = Visibility.Visible;


            // Empty the text box
            roomTextBox.Text = string.Empty;
        }


        // Function to remove a resident by the room number
        private void RemoveByRoom(object sender, RoutedEventArgs e)
        {
            TextBox roomTextBox = searchResident_txtbx;
            string roomNumber = roomTextBox.Text;

            // Remove the resident
            (bool, string) res = RemoveResident(roomNumber);

            // Empty the text box
            roomTextBox.Text = string.Empty;

            // Clear all labels
            findResidentFirstname_label.Visibility = Visibility.Hidden;
            findResidentLastname_label.Visibility = Visibility.Hidden;
            findResidentLicense_label.Visibility = Visibility.Hidden;
            findResidentRoom_label.Visibility = Visibility.Hidden;
            findResidentUsername_label.Visibility = Visibility.Hidden;


            // TODO: Show a message with fail / success message (Using res.Item2)
        }
    }
}

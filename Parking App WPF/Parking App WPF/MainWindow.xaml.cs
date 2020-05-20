using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static global_funcs.global_functions;

namespace Parking_App_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Function that runs when a user presses ENTER in a textbox
        private void SubmitForm(object sender, KeyEventArgs e)
        {
            // Check if the key is enter, then run the login function.
            if (e.Key == Key.Return) Login(sender, e);
        }


        // Function to take the information from the textboxes and verificate the 
        // users and open corresponding windows depending the user ranks.
        private void Login(object sender, RoutedEventArgs e)
        {
            // Gather the textbox objects and the content in them.
            TextBox usnTextBox = username_txtbx;
            PasswordBox pwTextBox = password_txtbx;
            string username = usnTextBox.Text;
            string password = pwTextBox.Password;

            // Get a user object from the provided credentials, if the user is invalud exit the function.
            User user = AuthenticateUser(username, password);
            if (user == null)
            {
                invalidUsername_password_label.Visibility = Visibility.Visible;
                Debug.WriteLine("No user found with provided credentials");
                return;
            }


            // Check the rank of the user
            if (user.Rank == "Admin")
            {
                // If the user is an admin, open the management window (LandignWindowManager)
                LandingWindowManager lwm = new LandingWindowManager()
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                lwm.Show();
                lwm.Owner = null;
            }
            else
            {
                // If the user is not an admin, open the Resident window (LandingWindowResident)
                LandingWindowResident l = new LandingWindowResident(user)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                l.Show();
                l.Owner = null;
            }
            Close();
        }
    }
}

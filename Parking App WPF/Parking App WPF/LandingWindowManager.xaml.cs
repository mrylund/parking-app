﻿using System;
using System.Collections.Generic;
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
        public LandingWindowManager()
        {
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

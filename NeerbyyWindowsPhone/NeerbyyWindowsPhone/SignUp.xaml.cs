using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

namespace NeerbyyWindowsPhone
{
    public partial class SignUp : PhoneApplicationPage
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void register(object sender, RoutedEventArgs e)
        {
            // Tentative de login
            var errorMsg = "Les mots de passe ne correspondent pas";
            Debug.WriteLine(username.Text);
            Debug.WriteLine(mail.Text);
            Debug.WriteLine(password.Password);
            Debug.WriteLine(password2.Password);

            display_progress_bar.Visibility = System.Windows.Visibility.Visible;
            // Ca marche ?


            // Ca marche pas 
            password.Password = "";
            //display_status.Text = errorMsg;

            // Fini
            //display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
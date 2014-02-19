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
    /// <summary>
    /// Page pour récupérer son mot de passe
    /// </summary>
    public partial class RestorePassword : PhoneApplicationPage
    {
        public RestorePassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Callback appelé une fois les informations rentrées par l'utilisateur
        /// </summary>
        private void restorePassword(object sender, RoutedEventArgs e)
        {
            // Tentative de login
            var errorMsg = "Les identifiants que vous avez entré sont incorrects.";
            Debug.WriteLine(mail.Text);

            display_status.Text = "Envoi du mail";
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;
            // Ca marche ?


            //display_status.Text = errorMsg;

            // Fini
            //display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
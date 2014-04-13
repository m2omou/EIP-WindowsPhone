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
    /// Forgotten password view
    /// </summary>
    public partial class RestorePassword : PhoneApplicationPage
    {
        public RestorePassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Callback called to send the procedure mail
        /// </summary>
        private void restorePassword(object sender, RoutedEventArgs e)
        {
            // Tentative de login
            var errorMsg = "Les identifiants que vous avez entré sont incorrects.";
            Debug.WriteLine(mail.Text);
            
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;
            // Ca marche ?


            //display_status.Text = errorMsg;

            // Fini

            WebApi.Singleton.RestorePassword(mail.Text, (String responseMessage, Object resultObject) =>
            {
                display_status.Text = "Envoi du mail";
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
            }, (String responseMessage, WebException exception) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                display_status.Text = responseMessage;
            });
        }
    }
}
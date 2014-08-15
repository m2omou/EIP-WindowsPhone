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

            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            // Ca marche ?


            //display_status.Text = errorMsg;

            // Fini

            WebApi.Singleton.RestorePasswordAsync((String responseMessage, Result result) =>
            {
                asynchronousDisplayer.display_status.Text = "Envoi du mail";
                asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            }, (String responseMessage, Exception exception) =>
            {
                asynchronousDisplayer.Visibility = System.Windows.Visibility.Collapsed;
                asynchronousDisplayer.display_status.Text = responseMessage;
            }, mail.Text);
        }
    }
}
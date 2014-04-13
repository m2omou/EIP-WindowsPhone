using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Page pour s'inscrire à Neerbyy
    /// </summary>
    public partial class SignUp : PhoneApplicationPage
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public SignUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Callback appelé pour créer le compte
        /// </summary>
        private void register(object sender, RoutedEventArgs args)
        {
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;

            WebApi.Singleton.CreateUser(mail.Text, username.Text, password.Password, (String responseMessage, User user) =>
                {
                    display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show("Compte crée.");
                }, (String responseMessage, WebException e) =>
                {
                    display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show(responseMessage);
                });
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
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
    /// Vue de connexion et acces aux differentes vues
    /// </summary>
    public partial class Login : PhoneApplicationPage
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public Login()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        
        /// <summary>
        /// Callback appelé pour tenter de se connecter à Neerbyy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void tryToLogin(object sender, RoutedEventArgs args)
        {
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;

            display_status.Text = "Connexion en cours...";

            WebApi.Singleton.Authenticate(mail.Text, password.Password, (User user) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
            }, (WebException e) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                display_status.Text = e.Message;
            });
        }

        /// <summary>
        /// Callback afin d'accéder à la page d'inscription
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToSignUp(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUp.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Fonction à n'utiliser que pendant le phase de développement afin d'accéder rapidement à la map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToMap(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Callback appelé pour récuper un mot de passe oublié
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToRestorePassword(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RestorePassword.xaml", UriKind.Relative));
        }
    }
}
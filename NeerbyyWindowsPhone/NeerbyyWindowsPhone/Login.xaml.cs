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
        /// <param name="e"></param>
        private void tryToLogin(object sender, RoutedEventArgs args)
        {
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;

            display_status.Text = "Tentative de connexion...";

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
        /// Callback de la requête asynchrone faite au server. Est appelé quand le serveur envoie sa réponse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Reponse du serveur</param>
        private void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            
            // Fini
            display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
            Dispatcher.BeginInvoke(() =>
            {
                if (e.Error == null)
                {
                    string result = e.Result;
                    Debug.WriteLine(result);
                    JObject j_user = JObject.Parse(result);

                    int error = Convert.ToInt32((string)j_user["responseCode"]);
                    if (error == 1)
                    {
                        display_status.Text = (string)j_user["responseMessage"];
                    }
                    else
                    {
                       // Users user = j_user.ToObject<Users>();
                       // Datas.my_account = user;
                        display_status.Text = "Connecté";
                        NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
                    }
                    
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                    MessageBox.Show("Error occured : " + response.StatusCode + response.StatusDescription);
                }
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
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
        private void register(object sender, RoutedEventArgs e)
        {

            Users user = new Users();
            user.id = -1;
            user.username = username.Text;
            user.password = password.Password;
            user.email = mail.Text;

            StringBuilder postData = new StringBuilder();
            postData.AppendFormat("{0}={1}", "user[username]", HttpUtility.UrlEncode(user.username));
            postData.AppendFormat("&{0}={1}", "user[email]", HttpUtility.UrlEncode(user.email));
            postData.AppendFormat("&{0}={1}", "user[password]", HttpUtility.UrlEncode(user.password));

                

            Uri uri = new Uri(string.Format("{0}/users.json", Datas.url_webservice));
            WebClient webClient = new WebClient();

            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            webClient.Headers["Content-Length"] = postData.Length.ToString();

            webClient.UploadStringAsync(uri, "POST", postData.ToString());
            
            // Tentative de login
            var errorMsg = "Les mots de passe ne correspondent pas";

            display_progress_bar.Visibility = System.Windows.Visibility.Visible;
            // Ca marche ?


            // Ca marche pas 
            //display_status.Text = errorMsg;

        }


        /// <summary>
        /// Réponse asynchrone du serveur pour savoir si la création à fonctionné ou non
        /// </summary>
        private void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
            // Fini
            display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
            Dispatcher.BeginInvoke(() =>
            {
                if (e.Error == null)
                {
                    display_status.Text= "Compte créé";
                    //MessageBox.Show("User Created : " + e.Result);
                }
                else
                {
                    WebException we = (WebException)e.Error;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                    MessageBox.Show("Compte créé"); //+ response.StatusCode + response.StatusDescription);
                }
            });
        }



        ///
        ///
        /// Envoi de la requête pour créer un utilisateur
        /// 

        
    }
}
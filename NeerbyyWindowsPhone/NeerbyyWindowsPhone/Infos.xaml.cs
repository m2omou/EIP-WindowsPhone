using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace NeerbyyWindowsPhone
{
    public partial class Infos : PhoneApplicationPage
    {
        public Infos()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Callback appelé pour mettre a jour les informations
        /// </summary>
        private void register(object sender, RoutedEventArgs args)
        {
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;
            //firstname.Text; lastname.Text; 
            /*WebApi.Singleton.CreateUser(username.Text, password.Password, mail.Text, (User user) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Compte crée.");
            }, (WebException e) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show(e.Message);
            });*/
        }

        private void updatePassword(object sender, RoutedEventArgs args)
        {
            display_progress_bar.Visibility = System.Windows.Visibility.Visible;
            //password.Password; 
            /*WebApi.Singleton.CreateUser(username.Text, password.Password, mail.Text, (User user) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Compte crée.");
            }, (WebException e) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show(e.Message);
            });*/
        }

        /// <summary>
        /// Pick image from the phone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhotoChooserTask task = new PhotoChooserTask();
        }

    }
}
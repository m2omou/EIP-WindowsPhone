﻿using System;
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
    /// Connection view
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
        /// Do nothing when back button is pressed
        /// </summary>
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            e.Cancel = true;
        }

        /// <summary>
        /// Callback called to try to connect to Neerbyy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void tryToLogin(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.stack_panel.Visibility = System.Windows.Visibility.Visible;

            asynchronousDisplayer.display_status.Text = "Connexion en cours...";

            WebApi.Singleton.AuthenticateAsync((String responseMessage, UserResult result) =>
            {
                asynchronousDisplayer.stack_panel.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
            }, (String responseMessage, Exception e) =>
            {
                asynchronousDisplayer.stack_panel.Visibility = System.Windows.Visibility.Collapsed;
                asynchronousDisplayer.display_status.Text = responseMessage;
            }, mail.Text, password.Password);
        }

        /// <summary>
        /// Callback called to go to the register view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToSignUp(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUp.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Use Neerbyy without account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToMap(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Callback to go to the forgotten password view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToRestorePassword(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RestorePassword.xaml", UriKind.Relative));
        }
    }
}
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
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("SignUp");
        }

        /// <summary>
        /// Callback appelé pour créer le compte
        /// </summary>
        private void register(object sender, RoutedEventArgs args)
        {

            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;

            WebApi.Singleton.CreateUserAsync((String responseMessage, UserResult result) =>
                {
                    asynchronousDisplayer.Visibility = System.Windows.Visibility.Collapsed;
                    NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
                }, (String responseMessage, Exception e) =>
                {
                    asynchronousDisplayer.Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show(responseMessage);
                }, mail.Text, username.Text, password.Password);
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
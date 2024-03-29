﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NeerbyyWindowsPhone
{
    public partial class Menu : PhoneApplicationPage
    {
        /// <summary>
        /// Default constructor of the menu which handle the user navigation
        /// </summary>
        public Menu()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (WebApi.Singleton.IsUserAuthenticated() == true)
            {
                button_connect.Visibility = System.Windows.Visibility.Collapsed;
                button_disc.Visibility = System.Windows.Visibility.Visible;
                button_fav.Visibility = System.Windows.Visibility.Visible;
                button_flux.Visibility = System.Windows.Visibility.Visible;
                button_infos.Visibility = System.Windows.Visibility.Visible;
                button_msg.Visibility = System.Windows.Visibility.Visible;
                button_settings.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                button_connect.Visibility = System.Windows.Visibility.Visible;
                button_disc.Visibility = System.Windows.Visibility.Collapsed;
                button_fav.Visibility = System.Windows.Visibility.Collapsed;
                button_flux.Visibility = System.Windows.Visibility.Collapsed;
                button_infos.Visibility = System.Windows.Visibility.Collapsed;
                button_msg.Visibility = System.Windows.Visibility.Collapsed;
                button_settings.Visibility = System.Windows.Visibility.Collapsed;
            }
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
        /// Callback called to go to the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoMap(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback called to go to the flux
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoMyFlux(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Flux.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback called to go to the favorites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoMyFavorites(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Favorites.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback called to go to the messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoMessages(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ListingMessages.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback called to go to the personnal information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoMyInformation(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Infos.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback called to logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disconnect(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.LogOutAsync((string responseMessage, Result result) =>
                {

                    NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                }, (string responseMessage, Exception exception) =>
                {

                });
        }

        /// <summary>
        /// Callback called to go to the settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback called to go to the settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoSettings(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsView.xaml", UriKind.Relative));
        }

    }
}
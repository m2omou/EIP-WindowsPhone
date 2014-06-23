using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Display user information
    /// </summary>
    public partial class Profile : PhoneApplicationPage
    {
        public Profile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            username.Text = ((App)Application.Current).currentUser.username;
            fullname.Text = String.Format("{0} {1}", ((App)Application.Current).currentUser.firstname, ((App)Application.Current).currentUser.lastname);
            Uri uri = null;
            uri = new Uri(((App)Application.Current).currentUser.avatar, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            avatar.Source = bitmap;
        }

        /// <summary>
        /// Start messaging with a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Messenger.xaml", UriKind.Relative));
        }
    }
}
using System;
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
    /// <summary>
    /// View of the post creation
    /// </summary>
    public partial class CreatePost : PhoneApplicationPage
    {

        private Place CurrentPlace;
        /// <summary>
        /// Default constructor
        /// </summary>
        public CreatePost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentPlace = ((App)Application.Current).CurrentPlace;
        }

        /// <summary>
        /// Button pressed to create the post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePostPressed(object sender, RoutedEventArgs e)
        {

        }
    }
}
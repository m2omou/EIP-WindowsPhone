using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// User control to show a post preview
    /// </summary>
    public partial class PostPreview : UserControl
    {
        public Post my_post;
        /// <summary>
        /// Default constructor
        /// </summary>
        public PostPreview()
        {
            InitializeComponent();
        }

        private void DisplayPost(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).setRefPost(ref my_post);
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/DisplayPost.xaml", UriKind.Relative));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NeerbyyWindowsPhone.UI
{
    public partial class PlacePreview : UserControl
    {
        public Place my_place;

        public PlacePreview()
        {
            InitializeComponent();
        }

        private void DisplayPlace(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).setRefPlace(ref my_place);
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/ListingPosts.xaml", UriKind.Relative));
        }
    }
}

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
    public partial class FullScreenImage : PhoneApplicationPage
    {
        public FullScreenImage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string context = this.NavigationContext.QueryString["context"];
            myImage.Source = new BitmapImage(new Uri(context, UriKind.RelativeOrAbsolute));
            base.OnNavigatedTo(e);
        }
    }
}
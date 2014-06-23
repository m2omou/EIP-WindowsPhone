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
    public partial class PostComment : UserControl
    {
        public User user;
        public PostComment()
        {
            InitializeComponent();
        }

        private void Comment_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            ((App)Application.Current).currentUser = this.user;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Profile.xaml", UriKind.Relative));
        }


    }
}

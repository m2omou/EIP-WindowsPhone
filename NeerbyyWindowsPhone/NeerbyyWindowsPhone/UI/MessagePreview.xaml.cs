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
    /// <summary>
    /// Display a conversation
    /// </summary>
    public partial class MessagePreview : UserControl
    {
        public User user;
        /// <summary>
        ///  default constructor
        /// </summary>
        public MessagePreview()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Enter in the messenger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).currentUser = this.user;

            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Messenger.xaml", UriKind.Relative));
        }
    }
}

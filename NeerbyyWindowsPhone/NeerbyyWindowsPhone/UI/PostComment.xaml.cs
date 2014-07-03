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
        public Comment comment;
        public PostComment(Comment comment)
        {
            InitializeComponent();
            this.comment = comment;
            if (comment.id == 0 || comment.user.id != WebApi.Singleton.AuthenticatedUser.id)
                delete_button.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Comment_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            ((App)Application.Current).currentUser = this.comment.user;
            (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Profile.xaml", UriKind.Relative));
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Voulez vous vraiment supprimer votre commentaire ?", "Attention !", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.Cancel)
            {

            }
            else
            {
                WebApi.Singleton.DeleteCommentAsync((string responseMessage, Result result) =>
                {
                    this.Visibility = System.Windows.Visibility.Collapsed;
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, comment);
            } 

        }


    }
}

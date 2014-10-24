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
using NeerbyyWindowsPhone.UI;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Display user information
    /// </summary>
    public partial class Profile : PhoneApplicationPage
    {
        private int count;
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
            GoogleAnalytics.EasyTracker.GetTracker().SendView("UserInfo");

            count = 5;
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
            if (!WebApi.Singleton.IsUserAuthenticated())
            {
                ErrorDisplayer error = new ErrorDisplayer(e_err_status.LOGIN);
                return;
            }
            NavigationService.Navigate(new Uri("/Messenger.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Add a place to the stack listing
        /// </summary>
        /// <param name="place"></param>
        /// <param name="first"></param>
        private void AddPlaceToTheListing(Place place, bool first)
        {

            PlacePreview display_place = new PlacePreview();
            display_place.infos.Text = String.Format("{0}, {1}({2})", place.name, place.address, place.city);
            //display_place.number.Text = String.Format("{0} souvenirs", 12);
            display_place.number.Text = place.country;
            display_place.my_place = place;
            StackListing.Children.Add(display_place);
            StackListing.UpdateLayout();
        }

        /// <summary>
        /// Display Favorite places of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Places_Click(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.FollowedPlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                StackListing.Children.Clear();
                foreach (Place place in result.places) {
                    this.AddPlaceToTheListing(place, false);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, (int)((App)Application.Current).currentUser.id, null, null, count);
        }

        /// <summary>
        ///  Display a post preview
        /// </summary>
        /// <param name="post"></param>
        private void AddAPostToTheListing(Post post, bool first)
        {
            PostPreview display_post = new PostPreview();
            display_post.Title.Text = post.content;
            if (post.url != null && post.url != "")
            {
                Uri uri = null;
                if (post.url.StartsWith("http://"))
                    uri = new Uri(post.url, UriKind.Absolute);
                else
                    uri = new Uri("http://" + post.url, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                display_post.Preview.Source = bitmap;
                ScrollingView.UpdateLayout();
            }
            display_post.my_post = post;
            display_post.Infos.Text = "Le " + post.created_at;
            display_post.NBComments.Text = String.Format("Commentaires {0}", post.comments);
            display_post.NBDislikes.Text = String.Format("Downvotes {0}", post.downvotes);
            display_post.NBLikes.Text = String.Format("Upvotes {0}", post.upvotes);
            if (first)
            {
                StackListing.Children.Insert(0, display_post);
            }
            else
            {
                StackListing.Children.Add(display_post);
            }
        }

        /// <summary>
        /// Display Last posts of the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Posts_Click(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                StackListing.Children.Clear();
                foreach (Post post in result.publications)
                {
                    this.AddAPostToTheListing(post, false);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, null, ((App)Application.Current).currentUser, null, null, this.count);
        }
    }
}
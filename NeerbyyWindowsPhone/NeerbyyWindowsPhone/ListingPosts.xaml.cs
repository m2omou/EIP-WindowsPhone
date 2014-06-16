using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NeerbyyWindowsPhone.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace NeerbyyWindowsPhone
{

    /// <summary>
    /// View displaying all the post of a plac
    /// </summary>
    public partial class ListingPosts : PhoneApplicationPage
    {
        private int max_id;
        private int since_id;
        private int count;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ListingPosts()
        {
            InitializeComponent();
            since_id = 0;
            max_id = 0;
            count = 5;
        }

        /// <summary>
        /// Display follow/unfollow button
        /// </summary>
        private void DisplayFollowButton()
        {
            if (((App)Application.Current).currentPlace.followed_place_id == null)
            {
                button_follow.Content = "SUIVRE LE LIEU";
            }
            else
            {
                button_follow.Content = "ARRETER DE SUIVRE LE LIEU";
            }
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
        ///  Load the new posts after a back event
        /// </summary>
        private void LoadNewPosts()
        {
            WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                bool first = false;
                foreach (Post post in result.publications)
                {
                    if (!first)
                    {
                        since_id = post.id;
                        first = true;
                    }
                    this.AddAPostToTheListing(post, true);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, ((App)Application.Current).currentPlace, null, this.since_id , null, this.count);
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                this.LoadNewPosts();
                return;
            }
            else
            {
                StackListing.Children.Clear();
            }
            this.DisplayFollowButton();
            Title.Text = ((App)Application.Current).currentPlace.name;
            WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                StackListing.Children.Clear();
                bool first = false;
                foreach (Post post in result.publications)
                {
                    if (!first)
                    {
                        since_id = post.id;
                        first = true;
                    }
                    this.AddAPostToTheListing(post, false);
                    max_id = post.id;
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, ((App)Application.Current).currentPlace, null, null, null, this.count);
        }


        /// <summary>
        /// Callback to go to creating post view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToCreatePost(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CreatePost.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Callback to load old posts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMore(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                foreach (Post post in result.publications)
                {
                    this.AddAPostToTheListing(post, false);
                    max_id = post.id;
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, ((App)Application.Current).currentPlace, null, null, this.max_id, this.count);
        }

        /// <summary>
        /// Callback to follow/unfollow a place
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FollowPlace(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).currentPlace.followed_place_id == null)
            {
                WebApi.Singleton.FollowPlaceAsync((string responseMessage, PlaceResult result) =>
                {
                    ((App)Application.Current).currentPlace = result.place;
                    this.DisplayFollowButton();
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, ((App)Application.Current).currentPlace);
            }
            else
            {
                WebApi.Singleton.StopFollowingPlaceAsync((string responseMessage, Result result) =>
                {
                    ((App)Application.Current).currentPlace.followed_place_id = null;
                    this.DisplayFollowButton();
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, ((App)Application.Current).currentPlace);
            }
        }
    }
}
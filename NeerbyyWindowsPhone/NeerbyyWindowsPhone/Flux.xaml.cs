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
using System.Windows.Media;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Listing of the recent content of followed places
    /// </summary>
    public partial class Flux : PhoneApplicationPage
    {
        private int max_id;
        private int since_id;
        private int count;
        public Flux()
        {
            InitializeComponent();
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
        ///  Load new posts after a back
        /// </summary>
        private void LoadNewPosts()
        {
            WebApi.Singleton.FeedAsync((string responseMessage, PostListResult result) =>
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
            }, this.since_id, null, this.count);
        }

        /// <summary>
        /// Callback to load old posts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMore(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.FeedAsync((string responseMessage, PostListResult result) =>
            {
                foreach (Post post in result.publications)
                {
                    this.AddAPostToTheListing(post, false);
                    max_id = post.id;
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, null, this.max_id, this.count);
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Feed");

            if (e.NavigationMode == NavigationMode.Back)
            {
                this.LoadNewPosts();
                return;
            }
            StackListing.Children.Clear();
            max_id = 0;
            since_id = 0;
            count = 5;
            WebApi.Singleton.FeedAsync((string responseMessage, PostListResult result) =>
            {
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
            }, null, null, this.count);
        }
    }
}
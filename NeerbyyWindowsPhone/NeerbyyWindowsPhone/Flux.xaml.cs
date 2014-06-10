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
        public Flux()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StackListing.Children.Clear();
            /*WebApi.Singleton.FollowedPlacesAsync((string responseMessage, PlaceListResult result) =>
                {

                }, (String responseMessage, Exception exception) => {

                }, (*/
            WebApi.Singleton.PostsForPlaceAsync((string responseMessage, PostListResult result) =>
            {
                StackListing.Children.Clear();
                foreach (Post post in result.publications)
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
                    StackListing.Children.Add(display_post);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, ((App)Application.Current).currentPlace);
        }
    }
}
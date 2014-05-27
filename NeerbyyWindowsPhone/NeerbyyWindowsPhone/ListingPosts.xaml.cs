﻿using System;
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
    private Place currentPlace;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ListingPosts()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
                StackListing.Children.Clear();
            currentPlace = ((App)Application.Current).currentPlace;
            Title.Text = currentPlace.name;

            WebApi.Singleton.PostsForPlace(currentPlace, (string responseMessage, List<Post> posts) =>
            {
                StackListing.Children.Clear();
                foreach (Post post in posts)
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
                    display_post.NBDislikes.Text = String.Format("Dislikes {0}", post.dislike);
                    display_post.NBLikes.Text = String.Format("Likes {0}", post.like);
                    StackListing.Children.Add(display_post);
                }
            }, (String responseMessage, WebException exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            });
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
    }
}
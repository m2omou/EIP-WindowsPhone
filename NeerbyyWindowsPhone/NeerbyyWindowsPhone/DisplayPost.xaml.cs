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
using Coding4Fun.Toolkit.Controls;

namespace NeerbyyWindowsPhone
{

   
    /// <summary>
    /// View that display the souvenir
    /// </summary>
    public partial class DisplayPost : PhoneApplicationPage
    {
        private Post currentPost;
        /// <summary>
        /// Default constructor
        /// </summary>
        public DisplayPost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            button_vote_down.BorderBrush.Opacity = 0;
            button_vote_up.BorderBrush.Opacity = 0;
            Place currentPlace = ((App)Application.Current).currentPlace;
            currentPost = ((App)Application.Current).currentPost;
            this.DisplayVotes();
            //Title.Text = currentPost.;
            //this.text_content.Text = currentPost.content;
            this.Place.Text = currentPlace.city;
            this.Title.Text = currentPlace.name;
            if (currentPost.url != null && currentPost.url != "")
            {
                Uri uri = null;
                if (currentPost.url.StartsWith("http://"))
                    uri = new Uri(currentPost.url, UriKind.Absolute);
                else
                    uri = new Uri("http://" + currentPost.url, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                this.image_content.Source = bitmap;
                this.image_content.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.image_content.Visibility = System.Windows.Visibility.Collapsed;
            }
            this.DisplayComments();
        }

        /// <summary>
        /// Update the vote buttons' content
        /// </summary>
        private void DisplayVotes()
        {
            this.button_vote_down.Content = string.Format("{0}", currentPost.dislike);
            this.button_vote_up.Content = string.Format("{0}", currentPost.like);
        }

        /// <summary>
        /// Add a comment to the displayed one
        /// </summary>
        private void AddComment(Comment comment)
        {
            PostComment display_comment = new PostComment();

            display_comment.Value.Text = comment.content;
            display_comment.Username.Text = "bobb";
            //var bitmap = new BitmapImage(uri);
            //display_comment.Avatar.Source = bitmap;
            ListingComments.Children.Add(display_comment);
            ScrollingView.UpdateLayout();
        }


        /// <summary>
        /// Display all comments
        /// </summary>
        private void DisplayComments()
        {
            ListingComments.Children.Clear();
            WebApi.Singleton.CommentsForPost(currentPost, 0, (string responseMessage, List<Comment> comments) =>
            {
                foreach (Comment comment in comments)
                {
                    this.AddComment(comment);
                }
            }, (String responseMessage, WebException exception) =>
            {

            });
            
        }

        /// <summary>
        /// Input box to post a comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteComment(object sender, RoutedEventArgs e)
        {
            InputPrompt input = new InputPrompt();
            input.Completed += input_Completed;
            input.Title = "Poster un commentaire";
            input.Message = "Commentez le souvenir";
            input.Show();
        }

        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            // Le contenu du commentaire a poster
            if (e.Result != "")
            WebApi.Singleton.AddCommentToPost(currentPost, e.Result, (string responseMessage, Comment result) =>
            {
                this.AddComment(result);   
            }, (String responseMessage, WebException exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            });
        }

        /// <summary>
        /// Upvote a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoteUp(object sender, RoutedEventArgs e)
        {

            WebApi.Singleton.SetVoteOnPost(currentPost, true, (string responseMessage, Vote result) =>
                {
                 button_vote_up.BorderBrush.Opacity = 100;
                 button_vote_down.BorderBrush.Opacity = 0;
                 currentPost.like += 1;
                 this.DisplayVotes();

                }, (String responseMessage, WebException exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    });
        }

        /// <summary>
        /// DownVote a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoteDown(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.SetVoteOnPost(currentPost, false, (string responseMessage, Vote result) =>
                {
                 button_vote_up.BorderBrush.Opacity = 0;
                 button_vote_down.BorderBrush.Opacity = 100;
                 currentPost.dislike += 1;
                 this.DisplayVotes();

                }, (String responseMessage, WebException exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    });
                }

    }
}
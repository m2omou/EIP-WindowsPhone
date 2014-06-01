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
            this.Place.Text = currentPost.content;
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
            this.button_vote_down.Content = string.Format("{0}", currentPost.downvotes);
            this.button_vote_up.Content = string.Format("{0}", currentPost.upvotes);
        }

        /// <summary>
        /// Add a comment to the displayed one
        /// </summary>
        private void AddComment(Comment comment)
        {
            PostComment display_comment = new PostComment();

            display_comment.Value.Text = comment.content;
            display_comment.Username.Text = "Bobby";
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
            WebApi.Singleton.CommentsForPostAsync((string responseMessage, CommentListResult result) =>
            {
                foreach (Comment comment in result.comments)
                {
                    this.AddComment(comment);
                }
            }, (String responseMessage, Exception exception) =>
            {

            }, currentPost);
            
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

        /// <summary>
        /// Commenter un poste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            // Le contenu du commentaire a poster
            if (e.Result != "")
                WebApi.Singleton.AddCommentToPostAsync((string responseMessage, CommentResult result) =>
            {
                this.AddComment(result.comment);   
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, currentPost, e.Result);
        }

        /// <summary>
        /// Upvote a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoteUp(object sender, RoutedEventArgs e)
        {

            WebApi.Singleton.SetVoteOnPostAsync((string responseMessage, VoteResult result) =>
                {
                 button_vote_up.BorderBrush.Opacity = 100;
                 button_vote_down.BorderBrush.Opacity = 0;
                 currentPost.upvotes = result.publication.upvotes;
                 currentPost.downvotes = result.publication.downvotes;
                 this.DisplayVotes();

                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, currentPost, true);
        }

        /// <summary>
        /// DownVote a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoteDown(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.SetVoteOnPostAsync((string responseMessage, VoteResult result) =>
                {
                 button_vote_up.BorderBrush.Opacity = 0;
                 button_vote_down.BorderBrush.Opacity = 100;
                 currentPost.upvotes = result.publication.upvotes;
                 currentPost.downvotes = result.publication.downvotes;
                 this.DisplayVotes();

                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, currentPost, false);
                }

        /// <summary>
        /// Display the image in fullscreen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_content_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string context = ((sender as Image).Source as BitmapImage).UriSource.ToString();
            NavigationService.Navigate(new Uri(String.Concat("/FullScreenImage.xaml?context=", context), UriKind.RelativeOrAbsolute));
        }

    }
}
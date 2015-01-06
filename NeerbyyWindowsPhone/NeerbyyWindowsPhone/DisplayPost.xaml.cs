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
        private int max_id;
        private int since_id;
        private int count;
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
            GoogleAnalytics.EasyTracker.GetTracker().SendView("DisplaySouvenir");

            max_id = 0;
            since_id = 0;
            count = 5;

            if (!WebApi.Singleton.IsUserAuthenticated() || ((App)Application.Current).currentPost.user.id != WebApi.Singleton.AuthenticatedUser.id)
                delete_button.Visibility = System.Windows.Visibility.Collapsed;

            this.DisplayVotes();
            //Title.Text = ((App)Application.Current).currentPost.;
            //this.text_content.Text = ((App)Application.Current).currentPost.content;
            this.Title.Text = ((App)Application.Current).currentPost.content;
            if (((App)Application.Current).currentPost.url != null && ((App)Application.Current).currentPost.url != "" && ((App)Application.Current).currentPost.type == "2")
            {
                Uri uri = null;
                if (((App)Application.Current).currentPost.url.StartsWith("http://"))
                    uri = new Uri(((App)Application.Current).currentPost.url, UriKind.Absolute);
                else
                    uri = new Uri("http://" + ((App)Application.Current).currentPost.url, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                this.image_content.Source = bitmap;
                this.image_content.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.image_content.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (((App)Application.Current).currentPost.url != null && ((App)Application.Current).currentPost.url != "" && ((App)Application.Current).currentPost.type == "0")
            {
                web_browser.Visibility = System.Windows.Visibility.Visible;
                String url = ((App)Application.Current).currentPost.url;
                web_browser.Content = url;
            }
            else
            {
                web_browser.Visibility = System.Windows.Visibility.Collapsed;
            }
            this.DisplayComments();
        }

        /// <summary>
        /// Update the vote buttons' content
        /// </summary>
        private void DisplayVotes()
        {
            button_vote_down.BorderBrush.Opacity = 0;
            button_vote_up.BorderBrush.Opacity = 0;
            if (((App)Application.Current).currentPost.vote != null)
            if (((App)Application.Current).currentPost.vote.value == false)
            {
                button_vote_down.BorderBrush.Opacity = 100;
            }
            else
            {
                button_vote_up.BorderBrush.Opacity = 100;
            }
            this.button_vote_down.Content = string.Format("{0}", ((App)Application.Current).currentPost.downvotes);
            this.button_vote_up.Content = string.Format("{0}", ((App)Application.Current).currentPost.upvotes);
        }

        /// <summary>
        /// Add a comment to the displayed one
        /// </summary>
        private void AddComment(Comment comment, bool first)
        {
            PostComment display_comment = new PostComment(comment);

            display_comment.Value.Text = comment.content;
            String infos = string.Format("{0} le {1}", comment.user.username, comment.created_at);
            display_comment.Username.Text = infos;
            Uri uri = null;
            uri = new Uri(comment.user.avatar, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            display_comment.Avatar.Source = bitmap;
            if (first)
            {
                ListingComments.Children.Insert(0, display_comment);
            }
            else
            {
                ListingComments.Children.Add(display_comment);
            }
            ScrollingView.UpdateLayout();
            ScrollingView.ScrollToVerticalOffset(ScrollingView.Height);
        }

        /// <summary>
        /// DELETE A POST IF YOU ARE THE OWNER
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Voulez vous vraiment supprimer votre publication ?", "Attention !", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.Cancel)
            {
            }
            else
            {
                WebApi.Singleton.DeletePostAsync((string responseMessage, Result result) =>
                {
                    this.NavigationService.GoBack();
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, ((App)Application.Current).currentPost);
            }

        }

        /// <summary>
        /// Display all comments
        /// </summary>
        private void DisplayComments()
        {
            ListingComments.Children.Clear();
            bool first = false;
            Comment content = new Comment();
            content.created_at = ((App)Application.Current).currentPost.created_at;
            content.content = ((App)Application.Current).currentPost.content;
            content.user = ((App)Application.Current).currentPost.user;
            this.AddComment(content, false);
            WebApi.Singleton.CommentsForPostAsync((string responseMessage, CommentListResult result) =>
            {
                foreach (Comment comment in result.comments)
                {
                    if (!first)
                    {
                        since_id = comment.id;
                        first = true;
                    }
                    this.AddComment(comment, false);
                    max_id = comment.id;
                }
            }, (String responseMessage, Exception exception) =>
            {

            }, ((App)Application.Current).currentPost, null, null, this.count);
        }

        /// <summary>
        /// Input box to post a comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreComments(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.CommentsForPostAsync((string responseMessage, CommentListResult result) =>
            {
                foreach (Comment comment in result.comments)
                {
                    this.AddComment(comment, false);
                    max_id = comment.id;
                }
            }, (String responseMessage, Exception exception) =>
            {

            }, ((App)Application.Current).currentPost, null, this.max_id, this.count); 
        }

        /// <summary>
        /// Input box to post a comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteComment(object sender, RoutedEventArgs e)
        {
            if (!WebApi.Singleton.IsUserAuthenticated())
            {
                ErrorDisplayer error = new ErrorDisplayer(e_err_status.LOGIN);
                return;
            }
            InputPrompt input = new InputPrompt();
            input.Completed += input_Completed;
            input.Title = "Poster un commentaire";
            input.Message = "Commentez le souvenir";
            input.Show();
        }

        /// <summary>
        /// Load new comments
        /// </summary>
        private void NewComments()
        {
            bool first = false;
            WebApi.Singleton.CommentsForPostAsync((string responseMessage, CommentListResult result) =>
            {
                foreach (Comment comment in result.comments)
                {
                    if (!first)
                    {
                        since_id = comment.id;
                        first = true;
                    }
                    this.AddComment(comment, false);
                }
            }, (String responseMessage, Exception exception) =>
            {

            }, ((App)Application.Current).currentPost, this.since_id, null, this.count);
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
                this.NewComments();
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, ((App)Application.Current).currentPost, e.Result);
        }

        /// <summary>
        /// Upvote a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoteUp(object sender, RoutedEventArgs e)
        {
            if (!WebApi.Singleton.IsUserAuthenticated())
            {
                ErrorDisplayer error = new ErrorDisplayer(e_err_status.LOGIN);
                return;
            }
            if (((App)Application.Current).currentPost.vote == null || ((App)Application.Current).currentPost.vote.value == false)
            {
                WebApi.Singleton.SetVoteOnPostAsync((string responseMessage, VoteResult result) =>
                    {
                        ((App)Application.Current).currentPost.upvotes = result.publication.upvotes;
                        ((App)Application.Current).currentPost.downvotes = result.publication.downvotes;
                        ((App)Application.Current).currentPost.vote = result.vote;
                        this.DisplayVotes();
                    }, (String responseMessage, Exception exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    }, ((App)Application.Current).currentPost, true);
            }
            else
            {
                WebApi.Singleton.CancelVoteAsync((string responseMessage, VoteResult result) =>
                {
                    ((App)Application.Current).currentPost.upvotes = result.publication.upvotes;
                    ((App)Application.Current).currentPost.downvotes = result.publication.downvotes;
                    ((App)Application.Current).currentPost.vote = result.vote;
                    this.DisplayVotes();
                    }, (String responseMessage, Exception exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                        }, ((App)Application.Current).currentPost.vote);
            }
        }

        /// <summary>
        /// DownVote a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoteDown(object sender, RoutedEventArgs e)
        {
            if (!WebApi.Singleton.IsUserAuthenticated())
            {
                ErrorDisplayer error = new ErrorDisplayer(e_err_status.LOGIN);
                return;
            }
            if (((App)Application.Current).currentPost.vote == null || ((App)Application.Current).currentPost.vote.value == true)
            {
                WebApi.Singleton.SetVoteOnPostAsync((string responseMessage, VoteResult result) =>
                    {
                        ((App)Application.Current).currentPost.upvotes = result.publication.upvotes;
                        ((App)Application.Current).currentPost.downvotes = result.publication.downvotes;
                        ((App)Application.Current).currentPost.vote = result.vote;
                        this.DisplayVotes();

                    }, (String responseMessage, Exception exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    }, ((App)Application.Current).currentPost, false);
            }
            else
            {
                WebApi.Singleton.CancelVoteAsync((string responseMessage, VoteResult result) =>
                {
                    ((App)Application.Current).currentPost.upvotes = result.publication.upvotes;
                    ((App)Application.Current).currentPost.downvotes = result.publication.downvotes;
                    ((App)Application.Current).currentPost.vote = result.vote;
                    this.DisplayVotes();
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, ((App)Application.Current).currentPost.vote);
            }
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

        /// <summary>
        /// Report a post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportPost(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Report.xaml", UriKind.RelativeOrAbsolute));

        }

        /// <summary>
        /// Handle web browser click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void web_browser_Click(object sender, RoutedEventArgs e)
        {
            String url = ((App)Application.Current).currentPost.url;
            if (url.Contains("http://"))
                Windows.System.Launcher.LaunchUriAsync(new Uri(url));
            else
                Windows.System.Launcher.LaunchUriAsync(new Uri("http://" + url));
        }

    }
}
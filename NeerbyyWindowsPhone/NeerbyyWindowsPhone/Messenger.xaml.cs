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
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Messaging with a specified user
    /// </summary>
    public partial class Messenger : PhoneApplicationPage
    {
        private int count = 20;
        private int since_id = -1;
        private bool refreshing;
        private DispatcherTimer refresh;
        private Conversation conversation;
        /// <summary>
        ///  default constructor
        /// </summary>
        public Messenger()
        {
            InitializeComponent();
            refresh = new DispatcherTimer();
            refresh.Interval = TimeSpan.FromMilliseconds(1000);
            refresh.Tick += OnTimerRefresh;
            refresh.Start();
        }

        /// <summary>
        /// Refresh the messaging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerRefresh(object sender, EventArgs e)
        {
            this.NewMessages();
        }

        /// <summary>
        /// Add a message to the list
        /// </summary>
        /// <param name="message"></param>
        private void AddMessage(Message message, bool first)
        {
            UI.Message display_message = new UI.Message();

            display_message.content.Text = message.content;
            //MessageBox.Show(String.Format("{0} {1}", message.sender_id, ((App)Application.Current).currentUser.id));
            if (message.sender.id == ((App)Application.Current).currentUser.id)
            {
                display_message.LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(0xFF, 209, 226, 226));
                display_message.Margin = new Thickness(0, 0, 30, 0);
            }
            else
            {
                display_message.LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(0xFF, 166, 209, 168));
                display_message.Margin = new Thickness(30, 0, 0, 0);
            }
            if (first)
            {
               StackListing.Children.Insert(0, display_message);
            }
            else
            {
                StackListing.Children.Add(display_message);
            }
            ScrollingView.UpdateLayout();
//            ScrollingView.ScrollToVerticalOffset(ScrollingView.Height);
            ScrollingView.ScrollToVerticalOffset(double.MaxValue);
        }

        /// <summary>
        /// Input box to post a comment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreMessages(object sender, RoutedEventArgs e)
        {
            /*WebApi.Singleton.CommentsForPostAsync((string responseMessage, CommentListResult result) =>
            {
                foreach (Comment comment in result.comments)
                {
                    this.AddMessage(comment, false);
                    max_id = comment.id;
                }
            }, (String responseMessage, Exception exception) =>
            {

            }, ((App)Application.Current).currentPost, null, this.max_id, this.count);*/
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("ConversationList");

            this.since_id = -1;
            this.refreshing = false;
            this.conversation = ((App)Application.Current).currentConversation;

            username.Text = ((App)Application.Current).currentUser.username;
            fullname.Text = String.Format("{0} {1}", ((App)Application.Current).currentUser.firstname, ((App)Application.Current).currentUser.lastname);
            Uri uri = null;
            uri = new Uri(((App)Application.Current).currentUser.avatar, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            avatar.Source = bitmap;
            this.NewMessages();
        }

        /// <summary>
        /// Send a message to a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            InputPrompt input = new InputPrompt();
            input.Completed += input_Completed;
            input.Title = "Messagerie";
            input.Message = "Contenu du message";
            input.Show();
        }

        /// <summary>
        /// send the message once written
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            // Le contenu du commentaire a poster
            if (e.Result != "")
                WebApi.Singleton.SendMessageAsync((string responseMessage, MessageResult result) =>
                {
                    this.conversation = result.conversation;
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, ((App)Application.Current).currentUser, e.Result);
        }

        /// <summary>
        /// Stop the refresh
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            this.refresh.Stop();
        }

        /// <summary>
        /// Display new Messages
        /// </summary>
        /// <param name="message"></param>
        private void NewMessages()
        {
            if (this.refreshing)
                return;

            int? since = null;
            if (this.since_id != -1)
                since = this.since_id;

            refreshing = true;
            WebApi.Singleton.MessagesAsync((string responseMessage, MessageListResult result) =>
            {
                this.since_id = result.messages.First<Message>().id;

                List<Message> messages = result.messages;
                messages.Reverse();

                foreach (Message msg in messages)
                {
                    this.AddMessage(msg, false);
                }
                refreshing = false;
            }, (String responseMessage, Exception exception) =>
            {
                refreshing = false;
            }, this.conversation, since, null, this.count);
        }
    }
}
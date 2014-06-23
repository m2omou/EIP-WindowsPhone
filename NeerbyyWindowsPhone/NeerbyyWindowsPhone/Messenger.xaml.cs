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

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Messaging with a specified user
    /// </summary>
    public partial class Messenger : PhoneApplicationPage
    {
        private int count;
        private int last_id;
        private int since_id;
        private Conversation conversation;
        /// <summary>
        ///  default constructor
        /// </summary>
        public Messenger()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add a message to the list
        /// </summary>
        /// <param name="message"></param>
        private void AddMessage(Message message, bool first)
        {
            UI.Message display_message = new UI.Message();

            display_message.content.Text = message.content;
            if (message.sender_id == ((App)Application.Current).currentUser.id)
            {
                display_message.LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(0xFF, 166, 209, 168));
                display_message.Margin = new Thickness(10, 0, 0, 0);
            }
            else
            {
                display_message.Margin = new Thickness(-10, 0, 0, 0);
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
            count = 5;
            last_id = 0;
            since_id = 0;
            this.conversation = null;

            username.Text = ((App)Application.Current).currentUser.username;
            fullname.Text = String.Format("{0} {1}", ((App)Application.Current).currentUser.firstname, ((App)Application.Current).currentUser.lastname);
            Uri uri = null;
            uri = new Uri(((App)Application.Current).currentUser.avatar, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            avatar.Source = bitmap;

            WebApi.Singleton.ConversationsAsync((string responseMessage, ConversationListResult result) =>
                {
                    foreach (Conversation conversation in result.conversations)
                    {
                        this.conversation = conversation;
                        bool first = false;
                        foreach (Message msg in this.conversation.messages)
                        {
                            if (!first)
                            {
                                since_id = msg.id;
                                first = true;
                            }
                            this.AddMessage(msg, false);
                            this.last_id = msg.id;
                        }
                    }
                }, (String responseMessage, Exception exception) =>
                    {
                        ErrorDisplayer edisp = new ErrorDisplayer();
                    }, ((App)Application.Current).currentUser);
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
                    this.NewMessages();
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                }, ((App)Application.Current).currentUser, e.Result);
        }

        /// <summary>
        /// Display new Messages
        /// </summary>
        /// <param name="message"></param>
        private void NewMessages()
        {
            bool first = false;
            WebApi.Singleton.MessagesAsync((string responseMessage, MessageListResult result) =>
            {
                foreach (Message msg in result.messages)
                {
                    if (!first)
                    {
                        since_id = msg.id;
                        first = true;
                    }
                    this.AddMessage(msg, true);
                }
            }, (String responseMessage, Exception exception) =>
            {

            }, this.conversation, this.since_id, null, this.count);
        }
    }
}
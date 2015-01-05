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
using System.Windows.Threading;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Conversations listing
    /// </summary>
    public partial class ListingMessages : PhoneApplicationPage
    {
        private int count;
        private int max_id;
        private int since_id;
        /// <summary>
        ///  default constructor
        /// </summary>
        public ListingMessages()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Display a conversation in the listing
        /// </summary>
        /// <param name="conversation"></param>
        /// <param name="first"></param>
        private void AddConversation(Conversation conversation, bool first) {
            UI.MessagePreview display_message = new UI.MessagePreview();

            display_message.preview.Text = conversation.messages.Last<Message>().content;
            display_message.username.Text = conversation.recipient.username;
            display_message.user = conversation.recipient;
            display_message.conversation = conversation;
            display_message.date.Text = "";
            Uri uri = null;
            uri = new Uri(conversation.recipient.avatar, UriKind.Absolute);
            var bitmap = new BitmapImage(uri);
            display_message.avatar.Source = bitmap;
            
            if (first)
            {
                StackListing.Children.Insert(0, display_message);
            }
            else
            {
                StackListing.Children.Add(display_message);
            }
            ScrollViewer.UpdateLayout();
        }

        /// <summary>
        /// Display New conversations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Conversations()
        {
            WebApi.Singleton.ConversationsAsync((string responseMessage, ConversationListResult result) =>
            {
                bool first = false;
                foreach (Conversation conversation in result.conversations)
                {
                    if (!first)
                    {
                        this.since_id = conversation.id;
                        first = true;
                    }

                    this.AddConversation(conversation, true);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer edisp = new ErrorDisplayer();
            }, null, this.since_id, 0, count);

        }

        /// <summary>
        /// view will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Conversation");
            loading_bar.IsIndeterminate = true;
            if (e.NavigationMode == NavigationMode.Back)
            {
                this.New_Conversations();
                return;
            }
                count = 100;
                max_id = 0;
                since_id = 0;
            WebApi.Singleton.ConversationsAsync((string responseMessage, ConversationListResult result) =>
            {
                bool first = false;
                loading_bar.IsIndeterminate = false;
                foreach (Conversation conversation in result.conversations)
                {
                        if (!first)
                        {
                            since_id = conversation.id;
                            first = true;
                        }
                    this.AddConversation(conversation, false);
                        this.max_id = conversation.id;
                }
            }, (String responseMessage, Exception exception) =>
            {
                loading_bar.IsIndeterminate = false;
                ErrorDisplayer edisp = new ErrorDisplayer();
            }, null, since_id, max_id, count);
        }

        /// <summary>
        /// Display More Conversations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_More(object sender, RoutedEventArgs e)
        {
            loading_bar.IsIndeterminate = true;
            WebApi.Singleton.ConversationsAsync((string responseMessage, ConversationListResult result) =>
            {
                loading_bar.IsIndeterminate = false;
                foreach (Conversation conversation in result.conversations)
                {
                    this.AddConversation(conversation, false);
                    this.max_id = conversation.id;
                }
            }, (String responseMessage, Exception exception) =>
            {
                loading_bar.IsIndeterminate = false;
                ErrorDisplayer edisp = new ErrorDisplayer();
            }, null, this.since_id, this.max_id, count);

        }
    }
}
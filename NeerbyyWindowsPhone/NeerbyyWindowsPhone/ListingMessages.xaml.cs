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
using Microsoft.Phone.Notification;
using System.Text;
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

            HttpNotificationChannel pushChannel;

            // The name of our push channel.
            string channelName = "ToastChannel";

            // Try to find the push channel.
            pushChannel = HttpNotificationChannel.Find(channelName);

            // If the channel was not found, then create a new connection to the push service.
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(channelName);

                // Register for all the events before attempting to open the channel.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                pushChannel.Open();

                // Bind this new channel for toast events.
                pushChannel.BindToShellToast();

            }
            else
            {
                // The channel was already open, so just register for all the events.
                pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
                MessageBox.Show(String.Format("Channel Uri is {0}",
                    pushChannel.ChannelUri.ToString()));

            }
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
            WebApi.Singleton.ConversationsAsync((string responseMessage, ConversationListResult result) =>
            {
                foreach (Conversation conversation in result.conversations)
                {
                    this.AddConversation(conversation, false);
                    this.max_id = conversation.id;
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer edisp = new ErrorDisplayer();
            }, null, this.since_id, this.max_id, count);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                MessageBox.Show(String.Format("Channel Uri is {0}",
                    e.ChannelUri.ToString()));

            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() => MessageBox.Show(message.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }
    }
}
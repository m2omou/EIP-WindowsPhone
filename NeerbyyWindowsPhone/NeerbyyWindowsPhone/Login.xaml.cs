using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using Microsoft.Phone.Notification;
using System.Text;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Connection view
    /// </summary>
    public partial class Login : PhoneApplicationPage
    {
        private static readonly string toastNotificationURIKey = "toastNotificationURIKey";

        /// <summary>
        /// Constructeur
        /// </summary>
        public Login()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

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
                //pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

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
                //pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                //System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    pushChannel.ChannelUri.ToString()));

                ApplicationSettings.SetSetting(toastNotificationURIKey, pushChannel.ChannelUri.ToString());
            }
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Login");

            if (ApplicationSettings.HasSetting<string>(WebApi.usernameKey))
            {
                mail.Text = ApplicationSettings.GetSetting<string>(WebApi.usernameKey, "");
                password.Password = ApplicationSettings.GetSetting<string>(WebApi.passwordKey, "");

                this.tryToLogin(null, null);
            }
        }

        /// <summary>
        /// Do nothing when back button is pressed
        /// </summary>
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            e.Cancel = true;
        }

        /// <summary>
        /// Callback called to try to connect to Neerbyy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void tryToLogin(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.stack_panel.Visibility = System.Windows.Visibility.Visible;

            asynchronousDisplayer.display_status.Text = "Connexion en cours...";

            WebApi.Singleton.AuthenticateAsync((String responseMessage, UserResult result) =>
            {
                asynchronousDisplayer.stack_panel.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));

                ApplicationSettings.SetSetting(WebApi.usernameKey, mail.Text);
                ApplicationSettings.SetSetting(WebApi.passwordKey, password.Password);
            }, (String responseMessage, Exception e) =>
            {
                asynchronousDisplayer.stack_panel.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show(responseMessage);
            }, mail.Text, password.Password, ApplicationSettings.GetSetting<string>(toastNotificationURIKey, null));
        }

        /// <summary>
        /// Callback called to go to the register view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToSignUp(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUp.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Use Neerbyy without account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToMap(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }


        /// <summary>
        /// Callback to go to the forgotten password view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToRestorePassword(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RestorePassword.xaml", UriKind.Relative));
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
                //System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    e.ChannelUri.ToString()));

                ApplicationSettings.SetSetting(toastNotificationURIKey, e.ChannelUri.ToString());
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Setup the user preferences
    /// </summary>
    public partial class SettingsView : PhoneApplicationPage
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Settings");

            if (WebApi.Singleton.AuthenticatedUser.setting != null) {
               if ((WebApi.Singleton).AuthenticatedUser.setting.allow_messages.HasValue)
                if ((bool)(WebApi.Singleton).AuthenticatedUser.setting.allow_messages)
                    allow_messages_checkbox.IsChecked = true;

               if ((WebApi.Singleton).AuthenticatedUser.setting.send_notification_for_comments.HasValue)
                if ((bool)(WebApi.Singleton).AuthenticatedUser.setting.send_notification_for_comments)
                    notify_comment.IsChecked = true;

               if ((WebApi.Singleton).AuthenticatedUser.setting.send_notification_for_messages.HasValue)
                if ((bool)(WebApi.Singleton).AuthenticatedUser.setting.send_notification_for_messages)
                    notify_message.IsChecked = true;
            }
        }

        /// <summary>
        /// Save the preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            (WebApi.Singleton).SetSettingsAsync((string responseMessage, SettingsResult result) =>
            {
                // SET BY LA REQUEST ?
               /* (WebApi.Singleton).AuthenticatedUser.settings.send_notification_for_comments = notify_comment.IsChecked;
                (WebApi.Singleton).AuthenticatedUser.settings.send_notification_for_messages = notify_message.IsChecked;
                (WebApi.Singleton).AuthenticatedUser.settings.allow_messages = allow_messages_checkbox.IsChecked;*/
                MessageBox.Show("Vos préférences ont bien été sauvegardées !");
            }, (String responseMessage, Exception exception) =>
            {
                MessageBox.Show(responseMessage);
            }, allow_messages_checkbox.IsChecked, notify_comment.IsChecked, notify_message.IsChecked);
        }
    }
}
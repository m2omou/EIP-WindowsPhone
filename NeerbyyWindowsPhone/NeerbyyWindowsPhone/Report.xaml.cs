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
    /// Report view
    /// </summary>
    public partial class Report : PhoneApplicationPage
    {
        /// <summary>
        ///  Default constructor
        /// </summary>
        public Report()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Report");
        }

        /// <summary>
        /// Send the report to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Report_Click(object sender, RoutedEventArgs e)
        {
            ReportType report_type = ReportType.Custom;
            if ((bool)reason.IsChecked)
                report_type = ReportType.Copyright;
            if ((bool)reason1.IsChecked)
                report_type = ReportType.ImageRights;
            if ((bool)reason2.IsChecked)
                report_type = ReportType.DiscriminatoryContent;
            if ((bool)reason3.IsChecked)
                report_type = ReportType.Custom;
            (WebApi.Singleton).ReportPostAsync((string responseMessage, Result result) =>
            {
                MessageBox.Show("Merci de contribuer au bon fonctionnement de Neerbyy !");
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer edisp = new ErrorDisplayer();
            }, ((App)Application.Current).currentPost, report_type, details.Text);
        }
    }
}
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
        /// view will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            count = 5;
            max_id = 0;
            since_id = 0;
        }
    }
}
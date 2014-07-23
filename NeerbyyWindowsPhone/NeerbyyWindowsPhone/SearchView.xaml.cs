using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NeerbyyWindowsPhone.UI;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Results of a search
    /// </summary>
    public partial class SearchView : PhoneApplicationPage
    {
        private int count;
        /// <summary>
        /// default constructor
        /// </summary>
        public SearchView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// view will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                return;
            }
            this.count = 5;
            if (NavigationContext.QueryString.ContainsKey("query"))
            {
                string query_str = NavigationContext.QueryString["query"];
            StackListing.Children.Clear();
            WebApi.Singleton.SearchPlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                foreach (Place place in result.places)
                {
                    this.AddPlaceToTheListing(place, false);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, query_str, ((App)Application.Current).myLatitude, ((App)Application.Current).myLongitude, ((App)Application.Current).currentCategory, count);
            }
        }

        /// <summary>
        /// Add a place to the stack listing
        /// </summary>
        /// <param name="place"></param>
        /// <param name="first"></param>
        private void AddPlaceToTheListing(Place place, bool first)
        {

            PlacePreview display_place = new PlacePreview();
            display_place.infos.Text = String.Format("{0}, {1}({2})", place.name, place.address, place.city);
            //display_place.number.Text = String.Format("{0} souvenirs", 12);
            display_place.number.Text = place.country;
            display_place.my_place = place;
            StackListing.Children.Add(display_place);
        }
    }
}
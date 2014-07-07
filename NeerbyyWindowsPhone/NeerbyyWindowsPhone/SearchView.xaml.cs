﻿using System;
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
        private int max_id;
        private int count;
        private int since_id;
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
            this.max_id = 0;
            this.since_id = 0;
            this.count = 5;
            if (NavigationContext.QueryString.ContainsKey("query"))
            {
                string query_str = NavigationContext.QueryString["query"];
            StackListing.Children.Clear();
            WebApi.Singleton.SearchPlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                bool first = false;
                foreach (Place place in result.places)
                {
                    if (!first)
                    {
                        this.since_id = place.followed_place_id.Value;
                        first = true;
                    }
                    this.AddPlaceToTheListing(place, false);
                    this.max_id = place.followed_place_id.Value;
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, query_str, null, null, ((App)Application.Current).currentCategory, count);
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

        /// <summary>
        /// Callback to load old posts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMore(object sender, RoutedEventArgs e)
        {
            WebApi.Singleton.FollowedPlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                foreach (Place place in result.places)
                {
                    this.AddPlaceToTheListing(place, false);
                    this.max_id = place.followed_place_id.Value;
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, null, null, this.max_id, count);
        }
    }
}
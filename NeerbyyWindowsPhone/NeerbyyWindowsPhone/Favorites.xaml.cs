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
    public partial class Favorites : PhoneApplicationPage
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public Favorites()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StackListing.Children.Clear();
            WebApi.Singleton.FollowedPlacesAsync((string responseMessage, PlaceListResult result) =>
            {
                StackListing.Children.Clear();
                foreach (Place place in result.places)
                {
                    PlacePreview display_place = new PlacePreview();
                    display_place.infos.Text = String.Format("{0}, {1}({2})", place.name, place.address, place.city);
                    display_place.number.Text = String.Format("{0} souvenirs", 12);
                    display_place.my_place = place;
                    StackListing.Children.Add(display_place);
                }
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            });
        }
    }
}
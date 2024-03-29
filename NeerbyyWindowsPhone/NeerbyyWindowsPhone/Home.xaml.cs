using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.IO.IsolatedStorage;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Map view
    /// </summary>
    public partial class Home : PhoneApplicationPage
    {
        private MapLayer layer;
        private GeoCoordinate map_center;
        private GeoCoordinate target;
        private double map_zoom;
        private DispatcherTimer timer_center;
        private DispatcherTimer timer_zoom;
        private List<Place> places;
        private Boolean isCentering = false;
        private Boolean isMoving = false;

        /// <summary>
        /// View constructor
        /// </summary>
        public Home()
        {
            InitializeComponent();

            map_zoom = 16;
            HomeMap.ZoomLevel = map_zoom;

            HomeMap.CenterChanged += HomeMap_CenterChanged;
            HomeMap.ViewChanged += HomeMap_ViewChanged;

            timer_center = new DispatcherTimer();
            timer_center.Interval = TimeSpan.FromMilliseconds(400);
            timer_center.Tick += OnTimerCenterTick;

            timer_zoom = new DispatcherTimer();
            timer_zoom.Interval = TimeSpan.FromMilliseconds(400);
            timer_zoom.Tick += OnTimerZoomTick;

            HomeMap.Center = new GeoCoordinate(48.8582, 2.2945);

            layer = new MapLayer();
            HomeMap.Layers.Add(layer);
        }

       /// <summary>
       /// Display HUD
       /// </summary>
        private void displayHUD(Boolean showHUD)
        {
            if (showHUD)
            {
                infoDisplayer.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                infoDisplayer.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get the user position
        /// </summary>
        private async void getLocation()
        {
            //map_center = new GeoCoordinate(48.81529956035847, 2.3629510402679443);
            //map_center = new GeoCoordinate(48.8581646494056, 2.294425964355468);
            //target = new GeoCoordinate(-map_center.Latitude, -map_center.Longitude);
            //return;
            // temporary hack to center on Paris
            try
            {
                if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
                {
                    // The user has opted out of Location.
                    return;
                }

                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracyInMeters = 50;

                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                map_center = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                target = new GeoCoordinate(-map_center.Latitude, -map_center.Longitude);
                HomeMap.SetView(map_center, HomeMap.ZoomLevel, MapAnimationKind.Linear);
                this.UpdatePlaces();
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("Location is disabled in phone settings.");
                }
                else
                {
                    // something else happened acquring the location
                }
            }
        }

       

        /// <summary>
        /// Ask user permission to access to its GPS
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView("Home");

            this.isCentering = true;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent") || IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent") == false)
            {
                MessageBoxResult result =
                    MessageBox.Show("Do you want to allow this app to access your phone's current location?",
                    "Location",
                    MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            this.getLocation();
            this.UpdatePlaces();
        }

        /// <summary>
        /// Update the list of places by quering the WebApi
        /// </summary>
        private void UpdatePlaces()
        {
            ((App)Application.Current).myLatitude = HomeMap.Center.Latitude;
            ((App)Application.Current).myLongitude = HomeMap.Center.Longitude;
            WebApi.Singleton.PlacesAsync((String responseMessage, PlaceListResult result) =>
            {
                layer.Clear();
                this.places = result.places;
                foreach (Place place in places)
                {
                    CreatePushpin(place);
                }
            }, (String responseMessage, Exception e) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, ((App)Application.Current).myLatitude,((App)Application.Current).myLongitude, null, null, ((App)Application.Current).currentCategory);
            //MessageBox.Show(String.Format("{0} {1}", ((App)Application.Current).myLatitude, ((App)Application.Current).myLongitude));
        }

        /// <summary>
        /// Callback called when the map stops moving
        /// </summary>
        /// 
        void HomeMap_ViewChanged(object sender, MapViewChangedEventArgs e)
        {
            if (!isMoving)
            {
                displayHUD(false);
            }
        }


        /// <summary>
        /// Pushpin creator on the map
        /// </summary>
        private void CreatePushpin(Place infos)
        {
            Pushpin pp = new Pushpin();
            Color pinColor;
            if (infos.publications == 0)
                pinColor = Color.FromArgb(80, 243, 40, 40);
            else if (infos.publications < 10)
                pinColor = Color.FromArgb(120, 255, 195, 65);
            else
                pinColor = Color.FromArgb(160, 62, 184, 142);
            pp.Background = new SolidColorBrush(pinColor);
            pp.Content = infos.name;
            pp.Tag = infos;
            pp.Tap += Pushpin_Tap;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = pp;
            layer.Add(overlay);
            layer[layer.Count - 1].GeoCoordinate = new GeoCoordinate(infos.latitude, infos.longitude);
        }


        /// <summary>
        /// Callback called when the user move the map
        /// </summary>
        /// 
        private void HomeMap_CenterChanged(object sender, MapCenterChangedEventArgs e)
        {
            if (!isMoving)
                displayHUD(false);
            timer_center.Start();
        }

        /// <summary>
        /// Timer to handle when the map stops moving
        /// </summary>
        private void OnTimerCenterTick(Object sender, EventArgs args)
        {
            timer_center.Stop();
            //MessageBox.Show("lol");
            isMoving = false;
            this.UpdatePlaces();
        }

        /// <summary>
        /// Timer to handle zoom event
        /// </summary>
        private void OnTimerZoomTick(Object sender, EventArgs args)
        {
            timer_zoom.Stop();
            String str = "New zoom : " + HomeMap.ZoomLevel;
            //MessageBox.Show(str);

            this.UpdatePlaces();
        }

        /// <summary>
        /// Callback called when the user zoom or unzoom
        /// </summary>
        /// 
        private void HomeMap_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            timer_zoom.Start();
            timer_center.Stop();
        }

        /// <summary>
        /// Go back to the menu when back button is pressed
        /// </summary>
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Menu.xaml", UriKind.Relative));
            base.OnBackKeyPress(e);
        }

        /// <summary>
        /// Callback called when we chose a place to show its information
        /// </summary>
        /// 
        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pp = (Pushpin)sender;
            Place infos = pp.Tag as Place;
            target = new GeoCoordinate(infos.latitude, infos.longitude);
            HomeMap.SetView(target, HomeMap.ZoomLevel, MapAnimationKind.Linear);

            infoDisplayer.Visibility = System.Windows.Visibility.Collapsed;
            popup_title.Text = infos.name;
            popup_description.Text = String.Format("{2}\n{0}({1})", infos.city, infos.country, infos.address);
            button_close.Content = "VOIR PLUS...";
            ((App)Application.Current).setRefPlace(ref infos);
            isMoving = true;
            displayHUD(true);
        }

        /// <summary>
        /// Search a palce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Search(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchView.xaml?query=" + search_input.Text, UriKind.Relative));
        }

        /// <summary>
        /// Setup the filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Filter(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FilterView.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Center the map on user location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterMap(object sender, RoutedEventArgs e)
        {
            this.isCentering = true;
            this.infoDisplayer.Visibility = System.Windows.Visibility.Collapsed;
            this.getLocation();
        }

        private void GoToListing(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).currentPlace == null)
            {
                this.infoDisplayer.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }
            NavigationService.Navigate(new Uri("/ListingPosts.xaml", UriKind.Relative));
        }
    }
}
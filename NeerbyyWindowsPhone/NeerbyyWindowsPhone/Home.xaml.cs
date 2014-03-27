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

        /// <summary>
        /// View constructor
        /// </summary>
        public Home()
        {
            InitializeComponent();

            map_center = new GeoCoordinate(39.9484462, 116.3371542);
            target = new GeoCoordinate(-map_center.Latitude, -map_center.Longitude);
            map_zoom = 12;
            HomeMap.Center = map_center;
            HomeMap.ZoomLevel = map_zoom;
            HomeMap.CenterChanged += HomeMap_CenterChanged;
            HomeMap.ViewChanged += HomeMap_ViewChanged;

            timer_center = new DispatcherTimer();
            timer_center.Interval = TimeSpan.FromMilliseconds(200);
            timer_center.Tick += OnTimerCenterTick;

            timer_zoom = new DispatcherTimer();
            timer_zoom.Interval = TimeSpan.FromMilliseconds(200);
            timer_zoom.Tick += OnTimerZoomTick;

            layer = new MapLayer();
            HomeMap.Layers.Add(layer);

            this.UpdatePlaces();
        }

        /// <summary>
        /// Update the list of places by quering the WebApi
        /// </summary>
        private void UpdatePlaces()
        {
            WebApi.Singleton.Places(HomeMap.Center.Latitude, HomeMap.Center.Longitude, (List<Place> places) =>
            {
                layer.Clear();
                this.places = places;
                foreach (Place place in places)
                {
                    CreatePushpin(place);
                }
            }, (WebException e) =>
            {

            });
        }

        /// <summary>
        /// Callback called when the map stops moving
        /// </summary>
        /// 
        void HomeMap_ViewChanged(object sender, MapViewChangedEventArgs e)
        {
            infoDisplayer.Visibility = System.Windows.Visibility.Visible;
        }


        /// <summary>
        /// Pushpin creator on the map
        /// </summary>
        private void CreatePushpin(Place infos) // Il faudrait passer le model de la place ici
        {
            Pushpin pp = new Pushpin();
            pp.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 200));
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
            infoDisplayer.Visibility = System.Windows.Visibility.Collapsed;
            timer_center.Start();
        }

        /// <summary>
        /// Timer to handle when the map stops moving
        /// </summary>
        private void OnTimerCenterTick(Object sender, EventArgs args)
        {
            timer_center.Stop();
            //MessageBox.Show("lol");

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
            popup_description.Text = infos.city;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        
        
    }
}
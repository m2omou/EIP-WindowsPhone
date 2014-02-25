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


namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Vue de la map et des lieux autour de notre position actuelle
    /// </summary>
    public partial class Home : PhoneApplicationPage
    {
        private MapLayer layer;
        private GeoCoordinate map_center;
        private double map_zoom;
        
        /// <summary>
        /// Constructeur de la vue
        /// </summary>
        public Home()
        {
            InitializeComponent();

            map_center = new GeoCoordinate(39.9484462, 116.3371542);
            map_zoom = 12;
            HomeMap.Center = map_center;
            HomeMap.ZoomLevel = map_zoom;
            
            layer = new MapLayer();
            HomeMap.Layers.Add(layer);

            PushpinModel infos1 = new PushpinModel();
            infos1.title = "Lama temple";
            infos1.description = "Trop bien";
            infos1.latitude = 39.9084462;
            infos1.longitude = 116.321542;

            CreatePushpin(infos1);

            PushpinModel infos2 = new PushpinModel();
            infos2.title = "Forbidden City";
            infos2.description = "Trop bien aussi lol";
            infos2.latitude = 39.9684462;
            infos2.longitude = 116.3571542;

            CreatePushpin(infos2);

        }

        /// <summary>
        /// Fonction permettant l'affichage de pushpin sur la map avec les coordonnées passées en paramètre
        /// </summary>
        private void CreatePushpin(PushpinModel infos) // Il faudrait passer le model de la place ici
        {
            Pushpin pp = new Pushpin();
            pp.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 200));
            pp.Content = infos.title;
            pp.Tag = infos;
            pp.Tap += Pushpin_Tap;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = pp;
            layer.Add(overlay);
            layer[layer.Count - 1].GeoCoordinate = new GeoCoordinate(infos.latitude, infos.longitude);
        }


        /// <summary>
        /// Callback appelé lorsque l'utilisateur bouge la carte
        /// </summary>
        /// 
        private void HomeMap_CenterChanged(object sender, MapCenterChangedEventArgs e)
        {
            String str = "New coordinate : " + HomeMap.Center.Latitude + " - " + HomeMap.Center.Longitude;
         //   MessageBox.Show(str);
        }

        /// <summary>
        /// Callback appelé lorsque l'utilisateur zoom ou dezoom
        /// </summary>
        /// 
        private void HomeMap_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            String str = "New zoom : " + HomeMap.ZoomLevel;
            double diff = HomeMap.ZoomLevel - map_zoom;
            if (diff > 1)
            {
                MessageBox.Show(str);
                map_zoom = HomeMap.ZoomLevel;
            }
        }


        /// <summary>
        /// Callback appelé lorsque l'on choisi un lieu pour afficher ses postes
        /// </summary>
        /// 
        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pp = (Pushpin)sender;
            PushpinModel infos = pp.Tag as PushpinModel;
            String str = infos.description; // Au lieu de mettre un string on aurait pu mettre le model qui contient les infos dont on a besoin

            HomeMap.SetView(new GeoCoordinate(infos.latitude, infos.longitude), HomeMap.ZoomLevel, MapAnimationKind.Linear);
            MessageBox.Show(str);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        
        
    }
}
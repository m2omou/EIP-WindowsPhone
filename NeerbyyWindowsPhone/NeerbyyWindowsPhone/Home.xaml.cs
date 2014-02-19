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

        /// <summary>
        /// Fonction permettant l'affichage de pushpin sur la map avec les coordonnées passées en paramètre
        /// </summary>
        private void CreatePushpin(String content, String tag, double longitude, double latitude) // Il faudrait passer le model de la place ici
        {
            Pushpin pp = new Pushpin();
            pp.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 200));
            pp.Content = content;
            pp.Tag = tag;
            pp.Tap += Pushpin_Tap;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = pp;
            layer.Add(overlay);
            layer[layer.Count - 1].GeoCoordinate = new GeoCoordinate(longitude, latitude);
        }

        /// <summary>
        /// Constructeur de la vue
        /// </summary>
        public Home()
        {
            InitializeComponent();

            HomeMap.Center = new GeoCoordinate(39.9484462, 116.3371542);
            HomeMap.ZoomLevel = 12;
            layer = new MapLayer();
            HomeMap.Layers.Add(layer);
            CreatePushpin("Lama Temple", "Yop", 39.9084462, 116.321542);
            CreatePushpin("Forbidden City", "Lait", 39.9684462, 116.3571542);
            CreatePushpin("Temple of Heaven", "Lait", 39.9184462, 116.3171542);
            CreatePushpin("Sanlitun", "Lait", 40.0084462, 116.3271542);
            CreatePushpin("Wudaokou", "Lait", 39.9084462, 116.3571542);
        }

        /// <summary>
        /// Callback appelé lorsque l'on choisi un lieu pour afficher ses postes
        /// </summary>
        private void Pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            String str = (sender as Pushpin).Tag as String; // Au lieu de mettre un string on aurait pu mettre le model qui contient les infos dont on a besoin
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
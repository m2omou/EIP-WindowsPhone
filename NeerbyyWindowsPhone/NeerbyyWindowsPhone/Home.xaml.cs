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
    public partial class Home : PhoneApplicationPage
    {
        private MapLayer layer;

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

        public Home()
        {
            InitializeComponent();

            HomeMap.Center = new GeoCoordinate(36, -120);
            HomeMap.ZoomLevel = 9;
            layer = new MapLayer();
            HomeMap.Layers.Add(layer);
            CreatePushpin("Ni hao", "Yop", 36.1, -120);
            CreatePushpin("Titi", "Lait", 36.5, -120.3);
        }

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
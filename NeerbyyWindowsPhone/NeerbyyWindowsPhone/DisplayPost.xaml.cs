using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NeerbyyWindowsPhone.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace NeerbyyWindowsPhone
{

   
    /// <summary>
    /// View that display the souvenir
    /// </summary>
    public partial class DisplayPost : PhoneApplicationPage
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DisplayPost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            Place currentPlace = ((App)Application.Current).currentPlace;
            Post currentPost = ((App)Application.Current).currentPost;
            //Title.Text = currentPost.;
            this.text_content.Text = currentPost.content;
            this.Place.Text = currentPlace.city;
            this.Title.Text = currentPlace.name;
            if (currentPost.url != null && currentPost.url != "")
            {
                Uri uri = null;
                if (currentPost.url.StartsWith("http://"))
                    uri = new Uri(currentPost.url, UriKind.Absolute);
                else
                    uri = new Uri("http://" + currentPost.url, UriKind.Absolute);
                var bitmap = new BitmapImage(uri);
                this.image_content.Source = bitmap;
                this.image_content.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.image_content.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
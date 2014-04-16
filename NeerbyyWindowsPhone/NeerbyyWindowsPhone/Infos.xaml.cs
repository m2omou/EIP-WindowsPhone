using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace NeerbyyWindowsPhone
{
    public partial class Infos : PhoneApplicationPage
    {
        private PhotoChooserTask photoChooser;
        private CameraCaptureTask cameraCapture;
        private Image default_picture;
        public Infos()
        {
            InitializeComponent();
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += new EventHandler<PhotoResult>(this.photoChooserTask_Completed);

            cameraCapture = new CameraCaptureTask();
            cameraCapture.Completed += new EventHandler<PhotoResult>(this.cameraCaptureTask_Completed);

            default_picture = new Image();
            default_picture.Source = preview_image.Source;
        }

        /// <summary>
        /// Callback appelé pour mettre a jour les informations
        /// </summary>
        private void register(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            //firstname.Text; lastname.Text; 
            /*WebApi.Singleton.CreateUser(username.Text, password.Password, mail.Text, (User user) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Compte crée.");
            }, (WebException e) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show(e.Message);
            });*/
        }

        private void updatePassword(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            //password.Password; 
            /*WebApi.Singleton.CreateUser(username.Text, password.Password, mail.Text, (User user) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Compte crée.");
            }, (WebException e) =>
            {
                display_progress_bar.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show(e.Message);
            });*/
        }

        /// <summary>
        /// Pick image from the phone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            IAsyncResult result = Microsoft.Xna.Framework.GamerServices.Guide.BeginShowMessageBox(
                  "Photo",
                  "D'où souhaitez-vous prendre la photo ?",
                  new string[] { "Camera", "Gallerie" },
                  0,
                  Microsoft.Xna.Framework.GamerServices.MessageBoxIcon.None,
                  null,
                  null);

            result.AsyncWaitHandle.WaitOne();
            int? choice = Microsoft.Xna.Framework.GamerServices.Guide.EndShowMessageBox(result);
            if (choice.HasValue)
            {
                if (choice.Value == 0)
                    cameraCapture.Show();
                else if (choice.Value == 1)
                    photoChooser.Show();
            }
        }

       

        private void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                preview_image.Source = bmp;
                button_reset.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                preview_image.Source = bmp;
                button_reset.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            preview_image.Source = default_picture.Source;
            button_reset.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
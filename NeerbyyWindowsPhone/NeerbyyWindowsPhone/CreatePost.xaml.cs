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
    /// <summary>
    /// View of the post creation
    /// </summary>
    public partial class CreatePost : PhoneApplicationPage
    {

        private Place currentPlace;
        private PhotoChooserTask photoChooser;
        private CameraCaptureTask cameraCapture;
        /// <summary>
        /// Default constructor
        /// </summary>
        public CreatePost()
        {
            InitializeComponent();
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += new EventHandler<PhotoResult>(this.photoChooserTask_Completed);

            cameraCapture = new CameraCaptureTask();
            cameraCapture.Completed += new EventHandler<PhotoResult>(this.cameraCaptureTask_Completed);
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            currentPlace = ((App)Application.Current).currentPlace;
        }

        /// <summary>
        /// Button pressed to create the post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePostPressed(object sender, RoutedEventArgs e)
        {
            //preview_image.Source
            //content.Text
            if (content.Text != "")
                if (preview_image.Source == null) // creation de texte
                {
                    WebApi.Singleton.CreatePostWithUrl(currentPlace, content.Text, "", (string responseMessage, Post result) =>
                    {
                        MessageBox.Show("Votre souvenir a bien été créé");
                    }, (String responseMessage, WebException exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    });
                }
                else  // creation d'une image
                {
                    /*WebApi.Singleton.CreatePostWithFile(currentPlace, content.Text, preview_image.Source, (string responseMessage, Post result) =>
                    {
                        MessageBox.Show("Votre souvenir a bien été créé");
                    }, (String responseMessage, WebException exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    });*/
                }
        }

        /// <summary>
        /// Select a picture to post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TakePhoto(object sender, RoutedEventArgs e)
        {
           IAsyncResult result = Microsoft.Xna.Framework.GamerServices.Guide.BeginShowMessageBox(
                "Photo",
                "D'où souhaitez-vous prendre la photo ?",
                new string[] { "Camera", "Gallerie"},
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
                photo_uploader.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                preview_image.Source = bmp;
                photo_uploader.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            photo_uploader.Visibility = System.Windows.Visibility.Collapsed;
            preview_image.Source = null;
        }
    }
}
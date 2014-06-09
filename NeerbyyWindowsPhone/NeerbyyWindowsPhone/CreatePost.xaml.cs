﻿using System;
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
        private System.IO.Stream image_stream;
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
                    WebApi.Singleton.CreatePostWithUrlAsync((string responseMessage, PostResult result) =>
                    {
                        MessageBox.Show("Votre souvenir a bien été créé");
                    }, (String responseMessage, Exception exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    }, currentPlace, content.Text, "", ((App)Application.Current).myLongitude, ((App)Application.Current).myLatitude);
                }
                else  // creation d'une image
                {
                    WebApi.Singleton.CreatePostWithFileAsync((string responseMessage, PostResult result) =>
                    {
                        MessageBox.Show("Votre souvenir a bien été créé");
                    }, (String responseMessage, Exception exception) =>
                    {
                        ErrorDisplayer error = new ErrorDisplayer();
                    }, currentPlace, content.Text, image_stream, preview_image.Name, ((App)Application.Current).myLongitude, ((App)Application.Current).myLatitude);
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
                image_stream = e.ChosenPhoto;
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
                image_stream = e.ChosenPhoto;
                bmp.SetSource(e.ChosenPhoto);
                preview_image.Source = bmp;
                photo_uploader.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            photo_uploader.Visibility = System.Windows.Visibility.Collapsed;
            preview_image.Source = null;
            image_stream = null;
        }
    }
}
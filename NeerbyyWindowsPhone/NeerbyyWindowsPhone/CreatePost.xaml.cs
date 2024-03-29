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
            photoChooser.Completed += this.photoChooserTask_Completed;

            cameraCapture = new CameraCaptureTask();
            cameraCapture.Completed += this.cameraCaptureTask_Completed;
        }

        /// <summary>
        /// View will appear
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            object content;

            if (this.State.TryGetValue("content", out content))
            {
                this.content.Text = content as string;
            }

            GoogleAnalytics.EasyTracker.GetTracker().SendView("CreateSouvenir");
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.State["content"] = content.Text;
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
            {
                if (preview_image.Source == null) // creation de texte
                {
                    String link_str = link.Text;
                    if (link_str == "")
                        link_str = null;
                    WebApi.Singleton.CreatePostWithUrlAsync((string responseMessage, PostResult result) =>
                    {
                        MessageBox.Show("Votre souvenir a bien été créé");
                    }, (String responseMessage, Exception exception) =>
                    {
                        MessageBox.Show(responseMessage);
                    }, ((App)Application.Current).currentPlace, content.Text, link_str, ((App)Application.Current).myLatitude, ((App)Application.Current).myLongitude);
                }
                else  // creation d'une image
                {
                    WebApi.Singleton.CreatePostWithFileAsync((string responseMessage, PostResult result) =>
                    {
                        MessageBox.Show("Votre souvenir a bien été créé");
                    }, (String responseMessage, Exception exception) =>
                    {
                        MessageBox.Show(responseMessage);
                    }, ((App)Application.Current).currentPlace, content.Text, image_stream, preview_image.Name, ((App)Application.Current).myLatitude, ((App)Application.Current).myLongitude);
                }
                content.Text = "";
            }
        }

        /// <summary>
        /// Select a picture to post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TakePhoto(object sender, RoutedEventArgs e)
        {
            link_stack.Visibility = Visibility.Collapsed;

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
                preview_image.Name = System.IO.Path.GetFileName(e.OriginalFileName);
                photo_uploader.Visibility = System.Windows.Visibility.Visible;
                image_stream = e.ChosenPhoto;
                image_stream.Seek(0, System.IO.SeekOrigin.Begin);
            }
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                preview_image.Source = bmp;
                preview_image.Name = System.IO.Path.GetFileName(e.OriginalFileName);
                photo_uploader.Visibility = System.Windows.Visibility.Visible;
                image_stream = e.ChosenPhoto;
                image_stream.Seek(0, System.IO.SeekOrigin.Begin);
            }
        }

        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            photo_uploader.Visibility = System.Windows.Visibility.Collapsed;
            preview_image.Source = null;
            image_stream = null;
            link_stack.Visibility = Visibility.Collapsed;
        }

        private void SendLink(object sender, RoutedEventArgs e)
        {
            photo_uploader.Visibility = System.Windows.Visibility.Collapsed;
            preview_image.Source = null;
            image_stream = null;
            link_stack.Visibility = Visibility.Visible;
        }
    }
}
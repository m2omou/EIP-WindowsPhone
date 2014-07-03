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
using System.Windows.Media.Imaging;

namespace NeerbyyWindowsPhone
{
    public partial class Infos : PhoneApplicationPage
    {
        private PhotoChooserTask photoChooser;
        private CameraCaptureTask cameraCapture;
        private System.IO.Stream image_stream;
        private bool upload_avatar;
        public Infos()
        {
            upload_avatar = false;
            InitializeComponent();
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += new EventHandler<PhotoResult>(this.photoChooserTask_Completed);

            cameraCapture = new CameraCaptureTask();
            cameraCapture.Completed += new EventHandler<PhotoResult>(this.cameraCaptureTask_Completed);


            if (WebApi.Singleton.AuthenticatedUser.username != null)
                username.Text = WebApi.Singleton.AuthenticatedUser.username;
            if (WebApi.Singleton.AuthenticatedUser.email != null)
                mail.Text = WebApi.Singleton.AuthenticatedUser.email;
            if (WebApi.Singleton.AuthenticatedUser.lastname != null)
                lastname.Text = WebApi.Singleton.AuthenticatedUser.lastname;
            if (WebApi.Singleton.AuthenticatedUser.firstname != null)
                firstname.Text = WebApi.Singleton.AuthenticatedUser.firstname;
        }

        /// <summary>
        /// Callback appelé pour mettre a jour les informations
        /// </summary>
        private void register(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            System.IO.Stream image = null;
            String name = null;
            if (upload_avatar)
            {
                image = image_stream;
                name = preview_image.Name;
            }
            WebApi.Singleton.UpdateUserAsync((string responseMessage, UserResult result) =>
            {
                MessageBox.Show("Informations bien prises en compte");
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, mail.Text, username.Text, null, firstname.Text, lastname.Text, image, name);
        }

        /// <summary>
        /// Update the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void updatePassword(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            if (password.Password != password2.Password)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas");
                return;
            }
            WebApi.Singleton.UpdateUserAsync((string responseMessage, UserResult result) =>
            {
                MessageBox.Show("Informations bien prises en compte");
            }, (String responseMessage, Exception exception) =>
            {
                ErrorDisplayer error = new ErrorDisplayer();
            }, null, null, password2.Password, null, null, null, null);
        }

        private void deleteAccount(object sender, RoutedEventArgs args)
        {
            asynchronousDisplayer.Visibility = System.Windows.Visibility.Visible;
            MessageBoxResult m = MessageBox.Show("Voulez vous vraiment supprimer votre compte ?", "Attention !", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.Cancel)
            {

            }
            else
            {
                WebApi.Singleton.DeleteUserAsync((string responseMessage, Result result) =>
                {
                    NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                }, (String responseMessage, Exception exception) =>
                {
                    ErrorDisplayer error = new ErrorDisplayer();
                });
            } 

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
                image_stream = e.ChosenPhoto;
                image_stream.Seek(0, System.IO.SeekOrigin.Begin);
                upload_avatar = true;
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
                image_stream = e.ChosenPhoto;
                image_stream.Seek(0, System.IO.SeekOrigin.Begin);
                upload_avatar = true;
            }
        }

        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            preview_image.Source = new BitmapImage(new Uri("default.jpg", UriKind.Relative));
            button_reset.Visibility = System.Windows.Visibility.Collapsed;
            upload_avatar = false;
        }
    }
}
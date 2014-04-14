using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NeerbyyWindowsPhone
{
    public partial class PostPreview : UserControl
    {
        public int id;
        public PostPreview()
        {
            InitializeComponent();
        }

        private void DisplayPost(object sender, RoutedEventArgs e)
        {
            String msg = String.Format("Mon id est le {0}", id);
            MessageBox.Show(msg);
        }
    }
}

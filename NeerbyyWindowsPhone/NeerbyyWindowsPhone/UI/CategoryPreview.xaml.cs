using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NeerbyyWindowsPhone.UI
{
    public partial class CategoryPreview : UserControl
    {
        public Category myCategory;

        public CategoryPreview()
        {
            InitializeComponent();
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ((App)Application.Current).currentCategory = myCategory;
        }
    }
}

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
    /// <summary>
    /// Error displayer when an error ocurs
    /// </summary>
    public partial class ErrorDisplayer : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ErrorDisplayer()
        {
            InitializeComponent();
            MessageBox.Show("Une erreur est survenue.");
        }
    }
}

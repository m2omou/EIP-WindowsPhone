﻿#pragma checksum "C:\Users\Wassim SAMAD\Documents\Visual Studio 2012\Projects\NeerbyyWindowsPhone\NeerbyyWindowsPhone\NeerbyyWindowsPhone\RestorePassword.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D2438C760ABD4D988B91E068000C09C2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace NeerbyyWindowsPhone {
    
    
    public partial class RestorePassword : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock display_status;
        
        internal System.Windows.Controls.ProgressBar display_progress_bar;
        
        internal System.Windows.Controls.TextBox mail;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/NeerbyyWindowsPhone;component/RestorePassword.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.display_status = ((System.Windows.Controls.TextBlock)(this.FindName("display_status")));
            this.display_progress_bar = ((System.Windows.Controls.ProgressBar)(this.FindName("display_progress_bar")));
            this.mail = ((System.Windows.Controls.TextBox)(this.FindName("mail")));
        }
    }
}


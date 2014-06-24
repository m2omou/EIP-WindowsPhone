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
    public enum e_err_status
    {
        DEFAULT,
        LOGIN
    }
    /// <summary>
    /// Error displayer when an error ocurs
    /// </summary>
    public partial class ErrorDisplayer : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ErrorDisplayer(e_err_status err = e_err_status.DEFAULT)
        {
            InitializeComponent();
            if (err == e_err_status.DEFAULT)
                MessageBox.Show("Une erreur est survenue.");
            else if (err == e_err_status.LOGIN)
                MessageBox.Show("Cette fonctionnalité n'est disponible que pour les utilisateurs enregistrés. Si vous disposez d'un compte veuillez vous connecter. Dans le cas contraire nous vous invitons à créer un compte.");
        }


    }
}

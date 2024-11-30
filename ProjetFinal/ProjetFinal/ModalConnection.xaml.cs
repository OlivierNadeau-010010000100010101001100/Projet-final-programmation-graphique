using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    public sealed partial class ModalConnection : ContentDialog
    {
        //public String Nom_utilisateur { get; set; }
        //public String Mot_de_passe { get; set; }

        bool validation = false;

        public ModalConnection()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            string nomUtilisateur = tbx_utilisateur.Text;
            string mdp = tbx_password.Password;
            int combobox = user_type.SelectedIndex;

            if (Singleton.Instance().UserConnection(nomUtilisateur, mdp, combobox))
            {
                // si l'utilisateur est conencté
                Singleton.SetUserConn(true);
            }
            else
            {
                // s'il n'est pas connecté
                tbx_password.Password = "";
                Singleton.SetUserConn(false);
            }

        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {

        }
    }
}

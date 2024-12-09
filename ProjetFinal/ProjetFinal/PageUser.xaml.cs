using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageUser : Page
    {
        public PageUser()
        {

            this.InitializeComponent();
            Singleton.Instance().SetMessageErreur(MessageConn);

            VerificationConnection();

        }
        private void VerificationConnection()
        {
            if(Singleton.GetUserConnection())
            {
                xamlAffichageLorsqueConnected();
            } else
            {
                xamlAffichageLorsqueDisconnected();
            }
        }

        private async void Connection_Click(object sender, RoutedEventArgs e)
        {
            ModalConnection modalconnection = new ModalConnection();

            modalconnection.XamlRoot = grid_user.XamlRoot;

            modalconnection.PrimaryButtonText = "Se connecter";
            modalconnection.Title = "Page de connection";
            modalconnection.CloseButtonText = "Annuler";

            var result = await modalconnection.ShowAsync();


            Singleton.GetUserConnection();
            if (Singleton.GetUserConnection())
            {
                ConnectionModifications(true);

            }
            else
            {
                ConnectionModifications(false);
            }

        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName) 
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}




        private void Deconnection_Click(object sender, RoutedEventArgs e)
        {
            Singleton.SetUserConn(false);
            ConnectionModifications(false);
            Singleton.Instance().ResetUserType();
            MainWindow.Instance().ConnectionXamlVisibilityModifications();

            // Parties du xaml à mettre en hidden lorsque un user se déconnecte
            xamlAffichageLorsqueDisconnected();

        }

        private void ConnectionModifications(bool connection_affichage)
        {
            if (connection_affichage)
            {
                // Si la connection a bel et bien été établis, ont affiche les informations de l'utilisateur
                nom_utilisateur_run.Text = Singleton.GetUsername();

                connection.Visibility = Visibility.Collapsed;
                deconnection.Visibility = Visibility.Visible;
                tbl_bonjour.Visibility = Visibility.Visible;
                
                MainWindow.Instance().ConnectionXamlVisibilityModifications();

            } else 
            {

                xamlAffichageLorsqueDisconnected();
                MessageConn.Text = "";
            }
        }

        private void xamlAffichageLorsqueDisconnected()
        {
            connection.Visibility = Visibility.Visible;
            deconnection.Visibility = Visibility.Collapsed;
            tbl_bonjour.Visibility = Visibility.Collapsed;
        }

        private void xamlAffichageLorsqueConnected()
        {
            connection.Visibility = Visibility.Collapsed;
            deconnection.Visibility = Visibility.Visible;
            tbl_bonjour.Visibility = Visibility.Visible;
        }
    }
}

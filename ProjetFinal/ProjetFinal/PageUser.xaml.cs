using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageUser : Page
    {
        public PageUser()
        {

            this.InitializeComponent();
            CheckConnection();
            Singleton.Instance().SetMessageErreur(MessageConn);



        }

        private async void Connection_Click(object sender, RoutedEventArgs e)
        {
            ModalConnection modalconnection = new ModalConnection();

            modalconnection.XamlRoot = grid_user.XamlRoot;

            modalconnection.PrimaryButtonText = "Se connecter";
            modalconnection.Title = "Page de connection";
            modalconnection.CloseButtonText = "Annuler";

            var result = await modalconnection.ShowAsync();

            //string nomUtilisateur = Username.Text;   
            //string mdp = Password.Password;
            //int adminCheck = (IsAdminCheckBox.IsChecked == true) ? 1 : 0;

            //if (Singleton.Instance().UserConnection(nomUtilisateur, mdp, adminCheck))
            //{
            //    Password.Password = "";
            //    Singleton.SetUserConn(false);
            //}
            //else
            //{
            //    Singleton.SetUserConn(true);
            //    IsConnected();
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        private void Deconnection_Click(object sender, RoutedEventArgs e)
        {
            IsDisconnected();
            Singleton.SetUserConn(false);
        }

        private void IsConnected()
        {
            Username.Visibility = Visibility.Collapsed;         //A regrouper plus tard dans le visuel pour juste avoir 1 seul visiblility change a faire
            Password.Visibility = Visibility.Collapsed;
            connection.Visibility = Visibility.Collapsed;
            deconnection.Visibility = Visibility.Visible;
            PasswordText.Visibility = Visibility.Collapsed;
            UsernameText.Visibility = Visibility.Collapsed;
            IsAdminCheckBox.Visibility = Visibility.Collapsed;
        }

        private void IsDisconnected() 
        {
            Username.Visibility = Visibility.Visible;               //A regrouper plus tard dans le visuel pour juste avoir 1 seul visiblility change a faire
            Password.Visibility = Visibility.Visible;
            connection.Visibility = Visibility.Visible;
            deconnection.Visibility = Visibility.Collapsed;
            PasswordText.Visibility = Visibility.Visible;
            UsernameText.Visibility = Visibility.Visible;
            IsAdminCheckBox.Visibility= Visibility.Visible;
            IsAdminCheckBox.IsChecked = false;
            MessageConn.Text = "";
            Username.Text = "";
            Password.Password = "";
        }

        private void CheckConnection()
        {
            if (Singleton.CheckConnection())
            {
                IsConnected();
            }
            else
            {
                IsDisconnected();
            }
        }

    }
}

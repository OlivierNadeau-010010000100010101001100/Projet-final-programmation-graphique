using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.AppBroadcasting;
using Windows.Media.Core;
using Windows.Networking.Vpn;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageAjoutAdherent : Page
    {
        public PageAjoutAdherent()
        {
            this.InitializeComponent();
            Singleton.Instance().SetMessageErreur(MessageErreur);
        }

       

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
            bool validation = true;
            string messageErreur = "";

            string prenom = TextPrenom.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(prenom))
            {
                messageErreur += "Il faut un prenom valide \n";
                validation = false;
            }

            string nom = TextNom.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(nom))
            {
                messageErreur += "Il faut un nom valide \n";
                validation = false;
            }

            string mdp = Password.Text?.Trim() ?? "";
            string mdpconf = PasswordConf.Text?.Trim() ?? "";

            if(string.IsNullOrEmpty(mdp))
            {
                messageErreur += "Il faut entrer un mot de passe\n";
                validation = false;
            } else if(mdp != mdpconf)
            {
                messageErreur += "Le mot de passe ne corresponds pas\n";
                validation = false;
            }
          

            if (!TextDate.SelectedDate.HasValue)
            {
                messageErreur += "Il faut selectionner une date valide\n";
                validation = false;
            } 


            if (!validation)
            {
                MessageErreur.Text = messageErreur;
            }
            else
            {
                    
                    string date = TextDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                    string adresse = TextAdresse.Text?.Trim() ?? "";
                    
                    
                if(Singleton.Instance().AjoutAdherent(prenom, nom, adresse, date, mdp))
                {
                    MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    MessageErreur.Text = "succes";
                    TextPrenom.Text = "";
                    TextNom.Text = "";
                    TextAdresse.Text = "";
                    Password.Text = "";
                    PasswordConf.Text = "";
                    TextDate.SelectedDate = null;
                }
               
               
                

            }

        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            //jsais pas trop encore pk jai fait ce bouton la xD
        }
    }
}

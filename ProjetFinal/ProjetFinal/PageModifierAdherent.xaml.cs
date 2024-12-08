using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageModifierAdherent : Page
    {
        private string adherentId;
        public PageModifierAdherent()
        {
            this.InitializeComponent();
            Singleton.Instance().SetMessageErreur(MessageErreur);
            

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var adherent = e.Parameter as Adherent;

            if(adherent != null)
            {
                adherentId = adherent.Adherent_id;
                TextPrenom.Text = adherent.Adherent_Prenom;
                TextNom.Text = adherent.Adherent_nom;
                Age.Text = $"Date de naissance, Age: {adherent.Adherent_age}";
                TextAdresse.Text = adherent.Adherent_adresse;
                TextDate.SelectedDate = DateTime.Parse(adherent.Adherent_date_naissance);
               
                
            }
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
                  

                if(Singleton.Instance().ModifierAdherent(adherentId ,prenom, nom, adresse, date))
                {
                    this.Frame.Navigate(typeof(PageGestionUsager));
                }



            }

        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PageGestionUsager));
        }
    }
}

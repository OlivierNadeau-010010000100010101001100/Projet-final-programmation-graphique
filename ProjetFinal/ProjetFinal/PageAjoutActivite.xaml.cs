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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageAjoutActivite : Page
    {
        public PageAjoutActivite()
        {
            this.InitializeComponent();
            OnLoad();
        }

        private void OnLoad()
        {
            try
            {
                var categories = Singleton.Instance().GetCategoriesDictionary();
                CategoriesCombo.ItemsSource = categories.Values.ToList();
            }
            catch
            {

            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
            bool validation = true;
            string messageErreur = "";
            int categorieID = 0;

            string nomActivite = TextNomActivite.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(nomActivite))
            {
                messageErreur += "Il faut un nom d'activité valide \n";
                validation = false;
            }

            
            if (!Int32.TryParse(PrixOrganisation.Text, out int prixOrganisation))
            {
                messageErreur += "Il un nombre valide pour le montant \n";
                validation = false;
            }
            else if (prixOrganisation <= 0)
            {
                messageErreur += "Il un nombre au dessus de 0 \n";
                validation = false;
            }

            if (!Int32.TryParse(PrixVente.Text, out int prixVente))
            {
                messageErreur += "Il un nombre valide pour le montant \n";
                validation = false;
            } 
            else if (prixVente <= 0)
            {
                messageErreur += "Il un nombre au dessus de 0 \n";
                validation = false;
            }

            if(CategoriesCombo.SelectedIndex == -1)
            {
                messageErreur += "Il faut selectionner une categorie \n";
                validation = false;
            } else
            {
                string categorieSelection = CategoriesCombo.SelectedItem as string;
                categorieID = Singleton.Instance().GetCategoriesDictionary().FirstOrDefault(c => c.Value == categorieSelection).Key;
            }

            


            if (!validation)
            {
                MessageErreur.Text = messageErreur;
            }
            else
            {
                //MessageErreur.Text = $"Nom activite:{nomActivite} - Prix Organisation:{prixOrganisation} - Prix de vente{prixVente} - FK activiteID:{categorieID}";

                if(Singleton.Instance().AjoutActivite(nomActivite, prixOrganisation, prixVente, categorieID))
                {
                    MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    MessageErreur.Text = "succes";
                    TextNomActivite.Text = "";
                    PrixOrganisation.Text = "";
                    PrixVente.Text = "";
                    CategoriesCombo.SelectedIndex = -1;
                }
            }
        }
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string categorieSelection = CategoriesCombo.SelectedItem as string;

            var categorieID = Singleton.Instance().GetCategoriesDictionary().FirstOrDefault(c => c.Value == categorieSelection).Key;
            



        }
    }
}

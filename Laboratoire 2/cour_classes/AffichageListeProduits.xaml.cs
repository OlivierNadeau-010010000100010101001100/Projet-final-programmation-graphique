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

namespace cour_classes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AffichageListeProduits : Page
    {
        double prix_min_set = 0;
        double prix_max_set = double.MaxValue;
        string selection = "Tous les produits";
        public AffichageListeProduits()
        {
            this.InitializeComponent();
            LVliste.ItemsSource = Singleton.getInstance().getListe();

            //AffichageCategorieComboBox();
        }

        //private void AffichageCategorieComboBox()
        //{
        //    var v = Singleton.getInstance().getListeCategorie();
        //    // association de combo box avec categorie
        //    categorie_combo_box.ItemsSource = v;
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Appelle la méthode pour récupérer les données depuis la base de données
            Singleton.getInstance().getProduit();

            // Assigne la liste à l'ItemsSource de la ListView
            LVliste.ItemsSource = Singleton.getInstance().getListe();
        }



        private void Suppression_Click(object sender, RoutedEventArgs e)
        {
            var button_cliquer_xaml = sender as Button;
            // Récupérer l'objet Produit à partir du Tag
            Produit produit = button_cliquer_xaml.Tag as Produit;

            Singleton.getInstance().supprimerProduit(produit.Nom, produit.Prix, produit.Categorie);


            // récuperation la sélection actuelle
            selection = categorie_combo_box.SelectedItem as string;
            filtration();
        }




        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Récupérer la sélection actuelle
            selection = categorie_combo_box.SelectedItem as string;
            filtration();
            
        }
        private void prix_min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(prix_min.Text, out prix_min_set))
            {

            }
            else
            {
                prix_min_set = 0;
            }

            filtration();
        }

        private void prix_max_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(prix_max.Text, out prix_max_set))
            {

            }
            else
            {
                prix_max_set = double.MaxValue;
            }

            filtration();
        }

        public void filtration()
        {
            if(LVliste != null)
            {
                Singleton.getInstance().getProduitFiltrer(selection, prix_min_set, prix_max_set);

                var listeProduitFiltrer = Singleton.getInstance().getListeActuelle();
                LVliste.ItemsSource = listeProduitFiltrer;

            }
        }
    }
}

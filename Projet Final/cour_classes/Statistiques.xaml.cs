using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace cour_classes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Statistiques : Page
    {
        public Statistiques()
        {
            this.InitializeComponent();
            AfficherDataStatistiques();
            LVnbrCategorieProduit.ItemsSource = Singleton.getInstance().getListeCategorie();
        }

        public void AfficherDataStatistiques()
        {
            string command;

            // 1 - nombre de produits totaux
            command = "SELECT count(*) FROM produits";

            nbrTotal.Text = Singleton.getInstance().commandExecuter1reponse(command);




            // 2 - produit ayant le prix maximal
            //command = "SELECT nom, MAX(prix) FROM produits"; --> retourne le bon max, mais avec le nom du premier 'objet' dans la BD
            command = "SELECT CONCAT(nom, ',', prix) AS resultat FROM produits ORDER BY prix desc LIMIT 1";
            string produitPlusCher = Singleton.getInstance().commandExecuter1reponse(command);
            
            
            // Doit 'décomposer' le retour de requete avec un split
            var v = produitPlusCher.Split(",");
            nomPlusCher.Text = v[0];
            prixPlusCher.Text = v[1];




            // 3 - liste des categorie avec leurs nombre
            //command = "/*SELECT categorie, COUNT(nom) AS nbr_produit FROM produits GROUP BY categorie*/";


            //foreach (var categorie in categories)
            //{
            //    Debug.WriteLine($"Catégorie: {categorie.categorie}, Nombre: {categorie.nbrParCategorie}");
            //}



        }
    }
}

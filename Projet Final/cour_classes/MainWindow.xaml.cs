using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Protection.PlayReady;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace cour_classes
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(AffichageListeProduits));
        }

        private void nav_view_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;

            switch (item.Name)
            {
                case "iProduits":
                    mainFrame.Navigate(typeof(AffichageListeProduits));
                    break;
                case "iStatistiques":
                    mainFrame.Navigate(typeof(Statistiques));
                    break;
                case "iCSV":
                    mainFrame.Navigate(typeof(BlankPage1));
                    chargerProduitsCSV();
                    break;
                default:
                    break;
            }

        }


        private async void chargerProduitsCSV()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.FileTypeFilter.Add(".csv");

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            Windows.Storage.StorageFile monFichier = await picker.PickSingleFileAsync();

            //if (monFichier != null)
            //{
            //    // J'ai du commenter pour éviter que la BD s'écrase lors qu'un nouveau fichier csv est ajouter dans la BD
            //    //Singleton.getInstance().toutSupprimerBD();

            //    var lignes = await Windows.Storage.FileIO.ReadLinesAsync(monFichier);


            //    foreach(var produit in lignes)
            //    {
            //        var v = produit.Split(";");

            //        List<Produit> liste = new List<Produit>();

            //        // Convertir le string prix en double
            //        double prix;
            //        if (double.TryParse(v[1], out prix))
            //        {
            //            liste.Add(new Produit( v[0], prix, v[2]));
            //        }
                    
            //        Singleton.getInstance().setProduit(liste);
            //    }




            //}
        }

    }
}

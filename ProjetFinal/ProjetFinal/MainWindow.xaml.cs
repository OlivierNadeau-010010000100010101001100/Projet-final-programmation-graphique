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
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 


    public sealed partial class MainWindow : Window
    {

        // Pour pouvoir utiliser mainwindow partout dans toutes les autres pages
        static MainWindow instance = null;
        public static MainWindow Instance() => instance ??= new();

        public MainWindow()
        {
            this.InitializeComponent();
            ConnectionXamlVisibilityModifications();
            mainFrame.Navigate(typeof(PageActivites));
            Singleton.UserConnectionChange += () => iUser.Content = Singleton.GetUsername();
            instance = this;


        }

        private void nav_view_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;
            switch (item.Name)
            {
                case "iActivite":
                    mainFrame.Navigate(typeof(PageActivites));
                    break;
                case "iStat":
                    mainFrame.Navigate(typeof(PageStatistiques));
                    break;
                case "iCsvUsers":
                    mainFrame.Navigate(typeof(PageExportationAdherent));
                    break;
                case "iCsvActivities":
                    mainFrame.Navigate(typeof(PageExportationActivite));
                    break;
                case "iUser":
                    mainFrame.Navigate(typeof(PageUser));
                    break;
                case "iQuitter":
                    Application.Current.Exit();
                    break;

                    /* Listes de gestions des 3 nav itemz */
                case "gestionAdherent":
                    mainFrame.Navigate(typeof(PageGestionUsager));
                    break;
                case "gestionActivite":
                    mainFrame.Navigate(typeof(PageGestionActivite));

                    break;
                case "listeSeance":
                    mainFrame.Navigate(typeof(PageSupprimerSeance));
                    break;
                case "ajoutAdherent":

                    /* Page d'ajout des 3 nav items */
                    mainFrame.Navigate(typeof(PageAjoutAdherent));
                    break;
                case "ajoutActivitee":
                    mainFrame.Navigate(typeof(PageAjoutActivite));
                    break;
                case "ajoutSeances":
                    mainFrame.Navigate(typeof(PageAjoutSeance));
                    break;
                default:
                    break;
            }
            
        }
        
        public void ConnectionXamlVisibilityModifications()
        {
            string userType = Singleton.GetUserType();

            // Une nouvelle variable a été créer dans le Singleton aisni qu'une fonction, si le user n'Est rien, cela veut dire qu'il a été déconnecté, si il est admin, cela veut dire qu'il voit les items de admins

            if (userType == "")
            {
                HeaderAdmin.Visibility = Visibility.Collapsed;

                ActiviteDeroulant.Visibility = Visibility.Collapsed;
                SeanceDeroulant.Visibility = Visibility.Collapsed;
                AdherentDeroulant.Visibility= Visibility.Collapsed;

                iCsvActivities.Visibility = Visibility.Collapsed;
                iCsvUsers.Visibility = Visibility.Collapsed;
                iStat.Visibility = Visibility.Collapsed;

            } 
            else if (userType == "admin")
            {
                HeaderAdmin.Visibility = Visibility.Visible;

                ActiviteDeroulant.Visibility = Visibility.Visible;
                SeanceDeroulant.Visibility = Visibility.Visible;
                AdherentDeroulant.Visibility= Visibility.Visible;

                iCsvActivities.Visibility= Visibility.Visible;
                iCsvUsers.Visibility = Visibility.Visible;
                iStat.Visibility= Visibility.Visible;

            }
            else if (userType == "user")
            {

                

                iCsvActivities.Visibility = Visibility.Collapsed;
                iCsvUsers.Visibility = Visibility.Collapsed;
            }
        }
    }
}

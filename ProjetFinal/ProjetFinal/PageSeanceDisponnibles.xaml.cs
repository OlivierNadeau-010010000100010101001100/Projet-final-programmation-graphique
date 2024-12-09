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
    public sealed partial class PageSeanceDisponnibles : Page
    {
        public PageSeanceDisponnibles()
        {
            this.InitializeComponent();  

            if (Singleton.GetUserConnection()) 
            {
                tbx_inscription_seance.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Récupérer le paramètre passé via Frame.Navigate
            if (e.Parameter != null)
            {
                string nomActivite = e.Parameter.ToString();
                // Utiliser nomActivite dans votre logique ici
                txt_block.Text = $"Nom de l'activité: {nomActivite}";
                LVseances.ItemsSource =  Singleton.Instance().GetSeanceCliquer(nomActivite);

            }
            


        }
        private void button_inscription_seance_Click(object sender, RoutedEventArgs e)
        {
            var seance = (Seance)LVseances.SelectedItem;
            Singleton.Instance().AjoutInscription(seance.Id);

            Frame.Navigate(typeof(PageActivites));
        }

        private void LVseances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Singleton.GetUserConnection())
            {
                tbx_inscription_seance.Visibility = Visibility.Visible;



                if (LVseances.SelectedItem != null)
                {
                    var seance = (Seance)LVseances.SelectedItem;
                    if(Singleton.GetUserType() == "user")
                    {
                        if (Singleton.Instance().checkInscriptionSeance(seance.Id))
                        {

                            button_inscription_seance.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            test_tbx.Text = "Vous êtes déja inscrit à cette séance";
                        }
                    }
                    




                }
            }
        }
    }
}

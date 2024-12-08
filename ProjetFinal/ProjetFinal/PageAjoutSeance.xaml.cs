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
    public sealed partial class PageAjoutSeance : Page
    {
        public PageAjoutSeance()
        {
            this.InitializeComponent();
            OnLoad();
        }


        private void OnLoad() 
        { 
            var activites = Singleton.Instance().GetAllActivites();

            ActiviteCombo.ItemsSource = activites;
            ActiviteCombo.DisplayMemberPath = "Nom_activite";


        }
        private void CategoriesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {

            MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
            bool validation = true;
            string messageErreur = "";





            if (!DateSeance.SelectedDate.HasValue)
            {
                messageErreur += "Il faut selectionner une date\n";
                validation = false;
            }


            if (HeureSeance.SelectedTime == null)
            {
                
                messageErreur += "Il faut selectionner une heure\n";
                validation = false;
            }

            

            if (!Int32.TryParse(NbrPlaces.Text, out int places))
            {
                messageErreur += "Il faut entrer un nombre de places disponible\n";
                validation = false;
                
            } else if (places < 0)
            {
                messageErreur += "Il faut que le nombre de places soit positif\n";
                validation = false;
            }

            if(ActiviteCombo.SelectedIndex == -1)
            {
                messageErreur += "Il faut selectioner une categorie valide\n";
                validation = false;
            }


            if (!validation)
            {
                MessageErreur.Text = messageErreur;
                
            } 
            else
            {
                string date = DateSeance.SelectedDate.Value.ToString("yyyy-MM-dd");
                string temps = HeureSeance.Time.ToString(@"hh\:mm\:ss");
                MessageErreur.Text = "ok";
                var activite = (Activite)ActiviteCombo.SelectedItem;
                int id = activite.Activite_id;
                MessageErreur.Text = id.ToString();

                if(Singleton.Instance().AjoutSeance(date, temps, places, id))
                {
                    MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    MessageErreur.Text = "succes";
                    HeureSeance.SelectedTime = null;
                    DateSeance.SelectedDate = null;
                    NbrPlaces.Text = "";
                    ActiviteCombo.SelectedIndex = -1;
                }
            }
        }
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

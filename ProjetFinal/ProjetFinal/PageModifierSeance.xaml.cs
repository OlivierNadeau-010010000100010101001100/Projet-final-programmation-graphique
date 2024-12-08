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
    public sealed partial class PageModifierSeance : Page
    {
        private static int _id;
        public PageModifierSeance()
        {
            this.InitializeComponent();
            OnLoad();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var seance = e.Parameter as Seance;
            var activites = Singleton.Instance().GetAllActivites();

            ActiviteCombo.ItemsSource = activites;
            ActiviteCombo.DisplayMemberPath = "Nom_activite";


            if (seance != null)
            {
                NbrPlaces.Text = seance.NbrPlaces.ToString();
                DateSeance.SelectedDate = DateTime.Parse(seance.Date);
                HeureSeance.SelectedTime = TimeSpan.Parse(seance.Heure);
                var nomActivite = activites.FirstOrDefault(a => a.Nom_activite == seance.NomActivite);
                ActiviteCombo.SelectedItem = nomActivite;
                _id = seance.Id;
            }
        }


        private void OnLoad()
        {
           

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

            }
            else if (places < 0)
            {
                messageErreur += "Il faut que le nombre de places soit positif\n";
                validation = false;
            }

            if (ActiviteCombo.SelectedIndex == -1)
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
                var activite = (Activite)ActiviteCombo.SelectedItem;
                int id = activite.Activite_id;

                if (Singleton.Instance().ModifierSeance(_id, date, temps, places, id))
                {
                    this.Frame.Navigate(typeof(PageSupprimerSeance));
                }
            }
        }
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PageSupprimerSeance));
        }
    }
}

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
    public sealed partial class PageGestionSeanceClient : Page
    {
        public PageGestionSeanceClient()
        {
            this.InitializeComponent();

            reload();
        }

        private void button_unsub_seance_Click(object sender, RoutedEventArgs e)
        {
            var seanceSelected = (Seance)LVMesSeances.SelectedItem;
            Singleton.Instance().DeleteInscription(seanceSelected.Id);
            reload();

            button_confirm_rating.Visibility = Visibility.Collapsed;
            ComboBox_seance.Visibility = Visibility.Collapsed;
            button_unsub_seance.Visibility = Visibility.Collapsed;
            TextAnnoncement.Visibility = Visibility.Collapsed;
            ComboBox_seance.SelectedIndex = -1;
        }

        private void LVMesSeances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVMesSeances.SelectedItem != null)
            {
                var seance = (Seance)LVMesSeances.SelectedItem;
                button_unsub_seance.Visibility = Visibility.Visible;
                ComboBox_seance.Visibility = Visibility.Visible;
                button_confirm_rating.Visibility = Visibility.Visible;
                TextAnnoncement.Visibility = Visibility.Visible;
                int valeurIndex = string.IsNullOrEmpty(seance.Rating) || !int.TryParse(seance.Rating, out int r) ? -1 : r - 1;
                ComboBox_seance.SelectedIndex = valeurIndex;
            } 
        }

        private void button_confirm_rating_Click(object sender, RoutedEventArgs e)
        {
            var seance = (Seance)LVMesSeances.SelectedItem;
            Singleton.Instance().UpdateRating(seance.Id, ComboBox_seance.SelectedIndex + 1);
            ComboBox_seance.Visibility = Visibility.Collapsed;
            button_confirm_rating.Visibility = Visibility.Collapsed;
            TextAnnoncement.Visibility = Visibility.Collapsed;
            button_unsub_seance.Visibility = Visibility.Collapsed;
            ComboBox_seance.SelectedIndex = -1;

            reload();
        }

        private void reload()
        {
            LVMesSeances.ItemsSource = Singleton.Instance().GetMesSeance();

            if(LVMesSeances.Items.Count == 0)
            {
                LVMesSeances.Visibility = Visibility.Collapsed;
                NonSeances.Visibility = Visibility.Visible;
                OuiSeances.Visibility = Visibility.Collapsed;
            } else
            {
                LVMesSeances.Visibility = Visibility.Visible;
                NonSeances.Visibility = Visibility.Collapsed;
                OuiSeances.Visibility = Visibility.Visible;
            }
        }
    }
}

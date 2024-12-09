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

            LVMesSeances.ItemsSource = Singleton.Instance().GetMesSeance();
        }

        private void button_unsub_seance_Click(object sender, RoutedEventArgs e)
        {
            var seanceSelected = (Seance)LVMesSeances.SelectedItem;
            Singleton.Instance().DeleteInscription(seanceSelected.Id);
        }

        private void LVMesSeances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVMesSeances.SelectedItem != null)
            {
                var seance = (Seance)LVMesSeances.SelectedItem;
                button_unsub_seance.Visibility = Visibility.Visible;
                ComboBox_seance.Visibility = Visibility.Visible;
                button_confirm_rating.Visibility = Visibility.Visible;
            }
        }

        private void button_confirm_rating_Click(object sender, RoutedEventArgs e)
        {
            var seance = (Seance)LVMesSeances.SelectedItem;
            Singleton.Instance().UpdateRating(seance.Id, ComboBox_seance.SelectedIndex + 1);
            ComboBox_seance.Visibility = Visibility.Collapsed;
            button_confirm_rating.Visibility = Visibility.Collapsed;
        }
    }
}

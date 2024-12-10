using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageActivites : Page
    {
        public PageActivites()
        {
            
            this.InitializeComponent();
            OnLoad();
            Singleton.Instance().SetMessageErreur(MessageErreur);
        }

        private void OnLoad()
        {
            try
            {
                var liste = Singleton.Instance().GetAllActivites();
                LVactivite.ItemsSource = liste;
            }
            catch (Exception ex)
            {
                MessageErreur.Text = $"Erreur lors du chargement: {ex.Message}";
            }
        }

        private void LVactivite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVactivite.SelectedIndex != -1) 
            {
                var activite = (Activite)LVactivite.SelectedItem;
                int idActivite = activite.Activite_id;
                string categorieActivite = activite.Categorie_activite;
                TextBlockTest.Text = $"{idActivite} : {categorieActivite}";
                Frame.Navigate(typeof(PageSeanceDisponnibles), idActivite);
            }
        }


    }
}

using System;
using System.Collections.Generic;
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
    public sealed partial class PageStatistiques : Page
    {
        public PageStatistiques()
        {
            this.InitializeComponent();
            GetData();
        }

        public void GetData()
        {
            int nbrAdherent = Singleton.Instance().getNbrAdherent();
            int nbrActivite = Singleton.Instance().getNbrActivites();
            string prixMoyen = Singleton.Instance().getMoyennePrixClient();
            int nbrRatingManquant = Singleton.Instance().getNbrRatingManquant();
            int nbrSeancePasserDate = Singleton.Instance().getNbrSeancePasserDate();

            affichageNbrAdherent.Text = nbrAdherent.ToString();
            affichageNbrActivites.Text = nbrActivite.ToString();
            affichagePrixMoyen.Text = prixMoyen.ToString();
            affichageRatingManquants.Text = nbrRatingManquant.ToString();
            affichageNbSeancePasserDate.Text = nbrSeancePasserDate.ToString();

            LVcategorieList.ItemsSource = Singleton.Instance().GetAllActivites();
            LVNbrPersonneActivite.ItemsSource = Singleton.Instance().GetAllNbrPersonneActivite();
            LVRatingMoyenParActivitee.ItemsSource = Singleton.Instance().GetAllRatingActivite();

        }
        
    }
}

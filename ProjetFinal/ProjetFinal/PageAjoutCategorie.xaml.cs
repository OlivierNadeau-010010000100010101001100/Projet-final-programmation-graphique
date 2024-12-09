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
    public sealed partial class PageAjoutCategorie : Page
    {
        public PageAjoutCategorie()
        {
            this.InitializeComponent();
            Singleton.Instance().SetMessageErreur(MessageErreur);
        }


        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);


            string categorie = TextCategorie.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(categorie))
            {
                MessageErreur.Text = "Le nom de la categorie ne peut pas être vide \n";

            } else
            {
                if (Singleton.Instance().AjoutCategorie(categorie))
                {
                    MessageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    MessageErreur.Text = "succes";
                    TextCategorie.Text = "";

                }
            }


            


        }

            

        
    }
}

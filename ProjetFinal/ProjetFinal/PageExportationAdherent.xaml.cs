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
    public sealed partial class PageExportationAdherent : Page
    {
        public PageExportationAdherent()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance());
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "Liste Activitées";
            picker.FileTypeChoices.Add("Fichier CSV", new List<string>() { ".csv" });


            //crée le fichier



            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            var activites = Singleton.Instance().GetAllActivites();




            // La fonction ToString de la classe Client retourne: nom + ";" + prenom
            if (monFichier != null)
            {
                await Windows.Storage.FileIO.WriteLinesAsync(monFichier, activites.ConvertAll(x => x.ToString()), Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
        }
    }
}

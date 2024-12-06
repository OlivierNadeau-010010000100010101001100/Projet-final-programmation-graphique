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
    public sealed partial class PageSupprimerActivite : Page
    {
        public PageSupprimerActivite()
        {
            this.InitializeComponent();
            OnLoad();
            Singleton.Instance().SetMessageErreur(TestMessage);
        }

        public void OnLoad()
        {
            LVTestActivite.ItemsSource = Singleton.Instance().GetAllActivites();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var activite = (Activite)button.Tag;

                if (activite != null)
                {
                    Singleton.Instance().SupprimerActivite(activite.Activite_id);

                    OnLoad();
                }
            }
            catch (Exception ex)
            {
                TestMessage.Text = ex.Message;
            }
        }


    }
}

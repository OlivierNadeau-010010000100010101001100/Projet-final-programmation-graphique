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
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProjetFinal
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(PageActivites));
            //ifConnected();
            Singleton.UserConnectionChange += () => iUser.Content = Singleton.GetUsername();
            
            

        }


       
        private void nav_view_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;

            switch (item.Name)
            {
                case "iActivite":
                    mainFrame.Navigate(typeof(PageActivites));
                    break;
                case "iStat":
                    mainFrame.Navigate(typeof(PageStatistiques));
                    break;
                case "iCSV":
                    mainFrame.Navigate(typeof(PageUser));
                    break;
                case "iUser":
                    mainFrame.Navigate(typeof(PageUser));
                    break;
                case "Settings":
                    mainFrame.Navigate(typeof(PageUser));
                    break;
                default:
                    break;
            }

        }

        //private void ifConnected()
        //{
        //    bool connected;
        //    connected = Singleton.Instance().getConnectionUser();


        //    if (connected == true)
        //    {
        //        testVisibility.Visibility = Visibility.Visible;
        //    }
        //}
    }
}

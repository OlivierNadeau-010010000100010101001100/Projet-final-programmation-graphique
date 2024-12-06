﻿using Microsoft.UI.Xaml;
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
    public sealed partial class PageSupprimerSeance : Page
    {
        public PageSupprimerSeance()
        {
            this.InitializeComponent();
            OnLoad();
            Singleton.Instance().SetMessageErreur(TestMessage);
        }

        public void OnLoad()
        {
            LVTestSeance.ItemsSource = Singleton.Instance().GetAllSeances();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var seance = (Seance)button.Tag;

                if (seance != null)
                {
                    Singleton.Instance().SupprimerSeance(seance.Id);

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

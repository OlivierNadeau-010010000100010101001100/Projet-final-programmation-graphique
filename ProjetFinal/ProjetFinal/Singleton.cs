using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Singleton
    {


        static Singleton instance = null;
        private TextBlock _messageErreur;

        public string conn { get; private set; }

        private Singleton()
        {
            conn = "Server=cours.cegep3r.info;Database=420345ri_gr00002_2011835-dylann-palardy;Uid=2011835;Pwd=2011835;";
        }

        public static Singleton Instance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }

        public void SetMessageErreur(TextBlock messageErreur)
        {
            _messageErreur = messageErreur;
        }


        



    }
}

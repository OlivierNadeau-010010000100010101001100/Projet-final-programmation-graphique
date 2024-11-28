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

        public string connectionQuery { get; private set; }

        private Singleton()
        {
            connectionQuery = "Server=cours.cegep3r.info;Database=a2024_420335ri_eq8;Uid=2011835;Pwd=2011835;";
            
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

        public void TestConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionQuery);  
            try
            {
                
                conn.Open();
                _messageErreur.Text = "Reussi";
            }
            catch (Exception ex)
            {
                _messageErreur.Text = ex.Message;  
            }
            finally
            {
                    conn.Close();
            }
        }


        public bool getConnectionUser()
        {
            return true;
        }
        



    }
}

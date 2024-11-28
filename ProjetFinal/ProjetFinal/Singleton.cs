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
                _messageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                _messageErreur.Text = "Reussi";
            }
            catch (Exception ex)
            {
                _messageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                _messageErreur.Text = $"{ex.Message}";  
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

        public List<Categorie> GetAllCategories()
        {
            List<Categorie> categories = new List<Categorie>();


            MySqlConnection conn = new MySqlConnection(connectionQuery);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM categorie", conn);
                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    var categorie = new Categorie
                    {
                        Categorie_id = mySqlDataReader.GetInt32(0),
                        Categorie_nom = mySqlDataReader.GetString(1)
                    };
                    categories.Add(categorie);
                }




            }
            catch (Exception ex)
            {

                _messageErreur.Text = $"Erreur base de donnée: {ex.Message}";
                _messageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                _messageErreur.Visibility = Visibility.Visible;

                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

                return categories;
        }

        public List<Activite> GetAllActivites()
        {
            List<Activite> activites = new List<Activite>();


            MySqlConnection conn = new MySqlConnection(connectionQuery);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM activites", conn);
                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    var activite = new Activite
                    {
                        Activite_id = mySqlDataReader.GetInt32(0),
                        Nom_activite = mySqlDataReader.GetString(1),
                        Categorie_id_fk = mySqlDataReader.GetInt32(2),
                        Cout_organisation_client = mySqlDataReader.GetInt32(3),
                        Prix_vente = mySqlDataReader.GetInt32(4),
                        categorie_activite = "test"



                    };

                    //MySqlCommand categoryCmd = new MySqlCommand("SELECT nom_categorie FROM categorie WHERE categorie_id = @Categorie_id", conn);
                    //categoryCmd.Parameters.AddWithValue("@Categorie_id", activite.Categorie_id_fk);

                    //// Execute the query for category name
                    //MySqlDataReader categoryReader = categoryCmd.ExecuteReader();
                    //if (categoryReader.Read())
                    //{
                    //    activite.categorie_activite = categoryReader.GetString(0); // Get category name
                    //}
                    //categoryReader.Close();






                    activites.Add(activite);
                }




            }
            catch (Exception ex)
            {

                _messageErreur.Text = $"Erreur base de donnée: {ex.Message}";
                _messageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                _messageErreur.Visibility = Visibility.Visible;

                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return activites;
        }


    }
}

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

        public static bool isConnected = false;
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
                var categoriesDictionary = GetCategoriesDictionary();

                while (mySqlDataReader.Read())
                {
                    var activite = new Activite
                    {
                        Activite_id = mySqlDataReader.GetInt32(0),
                        Nom_activite = mySqlDataReader.GetString(1),
                        Categorie_id_fk = mySqlDataReader.GetInt32(2),
                        Cout_organisation_client = mySqlDataReader.GetInt32(3),
                        Prix_vente = mySqlDataReader.GetInt32(4),
                        Categorie_activite = categoriesDictionary.ContainsKey(mySqlDataReader.GetInt32(2)) ? categoriesDictionary[mySqlDataReader.GetInt32(2)] : "Invalide"



                    };

                    






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

        public Dictionary<int, string> GetCategoriesDictionary()
        {
            Dictionary<int, string> categories = new Dictionary<int, string>();

            MySqlConnection conn = new MySqlConnection(connectionQuery);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM GetCategorieDictionaire", conn); //j'ai cree une view 
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(reader.GetInt32(0), reader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                _messageErreur.Text = $"Erreur base de donnée: {ex.Message}";
                _messageErreur.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                _messageErreur.Visibility = Visibility.Visible;
            }
            finally
            {
                conn.Close();
            }

            return categories;
        }

        public bool checkUserConn(string username, string password, int value) //le checkbox qui check si cest admin ou pas change la value de 0 a 1
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _messageErreur.Text = $"Il faut remplir les 2 champs";
                return true;

            }


            MySqlConnection conn = new MySqlConnection(connectionQuery);

            try
            {
                string fonction = value == 1 ? "loginAdmin" : "login";

                MySqlCommand cmd = new MySqlCommand($"SELECT {fonction}(@username, @password)", conn); //fonction login que jai fait



                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                conn.Open();
                int resultat = Convert.ToInt32(cmd.ExecuteScalar());

                if (resultat == 1)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                _messageErreur.Text = $"Erreur base de donnée: {ex.Message}";
            }
            finally { conn.Close(); }

            return true;
        }


        public static void setUserConn(bool _isConnected)
        {
            isConnected = _isConnected;
        }

    }
}

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
        
        public static bool _isConnected = false; // vérifie si l'utilisateur est bien connecté
        static Singleton instance = null;
        private TextBlock _message; //gestion des messages et messages d'erreur
        private string _connectionQuery;
        public static Singleton Instance() => instance ??= new Singleton();

        /* ********************************************************** CONFIGURATION INITIALE **************************************************** */

        private Singleton()
        {
            _connectionQuery = "Server=cours.cegep3r.info;Database=a2024_420335ri_eq8;Uid=2011835;Pwd=2011835;"; //connection a la base de donnée
        }

        private MySqlConnection Connection()    //Methode pour la connection DB
        {
            return new MySqlConnection(_connectionQuery);
        }

        public void TestConnection()    //Juste un test de connection
        {
            MySqlConnection conn = Connection();
            try
            {
                conn.Open();
                Message("Reussi");
            }
            catch (Exception ex) { MessageErreur("", ex.Message); }
            finally { conn.Close(); }
        }

        /* ********************************************************** GESTION DES TABLES DE BASE DE DONNÉE **************************************************** */

        public List<Categorie> GetAllCategories()   //vas chercher tout les categories et les mets dans une liste
        {
            List<Categorie> categories = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT * FROM categorie", conn);
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
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return categories;
        }

        public Dictionary<int, string> GetCategoriesDictionary()    //cree un dictionaire pour mettre les id de categorie et leur nom ensemble
        {
            Dictionary<int, string> categories = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT * FROM GetCategorieDictionaire", conn); //j'ai cree une view 
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(reader.GetInt32(0), reader.GetString(1));
                }
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return categories;
        }

        public List<Activite> GetAllActivites()     //vas chercher tout les actrivite et les mets dans une liste
        {
            List<Activite> activites = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT * FROM activites", conn);
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
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return activites;
        }

        /* ********************************************************** GESTION DES CONNECTIONS UTILISATEURS **************************************************** */

        public bool UserConnection(string username, string password, int value) //la connection a l'usager, le "value" est pour savoir si cest un admin(1) ou un usager(0)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageErreur("Il faut remplir les 2 champs", "");
                return true;
            }
            var conn = Connection();

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
                    Message("Connection Réussi");
                    return false;
                }
                
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            MessageErreur("Nom d\'utilisateur ou mot de passe invalide", "");
            return true;
        }

        public bool getConnectionUser() //???
        {
            return true;
        }
        public static void SetUserConn(bool isConnected)    //change la connection pour vrai ou faux a travers l'instance du programme
        {
            _isConnected = isConnected;
        }

        public static bool CheckConnection()
        {
            return _isConnected;
        }
        /* ********************************************************** GESTION DES MESSAGES D'ERREURS **************************************************** */

        public void SetMessageErreur(TextBlock message)   //set le message qui sera envoye au visuel
        {
            _message = message;
        }

        public void Message(string message) //pour les messages positif VERT
        {
            _message.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
            _message.Text = message;
            _message.Visibility = Visibility.Visible;
        }

        public void MessageErreur(string message, string erreur)    //pour les messages d'erreur ROUGE
        {
            _message.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
            _message.Text = $"{message} {erreur}";
            _message.Visibility = Visibility.Visible;
        }



    }
}

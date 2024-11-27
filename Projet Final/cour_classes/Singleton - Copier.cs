using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cour_classes
{

    internal class Singleton
    {
        MySqlConnection con;
        ObservableCollection<Produit> liste;
        ObservableCollection<Categorie> listeCategorie;
        static Singleton instance = null;

        //int valeurMin;
        //int valeurMax;
        string categorieSelectionnerFiltre;
        bool categorieSet = false;

        public Singleton()
        {
            con = new MySqlConnection("");
            listeSeance = new ObservableCollection<Produit>();
            listeCategorie = new ObservableCollection<Categorie>();
        }

        public static Singleton getInstance()
        {
            if (instance == null)
                instance = new Singleton();
            return instance;
        }

        public void getProduit()
        {
            liste.Clear();

            MySqlCommand comm = new MySqlCommand();
            comm.Connection = con;
            comm.CommandText = "Select * from produits";

            con.Open();
            MySqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                string nom = reader[0].ToString();
                double prix = Convert.ToDouble(reader[1].ToString());
                string categorie = reader[2].ToString();


                Produit produit = new Produit(nom, prix, categorie);
                liste.Add(produit);
            }

            reader.Close();
            con.Close();
        }



        public void supprimerProduit(string nom_produit, double prix_produit, string categorie_produit )
        {

            using (MySqlCommand commandSetProduits = new MySqlCommand())
            {
                commandSetProduits.Connection = con;
                commandSetProduits.CommandText = "DELETE FROM produits WHERE nom = @nom_produit AND prix = @prix_produit AND categorie = @categorie_produit ";

                // Ajout des valeurs des paramètres
                commandSetProduits.Parameters.AddWithValue("@nom_produit", nom_produit);
                commandSetProduits.Parameters.AddWithValue("@prix_produit", prix_produit);
                commandSetProduits.Parameters.AddWithValue("@categorie_produit", categorie_produit);

                // Ouverture de la connexion, exécution de la commande, et fermeture
                con.Open();
                commandSetProduits.ExecuteNonQuery();
                con.Close();

            }

        }

        /*****************************************************************************************************************************************************/
        public void getProduitFiltrer(string categorieChoix, double prixMin, double prixMax)
        {
            liste.Clear();

            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.Connection = con;
                string buildDuQuerySql = string.Empty;

                // Si "Tous les produits" est sélectionné, ne pas ajouter de condition de catégorie
                if (!string.IsNullOrEmpty(categorieChoix) && categorieChoix != "Tous les produits")
                {
                    // Ajouter une condition pour la catégorie seulement si elle n'est pas "Tous les produits"
                    buildDuQuerySql = "SELECT * FROM produits WHERE prix BETWEEN @prixMin AND @prixMax AND categorie = @categorie";
                    comm.Parameters.AddWithValue("@categorie", categorieChoix);
                }
                else
                {
                    // Si "Tous les produits" est sélectionné, ne pas inclure de condition pour la catégorie
                    buildDuQuerySql = "SELECT * FROM produits WHERE prix BETWEEN @prixMin AND @prixMax";
                }

                comm.CommandText = buildDuQuerySql;

                // Ajouter les paramètres de prix
                comm.Parameters.AddWithValue("@prixMin", prixMin);
                comm.Parameters.AddWithValue("@prixMax", prixMax);

                // Exécuter la requête
                con.Open();
                using (MySqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nom = reader["nom"].ToString();
                        double prix = Convert.ToDouble(reader["prix"]);
                        string categorie = reader["categorie"].ToString();

                        // Créer l'objet Produit et l'ajouter à la liste
                        Produit produit = new Produit(nom, prix, categorie);
                        liste.Add(produit);
                    }
                }
                // Fermer la connexion à la base de données
                con.Close();
            }
        }




        public void setProduit(List<Produit> liste)
        {

            string nom;
            double prix;
            string categorie;


            try
            {
                foreach (Produit pItem in liste)
                {
                    nom = pItem.Nom;
                    prix = pItem.Prix;
                    categorie = pItem.Categorie;

                    using (MySqlCommand commandSetProduits = new MySqlCommand())
                    {
                        commandSetProduits.Connection = con;
                        commandSetProduits.CommandText = "INSERT INTO produits (nom, prix, categorie) VALUES (@nom, @prix, @categorie)";

                        // Ajout des valeurs des paramètres
                        commandSetProduits.Parameters.AddWithValue("@nom", nom);
                        commandSetProduits.Parameters.AddWithValue("@prix", prix);
                        commandSetProduits.Parameters.AddWithValue("@categorie", categorie);

                        // Ouverture de la connexion, exécution de la commande, et fermeture
                        con.Open();
                        commandSetProduits.ExecuteNonQuery();
                        con.Close();
                    }
                }
        }
            catch (Exception ex)
            {
                // Gestion de l'exception et fermeture de la connexion si nécessaire
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }

        public ObservableCollection<Produit> getListeActuelle()
        {
            return liste;
        }

        public ObservableCollection<Produit> getListe()
        {
            getProduit();
            return liste;
        }

        public ObservableCollection<Categorie> getListeCategorie()
        {
            getCategorie();
            return listeCategorie;
        }


        public void toutSupprimerBD()
        {
            using (MySqlCommand commandSetProduits = new MySqlCommand())
            {
                commandSetProduits.Connection = con;
                commandSetProduits.CommandText = "DELETE FROM produits";


                // Ouverture de la connexion, exécution de la commande, et fermeture
                con.Open();
                commandSetProduits.ExecuteNonQuery();
                con.Close();

            }
        }

        public string commandExecuter1reponse(string commandeEntrer)
        {
            string resultat = "";

            using (MySqlCommand commandSetProduits = new MySqlCommand())
            {
                commandSetProduits.Connection = con;
                commandSetProduits.CommandText = $"{commandeEntrer}";


                // Ouverture de la connexion, exécution de la commande/ recevoir les datas, et fermeture
                con.Open();
                // met le résultat de la requete dans un var
                var commandResult = commandSetProduits.ExecuteScalar();
                // convertir le var en string pour l'renvoyer le résultat de la requête
                resultat = commandResult.ToString();
                con.Close();

            }
            return resultat;
        }


        //public void getProduit()
        //{
        //    liste.Clear();

        //    MySqlCommand comm = new MySqlCommand();
        //    comm.Connection = con;
        //    comm.CommandText = "Select * from produits";

        //    con.Open();
        //    MySqlDataReader reader = comm.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        string nom = reader[0].ToString();
        //        double prix = Convert.ToDouble(reader[1].ToString());
        //        string categorie = reader[2].ToString();


        //        Produit produit = new Produit(nom, prix, categorie);
        //        liste.Add(produit);
        //    }

        //    reader.Close();
        //    con.Close();
        //}
        public void getCategorie()
        {
            // crée la liste de retour
            var resultatsListeRequete = new List<(string categorie, int nbrParCategorie)>();

            // réinitialisation de la listeCategorie
            listeCategorie.Clear();


            using (MySqlCommand comm = new MySqlCommand())
            {
                comm.Connection = con;
                comm.CommandText = "SELECT categorie, COUNT(nom) AS nbr_produit FROM produits GROUP BY categorie";


                con.Open();
                using (MySqlDataReader reader = comm.ExecuteReader())
                {
                    // met chaques retour dans la liste
                    while (reader.Read())
                    {
                        string categorie_produit = reader["categorie"].ToString();
                        int nbrCategorie_produit = Convert.ToInt32(reader["nbr_produit"]);

                        Categorie produit = new Categorie(categorie_produit, nbrCategorie_produit);
                        listeCategorie.Add(produit);

                    }
                }
                con.Close();
            }
        }
    }
}

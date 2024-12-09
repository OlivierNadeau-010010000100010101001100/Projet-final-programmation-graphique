using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using MySqlX.XDevAPI;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace ProjetFinal
{
    internal class Singleton
    {
        public static event Action UserConnectionChange; //evenement qui refresh le nom d'utilisateur quand il est invoke
        private static string _userType = "";
        private static bool _isConnected = false; // vérifie si l'utilisateur est bien connecté
        private static string _username = string.Empty;
        private static string _userID = string.Empty;
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


        public List<Seance> GetAllSeances()
        {
            List<Seance> seances = new();
            var activites = GetAllActivites();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT * FROM seance", conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                
                while(reader.Read())
                {
                    var seance = new Seance
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        Heure = reader.GetString(2),
                        NbrPlaces = reader.GetInt32(3),
                        ActiviteID = reader.GetInt32(4),
                        NomActivite = activites.FirstOrDefault(a => a.Activite_id == reader.GetInt32(4))?.Nom_activite ?? "Inconnu"

                    };
                    seances.Add(seance);
                }
            }
            catch { }
            finally { conn.Close(); }

            return seances;
        }


        public List<Adherent> GetAllAdherents()     //vas chercher tout les adherents et les mets dans une liste
        {
            List<Adherent> adherents = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT * FROM adherents", conn);
                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    var adherent = new Adherent
                    {
                        Adherent_id = mySqlDataReader.GetString(0),
                        Adherent_nom = mySqlDataReader.GetString(1),
                        Adherent_Prenom = mySqlDataReader.GetString(2),
                        Adherent_adresse = mySqlDataReader.GetString(3),
                        Adherent_date_naissance = mySqlDataReader.GetDateTime(4).ToString("yyyy-MM-dd"),
                        Adherent_age = mySqlDataReader.IsDBNull(6) ? 0 : mySqlDataReader.GetInt32(6),

                    };
                    adherents.Add(adherent);
                }
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return adherents;
        }

        public List<ActivitePersonne> GetAllNbrPersonneActivite()
        {
            List<ActivitePersonne> liste = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT nom_activitee, COUNT(adherent_id_fk) AS nombre_participant FROM activites\r\nJOIN seance s on activites.activite_id = s.activite_id_fk\r\nJOIN inscription_seance i on s.seance_id = i.seance_id_fk\r\nGROUP BY nom_activitee;", conn);
                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();
                var categoriesDictionary = GetCategoriesDictionary(); //??

                while (mySqlDataReader.Read())
                {
                    var activitePersonnes = new ActivitePersonne
                    {
                        Nom_Activite = mySqlDataReader.GetString(0),
                        Nbr_personne = mySqlDataReader.GetInt32(1),
                    };
                    liste.Add(activitePersonnes);
                }
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return liste;
        }






        public int getNbrAdherent()
        {
            int nbrAdherent = 0;
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT getNbrAdherents()", conn);
                conn.Open();

                nbrAdherent = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée:", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return nbrAdherent;
        }

        public int getNbrActivites()
        {
            int nbrActivites = 0;
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT getNbrActivites()", conn);
                conn.Open();

                nbrActivites = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée:", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return nbrActivites;
        }
        


        public void SupprimerActivite(int activiteID)     //suppression d'activite, a revoir pour gestion correctement
        {
                var conn = Connection();
                
            try
            {
                MySqlCommand cmd = new("DELETE FROM activites WHERE activite_id = @activite", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@activite", activiteID);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageErreur("Erreur base de donnée", ex.Message);
            }
        }

        public void SupprimerUsager(string usagerID)     //suppression d'usager, a revoir pour gestion correctement
        {
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("DELETE FROM adherents WHERE adherent_id = @usager", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@usager", usagerID);
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée", ex.Message);
            }
            finally { conn.Close(); }
        }

        public void SupprimerSeance(int seanceID)     //suppression de seance, a revoir pour gestion correctement
        {
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("DELETE FROM seance WHERE seance_id = @seance", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@seance", seanceID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée", ex.Message);
            }
        }

        public void SupprimerCategorie(int categorieID)     //suppression de seance, a revoir pour gestion correctement
        {
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("DELETE FROM categorie WHERE categorie_id = @categorie", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@categorie", categorieID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée", ex.Message);
            }
        }

        public List<Seance> GetSeanceCliquer(string nom_activite)
        {
            List<Seance> liste = new();
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("SELECT seance_id, date_seance, heure_seance, nbrPlaceDispo, activite_id_fk FROM seance\r\nINNER JOIN a2024_420335ri_eq8.activites a on seance.activite_id_fk = a.activite_id\r\nWHERE nom_activitee = @nom_activite", conn);

                cmd.Parameters.AddWithValue("@nom_activite", nom_activite);

                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    var e = new Seance
                    {
                        Id = mySqlDataReader.GetInt32(0),
                        Date = mySqlDataReader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        Heure = mySqlDataReader.GetString(2),
                        NbrPlaces = mySqlDataReader.GetInt32(3),
                        ActiviteID = mySqlDataReader.GetInt32(4),
                    };
                    liste.Add(e);
                }
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée:", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return liste;
        }


        public List<MoyenneRatingParActivite> GetAllRatingActivite()
        {
            List<MoyenneRatingParActivite> liste = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT nom_activitee,FORMAT(AVG(note_appreciation),1) AS note_appreciation FROM activites\r\nJOIN seance s on activites.activite_id = s.activite_id_fk\r\nJOIN inscription_seance i on s.seance_id = i.seance_id_fk\r\nWHERE note_appreciation IS NOT NULL\r\nGROUP BY nom_activitee;", conn);
                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                var e = new MoyenneRatingParActivite
                {
                    Nom_activite = mySqlDataReader.GetString(0),
                    Rating_activite = mySqlDataReader.GetString(1),
                };
                liste.Add(e);
            }
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return liste;
        }


        public int getNbrRatingManquant()
        {
            int nbrRatingManquant = 0;
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT getNbrRatingManquant()", conn);
                conn.Open();

                nbrRatingManquant = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée:", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return nbrRatingManquant;
        }

        public int getNbrSeancePasserDate()
        {
            int nbrSeancePAsserDate = 0;
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT getNbrSeancePasserDate()", conn);
                conn.Open();

                nbrSeancePAsserDate = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée:", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return nbrSeancePAsserDate;
        }


        public string getMoyennePrixClient()
        {
            string MoyennePrix = "";
            var conn = Connection();  // Assurez-vous que Connection() crée une connexion valide.

            try
            {
                // Exécution de la requête en appelant la fonction SQL "getMoyennePrix"
                MySqlCommand cmd = new MySqlCommand("SELECT getMoyennePrix()", conn);
                conn.Open();

                // Utilisation de ExecuteScalar pour obtenir la première valeur de la colonne
                var result = cmd.ExecuteScalar();

                // Si le résultat est non null, on le convertit en string
                if (result != null)
                {
                    MoyennePrix = result.ToString();
                }
                else
                {
                    MoyennePrix = "Aucune donnée";  // Si pas de résultat, on peut afficher un message par défaut
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher un message d'erreur
                MessageErreur("Erreur base de données:", ex.Message);
            }
            finally
            {
                conn.Close();  // Assurez-vous de fermer la connexion dans le bloc finally
            }

            return MoyennePrix;
        }

        public List<MoyenneRatingParActivite> setInscriptionSeance()
        {
            List<MoyenneRatingParActivite> liste = new();
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("SELECT nom_activitee,FORMAT(AVG(note_appreciation),1) AS note_appreciation FROM activites\r\nJOIN seance s on activites.activite_id = s.activite_id_fk\r\nJOIN inscription_seance i on s.seance_id = i.seance_id_fk\r\nWHERE note_appreciation IS NOT NULL\r\nGROUP BY nom_activitee;", conn);
                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    var e = new MoyenneRatingParActivite
                    {
                        Nom_activite = mySqlDataReader.GetString(0),
                        Rating_activite = mySqlDataReader.GetString(1),
                    };
                    liste.Add(e);
                }
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            return liste;
        }

        public bool checkInscriptionSeance(int seanceID)
        {
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM inscription_seance WHERE adherent_id_fk = @adherentId AND seance_id_FK = @seanceID;", conn);
                cmd.Parameters.AddWithValue("@adherentId", _userID);
                cmd.Parameters.AddWithValue("@seanceID", seanceID);

                conn.Open();

                var result = cmd.ExecuteScalar();

                if (Convert.ToInt32(result) > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher un message d'erreur
                MessageErreur("Erreur base de données:", ex.Message);
                return false;  // Retourne faux en cas d'erreur
            }
            finally
            {
                conn.Close();  // Assurez-vous de fermer la connexion dans le bloc finally
            }
        }

        

        public List<Seance> GetMesSeance()
        {
            List<Seance> liste = new();
            Dictionary<int, int> rating = new();
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("SELECT seance_id, date_seance, heure_seance, nbrPlaceDispo, activite_id_fk, note_appreciation FROM inscription_seance\r\nINNER JOIN a2024_420335ri_eq8.seance s on inscription_seance.seance_id_fk = s.seance_id\r\nWHERE adherent_id_fk = @userID;", conn);

                cmd.Parameters.AddWithValue("@userID", _userID);

                conn.Open();
                MySqlDataReader mySqlDataReader = cmd.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    var e = new Seance
                    {
                        Id = mySqlDataReader.GetInt32(0),
                        Date = mySqlDataReader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        Heure = mySqlDataReader.GetString(2),
                        NbrPlaces = mySqlDataReader.GetInt32(3),
                        ActiviteID = mySqlDataReader.GetInt32(4),
                        Rating = mySqlDataReader.IsDBNull(5) ? null : mySqlDataReader.GetInt32(5)


                    };
                    

                    liste.Add(e);
                }
            }
            catch (Exception ex)
            {
                MessageErreur("Erreur base de donnée:", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            

            return liste;
        }

        //public void UpdateRatingSeance
        //public List<Seance> GetMesSeances()
        //{
        //    var conn = Connection();

        //    try
        //    {
        //        MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM inscription_seance WHERE adherent_id_fk = @adherentId AND seance_id_FK = @seanceID;", conn);
        //        cmd.Parameters.AddWithValue("@adherentId", adherentId);

        //        conn.Open();

        //        var result = cmd.ExecuteScalar();

        //        if (Convert.ToInt32(result) > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // En cas d'erreur, afficher un message d'erreur
        //        MessageErreur("Erreur base de données:", ex.Message);
        //        return false;  // Retourne faux en cas d'erreur
        //    }
        //    finally
        //    {
        //        conn.Close();  // Assurez-vous de fermer la connexion dans le bloc finally
        //    }
        //}

        public bool AjoutInscription(int seanceID)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("INSERT INTO inscription_seance(adherent_id_fk, seance_id_fk) VALUE (@adherentID, @seanceID);)", conn);
                conn.Open();

                cmd.Parameters.AddWithValue("@adherentID", _userID);
                cmd.Parameters.AddWithValue("@seanceID", seanceID);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool UpdateRating(int seanceID, int rating_user)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("UPDATE inscription_seance SET note_appreciation = @rating_user WHERE adherent_id_fk = @adherentID AND seance_id_fk = @seanceID;)", conn);
                conn.Open();

                cmd.Parameters.AddWithValue("@adherentID", _userID);
                cmd.Parameters.AddWithValue("@seanceID", seanceID);
                cmd.Parameters.AddWithValue("@rating_user", rating_user);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }



        public bool AjoutAdherent(string prenom, string nom, string adresse, string date, string mdp)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("INSERT INTO Adherents (nom_adherent, prenom_adherent, adresse_adherent, date_naissance_adherent, adherent_mot_de_passe)\r\nVALUES (@nom, @prenom, @adresse, @date, @mdp)", conn);
                conn.Open();

                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@mdp", mdp);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool ModifierAdherent(string id, string prenom, string nom, string adresse, string date)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("UPDATE adherents SET date_naissance_adherent = @date, prenom_adherent = @prenom, nom_adherent = @nom, adresse_adherent = @adresse WHERE adherent_id = @id;", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@date", date);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool AjoutActivite(string nom, int prixOrganisation, int prixClient, int fkCategorie)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("INSERT INTO activites (nom_activitee, categorie_id_fk, cout_organisation_client, prix_vente)\r\nVALUES (@nom, @fkCategorie, @prixOrganisation, @prixClient)", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@fkCategorie", fkCategorie);
                cmd.Parameters.AddWithValue("@prixOrganisation", prixOrganisation);
                cmd.Parameters.AddWithValue("@prixClient", prixClient);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally { conn.Close(); }

        }

        public bool ModifierActivite(int id, string nom, int prixOrganisation, int prixClient, int fkCategorie)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("UPDATE activites SET nom_activitee = @nom, categorie_id_fk = @fkCategorie, cout_organisation_client = @prixOrganisation, prix_vente = @prixClient WHERE activite_id = @id", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@fkCategorie", fkCategorie);
                cmd.Parameters.AddWithValue("@prixOrganisation", prixOrganisation);
                cmd.Parameters.AddWithValue("@prixClient", prixClient);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally { conn.Close(); }

        }

        public bool AjoutSeance(string date, string heure, int places, int fk_id)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("INSERT INTO seance (date_seance, heure_seance, nbrPlaceDispo, activite_id_fk) \r\nVALUES (@date, @heure, @places, @fk_id)", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@heure", heure);
                cmd.Parameters.AddWithValue("@places", places);
                cmd.Parameters.AddWithValue("@fk_id", fk_id);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally { conn.Close(); }
        }

        public bool AjoutCategorie(string categorie)
        {
            var conn = Connection();
            try
            {
                MySqlCommand cmd = new("INSERT INTO categorie (nom_categorie) VALUE (@categorie)", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@categorie", categorie);
                

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally { conn.Close(); }
        }

        public bool ModifierSeance(int id, string date, string heure, int places, int idfk)
        {
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("UPDATE seance SET date_seance = @date, heure_seance = @heure, nbrPlaceDispo = @places, activite_id_fk = @idfk WHERE seance_id = @id", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@heure", heure);
                cmd.Parameters.AddWithValue("@places", places);
                cmd.Parameters.AddWithValue("@idfk", idfk);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally { conn.Close(); }
        }

        public bool ModifierCategorie(int id, string categorie)
        {
            var conn = Connection();

            try
            {
                MySqlCommand cmd = new("UPDATE categorie SET nom_categorie = @categorie WHERE categorie_id = @id", conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@categorie", categorie);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageErreur("", ex.Message);
                return false;
            }
            finally { conn.Close(); }
        }


        /* ********************************************************** GESTION DES CONNECTIONS UTILISATEURS **************************************************** */

        public bool UserConnection(string username, string password, int value) //la connection a l'usager, le "value" est pour savoir si cest un admin(1) ou un usager(0)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageErreur("Il faut remplir les 2 champs", "");
                return false;
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

                if (resultat == 1 && value == 1)
                {
                    _username = username;
                    _userType = "admin";
                    Message("Connection Réussi");
                    return true;

                } else if(resultat == 1 && value == 0) { //si cest PAS un admin et que la session existe ca utilise la fonction nomUtilisateur pour retourner le nom de la personne
                    cmd.CommandText = "SELECT nomUtilisateur(@id)"; 
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", username);
                    string fullName = cmd.ExecuteScalar()?.ToString();
                    _username = fullName;
                    _userID = username;
                    Message("Connection Réussi");
                    _userType="user";
                    return true;
                }
            }
            catch (Exception ex) { MessageErreur("Erreur base de donnée:", ex.Message); }
            finally { conn.Close(); }

            MessageErreur("Nom d\'utilisateur ou mot de passe invalide", "");
            return false;
        }

        public static void SetUserConn(bool isConnected)    //change la connection pour vrai ou faux a travers l'instance du programme
        {
            _isConnected = isConnected;
            _username = isConnected ? _username : "Connection";
            UserConnectionChange.Invoke();  //invoque l'evenement pour refresh le nom d'utilisateur
        }

        public static bool GetUserConnection()    //retourne l'état de la connection 
        {
            return _isConnected;
        }

        public static string GetUsername()  //prends le username de la session
        {
            return _username;
        }

        public static string GetUserType() // prend le type du user de la session
        {
            return _userType;
        }

        public void ResetUserType() // modifie  le user à un "invité" dans le programme, comme cela quand qq1 se déconnecte, il ne voit plus la partie dédié aux users et admins
        {
            _userType = "";
            _userID = "";
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

        /* ******************************************************************* EXPORTATION CSV ********************************************************** */


        public async void ExportationCSV(List<Activite> activites)
        {

           
                var picker = new Windows.Storage.Pickers.FileSavePicker();

                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this); //erreur ici
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

                picker.SuggestedFileName = "test2";
                picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".txt" });
                picker.FileTypeChoices.Add("Fichier CSV", new List<string>() { ".csv" });

                //crée le fichier
                Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();




                // La fonction ToString de la classe Client retourne: nom + ";" + prenom

                await Windows.Storage.FileIO.WriteLinesAsync(monFichier, activites.ConvertAll(x => x.ToString()), Windows.Storage.Streams.UnicodeEncoding.Utf8);
            
           
        }

    }
}

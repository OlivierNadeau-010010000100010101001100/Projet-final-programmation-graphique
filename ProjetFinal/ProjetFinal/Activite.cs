using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Activite
    {
        string nom_activitee, categorie_activite;
        int categorie_id_fk, cout_organisation_client, prix_vente, activite_id;


        public Activite()
        {

        }

        public Activite(int activite_id, string nom_activitee, int categorie_id_fk, int cout_organisation_client, int prix_vente)
        {
            this.activite_id = activite_id;
            this.nom_activitee = nom_activitee;
            this.categorie_id_fk = categorie_id_fk;
            this.cout_organisation_client = cout_organisation_client;
            this.prix_vente = prix_vente;
        }

        public int Activite_id { get => activite_id; set => activite_id = value; }

        public string Nom_activite { get => nom_activitee; set => nom_activitee = value; }

        public int Categorie_id_fk { get => categorie_id_fk; set => categorie_id_fk = value; }

        public int Cout_organisation_client { get => cout_organisation_client; set => cout_organisation_client = value; }

        public int Prix_vente { get => prix_vente; set => prix_vente = value; }

        public string Categorie_activite { get => categorie_activite; set => categorie_activite = value; }

        public override string ToString()
        {
            return $"{Activite_id};{Nom_activite};{Categorie_id_fk};{Cout_organisation_client};{Prix_vente};{Categorie_activite}";
        }
    }
}

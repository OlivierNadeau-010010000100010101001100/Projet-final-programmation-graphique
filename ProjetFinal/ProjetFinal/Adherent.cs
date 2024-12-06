using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Adherent
    {
        string id, nom, prenom, adresse, date_naissance;
        int age;
        public Adherent() { }

        public Adherent(string id, string nom, string prenom, string adresse, string date_naissance, int age)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.date_naissance = date_naissance;
            this.age = age;
        }

        public string Adherent_id { get => id; set => id = value; }

        public string Adherent_nom { get => nom; set => nom = value; }

        public string Adherent_Prenom { get => prenom; set => prenom = value; }

        public string Adherent_adresse { get => adresse; set => adresse = value; }

        public string Adherent_date_naissance { get => date_naissance; set => date_naissance = value;}

        public int Adherent_age { get => age; set => age = value; }

        public override string ToString()
        {
            return $"{Adherent_id};{Adherent_nom};{Adherent_Prenom};{Adherent_adresse};{Adherent_date_naissance};{Adherent_age}";
        }

    }
}

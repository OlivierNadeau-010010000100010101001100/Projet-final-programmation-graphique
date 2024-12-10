using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Seance
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Heure { get; set; }
        public int NbrPlaces { get; set; }

        public string Rating { get; set; }
        public string NomActivite { get; set; }

        

        public Seance() { }

        public Seance(int id, string date, string heure, int nbrPlaces, string nom_activite)
        {
            Id = id;
            Date = date;
            Heure = heure;
            NbrPlaces = nbrPlaces;
            NomActivite = nom_activite;
        }

        public Seance(int id, string date, string heure, int nbrPlaces, string nom_activite, string rating)
        {
            Id = id;
            Date = date;
            Heure = heure;
            NbrPlaces = nbrPlaces;
            NomActivite = nom_activite;
            Rating = rating;
        }

        public override string ToString()
        {
            return $"Seance {Id}, Date: {Date}, Heure: {Heure}, NbrPlaces: {NbrPlaces}, Nom Activité: {NomActivite}, Rating: {Rating}";
        }
    }
}
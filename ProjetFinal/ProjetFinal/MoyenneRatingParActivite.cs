using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class MoyenneRatingParActivite
    {
        string nom_activite;
        string rating_activite;

        public MoyenneRatingParActivite() { }

        public MoyenneRatingParActivite(string nom_activite, string rating_activite)
        {
            this.nom_activite = nom_activite;
            this.rating_activite = rating_activite;
        }

        public string Nom_activite { get => nom_activite; set => nom_activite = value; }

        public string Rating_activite { get => rating_activite; set => rating_activite = value; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cour_classes
{
    class Categorie
    {

        string nom;
        int nbr_produit;


        public Categorie(string nom, int nbr_produit)
        {
            this.nom = nom;
            this.nbr_produit = nbr_produit;
        }

        public string Nom_categorie { get => nom; set => nom = value; }
        public int Nbr_produit { get => nbr_produit; set => nbr_produit = value; }

    }
}

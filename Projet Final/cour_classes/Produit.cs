using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cour_classes
{
    internal class Produit
    {
        string nom, categorie;
        double prix;


        public Produit (string nom, double prix, string categorie)
        {
            this.nom = nom;
            this.prix = prix;
            this.categorie = categorie;
        }

        public string Nom { get => nom; set => nom = value; }
        public double Prix { get => prix; set => prix = value; }
        public string Categorie { get => categorie; set => categorie = value; }
    }
}
